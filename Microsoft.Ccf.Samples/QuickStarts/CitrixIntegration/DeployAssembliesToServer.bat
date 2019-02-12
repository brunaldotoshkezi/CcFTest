@echo off
cls

@echo Setting the server install target to %target%
rem BE SURE to set the path to the target folder correctly because it will del *.* the target 
set target=X:\CCF

rem CLEAR the target folder of all contents
@echo erasing all the contents of the target folder %target% 
del %target%\*.* /Q /F

set Ref=Ref
set Bin=..\..\..\..\..\..\Public\Framework
set LogPro=..\..\..\Common\Logging.Providers\bin\Debug
set DemoApps=..\..\..\..\..\..\Public\Microsoft.Ccf.Samples\DemoCode\CCFDemoApps
set CitrixIntegraton=..\..\..\..\..\..\Public\Microsoft.Ccf.Samples\DemoCode\CitrixIntegration
set AgentDeskTop=..\..\..\..\..\..\Public\Microsoft.Ccf.Samples\Csr\AgentDesktop\bin\Debug

@echo Copying all StandAloneTestAppStub.exe and it dependencies to %target%

xcopy %Bin%\Interop.WFICALib.dll %target% /Y /R
xcopy %Bin%\AxInterop.SHDocVw.dll %target% /Y /R
xcopy %Bin%\AxInterop.WFICALib.dll %target% /Y /R
xcopy %Bin%\CitrixReadInterop.dll %target% /Y /R
xcopy %Bin%\CitrixReadInterop.pdb %target% /Y /R
xcopy %Bin%\Interop.MetaFrameCOM.dll %target% /Y /R
xcopy %Bin%\Interop.SHDocVw.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Listeners.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Listeners.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Logging.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Logging.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Logging.Providers.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Logging.Providers.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Providers.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.Providers.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.dll.config %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Core.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Core.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Cti.Providers.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Cti.Providers.pdb %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Win32Api.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Win32Api.pdb %target% /Y /R

xcopy %DemoApps%\StandAloneTestApp\bin\Debug\Microsoft.Ccf.Samples.StandAloneTestApp.exe %target% /Y /R
xcopy %DemoApps%\StandAloneTestApp\bin\Debug\Microsoft.Ccf.Samples.StandAloneTestApp.pdb %target% /Y /R

xcopy %CitrixIntegraton%\StandAloneTestAppStub\bin\Debug\StandAloneTestAppStub.exe %target% /Y /R
xcopy %CitrixIntegraton%\StandAloneTestAppStub\bin\Debug\StandAloneTestAppStub.pdb %target% /Y /R
xcopy %CitrixIntegraton%\StandAloneTestAppStub\bin\Debug\StandAloneTestAppStub.exe.config %target% /Y /R
xcopy %CitrixIntegraton%\ApplicationAdapter\bin\Debug\Microsoft.Ccf.Samples.Citrix.ApplicationAdapter.dll %target% /Y /R
xcopy %CitrixIntegraton%\ApplicationAdapter\bin\Debug\Microsoft.Ccf.Samples.Citrix.ApplicationAdapter.pdb %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorldTestAppStub\bin\Debug\WinFormHelloWorldTestAppStub.exe %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorldTestAppStub\bin\Debug\WinFormHelloWorldTestAppStub.pdb %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorldTestAppStub\bin\Debug\WinFormHelloWorldTestAppStub.exe.config %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorld\bin\Debug\Microsoft.Ccf.Samples.Citrix.WinFormHelloWorld.dll %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorld\bin\Debug\Microsoft.Ccf.Samples.Citrix.WinFormHelloWorld.pdb %target% /Y /R

rem Must copy these to the AgentDeskTop folder to debug because they can't be found by ChannelData serialization code
@echo Copy Files to AgentDeskTop for debugging
xcopy %Bin%\Interop.WFICALib.dll %AgentDeskTop% /Y /R
xcopy %Bin%\AxInterop.WFICALib.dll %AgentDeskTop% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.dll %AgentDeskTop% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.pdb %AgentDeskTop% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.dll.config %AgentDeskTop% /Y /R

dir %target%
