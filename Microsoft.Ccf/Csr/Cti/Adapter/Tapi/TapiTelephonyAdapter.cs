//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file contains the methods and propeties concerned to Tapi implementation.
// 
//===============================================================================
using CtiLayerTapiLib;
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Csr.Cti
{
	/// <summary>
	/// The class for the primary part of the TAPI Telephony system.
	/// </summary>
	public class TapiTelephonyAdapter : TelephonyProvider
	{
		internal CtiLayerTapiClass CtiTapi;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public TapiTelephonyAdapter(string initializer) : base(initializer)
		{
			CtiTapi = new CtiLayerTapiClass();
		}

		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TapiTelephonyAdapter()
		{}

		/// <summary>
		/// Destructor
		/// </summary>
		~TapiTelephonyAdapter()
		{
			CtiTapi = null;
		}

		/// <summary>
		/// Initialization Method
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="user">User</param>
		/// <param name="password">Password</param>
		/// <param name="agentID">Agent ID</param>
		/// <param name="agentPassword">Agent Password</param>
		/// <param name="agentNumber">Agent Number</param>
		/// <returns>Boolean indicating success or failure.</returns>
		public override bool Init(TelephonyProvider.CtiType type, string user, string password, string agentID, string agentPassword, string agentNumber)
		{
			base.Init (type, user, password, agentID, agentPassword, agentNumber);
			if (CtiTapi.Init() != 0 )
			{
				Lines = LinesClassProvider.Instance();
				Lines.Init( this, type );
				CtiTapi.CallChanged += new CtiLayerTapiLib._DCtiLayerTapiEvents_CallChangedEventHandler( this.Cti_CallChanged );

				alreadyInited = true;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
