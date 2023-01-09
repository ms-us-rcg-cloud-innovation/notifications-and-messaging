$tfPlan = "acs.tfplan"
$tfDir  = "../../../infrastructure/terraform/acs-demo/"

function Assert-TerraformLastExitCode([string] $messageOnFailure) {
    if($LASTEXITCODE -eq 1) {
        Write-Host $messageOnFailure
        Exit 1
    }
}

function Get-TerraformOutputs {
    $tf_output = terraform -chdir="$tfDir" output -json | ConvertFrom-Json
    Write-Host $tf_output
    Assert-TerraformLastExitCode("Failed to get terraform outputs")

    $tf_output
}

function Build-Infrastructure {
    Write-Host "Initializing Terraform"
    terraform -chdir="$tfDir" init
                    
    Write-Host "Validating Terraform"
    terraform -chdir="$tfDir" validate

    Write-Host "Terraform Plan"
    terraform -chdir="$tfDir" `
              plan `
              -detailed-exitcode `
              -out="$tfPlan"

    Assert-TerraformLastExitCode("terraform plan failed")
      
    terraform -chdir="$tfDir" output

    do {
        $apply_tf = (Read-Host -Prompt "Do you wish to apply the plan [y/n]?").ToLowerInvariant()
    } while (($apply_tf -ne "y") -and ($apply_tf -ne "n"))
                    
    if ($apply_tf -eq "y") {        
        Write-Host "Terraform Apply"
        terraform -chdir="$tfDir" apply "$tfPlan"

        Assert-TerraformLastExitCode("terraform apply failed")                  
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
    $terraform_output = Get-TerraformOutputs
    $resourceGroupName = $terraform_output.resource_group_name.value
    $functionAppName = $terraform_output.function_app_name.value

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
    
    if($LASTEXITCODE -gt 0) {
        Write-Host "dotnet publish failed"
        exit 1
    }

    # create zip for publishing
    Get-ChildItem $publishPath | Compress-Archive -CompressionLevel "Fastest" -DestinationPath $zipFile

    Write-Host "Deploying fucntion app '$functionAppName'"
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
