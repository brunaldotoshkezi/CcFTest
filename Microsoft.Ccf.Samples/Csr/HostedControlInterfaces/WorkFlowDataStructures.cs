//===============================================================================
//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// This file contains the definitions of DataStructures for workflow.
// 
//===============================================================================
#pragma warning disable 1591

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.HostedControlInterfaces
{
	// WorkflowStepArray serves to allow XmlSerializer to serialize/deserialize the WorkflowStepArray
	// A class that implements ICollection must have certain public properties
	// see MSDN documentation for XmlSerializer for more information
	public class WorkflowStepArray : ICollection
	{
		public string CollectionName;
		private ArrayList WorkflowSteps = new ArrayList();

		/// <summary>
		/// Public Item indexed property to allow serialization/deserialization
		/// </summary>
		public WorkflowStep this[int i]
		{
			get { return (WorkflowStep)WorkflowSteps[i]; }
		}
		
		/// <summary>
		/// Public to allow serialization/deserialization
		/// </summary>
		public void CopyTo(Array a, int i)
		{
			WorkflowSteps.CopyTo(a, i);
		}

		/// <summary>
		/// Public to allow serialization/deserialization
		/// </summary>
		public int Count
		{
			get { return WorkflowSteps.Count; }
		}

		/// <summary>
		/// Public to allow serialization/deserialization
		/// </summary>
		public object SyncRoot
		{
			get { return this; }
		}

		/// <summary>
		/// Public to allow serialization/deserialization
		/// </summary>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Public to allow serialization/deserialization
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			return WorkflowSteps.GetEnumerator();
		}

		/// <summary>
		/// Public to allow serialization/deserialization
		/// </summary>
		public void Add(WorkflowStep wfs)
		{
			WorkflowSteps.Add(wfs);
		}
	}
	
	/// <summary>
	/// Structure to maintain the Agent and the session IDs
	/// </summary>
	public class AgentAndSessionData
	{
		// Session ID passed by the AgentDesktop
		private Guid sessionId;
		// Agent ID passed by the AgentDesktop
		private int  agentID;

		public AgentAndSessionData()
		{
			sessionId = Guid.Empty;
			agentID = -1;
		}

		/// <summary>
		/// Get and set the SessionId
		/// </summary>
		public Guid SessionId
		{
			get { return this.sessionId; }
			set { this.sessionId = value; }
		}

		/// <summary>
		/// Get and set the AgentId
		/// </summary>
		public int AgentId
		{
			get { return agentID; }
			set
			{
				if (value > 0)
				{
					agentID = value;
				}
			}
		}
	}

	/// <summary>
	/// structure to maintain a workflow step
	/// </summary>
	public class WorkflowStep
	{
		// Step id of a workflow step.
		private int nWorkflowStepId;
		// Step name to be displayed as part of the list view.
		private string strWorkflowStepName;
		// Description of the step
		private string strWorkflowStepDescription;
		// Action for the step
		private string strWorkflowStepAction;
		// The application ID of the hosted application corresponding to this step.
		private int nHostedApplicationId;
		// This indicates if this step is complete wrt to the current workflow, default is false.
		private bool bIsStepComplete;

		public WorkflowStep()
		{
			nWorkflowStepId = -1;
			strWorkflowStepName = "";
			strWorkflowStepDescription = "";
			strWorkflowStepAction = "";
			nHostedApplicationId = -1;
			bIsStepComplete = false;
		}

		/// <summary>
		/// Get and set the Workflowstepid
		/// </summary>
		public int Workflowstepid
		{
			get { return this.nWorkflowStepId; }
			set
			{
				if (value > 0)
				{
					this.nWorkflowStepId = value;
				}
				else
				{
					this.nWorkflowStepId = -1;
				}
			}
		}
		
		/// <summary>
		/// Get and set the Id of the HostedApplicationId that maps to the WorkflowStep
		/// </summary>
		public int HostedApplicationId
		{
			get { return this.nHostedApplicationId; }
			set
			{
				if (value > 0)
				{
					this.nHostedApplicationId = value;
				}
				else
				{
					this.nHostedApplicationId = -1;
				}
			}
		}

		/// <summary>
		/// Get and set whether a step is complete
		/// </summary>
		public bool IsStepComplete
		{
			get { return this.bIsStepComplete; }
			set { this.bIsStepComplete = value; }
		}
		/// <summary>
		/// Get and set the name of WorkflowStep
		/// </summary>
		public string WorkflowStepName
		{
			get { return this.strWorkflowStepName; }
			set
			{
				if (value != string.Empty)
				{
					this.strWorkflowStepName = value;
				}
				else
				{
					this.strWorkflowStepName = string.Empty;
				}
			}
		}
		
		/// <summary>
		/// Gets and sets the description of this workflow step
		/// </summary>
		public string WorkflowStepDescription
		{
			get { return this.strWorkflowStepDescription; }
			set
			{
				if ( null != value && 0 < value.Length )
				{
					this.strWorkflowStepDescription = value;
				}
				else
				{
					this.strWorkflowStepDescription = string.Empty;
				}
			}
		}

		/// <summary>
		/// Gets and sets the action for the workflow step
		/// </summary>
		public string WorkflowStepAction
		{
			get { return this.strWorkflowStepAction; }
			set
			{
				if( null != value && 0 < value.Length )
				{
					this.strWorkflowStepAction = value;
				}
				else
				{
					this.strWorkflowStepAction = string.Empty;
				}
			}
		}

		/// <summary>
		/// Deserialize workflowstep data
		/// </summary>
		/// <param name="strStepDataXML"></param>
		/// <returns></returns>
		public bool SetStepData(string strStepDataXML)
		{
			if (strStepDataXML == string.Empty)
			{
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(strStepDataXML);
				XmlNode root = doc.DocumentElement;

				//fill in the values.
				nWorkflowStepId = Convert.ToInt32(root.Attributes["ID"].InnerText,10);
				strWorkflowStepName = root.SelectSingleNode("Name").InnerText;
				nHostedApplicationId = Convert.ToInt32(root.SelectSingleNode("HostedAppID").InnerText,10);

				XmlNode descriptionNode = root.SelectSingleNode("Description");
				if ( null != descriptionNode )
				{
					strWorkflowStepDescription = descriptionNode.InnerText;
				}

				XmlNode actionNode = root.SelectSingleNode("WorkflowAction");
				if ( actionNode != null )
				{
					strWorkflowStepAction = actionNode.InnerText;
				}
			}
			catch ( Exception exp)
			{
				Logging.Error(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_STEP_ERR_SET_STEP_DATA,exp);
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Create a copy of workflow step
		/// </summary>
		/// <returns>Copy of workflow step</returns>
		public WorkflowStep ReturnACopy()
		{
			WorkflowStep CopiedStep = new WorkflowStep();

			CopiedStep.nWorkflowStepId = nWorkflowStepId;
			CopiedStep.strWorkflowStepName = strWorkflowStepName;
			CopiedStep.strWorkflowStepDescription = strWorkflowStepDescription;
			CopiedStep.strWorkflowStepAction = strWorkflowStepAction;
			CopiedStep.nHostedApplicationId = nHostedApplicationId ;
			CopiedStep.bIsStepComplete = bIsStepComplete;

			return CopiedStep;
		}
	}

	/// <summary>
	/// Class to maintain workflow
	/// 
	/// </summary>
	public class WorkflowData
	{
		#region Variables for three functions below that were refactored out
		//These variables are hanging out here because both old and new WorkflowControl
		//need to work. If it was not for the old they would be gone.
		//Xml constants
		private const string STEP_COMPLETE_VAL = "1";
		private const string STEP_NOT_COMPLETE_VAL = "0";
		private const string XML_XML = "XML";
		private const string XML_SESSION = "Session";
		private const string XML_SESSION_ID = "ID";
		private const string XML_WORKFLOW_STATUS = "Status";
		private const string XML_STEP_ID = "ID";
		private const string XML_STEP_COMPLETE = "Complete";

		#endregion

		//Xml constants
		private const string XML_STEP = "Step";
		private const string XML_WORKFLOW = "Workflow";
		private const string XML_WORKFLOW_ID = "ID";
		private const string XML_WORKFLOW_NAME = "Name";
		private const string XML_WORKFLOW_FORCED = "Forced";

		// Workflow Id that is stored in the database
		private int workflowId;
		// The id of the step that is active, default is -1. This is the Id that is stored in the database
		private int activeStepId;
		// Workflow name to be displayed
		private string strWorkflowName;
		// Is the workflow forced?
		private bool isForced;
		// Array of the workflowstep structure.
		private WorkflowStepArray wfSteps;
	
		public WorkflowData()
		{
			workflowId = -1;
			strWorkflowName = string.Empty;
			isForced = false;
			wfSteps = null;
			activeStepId = -1;
			wfSteps = new WorkflowStepArray();
		}

		/// <summary>
		/// Get and set the workflowId.
		/// </summary>
		public int WorkflowId
		{
			get { return this.workflowId; }
			set
			{
				if (value > 0)
				{
					this.workflowId = value;
				}
				else
				{
					this.workflowId = -1;
				}
			}
		}

		/// <summary>
		/// Get and set the activeStepId
		/// </summary>
		public int ActiveStepId
		{
			get { return this.activeStepId; }
			set
			{
				if (value > 0)
				{
					this.activeStepId = value;
				}
				else
				{
					this.activeStepId = -1;
				}
			}
		}
		
		/// <summary>
		/// Returns a boolean value indicating whether workflow is forced or not
		/// </summary>
		public bool IsForced
		{
			get { return this.isForced; }
			set { this.isForced = value; }
		}

		/// <summary>
		/// Get and set the name for the workflow
		/// </summary>
		public string WorkflowName
		{
			get { return this.strWorkflowName; }
			set
			{
				if (value != string.Empty)
				{
					this.strWorkflowName = value;
				}
				else
				{
					this.strWorkflowName = "";
				}
			}
		}

		/// <summary>
		/// Returns a WorkflowStepArray which can be serialized/deserialized
		/// </summary>
		public WorkflowStepArray Steps
		{
			get { return this.wfSteps; }
		}

		/// <summary>
		/// creates a copy of workflow
		/// </summary>
		/// <returns>copy of workflow</returns>
		public WorkflowData ReturnACopy()
		{
			WorkflowData CopiedWorkflow = new WorkflowData();
			
			CopiedWorkflow.workflowId = workflowId;
			CopiedWorkflow.strWorkflowName = strWorkflowName;
			CopiedWorkflow.isForced = isForced;

			if ( wfSteps != null && wfSteps.Count != 0)
			{
				CopiedWorkflow.wfSteps = new WorkflowStepArray();
				foreach ( WorkflowStep Step in wfSteps)
				{
					WorkflowStep StepOfalSteps = new WorkflowStep();
					StepOfalSteps = Step.ReturnACopy();
					CopiedWorkflow.wfSteps.Add(StepOfalSteps);
				}
				CopiedWorkflow.activeStepId = activeStepId;
			}
			
			return CopiedWorkflow;
		}

		/// <summary>
		/// Set the workflow content from the XML to the data structure.
		/// </summary>
		/// <param name="strWorkflowDataXML">workflow content in XML.</param>
		/// <returns>true if successful and false if failed.</returns>
		public bool SetWorkflowData(string strWorkflowDataXML)
		{
			if ( strWorkflowDataXML == string.Empty )
			{
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(strWorkflowDataXML);
				XmlNode root = doc.DocumentElement;

				//fill in the values.
				workflowId = Convert.ToInt32(root.Attributes[XML_WORKFLOW_ID].InnerText,10);
				strWorkflowName = root.SelectSingleNode(XML_WORKFLOW_NAME).InnerText;
				isForced = Convert.ToBoolean(root.SelectSingleNode(XML_WORKFLOW_FORCED).InnerText);
			}
			catch ( Exception exp)
			{
				Logging.Error(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_DATA,exp);
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Sets the workflow steps' content from the XML to the data structure.
		/// </summary>
		/// <param name="strWorkflowStepsXML">steps' content in the form of XML</param>
		/// <returns>true if successful and false if failed.</returns>
		public bool SetWorkflowStepsData(string strWorkflowStepsXML)
		{
			if ( strWorkflowStepsXML == string.Empty)
			{
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(strWorkflowStepsXML);
			
				XmlNode root = doc.DocumentElement;
				XmlNode WorkflowNode = root.SelectSingleNode(XML_WORKFLOW);

				//verify if the workflow ids are correct.
				//there should not be a mismatch, but still.
				int nWorkflowIDFromXML = Convert.ToInt32(WorkflowNode.Attributes[XML_WORKFLOW_ID].InnerText,10);
				if ( nWorkflowIDFromXML  != workflowId)
				{
					return false;
				}
				
				XmlNodeList Steps = WorkflowNode.SelectNodes(XML_STEP);
				if ( Steps.Count == 0)//no steps for this workflow.
				{
					Logging.Warn(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_STEPS_DATA);
					return false;
				}

				wfSteps = new WorkflowStepArray();
				foreach ( XmlNode StepNode in Steps)
				{
					//form the workflowstep structure
					WorkflowStep Step = new WorkflowStep();
					Step.SetStepData(StepNode.OuterXml);

					//add the structure to the list.
					wfSteps.Add(Step);
				}
			}
			catch ( Exception exp)
			{
				Logging.Error(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_STEPS_DATA, exp );
				throw exp;
			}
			
			return true;
		}
		/// <summary>
		/// Checks if each of the step is marked complete.
		/// </summary>
		/// <returns>true if all are complete, else false</returns>
		public bool AreAllStepsComplete()
		{
			// check if each of the step is complete.
			// ignore if only last step is pending
			foreach ( WorkflowStep Step in wfSteps)
			{
				if ( !Step.IsStepComplete)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the step by step id
		/// </summary>
		/// <param name="nStepId">step id</param>
		/// <returns>step</returns>
		public WorkflowStep GetStepByStepId(int nStepId)
		{
			foreach ( WorkflowStep Step in wfSteps)
			{
				if (Step.Workflowstepid == nStepId)
				{
					return Step;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the step by index
		/// </summary>
		/// <param name="nStepIndex">index</param>
		/// <returns>step</returns>
		public WorkflowStep GetStepByIndex(int nStepIndex)
		{
			return ((WorkflowStep)wfSteps[nStepIndex]);
		}

		/// This function used to be in in the Microsoft.Ccf.Samples.HostedControlInterfaces as part of
		/// WorkflowData class. In the WfWorkflowControl this has been refactored out and replaced with
		/// a Serialization function
		/// <summary>
		/// Forms an XML of the workflow content.
		/// </summary>
		/// <returns>true is successful else false</returns>
		[ObsoleteAttribute ("This method is removed and should not be used.")]
		public string WorkflowDataInXML(Guid sessionId, int nWorkflowStatus, int nActivatedStepId)
		{
			XmlDocument doc = new XmlDocument();
			try
			{
				if (sessionId == Guid.Empty)
				{
					throw new ArgumentOutOfRangeException("sessionId", localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SESSIONID_INVALID);
				}

				if (nWorkflowStatus != 0 && nWorkflowStatus != 1 && nWorkflowStatus != 2)
				{
					throw new ArgumentOutOfRangeException("nWorkflowStatus", localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_STATUS_INVALID);
				}

				XmlNode XMLNode;
				XMLNode = doc.CreateNode(XmlNodeType.Element, XML_XML, "");

				XmlNode SessionNode;
				SessionNode = doc.CreateNode(XmlNodeType.Element, XML_SESSION, "");
				XmlAttribute SessionId = doc.CreateAttribute("", XML_SESSION_ID, "");
				SessionId.InnerText = sessionId.ToString();
				SessionNode.Attributes.Append(SessionId);

				XmlNode Workflow;
				Workflow = doc.CreateNode(XmlNodeType.Element, XML_WORKFLOW, "");
				XmlAttribute WorkflowId = doc.CreateAttribute("", XML_WORKFLOW_ID, "");
				WorkflowId.InnerText = workflowId.ToString();
				XmlAttribute WorkflowStatus = doc.CreateAttribute("", XML_WORKFLOW_STATUS, "");
				WorkflowStatus.InnerText = nWorkflowStatus.ToString();
				Workflow.Attributes.Append(WorkflowId);
				Workflow.Attributes.Append(WorkflowStatus);

				foreach (WorkflowStep step in wfSteps)
				{
					if (step.Workflowstepid == nActivatedStepId)
					{
						XmlNode StepActive = doc.CreateNode(XmlNodeType.Element, XML_STEP, "");
						XmlAttribute StepId = doc.CreateAttribute("", XML_STEP_ID, "");
						StepId.InnerText = nActivatedStepId.ToString();
						StepActive.Attributes.Append(StepId);

						XmlNode Complete;
						Complete = doc.CreateNode(XmlNodeType.Element, XML_STEP_COMPLETE, "");
						Complete.InnerText = STEP_NOT_COMPLETE_VAL;
						StepActive.AppendChild(Complete);
						Workflow.AppendChild(StepActive);

					}
					else
					{
						if (step.IsStepComplete)
						{
							XmlNode StepComplete = doc.CreateNode(XmlNodeType.Element, XML_STEP, "");
							XmlAttribute StepId = doc.CreateAttribute("", XML_STEP_ID, "");
							StepId.InnerText = step.Workflowstepid.ToString();
							StepComplete.Attributes.Append(StepId);

							XmlNode Complete;
							Complete = doc.CreateNode(XmlNodeType.Element, XML_STEP_COMPLETE, "");
							Complete.InnerText = STEP_COMPLETE_VAL;
							StepComplete.AppendChild(Complete);
							Workflow.AppendChild(StepComplete);
						}
					}
				}

				XMLNode.AppendChild(SessionNode);
				XMLNode.AppendChild(Workflow);
				doc.AppendChild(XMLNode);
			}
			catch (Exception exp)
			{
				Logging.Error(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_WORKFLOW_DATA_IN_XML, exp);
				throw exp;
			}

			return doc.OuterXml;
		}

		/// This function used to be in in the Microsoft.Ccf.Samples.HostedControlInterfaces as part of
		/// WorkflowData class. In the WfWorkflowControl this has been refactored out and replaced with
		/// a Serialization function
		/// <summary>
		/// s the pending workflow
		/// </summary>
		/// <param name="pendingWorkflowData">workflow xml</param>
		/// <param name="bShowErr">Flag saying if any errors are to be displayed or not</param>
		/// <returns>True for success else false</returns>
		[ObsoleteAttribute ("This method is removed and should not be used.")]
		public bool SetPendingWorkflowData(string pendingWorkflowData, bool bShowErr)
		{
			if (pendingWorkflowData == string.Empty)
			{
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(pendingWorkflowData);

				XmlNode root = doc.DocumentElement;
				if (root.ChildNodes.Count == 0)
				{
					// No steps in this workflow.
					return false;
				}

				XmlNodeList Steps = root.SelectNodes(XML_STEP);
				if (Steps.Count == 0)
				{
					return false;
				}

				if (wfSteps == null || wfSteps.Count == 0)
				{
					Logging.Warn(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA);
					return false;
				}

				//parse through the xml to get to each of the steps.
				foreach (XmlNode StepNode in Steps)
				{
					foreach (WorkflowStep Step in wfSteps)
					{
						if (Convert.ToInt32(StepNode.Attributes[XML_STEP_ID].InnerText, 10) == Step.Workflowstepid)
						{
							//step found. set the value, complete or not.
							if (StepNode.SelectSingleNode(XML_STEP_COMPLETE).InnerText == STEP_COMPLETE_VAL)
								Step.IsStepComplete = true;
							else
								//set the active step.
								activeStepId = Step.Workflowstepid;
						}
					}
				}
				// Check if the active step was set in the previous iterations
				if (activeStepId == -1)
				{
					if (bShowErr)
					{
						Logging.Error(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA_SET_TO_FIRST_STEP);
					}
					// Active step of the pending workflow was removed.
					// set the active step to the first step in the workflow.
					WorkflowStep firstStep = wfSteps[0] as WorkflowStep;
					activeStepId = firstStep.Workflowstepid;
				}
			}
			catch (Exception exp)
			{
				if (bShowErr)
				{
					Logging.Error(this.ToString(), localizeWF.WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA, exp);
					throw exp;
				}
			}

			return true;
		}

		/// <summary>
		/// set the pending workflow
		/// </summary>
		/// <param name="strPendingWorkflowData">workflow xml</param>
		/// <returns>true if success else false</returns>
		[ObsoleteAttribute ("This method is removed and should not be used.")]
		public bool SetPendingWorkflowData(string strPendingWorkflowData)
		{
			return SetPendingWorkflowData(strPendingWorkflowData, true);
		}
	}
}
#pragma warning restore 1591