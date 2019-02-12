//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// This file contains the the Sqlprovider definition for the Customer web service.
//
//===================================================================================

#region Usings
using System;
using System.IO;
using System.Data;
using System.Text;
using System.Xml;
using System.Collections;
using Microsoft.Ccf.Common;
using System.Data.SqlClient;
using Microsoft.Ccf.Common.Logging;
#endregion

namespace Microsoft.Ccf.Csr.WebServices.Customer.Provider
{
	/// <summary>
	/// This class contains the the SQL Provider definition for the Customer web service.
	/// </summary>
	public class SqlCustomerProvider : CustomerProvider
	{

		#region Declarations
		private StringBuilder xmlResponse = new StringBuilder();

		// Xml related fields.
		private const string ERR_SUCCESS = "0";
		private StringBuilder responseString = null;
		private StringWriter xmlStream = null;
		private XmlTextWriter responseWriter = null;
		#endregion

		/// <summary>
		/// Private constructor. Can be invoked through reflection.
		/// </summary>
		internal SqlCustomerProvider(string connectionString) : base(connectionString)
		{
		}

		#region WebService specific behaviour

		private const string ERROR_MUST_BE_POSITIVE = "The value for this parameter must be greater than zero.";
		private const string ERROR_CONNOT_BE_EMPTY = "The value for this parameter cannot be an empty string.";

