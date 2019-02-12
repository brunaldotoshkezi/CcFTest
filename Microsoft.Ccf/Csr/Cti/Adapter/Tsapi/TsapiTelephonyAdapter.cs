//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file consists of the details of implementation of Telephony Adapter
// 
//===============================================================================
using CtiLayerTsapiLib;
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Csr.Cti
{
	/// <summary>
	/// Summary Description for TsapiTelephonyAdapter.
	/// </summary>
	public class TsapiTelephonyAdapter : TelephonyProvider
	{
		internal CtiLayerTsapiClass  CtiTsapi;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public TsapiTelephonyAdapter(string initializer): base(initializer)
		{
			CtiTsapi = new CtiLayerTsapiClass();
		}

		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TsapiTelephonyAdapter()
		{}

		/// <summary>
		/// Destructor
		/// </summary>
		~TsapiTelephonyAdapter()
		{
			CtiTsapi = null;
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
			base.Init(type, user, password, agentID, agentPassword, agentNumber);

			string serverName = CtiTsapi.GetServerNames();
			string [] serverNames = serverName.Split(new char[] { '\n' });
			if (CtiTsapi.Init(serverNames[0],user,password) != 0 )
			{
				Lines = LinesClassProvider.Instance();
				Lines.Init( this, type );
				CtiTsapi.CallChanged += new CtiLayerTsapiLib._DCtiLayerTsapiEvents_CallChangedEventHandler( this.Cti_CallChanged );

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
