//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// A base class for the multichannel listener classes.
//
//===============================================================================
using System;
using System.Data;
using System.Data.SqlClient;

namespace Microsoft.Ccf.Adapter.Multichannel
{
	/// <summary>
	/// Summary description for Multichannel.
	/// </summary>
	public class MultichannelBase
	{
		/// <summary>
		/// A public structure of agent and client data
		/// </summary>
		protected struct AgentClientData
		{
			public int AgentID;
			public Guid ClientID;
			public string IPAddress;
			public string Port;
		}

		/// <summary>
		/// Retrieve client (agent desktop) information for the given agent.
		/// </summary>
		/// <param name="agentID">The agent id whose client information is needed.</param>
		/// <returns>AgentClientData structure</returns>
		protected static AgentClientData RetrieveAgentClient(int agentID)
		{
			//ArrayList IPAddressPorts = new ArrayList();
			AgentClientData agent = new AgentClientData();

			string connString = System.Configuration.ConfigurationManager.AppSettings["DBConnectionStringAif"];
			SqlConnection conn = new SqlConnection(connString);

			SqlCommand sqlCommand = new SqlCommand("GetRegistrationByID", conn);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParam = new SqlParameter("@agentid",SqlDbType.Int);
			sqlParam.Value = agentID;

			sqlCommand.Parameters.Add(sqlParam);
			conn.Open();

			using (SqlDataReader dr = sqlCommand.ExecuteReader(System.Data.CommandBehavior.Default))
			{
				try
				{
					while (dr.Read())
					{
						//IPAddressPorts.Add( new IpAddressPort( dr.GetString(0), dr.GetString(1) ) );
						agent.AgentID = dr.GetInt32(0);
						agent.ClientID = dr.GetGuid(1);
						agent.IPAddress = dr.GetString(2);
						agent.Port = dr.GetString(3);
					}
				}
				catch (Exception e)
				{
					Console.Write(e.Message);
				}
			}
			return agent;
		}
	}
}