		/// <summary>
		/// Get a customer record based on the customer ID.
		/// </summary>
		/// <param name="customerID"></param>
		/// <returns></returns>
		public override CustomerRecord GetCustomerByID(string customerID)
		{
			if (customerID == null)
			{
				throw new ArgumentNullException("customerID");
			}
			if (customerID == String.Empty)
			{
				throw new ArgumentOutOfRangeException("customerID", ERROR_CONNOT_BE_EMPTY);
			}
			// block wildcard searches
			if (customerID.IndexOf('%') >= 0)
			{
				customerID = customerID.Replace('%', ' ');
			}

			SqlConnection dbConnection = new SqlConnection(connStr);
			SqlCommand dbSqlCommand = new SqlCommand("GetCustomerByID", dbConnection);
			dbSqlCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter dbSqlParameter1 = new SqlParameter("@customerID", SqlDbType.NVarChar);
			dbSqlParameter1.Direction = ParameterDirection.Input;
			dbSqlParameter1.Value = customerID;
			dbSqlCommand.Parameters.Add(dbSqlParameter1);

			try
			{
				dbConnection.Open();
				using (SqlDataReader dataReader = dbSqlCommand.ExecuteReader())
				{
					if (dataReader.Read()) // There will be either 1 or 0 results; never more than 1
					{
						CustomerRecord custRec = new CustomerRecord();
						custRec.CustomerID = dataReader.GetString(0);
						custRec.FirstName = dataReader.GetString(1);
						custRec.LastName = dataReader.GetString(2);
						custRec.Street = (!dataReader.IsDBNull(3) ? dataReader.GetString(3) : null);
						custRec.City = (!dataReader.IsDBNull(4) ? dataReader.GetString(4) : null);
						custRec.State = (!dataReader.IsDBNull(5) ? dataReader.GetString(5) : null);
						custRec.ZipCode = (!dataReader.IsDBNull(6) ? dataReader.GetString(6) : null);
						custRec.Country = (!dataReader.IsDBNull(7) ? dataReader.GetString(7) : null);
						custRec.PhoneHome = (!dataReader.IsDBNull(8) ? dataReader.GetString(8) : null);
						custRec.PhoneWork = (!dataReader.IsDBNull(9) ? dataReader.GetString(9) : null);
						custRec.PhoneMobile = (!dataReader.IsDBNull(10) ? dataReader.GetString(10) : null);
						return custRec;
					}

					return null;
				}
			}
			catch (Exception exp)
			{
				Logging.Error(ToString(), exp.Message, exp);
				throw;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		/// <summary>
		/// (No-op) Get a customer record based on the customer Guid.
		/// </summary>
		/// <param name="customerGuid"></param>
		/// <returns></returns>
		public override CustomerRecord GetCustomerByGuid(string customerGuid)
		{
			return null;
		}

		/// <summary>
		/// (No-op) Get a customer record based on the customer's email address.
		/// </summary>
		/// <param name="customerEmail"></param>
		/// <returns></returns>
		public override CustomerRecord GetCustomerByEmail(string customerEmail)
		{
			return null;
		}

		/// <summary>
		/// (No-op) Get a set of customer records that this particular email (identify by an ID) belongs to.  Only
		/// 'maxRecords' number of customers are returned.
		/// </summary>
		/// <param name="emailID"></param>
		/// <param name="maxRecords"></param>
		/// <returns></returns>
		public override CustomerRecord[] GetCustomersByEmailID(string emailID, int maxRecords)
		{
			return null;
		}

		/// <summary>
		/// Get a set of customer records that match the given customer name.  Only
		/// 'maxRecords' number of customers are returned.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <param name="maxRecords"></param>
		/// <returns></returns>
		public override CustomerRecord[] GetCustomersByName(string first, string last, int maxRecords)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (last == null)
			{
				throw new ArgumentNullException("last");
			}
			if (maxRecords <= 0)
			{
				throw new ArgumentOutOfRangeException("maxRecords", maxRecords, ERROR_MUST_BE_POSITIVE);
			}
			if (last.Length.Equals(0) && first.Length.Equals(0))
			{
				throw new ArgumentOutOfRangeException("lastname and firstname", ERROR_CONNOT_BE_EMPTY);
			}

			// Block wildcard searches
			if (last.IndexOf('%') >= 0)
			{
				last = last.Replace('%', ' ');
			}
			if (first.IndexOf('%') >= 0)
			{
				first = first.Replace('%', ' ');
			}

			// Limit how many records can be requested
			maxRecords = (maxRecords > 20) ? 20 : maxRecords;

			SqlConnection dbConnection = new SqlConnection(connStr);
			SqlCommand dbSqlCommand = new SqlCommand("GetCustomersByName", dbConnection);
			dbSqlCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter dbSqlParameter1 = new SqlParameter("@firstName", SqlDbType.NVarChar);
			dbSqlParameter1.Direction = ParameterDirection.Input;
			dbSqlParameter1.Value = first;
			dbSqlCommand.Parameters.Add(dbSqlParameter1);

			SqlParameter dbSqlParameter2 = new SqlParameter("@lastName", SqlDbType.NVarChar);
			dbSqlParameter2.Direction = ParameterDirection.Input;
			dbSqlParameter2.Value = last;
			dbSqlCommand.Parameters.Add(dbSqlParameter2);

			try
			{
				dbConnection.Open();
				using (SqlDataReader dataReader = dbSqlCommand.ExecuteReader())
				{
					int i = 0;
					ArrayList arrList = new ArrayList();

					while (dataReader.Read() && i < maxRecords)
					{
						CustomerRecord custRec = new CustomerRecord();
						custRec.CustomerID = dataReader.GetString(0);
						custRec.FirstName = dataReader.GetString(1);
						custRec.LastName = dataReader.GetString(2);
						custRec.Street = dataReader.GetString(3);
						custRec.City = dataReader.GetString(4);
						custRec.State = dataReader.GetString(5);
						custRec.ZipCode = dataReader.GetString(6);
						custRec.Country = dataReader.GetString(7);
						custRec.PhoneHome = dataReader.GetString(8);
						custRec.PhoneWork = dataReader.GetString(9);
						custRec.PhoneMobile = dataReader.GetString(10);
						arrList.Add(custRec);
						i++;
					}

					if (i > 0)
					{
						return (CustomerRecord[])arrList.ToArray(typeof(CustomerRecord));
					}
					return null;
				}
			}
			catch (Exception exp)
			{
				Logging.Error(ToString(), exp.Message, exp);
				throw;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		/// <summary>
		/// Return a set of customers who have a phone number matching the
		/// given 'ANI' (Automatic Number ID).  The max number of returned records
		/// is set by maxRecords.
		/// </summary>
		/// <param name="ani"></param>
		/// <param name="maxRecords"></param>
		/// <returns></returns>
		public override CustomerRecord[] GetCustomersByANI(string ani, int maxRecords)
		{
			if (ani == null)
			{
				throw new ArgumentNullException("phone number (ANI)");
			}
			if (ani == String.Empty)
			{
				throw new ArgumentOutOfRangeException("phone number (ANI)", ERROR_CONNOT_BE_EMPTY);
			}
			if (maxRecords <= 0)
			{
				throw new ArgumentOutOfRangeException("maxRecords", maxRecords, ERROR_MUST_BE_POSITIVE);
			}
			// Block wildcard searches
			if (ani.IndexOf('%') >= 0)
			{
				ani = ani.Replace('%', ' ');
			}

			SqlConnection dbConnection = new SqlConnection(connStr);
			SqlCommand dbSqlCommand = new SqlCommand("GetCustomersByANI", dbConnection);
			dbSqlCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter dbSqlParameter1 = new SqlParameter("@customerANI", SqlDbType.NVarChar);
			dbSqlParameter1.Direction = ParameterDirection.Input;
			dbSqlParameter1.Value = ani;
			dbSqlCommand.Parameters.Add(dbSqlParameter1);

			try
			{
				dbConnection.Open();
				using (SqlDataReader dataReader = dbSqlCommand.ExecuteReader())
				{
					int i = 0;
					ArrayList arrList = new ArrayList();

					while (dataReader.Read() && i < maxRecords)
					{
						CustomerRecord custRec = new CustomerRecord();
						custRec.CustomerID = dataReader.GetString(0);
						custRec.FirstName = dataReader.GetString(1);
						custRec.LastName = dataReader.GetString(2);
						custRec.Street = (!dataReader.IsDBNull(3) ? dataReader.GetString(3) : null);
						custRec.City = (!dataReader.IsDBNull(4) ? dataReader.GetString(4) : null);
						custRec.State = (!dataReader.IsDBNull(5) ? dataReader.GetString(5) : null);
						custRec.ZipCode = (!dataReader.IsDBNull(6) ? dataReader.GetString(6) : null);
						custRec.Country = (!dataReader.IsDBNull(7) ? dataReader.GetString(7) : null);
						custRec.PhoneHome = (!dataReader.IsDBNull(8) ? dataReader.GetString(8) : null);
						custRec.PhoneWork = (!dataReader.IsDBNull(9) ? dataReader.GetString(9) : null);
						custRec.PhoneMobile = (!dataReader.IsDBNull(10) ? dataReader.GetString(10) : null);
						arrList.Add(custRec);
						i++;
					}

					if (i > 0)
					{
						return (CustomerRecord[])arrList.ToArray(typeof(CustomerRecord));
					}
					return null;
				}
			}
			catch (Exception exp)
			{
				Logging.Error(ToString(), exp.Message, exp);
				throw;
			}
			finally
			{
				dbConnection.Close();
			}
		}
		/// <summary>
		/// Sets the customer information
		/// </summary>
		/// <param name="custRec"></param>
		public override void SetCustomer(CustomerRecord custRec)
		{
			if (custRec == null)
				throw new ArgumentNullException("custRec");
			SqlConnection dbConnection = new SqlConnection(connStr);
			SqlCommand dbSqlCommand = new SqlCommand("SetCustomer", dbConnection);
			dbSqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter dbSqlParameter1 = new SqlParameter("@CustomerID", SqlDbType.NVarChar);
			dbSqlParameter1.Direction = ParameterDirection.Input;
			dbSqlParameter1.Value = custRec.CustomerID;
			dbSqlCommand.Parameters.Add(dbSqlParameter1);
			SqlParameter dbSqlParameter2 = new SqlParameter("@FirstName", SqlDbType.NVarChar);
			dbSqlParameter2.Direction = ParameterDirection.Input;
			dbSqlParameter2.Value = custRec.FirstName;
			dbSqlCommand.Parameters.Add(dbSqlParameter2);
			SqlParameter dbSqlParameter3 = new SqlParameter("@LastName", SqlDbType.NVarChar);
			dbSqlParameter3.Direction = ParameterDirection.Input;
			dbSqlParameter3.Value = custRec.LastName;
			dbSqlCommand.Parameters.Add(dbSqlParameter3);
			SqlParameter dbSqlParameter4 = new SqlParameter("@Street", SqlDbType.NVarChar);
			dbSqlParameter4.Direction = ParameterDirection.Input;
			dbSqlParameter4.Value = custRec.Street;
			dbSqlCommand.Parameters.Add(dbSqlParameter4);
			SqlParameter dbSqlParameter5 = new SqlParameter("@City", SqlDbType.NVarChar);
			dbSqlParameter5.Direction = ParameterDirection.Input;
			dbSqlParameter5.Value = custRec.City;
			dbSqlCommand.Parameters.Add(dbSqlParameter5);
			SqlParameter dbSqlParameter6 = new SqlParameter("@State", SqlDbType.NVarChar);
			dbSqlParameter6.Direction = ParameterDirection.Input;
			dbSqlParameter6.Value = custRec.State;
			dbSqlCommand.Parameters.Add(dbSqlParameter6);
			SqlParameter dbSqlParameter7 = new SqlParameter("@ZipCode", SqlDbType.NVarChar);
			dbSqlParameter7.Direction = ParameterDirection.Input;
			dbSqlParameter7.Value = custRec.ZipCode;
			dbSqlCommand.Parameters.Add(dbSqlParameter7);
			SqlParameter dbSqlParameter8 = new SqlParameter("@Country", SqlDbType.NVarChar);
			dbSqlParameter8.Direction = ParameterDirection.Input;
			dbSqlParameter8.Value = custRec.Country;
			dbSqlCommand.Parameters.Add(dbSqlParameter8);
			SqlParameter dbSqlParameter9 = new SqlParameter("@PhoneHome", SqlDbType.NVarChar);
			dbSqlParameter9.Direction = ParameterDirection.Input;
			dbSqlParameter9.Value = custRec.PhoneHome;
			dbSqlCommand.Parameters.Add(dbSqlParameter9);
			SqlParameter dbSqlParameter10 = new SqlParameter("@PhoneWork", SqlDbType.NVarChar);
			dbSqlParameter10.Direction = ParameterDirection.Input;
			dbSqlParameter10.Value = custRec.PhoneWork;
			dbSqlCommand.Parameters.Add(dbSqlParameter10);
			SqlParameter dbSqlParameter11 = new SqlParameter("@PhoneMobile", SqlDbType.NVarChar);
			dbSqlParameter11.Direction = ParameterDirection.Input;
			dbSqlParameter11.Value = custRec.PhoneMobile;
			dbSqlCommand.Parameters.Add(dbSqlParameter11);

			dbConnection.Open();
			dbSqlCommand.ExecuteNonQuery();
			dbSqlCommand.Dispose();
			dbConnection.Close();
		}

		/// <summary>
		/// This webmethod retrieves customer information based on given search criteria
		/// </summary>
		/// <param name="xmlRequest">Input parameters in xml string format</param>
		/// <returns>response string</returns>
		public override string GetCustomerInfo(string xmlRequest)
		{
			// Initialize the DB connection.
			SqlConnection dbConnection = new SqlConnection(connStrCSS);
			SqlCommand dbCommand = new SqlCommand("ReadMinimalCustomerInfo", dbConnection);
			dbCommand.CommandType = CommandType.StoredProcedure;
			// Initialize objects.
			responseString = new StringBuilder();
			xmlStream = new StringWriter(responseString);
			responseWriter = new XmlTextWriter(xmlStream);

			// Initialize the Xml output string.
			responseWriter.Formatting = Formatting.Indented;
			responseWriter.WriteStartElement("Response");

			try
			{
				//Input parameter validation
				if (xmlRequest.Length == 0)
				{
					return xmlResponse.ToString();
				}
				XmlDocument dom = new XmlDocument();
				dom.LoadXml(xmlRequest);
				dbConnection.Open();

				string customerId = dom.DocumentElement.SelectSingleNode("CustomerId").InnerText;
				string firstName = dom.DocumentElement.SelectSingleNode("FirstName").InnerText;
				string lastName = dom.DocumentElement.SelectSingleNode("LastName").InnerText;
				string phoneNum = dom.DocumentElement.SelectSingleNode("PhoneNum").InnerText;
				string accountNum = dom.DocumentElement.SelectSingleNode("AccountNum").InnerText;

				SqlParameter param = new SqlParameter();
				param.Direction = ParameterDirection.Input;
				param.ParameterName = "@CustomerId";
				param.Value = customerId;
				dbCommand.Parameters.Add(param);

				param = new SqlParameter();
				param.Direction = ParameterDirection.Input;
				param.SqlDbType = SqlDbType.NVarChar;
				param.ParameterName = "@FirstName";
				param.Value = firstName;
				dbCommand.Parameters.Add(param);

				param = new SqlParameter();
				param.Direction = ParameterDirection.Input;
				param.SqlDbType = SqlDbType.NVarChar;
				param.ParameterName = "@LastName";
				param.Value = lastName;
				dbCommand.Parameters.Add(param);

				param = new SqlParameter();
				param.Direction = ParameterDirection.Input;
				param.SqlDbType = SqlDbType.NVarChar;
				param.ParameterName = "@PhoneNum";
				param.Value = phoneNum;
				dbCommand.Parameters.Add(param);

				param = new SqlParameter();
				param.Direction = ParameterDirection.Input;
				param.SqlDbType = SqlDbType.NVarChar;
				param.ParameterName = "@AccountNum";
				param.Value = accountNum;
				dbCommand.Parameters.Add(param);

				// Use a SQL data reader to process the result set..
				SqlDataReader reader = dbCommand.ExecuteReader();

				// Parse the resulting data set.
				responseWriter.WriteElementString("Status", ERR_SUCCESS);

				string custId = "";
				string tempCustomerId = "";

				while (reader.Read())
				{
					custId = GeneralFunctions.GetValueFromReader(reader, 0);
					if (custId != tempCustomerId)
					{
						if (tempCustomerId.Length != 0)
						{
							// Close phone number tag
							responseWriter.WriteEndElement();
							// Close customer tag
							responseWriter.WriteEndElement();
						}
						responseWriter.WriteStartElement("Customer");

						responseWriter.WriteElementString("Firstname", GeneralFunctions.GetValueFromReader(reader, 1));
						responseWriter.WriteElementString("Middlename", GeneralFunctions.GetValueFromReader(reader, 2));
						responseWriter.WriteElementString("Lastname", GeneralFunctions.GetValueFromReader(reader, 3));
						responseWriter.WriteElementString("DateofBirth", GeneralFunctions.GetValueFromReader(reader, 4));
						responseWriter.WriteElementString("StreetAddress1", GeneralFunctions.GetValueFromReader(reader, 5));
						responseWriter.WriteElementString("StreetAddress2", GeneralFunctions.GetValueFromReader(reader, 6));
						responseWriter.WriteElementString("City", GeneralFunctions.GetValueFromReader(reader, 7));
						responseWriter.WriteElementString("State", GeneralFunctions.GetValueFromReader(reader, 8));
						responseWriter.WriteElementString("ZipCode", GeneralFunctions.GetValueFromReader(reader, 9));
						responseWriter.WriteElementString("Email", GeneralFunctions.GetValueFromReader(reader, 10));
						responseWriter.WriteElementString("MothersMaidenname", GeneralFunctions.GetValueFromReader(reader, 11));
						responseWriter.WriteElementString("SSN", GeneralFunctions.GetValueFromReader(reader, 12));
						responseWriter.WriteElementString("CustomerId", custId);

						// Start phone number tag
						responseWriter.WriteStartElement("PhoneNumbers");

						responseWriter.WriteElementString("Number", GeneralFunctions.GetValueFromReader(reader, 12));
						tempCustomerId = custId;
					}
					else
					{
						responseWriter.WriteElementString("Number", GeneralFunctions.GetValueFromReader(reader, 12));
					}
				}
				if (custId.Length != 0)
				{
					// End phone number tag
					responseWriter.WriteEndElement();
					// End customer tag
					responseWriter.WriteEndElement();
				}

				responseWriter.WriteFullEndElement();
				return responseString.ToString();
				//-------------- End of definition ---------------------------//
			}
			catch (Exception eX)
			{
				Logging.Error("CSR.CustomerInformation.GetCustomerInfo", eX.Message);
				xmlResponse.Append(eX.Message);
				return xmlResponse.ToString();
			}
			finally
			{
				dbConnection.Close();
			}

		}
		#endregion
	}
}
