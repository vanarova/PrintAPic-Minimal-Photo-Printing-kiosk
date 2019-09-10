REM Copy script for bin ,pics , properties folders from both projects, and config file to picstart folder, refer present files  and write script accordinly

REM delete setup foders
RMDIR "Setup\PicsDirectoryDisplayWin" /S /Q
RMDIR "Setup\PrintAPicStart" /S /Q

xcopy "..\PicsDirectoryDisplayWin\bin\x86\Release" "Setup\PicsDirectoryDisplayWin\bin\Release" /c /d /e /h /i /k /q /r /s /x /y

xcopy "..\PicsDirectoryDisplayWin\bin\AllReceipts" "Setup\PicsDirectoryDisplayWin\bin\AllReceipts" /c /d /e /h /i /k /q /r /s /x /y

xcopy "..\PrintAPicStart\bin\x86\Release" "Setup\PrintAPicStart\bin\Release" /c /d /e /h /i /k /q /r /s /x /y

xcopy "..\PrintAPicStart\bin\AllReceipts" "Setup\PrintAPicStart\bin\AllReceipts" /c /d /e /h /i /k /q /r /s /x /y

REM copy exe and config from application to starter app folder
xcopy "Setup\PicsDirectoryDisplayWin\bin\x86\Release\PicsDirectoryDisplayWin.exe" "Setup\PrintAPicStart\bin\Release" /c /d /e /h /i /k /q /r /s /x /y

xcopy "Setup\PicsDirectoryDisplayWin\bin\x86\Release\PicsDirectoryDisplayWin.exe.config" "Setup\PrintAPicStart\bin\Release" /c /d /e /h /i /k /q /r /s /x /y


