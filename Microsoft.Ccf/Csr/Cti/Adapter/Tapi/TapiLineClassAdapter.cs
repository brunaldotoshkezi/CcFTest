//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file contains the methods and propeties concerned to Tapi implementation 
// 
//===============================================================================
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Csr.Cti
{
	/// <summary>
	/// Represents a single line in a phone system.
	/// </summary>
	public class TapiLineClassAdapter : LineClassProvider
	{
		private TapiTelephonyAdapter tapiCti;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public TapiLineClassAdapter(string initializer) : base(initializer)
		{}

		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TapiLineClassAdapter() {}

		/// <summary>
		/// The init of the Line class.
		/// </summary>
		/// <param name="cti">The actualized Telephony object</param>
		/// <param name="devID">The Dev ID</param>
		/// <param name="type">The Telephony type </param>
		public override void Init(TelephonyProvider cti, int devID, Microsoft.Ccf.Csr.Cti.Providers.TelephonyProvider.CtiType type)
		{
			base.Init (cti, devID, type);
			this.tapiCti = (TapiTelephonyAdapter)cti;
			name = this.tapiCti.CtiTapi.GetDeviceName( devID );
			providerInfo = this.tapiCti.CtiTapi.GetDeviceProviderInfo( devID );
			switchInfo = this.tapiCti.CtiTapi.GetDeviceSwitchInfo( devID );
			features = this.tapiCti.CtiTapi.GetDeviceFeatures( devID );
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
				res = this.tapiCti.CtiTapi.Open( DevID );
				if ( res >= 0 )
				{
					Calls = CallsClassProvider.Instance();
					//new CallsClass( cti, this );
					Calls.Init( this.tapiCti, this);
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
				return this.tapiCti.CtiTapi.Close( DevID );
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
			int callID = this.tapiCti.CtiTapi.MakeCall( DevID, called, forceTollCall );

			// Give event a chance to run on other thread so call object is created
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
			// Is ringing updated here???????
			return this.tapiCti.CtiTapi.Pickup( DevID, ringing );
		}
	}
}
