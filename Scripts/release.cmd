@echo off

call ".\Scripts\compile.cmd"

if exist "EfficientBaseRepair.zip" DEL "EfficientBaseRepair.zip"

7z.exe a "EfficientBaseRepair.zip" ^
    Config ^
    UIAtlases ^
    EfficientBaseRepair.dll ^
    README.md ^
    ModInfo.xml > nul

DEL .\EfficientBaseRepair.dll
DEL .\EfficientBaseRepair.pdb

set MOD_PATH="%PATH_7D2D%\Mods\EfficientBaseRepair"

if exist %MOD_PATH% RMDIR /s /q %MOD_PATH%

mkdir %MOD_PATH%

cd %MOD_PATH%

7z.exe x "%~dp0..\EfficientBaseRepair.zip" > nul