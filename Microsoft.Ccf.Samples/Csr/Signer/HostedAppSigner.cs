//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This class provides the methods to perform automatic sign-in for the hosted
// windows applications.
//
//===============================================================================

using System;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Signer.Providers;
using System.ComponentModel;

namespace Microsoft.Ccf.Samples.Csr.Signer
{
	/// <summary>
	/// This class provides the methods to peroform automatic sign-in for hosted windows applications.
	/// </summary>
	public class HostedAppSigner : ISigner
	{
		// The operations need to done on the credential control elements
		private const string SET_OPERATION = "Set";
		private const string CLICK_OPERATION = "Click";
		private delegate int Callback(IntPtr hWnd,IntPtr lParam);
		IntPtr hWnd;
		int controlSequence;
		int controlCount = 1;

		/// <summary>
		/// This method performs the login operation with the given credential field 
		/// information to identify the controls in the form and the credentials to be populated.
		/// </summary>
		/// <param name="application"></param>
		public void DoLogin(object application)
		{
			HostedControl hostedApp = (HostedControl)application;
			IntPtr topWindowHandle = hostedApp.TopLevelWindow.Handle;
			BindingList<LoginFieldRecord> fields = hostedApp.LoginFields;
		
			if (null == fields || fields.Count == 0)
			{
				return;
			}

			int setFieldCount = 0;

			// For each credential field, perform the required operation.
			foreach (LoginFieldRecord field in fields)
			{
				// Get the handle of the child control corresponding to the login field.
				IntPtr controlHandle = GetControlHandle(topWindowHandle, field.ControlSequence);

				// If the credential field is not found, then the auto sign-on is failed.
				if (IntPtr.Zero == controlHandle)
				{
					throw new LoginFieldNotFoundException(field.LabelName);
				}

				switch (field.Operation)
				{
					// Set the text of the child window to the corresponding credential.
					case SET_OPERATION:
					{
						Set(controlHandle, hostedApp.AgentCreds[setFieldCount]);
						setFieldCount ++;
						break;
					}
					case CLICK_OPERATION:
					{
						Click(topWindowHandle, controlHandle);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets tha handle of a control at the given sequence in the given window
		/// </summary>
		/// <param name="topLevelHWnd">Handle of the top level window</param>
		/// <param name="controlSequence">Hierarchical sequence of the control to find.</param>
		/// <returns>Handle of the found control</returns>
		private IntPtr GetControlHandle(IntPtr topLevelHWnd, string controlSequence)
		{
			if (null == controlSequence || string.Empty == controlSequence)
			{
				return IntPtr.Zero;
			}

			string [] sequence = controlSequence.Split(',');
			IntPtr ctrlHWnd = topLevelHWnd;

			// Go down to the control at each hierarchical level till the control
			// at the last level is reached.
			foreach (string s in sequence)
			{
				int seq = int.Parse(s);
				if (IntPtr.Zero != ctrlHWnd)
				{
					ctrlHWnd = GetControlHandle(ctrlHWnd, seq);
				}
				else
				{
					break;
				}
			}

			// If there are no control found, return null
			if (ctrlHWnd == topLevelHWnd)
			{
				return IntPtr.Zero;
			}

			return ctrlHWnd;
		}

		/// <summary>
		/// Gets the handle of the control at the spcified sequence 
		/// in the child windows (child controls) of the given window (parent control)
		/// </summary>
		/// <param name="parentHWnd">Handle of the parent control.</param>
		/// <param name="sequence">Sequence of the child control to be found.</param>
		/// <returns>Handle of the child control found.</returns>
		private IntPtr GetControlHandle(IntPtr parentHWnd, int sequence)
		{
			Callback  controlCallBack = new Callback(ChildWindowHandler);
			controlSequence = sequence;
			hWnd = IntPtr.Zero;
			controlCount = 1;
			// Enumerate the child windows and find the righ child window.
			Win32API.EnumChildWindows(parentHWnd, controlCallBack, parentHWnd);

			return hWnd;
		}

		/// <summary>
		/// Child window callback for the application window.
		/// </summary>
		/// <param name="hWnd">Handle of the child window found</param>
		/// <param name="lParam">Extra info</param>
		/// <returns>Whether the callback has successfully handled the event.</returns>
		private int ChildWindowHandler(IntPtr hWnd,IntPtr lParam)
		{
			if (controlSequence == controlCount)
			{
				this.hWnd = hWnd;
			}
			if (Win32API.GetParent(hWnd).Equals(lParam))
			{
				controlCount++;
			}
			return 1;
		}

		/// <summary>
		/// Send the WM_SETTEXT message to the given control window for 
		/// setting its text to the given string.
		/// </summary>
		/// <param name="ctrlHWnd">Handle of the control whose text need to be set.</param>
		/// <param name="text">The text to be set.</param>
		private void Set(IntPtr ctrlHWnd, string text)
		{
			Win32API.SendMessage(ctrlHWnd, (int)Win32API.StandardMessage.WM_SETTEXT, 0, text); 
		}

		/// <summary>
		/// Send the WM_COMMAND message to the window to simulate button click.
		/// </summary>
		/// <param name="parentHWnd">Handle to the parent window</param>
		/// <param name="ctrlHWnd">Handle of the control.</param>
		private void Click(IntPtr parentHWnd, IntPtr ctrlHWnd)
		{
			Win32API.SendMessage(parentHWnd, (int)Win32API.StandardMessage.WM_COMMAND, 0, ctrlHWnd);			
		}
	}

	/// <summary>
	/// Exception need to be thrown when a particular login field could not be located.
	/// </summary>
	public class LoginFieldNotFoundException : Exception
	{
		private string fieldName;

		public LoginFieldNotFoundException(string fieldName)
		{
			this.fieldName = fieldName;
		}

		/// <summary>
		/// Gets the name of the login field which could not be located.
		/// </summary>
		public string LoginFieldName
		{
			get { return this.fieldName; }
		}
	}
}
