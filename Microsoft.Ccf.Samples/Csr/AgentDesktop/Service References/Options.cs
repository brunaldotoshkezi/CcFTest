﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop.Options
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://microsoft.com/ccf/ContactCenter/Options", ConfigurationName="Microsoft.Ccf.Samples.Csr.AgentDesktop.Options.Options")]
    public interface Options
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://microsoft.com/ccf/ContactCenter/Options/Options/GetOptionSetting", ReplyAction="https://microsoft.com/ccf/ContactCenter/Options/Options/GetOptionSettingResponse")]
        string GetOptionSetting(string itemName);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://microsoft.com/ccf/ContactCenter/Options/Options/GetCCFVersions", ReplyAction="https://microsoft.com/ccf/ContactCenter/Options/Options/GetCCFVersionsResponse")]
        string GetCCFVersions();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface OptionsChannel : Microsoft.Ccf.Samples.Csr.AgentDesktop.Options.Options, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class OptionsClient : System.ServiceModel.ClientBase<Microsoft.Ccf.Samples.Csr.AgentDesktop.Options.Options>, Microsoft.Ccf.Samples.Csr.AgentDesktop.Options.Options
    {
        
        public OptionsClient()
        {
        }
        
        public OptionsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public OptionsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public OptionsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public OptionsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public string GetOptionSetting(string itemName)
        {
            return base.Channel.GetOptionSetting(itemName);
        }
        
        public string GetCCFVersions()
        {
            return base.Channel.GetCCFVersions();
        }
    }
}
