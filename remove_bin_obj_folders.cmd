@echo off
REM Removes all "bin" and "obj" folders from subdirectories of the current directory

for /d /r %%d in (bin,obj) do (
    if exist "%%d" (
        echo Deleting folder: "%%d"
        rmdir /s /q "%%d"
    )
)

echo All "bin" and "obj" folders have been removed.
pause