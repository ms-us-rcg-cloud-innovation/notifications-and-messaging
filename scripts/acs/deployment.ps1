
$az_login = "x"
while(($az_login -ne "y") -or ($az_login -ne "n"))
{
    $az_login = (Read-Host -Prompt "Login to Azure [y/n]?").ToLowerInvariant()    
}

if($az_login -eq "y")
{
    # configure terraform
    Write-Host "Login"
    az login
}

Write-Host "Subscription"
az account show

$continue = "x"
while(($continue -ne "y") -or ($continue -ne "n"))
{
    $continue = (Read-Host -Prompt "Do you wish to proceed  [y/n]?").ToLowerInvariant()
}

if($continue.ToLowerInvariant() -eq "n")
{
    Write-Host "Terminating deployment"
    exit 0
}

Get-Location
$init_terra = "x"

while(($init_terra -ne "y") -or ($init_terra -ne "n"))
{
    init_terra = (Read-Host -Prompt "Ready to begin [y/n]?").ToLowerInvariant()
}

if($init_terra -eq "n")
{
    Write-Host "Terminating deployment"
    exit 0
}

Write-Host "Initializing Terraform"
terraform init

Write-Host "Validating Terraform"
terraform validate

Write-Host "Terraform Plan"
terraform plan

