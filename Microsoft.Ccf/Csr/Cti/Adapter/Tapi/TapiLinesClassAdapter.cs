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
	/// This class exists to allow users to enumerate over lines with the
	/// foreach operator or with for ( int i = 0;... ) syntax.  Either way,
	/// you're looking into an array of line devices.
	/// </summary>
	public class TapiLinesClassAdapter : LinesClassProvider
	{
		/// <summary>
		/// Tapi Cti
		/// </summary>
		private TapiTelephonyAdapter tapiCti
		{
			get
			{
				return (TapiTelephonyAdapter)this.Cti;
			}
		}

		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public TapiLinesClassAdapter() {}

		/// <summary>
		/// Provider constructor.
		/// </summary>
		/// <param name="initializer">Not used...</param>
		public TapiLinesClassAdapter(string initializer) : base(initializer) {}

		/// <summary>
		/// The init of the Line class.
		/// </summary>
		/// <param name="cti">The actualized Telephony object</param>
		/// <param name="type">The Telephony type </param>
		public override void Init(TelephonyProvider cti, Microsoft.Ccf.Csr.Cti.Providers.TelephonyProvider.CtiType type)
		{
			base.Init (cti, type);
			LineClassProvider line = null;
			DevCount = this.tapiCti.CtiTapi.DeviceCount();
			for ( int i = 0; i < DevCount; i++ )
			{
				//line = new LineClass( cti, cti.CtiTapi.GetDeviceID( i ), Telephony.CtiType.TAPI );
				line = LineClassProvider.Instance();
				line.Init(this.tapiCti, this.tapiCti.CtiTapi.GetDeviceID( i ), TelephonyProvider.CtiType.TAPI);
				Lines.Add( line );
			}
		}
	}
}
