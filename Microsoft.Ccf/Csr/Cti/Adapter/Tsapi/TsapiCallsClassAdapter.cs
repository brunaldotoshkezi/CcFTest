//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file details the implementation of Calls Class adapter
// 
//===============================================================================
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Csr.Cti
{
	/// <summary>
	/// TapiCallsClassAdapter
	/// This class exists to allow users to enumerate over lines with the
	/// foreach operator or with for ( int i = 0;... ) syntax.  Either way,
	/// you're looking into an array of calls.
	/// </summary>
	public class TsapiCallsClassAdapter : CallsClassProvider
	{
		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TsapiCallsClassAdapter() {}

		/// <summary>
		/// Provider constructor.
		/// </summary>
		/// <param name="initializer">Not used....</param>
		public TsapiCallsClassAdapter(string initializer) : base(initializer)
		{}
	}
}
