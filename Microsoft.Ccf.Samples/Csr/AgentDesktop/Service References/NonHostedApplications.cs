﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop.NonHostedApplications
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://microsoft.com/ccf/ContactCenter/NonHostedApplication", ConfigurationName="Microsoft.Ccf.Samples.Csr.AgentDesktop.NonHostedApplications.NonHostedApplication" +
        "")]
    public interface NonHostedApplication
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/ccf/ContactCenter/NonHostedApplication/NonHostedApplication/" +
            "GetNonHostedApplications", ReplyAction="http://microsoft.com/ccf/ContactCenter/NonHostedApplication/NonHostedApplication/" +
            "GetNonHostedApplicationsResponse")]
        string GetNonHostedApplications();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface NonHostedApplicationChannel : Microsoft.Ccf.Samples.Csr.AgentDesktop.NonHostedApplications.NonHostedApplication, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class NonHostedApplicationClient : System.ServiceModel.ClientBase<Microsoft.Ccf.Samples.Csr.AgentDesktop.NonHostedApplications.NonHostedApplication>, Microsoft.Ccf.Samples.Csr.AgentDesktop.NonHostedApplications.NonHostedApplication
    {
        
        public NonHostedApplicationClient()
        {
        }
        
        public NonHostedApplicationClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public NonHostedApplicationClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public NonHostedApplicationClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public NonHostedApplicationClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string GetNonHostedApplications()
        {
            return base.Channel.GetNonHostedApplications();
        }
    }
}