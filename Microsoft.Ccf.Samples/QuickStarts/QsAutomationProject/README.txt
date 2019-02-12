Automation Project QuickStart
=============================

This project contains a number of HAT WF/Automation examples that run against
two of the existing sample applications: Sample Web App and StandaloneTestApp.



Configuration Procedure

1.  Check assembly References.  There should be references to the following:

        C:\CCF\SourceCode\Framework\Microsoft.Ccf.HostedApplicationToolkit.Activities.dll
        C:\CCF\SourceCode\Framework\Microsoft.Ccf.HostedApplicationToolkit.AutomationHosting.dll

    "Copy Local" on these references should be false, but this is not strictly necessary.
    No other CCF assembly references are required.


2.  Configure Data Driven Adapter Bindings.  These are the list of controls that
    Automations interact with.  For convenience, the contents of the InitString*.xml
   .files can be copied into the application initstring configuration window on
    Admin Console.
    
    InitStringSampleWebApp.xml      -- copy-n-paste contents into --> Sample Web App
    InitStringStandAloneTestApp.xml -- copy-n-paste contents into --> StandAloneTestApp


3.  Configure Actions implemented by Automations:

    On the Sample Web App:
    
      DefaultActionSampleWebApp
      Microsoft.Ccf.QuickStarts.QsAutomationProject.DefaultActionSampleWebApp,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      CopyName
      Microsoft.Ccf.QuickStarts.QsAutomationProject.CopyName,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

         or paste the following in the Xaml window:
         
      <SequentialWorkflowActivity x:Name="CopyName" xmlns:ns0="clr-namespace:Microsoft.Ccf.HostedApplicationToolkit.Activity;Assembly=Microsoft.Ccf.HostedApplicationToolkit.Activity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4f00c1aa5320a4d9" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" xmlns="http://schemas.microsoft.com/winfx/2006/xaml/workflow">
          <ns0:GetControlValue x:Name="getControlValue1" ControlName="name textbox" ExceptionMessage="{x:Null}" ApplicationName="Sample Web App" />
          <ns0:SetControlValue Value="{ActivityBind getControlValue1,Path=Value}" x:Name="setControlValue1" ControlName="textbox_acc" ExceptionMessage="{x:Null}" ApplicationName="StandaloneTestApp" />
      </SequentialWorkflowActivity>

      ProcessFavoriteColor
      Microsoft.Ccf.QuickStarts.QsAutomationProject.ProcessFavoriteColor,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      TestFaulting
      Microsoft.Ccf.QuickStarts.QsAutomationProject.TestFaulting,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      MsgDocumentCompleted
      Microsoft.Ccf.QuickStarts.QsAutomationProject.MsgDocumentCompleted,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      MsgBeforeNavigate
      Microsoft.Ccf.QuickStarts.QsAutomationProject.MsgBeforeNavigate,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      MsgBeforeNewWindow
      Microsoft.Ccf.QuickStarts.QsAutomationProject.MsgBeforeNewWindow,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll



    On the StandAloneTestApp:
    
      DefaultActionStandAloneTestApp
      Microsoft.Ccf.QuickStarts.QsAutomationProject.DefaultActionStandAloneTestApp,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      HandleContextChanged
      Microsoft.Ccf.QuickStarts.QsAutomationProject.HandleContextChanged,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll
      
      ControlFinders
      Microsoft.Ccf.QuickStarts.QsAutomationProject.ControlFinders,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      FibonacciNumber
      Microsoft.Ccf.QuickStarts.QsAutomationProject.FibonacciNumber,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      TestFaulting
      Microsoft.Ccf.QuickStarts.QsAutomationProject.TestFaulting,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      CountDownAsync (select Asynchronous checkbox)
      Microsoft.Ccf.QuickStarts.QsAutomationProject.CountDown,Microsoft.Ccf.QuickStarts.QsAutomationProject

      MsgButtonPressed
      Microsoft.Ccf.QuickStarts.QsAutomationProject.MsgButtonPressed,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll

      MsgHelloWorld
      Microsoft.Ccf.QuickStarts.QsAutomationProject.MsgHelloWorld,file://C:\CCF\SourceCode\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll


4.  Exercising the Actions and Automations:

    CopyName
    This copies the context of the "Your name:" textobx  to the "Test:" textbox on StandAloneTestApp
    This demonstrates an executable Automation from pure Xaml
    
    ProcessFavoriteColor
    Reads the "Your favorite color:" drop-down on Sample Web App and writes a related phrase into the "Test:" textbox on StandAloneTestApp
    This demonstrates if/then/else decision processing
    
    ControlFinders
    Looks for the existence of two controls on separate applications, then updates the Context with the results of the operations.
    This demonstrates the ControlFinder and SetContext Activities
    
    FibonacciNumber
    Recursively calculates the nth number in the Fibonacci sequence.
    This demonstrates Actions operating on and returning the Action Data parameter.

    TestFaulting
    Demonstrates an Automation with a workflow fault handler responding to a Data Driven Adapter exception.
    It also shows that a single Automation can implement multiple configured Actions.

    CountDownAsync
    Demonstrates a long running asynchronous/background automation.  Include fault handler to detect session close.

    MsgHelloWorld
    MsgDocumentCompleted
    MsgButtonPressed
    These are all simple Automations that outputs a line of text from a code activity.
    This demonstrates simple Automations than can be included within an Automation assembly that can be useful for troubleshooting purposes.

    DefaultActionSampleWebApp
    DefaultActionStandAloneTestApp
    These Automations run on session startup and register Actions that execute in response to certain events
    This demonstrates the execution of Automations in response to events
    Events showcased are:
    
      DocumentCompleted on SampleWebApp
      When the web page completes loading, the MsgDocumentCompleted action popup runs
      
      ContextChanged on StandaloneTestApp
      When the AIF context changes, the MsgContextChange action popup runs--displaying CustomerFirstName
      
      ButtonPressed on StandaloneTestApp
      When the OK button is pressed, the MsgButtonPressed action popup runs



    The Visual Studio debugger can be used to debug Automations with Agent Desktop.
    Attach to the AgentDesktop process with the AutomationProject open to set breakpoints, etc.
    For this to work, the AutomationProject.dll will need to be in the same folder as the Agent Desktop
    and the Action configured on the Admin Console to reference the assembly by name, not by file:// reference.






DDA Control Names
=================

Applications and the available DDA-configured control names, illustrated here
for easy reference:


Sample Web App
--------------
name textbox
pw textbox
likepizza yes
likepizza no
likepizza maybe
likepizza nevertried
like our service?
comments
favorite color
submit button
reset button


StandaloneTestApp
-----------------
textbox_acc
button_acc
checkbox_acc
radio1
radio2
radio3
radio1_acc
radio2_acc
radio3_acc
tab1_acc
tab2_acc
crashbutton_acc