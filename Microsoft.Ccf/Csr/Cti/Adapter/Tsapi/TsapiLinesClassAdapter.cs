//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
// 
// This file details the implementation of Lines Class Adapter.
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
	public class TsapiLinesClassAdapter : LinesClassProvider
	{

		/// <summary>
		/// TsapiCti
		/// </summary>
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
		public TsapiLinesClassAdapter() {}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="t">t</param>
		public TsapiLinesClassAdapter(string t) : base(t)
		{}

		/// <summary>
		/// The init of the Line class.
		/// </summary>
		/// <param name="cti">The actualized Telephony object</param>
		/// <param name="type">The Telephony type </param>
		public override void Init(TelephonyProvider cti, Microsoft.Ccf.Csr.Cti.Providers.TelephonyProvider.CtiType type)
		{
			base.Init (cti, type);
			LineClassProvider line = null;
			DevCount = this.tsapiCti.CtiTsapi.DeviceCount();
			for ( int i = 0; i < DevCount; i++ )
			{
				//line = new LineClass( cti, cti.CtiTapi.GetDeviceID( i ), Telephony.CtiType.TAPI );
				line = LineClassProvider.Instance();
				line.Init(this.tsapiCti, this.tsapiCti.CtiTsapi.GetDeviceID( i ), TelephonyProvider.CtiType.TSAPI);
				Lines.Add( line );
			}
		}
	}
}
