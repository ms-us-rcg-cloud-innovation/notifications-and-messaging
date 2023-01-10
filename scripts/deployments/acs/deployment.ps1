$tfPlan = "acs.tfplan"
$tfDir  = "../../../infrastructure/terraform/acs-demo/"

function Assert-TerraformLastExitCode([string] $messageOnFailure) {
    if($LASTEXITCODE -eq 1) {
        Write-Host $messageOnFailure
        Exit 1
    }
}

function Get-TerraformOutputs {
    $tf_output = terraform -chdir="$tfDir" output -json 

    Write-Host ($tf_output | ConvertFrom-Json | ConvertTo-Json -Depth 100)

    $tf_output = $tf_output | ConvertFrom-Json
    
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

function Clear-PublishFiles([string] $publishPath, [string] $zipFile) {
    # if a previous depployment artifacts exist -- delete them
    if (Test-Path $publishPath) {
        Remove-item -Recurse $publishPath 
    }

    if (Test-Path $zipFile) {
        Remove-Item $zipFile
    }
}

function Deploy-AcsFunctions {
    
    # azure functions deployment
    $functionAppProjectPath = "../../../src/apps/AzureCommunicationServices/Functions"
    $publishPath = "func-apps"
    $zipFile = "fa-publish.zip"
    $terraform_output = Get-TerraformOutputs
    $resourceGroupName = $terraform_output.resource_group_name.value
    $functionAppName = $terraform_output.function_app_name.value

    Clear-PublishFiles $publishPath $zipFile

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

    if($LASTEXITCODE -eq 0) {
        Clear-PublishFiles $publishPath $zipFile
    }
}

function Deploy-AcsLogicApps {
        # azure functions deployment
        $logicAppProjectPath = "../../../src/apps/AzureCommunicationServices/Logic-Apps"
        $zipFile = "la-publish.zip"
        $terraform_output = Get-TerraformOutputs
        $resourceGroupName = $terraform_output.resource_group_name.value
        $logicAppName = $terraform_output.logic_app_name.value

        Clear-PublishFiles $publishPath $zipFile

        # create zip for publishing
        Get-ChildItem $logicAppProjectPath | Compress-Archive -CompressionLevel "Fastest" -DestinationPath $zipFile

        Write-Host "Deploying logic app '$logicAppName'"
        az logicapp  deployment source config-zip `
                        --resource-group $resourceGroupName `
                        --name $logicAppName `
                        --src $zipFile

        if($LASTEXITCODE -eq 0) {
            Clear-PublishFiles $publishPath $zipFile
        }
}

function Submit-PromptUntilValidInputDeploymentStep($yes_or_no_prompt, $func) {
    do {
        $run_tf = (Read-Host -Prompt $yes_or_no_prompt).ToLowerInvariant()
    } while (($run_tf -ne "y") -and ($run_tf -ne "n"))

    if ($run_tf -eq "y") {
        & $func
    }
}

# infrastructure deployment
Submit-PromptUntilValidInputDeploymentStep -yes_or_no_prompt "Deploy infrastructure [y/n]? " -func Build-Infrastructure

# func app deployment
Submit-PromptUntilValidInputDeploymentStep -yes_or_no_prompt "Deply function apps [y/n]? " -func Deploy-AcsFunctions

# logic app deployment
Submit-PromptUntilValidInputDeploymentStep -yes_or_no_prompt "Deploy logic apps [y/n]? " -func Deploy-AcsLogicApps