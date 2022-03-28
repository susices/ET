set WORKSPACE=..\

set GEN_CLIENT=%WORKSPACE%\Tools\Luban.ClientServer\Luban.ClientServer.exe
set Excel_ROOT=%WORKSPACE%\ConfigInput

@ECHO =======================Editor========================== 

set Export_Server_Code_ROOT=%WORKSPACE%\Unity\Assets\Editor\Config\Generate

%GEN_CLIENT% --template_search_path CustomTeplates -j cfg --^
 -d %Excel_ROOT%\__root__.xml ^
 --input_data_dir %Excel_ROOT% ^
 --output_code_dir %Export_Server_Code_ROOT% ^
 --gen_types code_cs_unity_editor_json ^
 -s editor
pause