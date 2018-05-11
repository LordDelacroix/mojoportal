xcopy /s /y /d ".\bin\ironclad.Features.UI.dll" "..\Web\bin\"
xcopy /s /y /d ".\bin\ironclad.Business.dll" "..\Web\bin\"
xcopy /s /y /d ".\bin\ironclad.Data.dll" "..\Web\bin\"

xcopy /s /y /d ".\App_GlobalResources\*.resx" "..\Web\App_GlobalResources\"

xcopy /s /y /d ".\Setup\*" "..\Web\Setup"

xcopy /s /y /d ".\Reservations\*.aspx" "..\Web\Reservations\"
xcopy /s /y /d ".\Reservations\*.ascx" "..\Web\Reservations\"
xcopy /s /y /d ".\Reservations\*.ashx" "..\Web\Reservations\"
xcopy /s /y /d ".\Data\*" "..\Web\Data"
xcopy /s /y /d ".\Services\*.ashx" "..\Web\Services\"

xcopy /s /y /d ".\Setup\*" "..\Web\Setup"

xcopy /s /y /d ".\Scripts\*.js" "..\Web\ClientScript\"
xcopy /s /y /d ".\Styles\*.css" "..\Web\Data\style\"