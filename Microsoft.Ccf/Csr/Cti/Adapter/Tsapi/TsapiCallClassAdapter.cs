//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file contains the methods and propeties concerned to Tsapi implementation 
// 
//===============================================================================
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Csr.Cti
{
	/// <summary>
	/// The call class for TSAPI
	/// </summary>
	public class TsapiCallClassAdapter : CallClassProvider
	{
		private TsapiTelephonyAdapter tsapiCti
		{
			get
			{
				return (TsapiTelephonyAdapter)this.Cti;
			}
		}

		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TsapiCallClassAdapter() {}

		/// <summary>
		/// Provider constructor.
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public TsapiCallClassAdapter(string initializer): base(initializer)
		{}

		/// <summary>
		/// The init of the Call class.
		/// </summary>
		/// <param name="cti">The actualized Telephony object</param>
		/// <param name="line">The actualized line object</param>
		/// <param name="callID">The ID of the call.</param>
		public override void Init(TelephonyProvider cti, LineClassProvider line, int callID)
		{
			base.Init (cti, line, callID);
		}

		/// <summary>
		/// A list of the parties.
		/// </summary>
		public override string Parties
		{
			get
			{
				string parties = this.tsapiCti.CtiTsapi.GetCallParties(Line.DevID,this.CallID );
				return parties.Trim();
			}
		}

		/// <summary>
		/// The caller's number.
		/// </summary>
		public override string CallerNumber
		{
			get
			{
				return this.Parties;
			}
		}

		/// <summary>
		/// The number called.
		/// </summary>
		public override string CalledNumber
		{
			get
			{
				string called = this.tsapiCti.CtiTsapi.GetCalledNumber( Line.DevID,this.CallID );
				return called.Trim();

		
			}
		}

		/// <summary>
		/// The state of the call.
		/// </summary>
		public override CallState State
		{
			get
			{
				CallState demoState = (CallState)this.tsapiCti.CtiTsapi.GetCallState( Line.DevID,this.CallID );
				return demoState;
				//return CallState.None ;
			}
		}

		/// <summary>
		/// Return the CallFeatures enumeration.
		/// </summary>
		public override CallFeatures Features
		{
			get
			{
				return (CallFeatures)this.tsapiCti.CtiTsapi.GetCallFeatures( Line.DevID,this.CallID );
			}
		}

		/// <summary>
		/// Answer the call.
		/// </summary>
		/// <returns>Integer indicating success or failure.</returns>
		public override int Answer()
		{
			return this.tsapiCti.CtiTsapi.Answer(Line.DevID,this.CallID );
		}

		/// <summary>
		/// Hold the call.
		/// </summary>
		/// <returns>Integer indicating success or failure.</returns>
		public override int Hold()
		{
			return this.tsapiCti.CtiTsapi.Hold( Line.DevID,this.CallID );
		}

		/// <summary>
		/// Un-hold the call.
		/// </summary>
		/// <returns>Integer indicating success or failure.</returns>
		public override int Unhold()
		{
			return this.tsapiCti.CtiTsapi.Unhold( Line.DevID,this.CallID );
		}

		/// <summary>
		/// Hangup the call.
		/// </summary>
		/// <returns>Integer indicating success or failure.</returns>
		public override int Hangup()
		{
			return this.tsapiCti.CtiTsapi.Hangup( Line.DevID,this.CallID );
		}

		/// <summary>
		/// Transfer the call to the number given.
		/// </summary>
		/// <param name="numberToTransferTo">Number to Transfer</param>
		/// <returns>Integer indicating success or failure.</returns>
		public override int Transfer(string numberToTransferTo)
		{
			base.Transfer (numberToTransferTo);
			return this.tsapiCti.CtiTsapi.BlindTransfer( Line.DevID,this.CallID, numberToTransferTo, 0 );
		}
	}
}
