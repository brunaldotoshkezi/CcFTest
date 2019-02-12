@echo off



rem BE SURE to set the path to the target folder correctly because it will del *.* the target 

rem public source code root
set source=C:\CCF\Public

rem Citrix integration component destination
set target=X:\CCFCITRIX



set CitrixIntegration=%source%\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\CitrixIntegration\bin\Debug
set StandaloneTestApp=%source%\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\StandAloneTestApp\bin\Debug
set AgentDesktop=%source%\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\Csr\AgentDesktop\bin\Debug

if not exist "%source%\Framework\CitrixReadInterop.dll" (
	@echo ----
	@echo %source%\Framework\CitrixReadInterop.dll not found
	@echo this .dll is required for CitrixIntegration
	pause
	exit
)

if exist "%target%" (
	@echo ----
	@echo erasing all the contents of the target folder: 
	rd /s "%target%"
	if exist "%target%" (
		@echo target folder NOT erased
		pause
		exit
	)
)

md "%target%"
if errorlevel 1 (
	@echo ----
	@echo unable to create target folder %target%
	pause
	exit
)

xcopy "%source%\Framework\CitrixReadInterop.dll" "%CitrixIntegration%" /Y /R

xcopy "%CitrixIntegration%\*.*" "%target%" /Y /R
xcopy "%StandaloneTestApp%\Microsoft.Ccf.Samples.StandAloneTestApp.*" "%target%" /Y /R

rem copy the CitrixIntegration components to the IAD execution folder to make them work with IAD
xcopy "%CitrixIntegration%\*.*" "%AgentDesktop%" /Y /R 

pause
