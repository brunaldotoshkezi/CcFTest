﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Ccf.Samples.WorkFlowControl.WorkflowWS
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://microsoft.com/nsp/contactcenter", ConfigurationName="Microsoft.Ccf.Samples.WorkFlowControl.WorkflowWS.WorkflowWs")]
    public interface WorkflowWs
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/nsp/contactcenter/WorkflowWs/GetWorkflowNames", ReplyAction="http://microsoft.com/nsp/contactcenter/WorkflowWs/GetWorkflowNamesResponse")]
        string GetWorkflowNames(int nAgentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/nsp/contactcenter/WorkflowWs/GetIntelligentWorkflow", ReplyAction="http://microsoft.com/nsp/contactcenter/WorkflowWs/GetIntelligentWorkflowResponse")]
        int GetIntelligentWorkflow(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/nsp/contactcenter/WorkflowWs/GetWorkflowSteps", ReplyAction="http://microsoft.com/nsp/contactcenter/WorkflowWs/GetWorkflowStepsResponse")]
        string GetWorkflowSteps(int nWorkflowId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface WorkflowWsChannel : Microsoft.Ccf.Samples.WorkFlowControl.WorkflowWS.WorkflowWs, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class WorkflowWsClient : System.ServiceModel.ClientBase<Microsoft.Ccf.Samples.WorkFlowControl.WorkflowWS.WorkflowWs>, Microsoft.Ccf.Samples.WorkFlowControl.WorkflowWS.WorkflowWs
    {
        
        public WorkflowWsClient()
        {
        }
        
        public WorkflowWsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public WorkflowWsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public WorkflowWsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public WorkflowWsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string GetWorkflowNames(int nAgentId)
        {
            return base.Channel.GetWorkflowNames(nAgentId);
        }
        
        public int GetIntelligentWorkflow(System.Guid sessionId)
        {
            return base.Channel.GetIntelligentWorkflow(sessionId);
        }
        
        public string GetWorkflowSteps(int nWorkflowId)
        {
            return base.Channel.GetWorkflowSteps(nWorkflowId);
        }
    }
}
