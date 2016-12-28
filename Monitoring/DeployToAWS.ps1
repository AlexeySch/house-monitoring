dotnet build --runtime ubuntu.14.04-x64 -c Release --no-incremental
& "c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /p:DeployOnBuild=true /P:Configuration=Release /p:PublishProfile=AWS

$pwd = ConvertTo-SecureString “ ” -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential ("ubuntu",$pwd)
$keypath = (Get-Location).Path + "\tunnel.pem"
$session = New-SSHSession -ComputerName 35.156.115.201 -Credential $Credentials -AcceptKey -KeyFile $keypath
Invoke-SSHCommand -SSHSession $session -Command "sudo rm -rf /tmp/Monitoring/"

Set-SCPFolder -ComputerName 35.156.115.201 -Credential $Credentials -LocalFolder .\bin\Release\PublishOutput -RemoteFolder /tmp/Monitoring -AcceptKey -KeyFile $keypath

Invoke-SSHCommand -SSHSession $session -Command "sudo rm -rf /var/house-monitoring/* && sudo cp -rf /tmp/Monitoring/* /var/house-monitoring/"

Remove-SSHSession -SSHSession $session