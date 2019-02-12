//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file details the implementation of Line Class Adapter
// 
//===============================================================================
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Csr.Cti
{
	/// <summary>
	/// Represents a single line in a phone system.
	/// </summary>
	public class TsapiLineClassAdapter : LineClassProvider
	{
		private TsapiTelephonyAdapter tsapiCti;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public TsapiLineClassAdapter(string initializer) : base(initializer)
		{}

		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TsapiLineClassAdapter() {}

		/// <summary>
		/// The init of the Line class.
		/// </summary>
		/// <param name="cti">The actualized Telephony object</param>
		/// <param name="devID">The Dev ID</param>
		/// <param name="type">The Telephony type </param>
		public override void Init(TelephonyProvider cti, int devID, Microsoft.Ccf.Csr.Cti.Providers.TelephonyProvider.CtiType type)
		{
			base.Init (cti, devID, type);
			this.tsapiCti = (TsapiTelephonyAdapter)cti;
			name = this.tsapiCti.CtiTsapi.GetDeviceName( devID );
			providerInfo = this.tsapiCti.CtiTsapi.GetDeviceProviderInfo( devID );
			switchInfo = this.tsapiCti.CtiTsapi.GetDeviceSwitchInfo( devID );
			features = this.tsapiCti.CtiTsapi.GetDeviceFeatures( devID );
		}

		/// <summary>
		/// Open the line.
		/// </summary>
		/// <returns>Indicator showing if the open was successful, 0 otherwise.</returns>
		public override int Open()
		{
			int res = -1;

			if ( this.openReferenceCount == 0 )
			{
				res = this.tsapiCti.CtiTsapi.Open( DevID );
				if ( res >= 0 )
				{
					Calls = CallsClassProvider.Instance();
					//new CallsClass( cti, this );
					Calls.Init( this.tsapiCti, this);
				}
			}
			this.openReferenceCount++;
			return res;
		}

		/// <summary>
		/// Close the line.
		/// </summary>
		/// <returns>Indicator showing if the close was successful, 0 otherwise.</returns>
		public override int Close()
		{
			if ( this.openReferenceCount > 0 )
			{
				this.openReferenceCount--;
			}

			if ( openReferenceCount == 0 )
			{
				return this.tsapiCti.CtiTsapi.Close( DevID );
			}
			return 0;
		}

		/// <summary>
		/// Make the call to the phone number given.
		/// </summary>
		/// <param name="called">The phone number to call.</param>
		/// <param name="forceTollCall">An indicator telling the method if it should force a toll call.</param>
		/// <returns>The actualized call object, null otherwise.</returns>
		public override CallClassProvider MakeCall(string called, int forceTollCall)
		{
			base.MakeCall (called, forceTollCall);
			//CallClassProvider call = null;
			int callID = this.tsapiCti.CtiTsapi.MakeCall( DevID, called, forceTollCall );

			// give event a chance to run on other thread so call object is created
			System.Threading.Thread.Sleep( 200 );
			System.Windows.Forms.Application.DoEvents();

			foreach ( CallClassProvider call in this.Calls )
			{
				if ( call.CallID == callID )
				{
					return call;
				}
			}
			return null;
		}

		/// <summary>
		/// Answer a call on someone else's phone but take it on your phone.
		/// </summary>
		/// <param name="ringing">The phone to answer</param>
		/// <returns>
		/// Indicator showing if the pickup was successful, 0 otherwise.
		/// </returns>
		public override int Pickup(string ringing)
		{
			base.Pickup (ringing);
			// is ringing updated here???????
			return this.tsapiCti.CtiTsapi.Pickup( DevID, ringing );
		}
	}
}