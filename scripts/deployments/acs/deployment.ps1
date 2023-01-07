[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)][string] $resourceGroupName,
    [Parameter(Mandatory = $true)][string] $acsName,
    [Parameter(Mandatory = $true)][string] $fromEmail
)


function Build-Infrastructure {
    $tfPlan = "acs.tfplan"
    $tfDir  = "../../../infrastructure/terraform/acs-demo/"


    Write-Host "Initializing Terraform"
    terraform -chdir="$tfDir" init
                    
    Write-Host "Validating Terraform"
    terraform -chdir="$tfDir" validate
            
    # if(Test-Path $planPath)
    # {# clean up old path
    #     Remove-Item $planPath
    # }
    
    Write-Host "Terraform Plan"
    terraform -chdir="$tfDir" `
              plan `
              -detailed-exitcode `
              -out="$tfPlan" `
              -var "resource_group_name=$resourceGroupName" `
              -var "comm_services_name=$acsName" `
              -var "from_email=$fromEmail"
                    
    do {
        $apply_tf = (Read-Host -Prompt "Do you wish to apply the plan [y/n]?").ToLowerInvariant()
    } while (($apply_tf -ne "y") -and ($apply_tf -ne "n"))
                    
    if ($apply_tf -eq "y") {        
        Write-Host "Terraform Apply"
        terraform -chdir="$tfDir" apply "$tfPlan"
    }                
}

function Start-AzLoginAndValidation {
    do {
        $login = (Read-Host -Prompt "Login to Azure [y/n]?").ToLowerInvariant()    
    } while (($login -ne "y") -and ($login -ne "n"))
    
    if ($login -eq "y") {
        # configure terraform
        Write-Host "Login"
        az login
    }
    
    Write-Host "Subscription"
    az account show
    
}

function Deploy-AcsFunctions {
    
    # azure functions deployment
    $functionAppProjectPath = "../../../src/apps/AzureCommunicationServices/Functions"
    $publishPath = "acs_funcs"
    $zipFile = "publish.zip"
    
    # the function name is currently hardcoded in the demo 
    # terraform script -- so we're just using it here
    # if you change it in terraform make sure you change it here 
    $functionAppName = 'rcg-acs-demo-funcs-app'

    # if a previous depployment artifacts exist -- delete them
    if (Test-Path $publishPath) {
        Remove-item -Recurse $publishPath 
    }

    if (Test-Path $zipFile) {
        Remove-Item $zipFile
    }

    # build and publish solution to local path
    dotnet publish $functionAppProjectPath `
                -c Release `
                -o $publishPath `
                --os "linux"

    # create zip for publishing
    Get-ChildItem $publishPath | Compress-Archive -CompressionLevel "Fastest" `
                                                  -DestinationPath $zipFile

    Write-Host "Deploying fucntion app 'rcg-acs-demo-funcs-app'"
    az functionapp deployment source config-zip `
                    --resource-group $resourceGroupName `
                    --name $functionAppName `
                    --src $zipFile
}


# infrastructure deployment
do {
    $run_tf = (Read-Host -Prompt "Do you wish to proceed with terraform [y/n]?").ToLowerInvariant()
} while (($run_tf -ne "y") -and ($run_tf -ne "n"))

if ($run_tf -eq "y") {
    Build-Infrastructure
}

# func app deployment
do {
    $deploy_az_funcs = (Read-Host -Prompt "Do you wish to deploy applications to infrastructure [y/n]?").ToLowerInvariant()
} while (($deploy_az_funcs -ne "y") -and ($deploy_az_funcs -ne "n"))

if($deploy_az_funcs -eq "y")
{
    Deploy-AcsFunctions
}