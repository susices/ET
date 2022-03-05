set WORKSPACE=..\

set GEN_CLIENT=%WORKSPACE%\Tools\Luban.ClientServer\Luban.ClientServer.exe
set Excel_ROOT=%WORKSPACE%\Excel


@ECHO =======================SERVER==========================

set Export_Server_Code_ROOT=%WORKSPACE%\Server\Model\Generate\Config
set Export_Server_Data_ROOT=%WORKSPACE%\Config\Excel

%GEN_CLIENT% -h %LUBAN_SERVER_IP% -j cfg --^
 -d %Excel_ROOT%\__root__.xml ^
 --input_data_dir %Excel_ROOT% ^
 --output_code_dir %Export_Server_Code_ROOT% ^
 --output_data_dir %Export_Server_Data_ROOT% ^
 --gen_types code_cs_bin,data_bin ^
 -s server 
 
@ECHO =======================CLIENT========================== 

set Export_Client_Code_ROOT=%WORKSPACE%\Unity\Codes\Model\Generate\Config
set Export_Client_Data_ROOT=%WORKSPACE%\Unity\Assets\Res\DataTables

%GEN_CLIENT% -h %LUBAN_SERVER_IP% -j cfg --^
 -d %Excel_ROOT%\__root__.xml ^
 --input_data_dir %Excel_ROOT% ^
 --output_code_dir %Export_Client_Code_ROOT% ^
 --output_data_dir %Export_Client_Data_ROOT% ^
 --gen_types code_cs_bin,data_bin ^
 -s client 

pause