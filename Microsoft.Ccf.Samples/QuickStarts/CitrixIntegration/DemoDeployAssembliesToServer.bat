@echo off
cls

@echo Setting the server install target to %target%
rem BE SURE to set the path to the target folder correctly because it will del *.* the target 
set target=X:\CCF

rem CLEAR the target folder of all contents
@echo erasing all the contents of the target folder %target% 
del %target%\*.* /Q /F

rem You may need to modify these environment variables if you did not use the default installation locations
set Bin="C:\Program Files\Microsoft\Microsoft.Ccf.Samples.AgentDesktop"
set DemoApps="C:\Program Files\Microsoft\Microsoft.Ccf.Samples.DemoApplications"
set CitrixIntegraton="C:\Program Files\Microsoft\Microsoft.Ccf.Samples.Citrix"


@echo Copying all sample applicaton and their dependencies to %target%

xcopy %Bin%\Interop.WFICALib.dll %target% /Y /R
xcopy %Bin%\AxInterop.SHDocVw.dll %target% /Y /R
xcopy %Bin%\AxInterop.WFICALib.dll %target% /Y /R
xcopy %Bin%\CitrixReadInterop.dll %target% /Y /R

xcopy %Bin%\Interop.MetaFrameCOM.dll %target% /Y /R
xcopy %Bin%\Interop.SHDocVw.dll %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Common.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Common.Listeners.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Common.Logging.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Common.Logging.Providers.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Common.Providers.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.dll %target% /Y /R
REM xcopy %Bin%\Microsoft.Ccf.Csr.Citrix.Adapter.dll.config %target% /Y /R
xcopy %Bin%\Microsoft.Ccf.Csr.Core.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Csr.Cti.Providers.dll %target% /Y /R

xcopy %Bin%\Microsoft.Ccf.Csr.Win32Api.dll %target% /Y /R


xcopy %DemoApps%\Microsoft.Ccf.Samples.StandAloneTestApp.exe %target% /Y /R


xcopy %CitrixIntegraton%\StandAloneTestAppStub.exe %target% /Y /R
xcopy %CitrixIntegraton%\StandAloneTestAppStub.exe.config %target% /Y /R
xcopy %CitrixIntegraton%\Microsoft.Ccf.Samples.Citrix.ApplicationAdapter.dll %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorldTestAppStub.exe %target% /Y /R
xcopy %CitrixIntegraton%\WinFormHelloWorldTestAppStub.exe.config %target% /Y /R
xcopy %CitrixIntegraton%\Microsoft.Ccf.Samples.Citrix.WinFormHelloWorld.dll %target% /Y /R

rem Must copy these to the AgentDeskTop folder to debug because they can't be found by ChannelData serialization code
@echo Copy Files to AgentDeskTop for debugging



dir %target%
