//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
//===============================================================================

#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Csr;
using System.Xml;

#endregion

// ** WARNING: do not use Application Isolation as it is incompatible with this sample **

namespace Microsoft.Ccf.QuickStarts
{
	/// <summary>
	/// This sample demonstrates how to construct a HostedControl that supports HAT Automations.
	/// It "parents" a .NET application (StandAloneTestApp.exe) and makes it available for
	/// interaction by Automations--much like hosted Windows applications.
	/// </summary>
	public partial class QsHatHostedControl : HostedControl
	{
		private Process _Process;
		private IntPtr _hWndMainWindow;
		private AutomationAdapter _AutomationAdapter;
		private XmlDocument _InitString = new XmlDocument();

		public QsHatHostedControl()
		{
			InitializeComponent();
		}

		// necessary constructor
		public QsHatHostedControl(int appID, string appName, string initString) : base(appID, appName, initString)
		{
			InitializeComponent();
			try
			{
				_InitString.LoadXml(initString);
			}
			catch (XmlException ex)
			{
				throw new ApplicationException("bad application initstring, not well-formed xml", ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				Close();
			}
			base.Dispose(disposing);
		}

		// necessary to propagate ContextChanges
		public override void NotifyContextChange(Context context)
		{
			_AutomationAdapter.NotifyContextChange(context);
		}

		public override void Initialize()
		{
			// application-specific
			ApplicationStartUp();

			// This block is required.
			// The first argument to the AutomationAdapter() ctor is an object reference that
			// will be passed to the DataDrivenAdapter.  The AppInitString.xml file, which
			// is the intended application initstring for this control, shows the WinDataDrivenAdapter
			// is configured--which will be expecting a handle to a window.
			// The second argument to the AutomationAdapter() is inherited from HostedControl, and
			// is required by HAT to distinguish sessions.
			_AutomationAdapter = new AutomationAdapter(_hWndMainWindow, AppHostWorkItem);
			_AutomationAdapter.Name = Name;
			_AutomationAdapter.ApplicationInitString = _InitString.OuterXml;
			_AutomationAdapter.Initialize();

			// This block is required.
			// These "implicit" actions are required by HAT. Specific ActionId values are not significant.
			int actionId = 2;
			string actionInit = "<ActionInit/>";
			AddAction(actionId++, AutomationImplicitAction.FindControl, actionInit);
			AddAction(actionId++, AutomationImplicitAction.GetControlValue, actionInit);
			AddAction(actionId++, AutomationImplicitAction.SetControlValue, actionInit);
			AddAction(actionId++, AutomationImplicitAction.ExecuteControlAction, actionInit);
			AddAction(actionId++, AutomationImplicitAction.AddDoActionEventTrigger, actionInit);
			AddAction(actionId, AutomationImplicitAction.RemoveDoActionEventTrigger, actionInit);
		}

		public override void Close()
		{
			// application-specific
			ApplicationShutDown();

			// This block is required.
			if (_AutomationAdapter != null)
			{
				_AutomationAdapter.Close();
				_AutomationAdapter = null;
			}
		}

		protected override void DoAction(RequestActionEventArgs raArgs)
		{
			switch (raArgs.Action)
			{
				case "CustomAction1":
					break;
				case "CustomAction2":
					break;
				default:
					// This block is required.
					// route unrecognized Actions to the AutomationAdapter so HAT will work
					if (actions.Contains(raArgs.Action))
					{
						_AutomationAdapter.DoAction(actions[raArgs.Action] as Action, raArgs);
					}
					break;
			}
		}

		// application-specific
		private void ApplicationStartUp()
		{
			XmlNode n = _InitString.DocumentElement.SelectSingleNode("//WorkingDirectory");
			string workingDir = (n != null) ? n.InnerText : Directory.GetCurrentDirectory();

			_Process = new Process();
			_Process.StartInfo.FileName = Path.Combine(workingDir, "Microsoft.Ccf.Samples.StandAloneTestApp.exe");
			_Process.StartInfo.WorkingDirectory = workingDir;
			_Process.StartInfo.Arguments = string.Empty;
			_Process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
			_Process.Start();

			int startTick = Environment.TickCount;
			while (Environment.TickCount - startTick < 5000)
			{
				_Process.Refresh();
				try
				{
					if (!_Process.MainWindowHandle.Equals(IntPtr.Zero))
					{
						_hWndMainWindow = _Process.MainWindowHandle;
						break;
					}
				}
				catch (InvalidOperationException)
				{
					// slow starting processes, with message pumps, may throw this exception
				}
				Application.DoEvents();
			}
			if (_hWndMainWindow.Equals(IntPtr.Zero))
			{
				throw new ApplicationException("Unable to acquire top-level window handle");
			}

			// not used in this QuickStart, but custom top-level window acquistion logic can go here
			// AcquireVB6Handle();

			_Process.WaitForInputIdle();
			_Process.Refresh();

			uint style = GetWindowLong(_hWndMainWindow, WinUserConstant.GWL_STYLE);
			style &= ~(uint)WindowStyleFlags.WS_POPUP;		// remove popup
			style &= ~(uint)WindowStyleFlags.WS_CAPTION;	// remove caption
			style &= ~(uint)WindowStyleFlags.WS_THICKFRAME;	// remove border
			style |= (uint)WindowStyleFlags.WS_CHILD;		// remove menu
			SetWindowLong(_hWndMainWindow, WinUserConstant.GWL_STYLE, style);

			SetParent(_hWndMainWindow, Handle);

			SetWindowPos(_hWndMainWindow, (IntPtr)WinUserConstant.HWND_TOP, 0, 0, Size.Width, Size.Height, WinUserConstant.SWP_SHOWWINDOW);
		}

		// application-specific
		private void ApplicationShutDown()
		{
			if (_Process != null)
			{
				_Process.Refresh();
				try
				{
					_Process.CloseMainWindow();
					_Process.WaitForExit(5000);
					if (!_Process.HasExited)
					{
						_Process.Kill();
					}
				}
				catch (SystemException)
				{
				}
				finally
				{
					_Process.Close();
					_Process.Dispose();
					_Process = null;
				}
			}
		}

		// not used in this QuickStart, but this is an example of custom top-level window acquistion logic
		private void AcquireVB6Handle()
		{
			int startTick = Environment.TickCount;
			while (Environment.TickCount - startTick < 5000)
			{
				_hWndMainWindow = FindWindow("ThunderRT6FormDC", null);
				if (!_hWndMainWindow.Equals(IntPtr.Zero))
				{
					break;
				}
				Application.DoEvents();
			}

			if (_hWndMainWindow.Equals(IntPtr.Zero))
			{
				throw new ApplicationException("unable to acquire window handle");
			}

			IntPtr pId;
			GetWindowThreadProcessId(_hWndMainWindow, out pId);
			_Process.Dispose();
			_Process = Process.GetProcessById(pId.ToInt32());
		}

		[Flags]
		internal enum WindowStyleFlags : uint
		{
			WS_POPUP = 0x80000000,
			WS_CHILD = 0x40000000,
			WS_CAPTION = 0x00C00000,	/* WS_BORDER | WS_DLGFRAME */
			WS_THICKFRAME = 0x00040000,
		}

		internal enum WinUserConstant
		{
			GWL_STYLE = (-16),
			SWP_SHOWWINDOW = 0x0040,
			HWND_TOP = 0,
		}

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint GetWindowLong(IntPtr hWnd, WinUserConstant nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, WinUserConstant uFlags);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern int SetWindowLong(IntPtr hWnd, WinUserConstant nIndex, uint dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);
	}
}
