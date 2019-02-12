//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
//===================================================================================

#region Usings

using System;
using System.Windows.Forms;
using Microsoft.Ccf.Common.Logging;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	class Program
	{
		[STAThread]
		static void Main()
		{
#if DEBUG
			Logging.Debug = true;
#endif
			Logging.Trace("StandAloneStub", "start");

			Application.Run(StandAloneStub.CreateInstance());

			Logging.Trace("StandAloneStub", "end");
		}
	}
}