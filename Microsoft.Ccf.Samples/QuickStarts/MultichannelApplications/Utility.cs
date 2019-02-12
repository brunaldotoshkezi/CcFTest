//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2007 Microsoft Corp.
//
//===================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Ccf.Multichannel.AdapterManager.Common;
using System.Xml;
using System.Security.Cryptography;

namespace Microsoft.Ccf.Samples.MultichannelApplications
{
	class Utility
	{
		/// <summary>
		/// Creates a MultichannelMessageInformation object from the passed paramteres
		/// </summary>
		/// <param name="appID">application id of current application</param>
		/// <param name="agentGuid">agent guid</param>
		/// <param name="message">message to be sent</param>
		/// <param name="action">action</param>
		/// <param name="messageType">message type</param>
		/// <returns>MMI object</returns>
		public static MultichannelMessageInformation CreateRequest(string appID, string clientID,  string message, string action, MultiChannelMessageType messageType)
		{
			MultichannelMessageInformation request = new MultichannelMessageInformation();
			request.AgentApplicationID = appID;
			request.AgentGuid = clientID;
			request.Message = message;
			request.MessageType = messageType;
			request.Action = action;
			request.Status = true;
			return request;
		}

		/// <summary>
		/// Gives a MD5 hash of the given string
		/// </summary>
		/// <param name="input">input string</param>
		/// <returns>MD5 hash of input string</returns>
		public static string CalculateMD5Hash(string input)
		{
			// calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}

	}
}
