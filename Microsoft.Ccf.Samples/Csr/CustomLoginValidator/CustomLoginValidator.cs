//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// A sample implementation for the third party authentication service in context of security
// extension
//
//===================================================================================
using Microsoft.Ccf.Csr.ICustomLoginValidator;
using System.Collections;


/// <summary>
/// Sample implementation of third party authentication service.
/// </summary>
namespace Microsoft.Ccf.Samples.Csr.CustomLoginValidator
{
	public class CustomLoginValidator : ICustomLoginValidator
	{

		/// <summary>
		/// This method has to be implemented as per the third party authentication required, for eg.
		/// siteminder, reading from database, no trust remote AD etc
		/// </summary>
		public bool ValidateLoginDetails(string username, string password, string domain, Hashtable customParameters)
		{
			return true; // dummy implemention. Always acknowledges the login details
		}
	}
}
