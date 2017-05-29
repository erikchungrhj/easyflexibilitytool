Clear-Host

#Imports
Import-Module Azure

Write-Host "Start the Windows Azure Storage local development emulator.."
$test = & 'C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe' status

Write-Host "Test to see if it is running..."
if (!$test.Contains("IsRunning: True")) {
    & 'C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe' start
}