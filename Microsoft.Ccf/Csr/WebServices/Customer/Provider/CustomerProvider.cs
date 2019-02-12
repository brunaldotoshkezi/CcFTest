//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// This file contains the the abstract provider definition for the ContactCenter 
// AddressBook web service.
//
//===============================================================================

using System.Collections.Specialized;
using Microsoft.Ccf.Common.Providers;
using System.Runtime.Serialization;

namespace Microsoft.Ccf.Csr.WebServices.Customer.Provider
{
	/// <summary>
	/// This class contains the the abstract provider definition for the ContactCenter 
	/// AddressBook web service.
	/// </summary>
	public abstract class CustomerProvider : ProviderBase
	{
		#region Constant definitions
		private const string _sectionName = "Microsoft.Ccf/Csr.WebServices.Customer.Provider";
		#endregion

		internal string connStr;
		internal string connStrCSS;
		internal string name;

		/// <summary>
		/// Creates an instance of the provider based on the configuration data.
		/// </summary>
		/// <returns></returns>
		public static CustomerProvider Instance()
		{
			ProviderConstructorInfo info = ProviderBase.GetConstructorInfo(_sectionName, 1);

			// Get the constructor parameter and create an instance of the provider.
			object[] paramArray = new object[1];
			paramArray[0] = info.Provider.Attributes["connectionString"];
			CustomerProvider instance = (CustomerProvider)(info.ConstructorInfo.Invoke(paramArray));

			instance.Initialize(info.Provider.Name, info.Provider.Attributes);
			return instance;
		}


		public CustomerProvider(string connectionString)
		{
			connStr = connectionString;
		}

		#region Provider specific behaviors

		// New implementation for connection string
		internal NameValueCollection configvalue;

		public override void Initialize(string name, NameValueCollection configValue)
		{
			this.name = name;
			this.configvalue = configValue;
			connStr = ConfigValue["connectionString"].ToString();
			connStrCSS = ConfigValue["connectionStringCSS"].ToString();
		}

		public NameValueCollection ConfigValue
		{
			get
			{
				return this.configvalue;
			}
		}

		public override string ProviderName
		{
			get
			{
				return name;
			}
		}
		#endregion Provider specific behaviors

		#region web service specific interface

		/// <summary>
		///  Get a customer record based on the customer ID.
		/// </summary>
		public abstract CustomerRecord GetCustomerByID(string customerID);

		/// <summary>
		///  Get a customer record based on the customer Guid.
		/// </summary>
		public abstract CustomerRecord GetCustomerByGuid(string customerGuid);

		/// <summary>
		///  Get a customer record based on the customer's email address.
		/// </summary>
		public abstract CustomerRecord GetCustomerByEmail(string customerEmail);

		/// <summary>
		/// Get a set of customer records that match the given customer name.  Only
		/// 'maxRecords' number of customers are returned.
		/// </summary>
		public abstract CustomerRecord[] GetCustomersByName(string first, string last, int maxRecords);

		/// <summary>
		/// Get a set of customer records that this particular email (identify by an ID) belongs to.  
		/// Only 'maxRecords' number of customers are returned.
		/// </summary>
		public abstract CustomerRecord[] GetCustomersByEmailID(string emailID, int maxRecords);

		/// <summary>
		/// Return a set of customers who have a phone number matching the
		/// given 'ANI' (Automatic Number ID).  The max number of returned records
		/// is set by maxRecords.
		/// </summary>
		public abstract CustomerRecord[] GetCustomersByANI(string ani, int maxRecords);

		/// <summary>
		/// Sets the customer 
		/// </summary>
		public abstract void SetCustomer(CustomerRecord custRec);

		/// <summary>
		/// Retrieves customer information based on search criteria
		/// </summary>
		/// <param name="xmlRequest">Input parameters in xml string format</param>
		/// <returns>response string</returns>
		public abstract string GetCustomerInfo(string xmlRequest);

		#endregion web service specific interface

		#region implementation helpers

		/// <summary>
		/// Sample customer record definition.
		/// </summary>
		[DataContract]
		public class CustomerRecord
		{
			/// <summary>
			/// The customer's ID
			/// </summary>
			[DataMember]
			public string CustomerID;
			/// <summary>
			/// The customer's first name
			/// </summary>
			[DataMember]
			public string FirstName;
			/// <summary>
			/// The customer's last name
			/// </summary>
			[DataMember]
			public string LastName;
			/// <summary>
			/// The street address part of the customer's address
			/// </summary>
			[DataMember]
			public string Street;
			/// <summary>
			/// The city part of the customer's address
			/// </summary>
			[DataMember]
			public string City;
			/// <summary>
			/// The state part of the customer's address
			/// </summary>
			[DataMember]
			public string State;
			/// <summary>
			/// The zipcode part of the customer's address
			/// </summary>
			[DataMember]
			public string ZipCode;
			/// <summary>
			/// The country part of the customer's address
			/// </summary>
			[DataMember]
			public string Country;
			/// <summary>
			/// The customer's home phone number
			/// </summary>
			[DataMember]
			public string PhoneHome;
			/// <summary>
			/// The customer's work phone number
			/// </summary>
			[DataMember]
			public string PhoneWork;
			/// <summary>
			/// The customer's mobile phone number
			/// </summary>
			[DataMember]
			public string PhoneMobile;
		}
		#endregion
	}
}
