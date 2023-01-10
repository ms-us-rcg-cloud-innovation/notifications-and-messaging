param (
    [Parameter()]
    [string]
    $location
)

# global variables used accross functions to properly execute terraform scripts
$tfPlan = "acs.tfplan"
$tfDir  = "infrastructure/terraform/acs-demo/"

function Assert-TerraformHasSuccessCode([string] $messageOnFailure) {
    if($LASTEXITCODE -eq 1) {
        Write-Host -ForegroundColor Red $messageOnFailure
        Exit 1
    }
}

function Assert-GenericSuccessCode([string] $messageOnFailure) {
    if($LASTEXITCODE -gt 0) {
        Write-Host -ForegroundColor Red $messageOnFailure
        Exit 1
    }
}

function Write-DarkYellowMessage([string] $message) {
    Write-Host -ForegroundColor DarkYellow $message
}

function Get-TerraformOutputs {
    $tf_output = terraform -chdir="$tfDir" output -json 

    # pretty print json
    Write-Host ($tf_output | ConvertFrom-Json | ConvertTo-Json -Depth 100)

    $tf_output = $tf_output | ConvertFrom-Json
    
    Assert-TerraformHasSuccessCode("Failed to get terraform outputs")

    $tf_output
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

function Submit-PromptUntilYesOrNoInput($yes_or_no_prompt, $func) {
    do {
        $run_tf = (Read-Host -Prompt $yes_or_no_prompt).ToLowerInvariant()
    } while (($run_tf -ne "y") -and ($run_tf -ne "n"))

    if ($run_tf -eq "y") {
        & $func
    }
}

# az cli setup
Submit-PromptUntilYesOrNoInput "Login to Azure [y/n]?" {
    # configure terraform
    Write-Host "az login"
    az login
}

# infrastructure deployment
Submit-PromptUntilYesOrNoInput "Execute terraform steps [y/n]?" {
    $infra_location = $location ?? "East US 2"
    
    Write-DarkYellowMessage "terraform init"
    terraform -chdir="$tfDir" init
                    
    Write-DarkYellowMessage "terraform validate"
    terraform -chdir="$tfDir" validate

    Write-DarkYellowMessage "terraform plan"
    terraform -chdir="$tfDir" `
            plan `
            -detailed-exitcode `
            -out="$tfPlan"
            -var="location=$infra_location"
            
    Assert-TerraformHasSuccessCode("terraform plan failed")

    Write-DarkYellowMessage "terraform output"
    terraform -chdir="$tfDir" output

    if($LASTEXITCODE -eq 2) { # diff has changes        
        Submit-PromptUntilYesOrNoInput "Apply terraform plan [y/n]?" {
            Write-DarkYellowMessage "terraform apply"
            terraform -chdir="$tfDir" apply "$tfPlan" -var="location=$infra_location"
        
            Assert-TerraformHasSuccessCode("terraform apply failed")  
        }
    }      
    # exit code == 0 -- diff has no changes      
}

# func app deployment
Submit-PromptUntilYesOrNoInput "Deply function apps [y/n]?" {
    # azure functions deployment
    $functionAppProjectPath = "src/apps/AzureCommunicationServices/Functions"
    $publishPath = "func-apps"
    $zipFile = "fa-publish.zip"
    $terraform_output = Get-TerraformOutputs
    $resourceGroupName = $terraform_output.resource_group_name.value
    $functionAppName = $terraform_output.function_app_name.value

    Clear-PublishFiles $publishPath $zipFile

    Write-DarkYellowMessage "dotnet publish"
    # build and publish solution to local path
    dotnet publish $functionAppProjectPath `
                -c Release `
                -o $publishPath `
                --os "linux"    
    
    Assert-GenericSuccessCode "dotnet publish failed"

    # create zip for publishing
    Get-ChildItem $publishPath | Compress-Archive -CompressionLevel "Fastest" -DestinationPath $zipFile

    Write-DarkYellowMessage "az functionapp deployment source config-zip | '$functionAppName'"
    az functionapp deployment source config-zip `
                    --resource-group $resourceGroupName `
                    --name $functionAppName `
                    --src $zipFile

    # if deployment succeded clean up
    # otherwise leave files for inspection
    if($LASTEXITCODE -eq 0) {
        Clear-PublishFiles $publishPath $zipFile
    }
}

# logic app deployment
Submit-PromptUntilYesOrNoInput "Deploy logic apps [y/n]?" {
    $logicAppProjectPath = "src/apps/AzureCommunicationServices/Logic-Apps"
    $zipFile = "la-publish.zip"
    $terraform_output = Get-TerraformOutputs
    $resourceGroupName = $terraform_output.resource_group_name.value
    $logicAppName = $terraform_output.logic_app_name.value

    Clear-PublishFiles $publishPath $zipFile

    # create zip for publishing
    Get-ChildItem $logicAppProjectPath | Compress-Archive -CompressionLevel "Fastest" -DestinationPath $zipFile

    Write-DarkYellowMessage "az logicapp  deployment source config-zip |'$logicAppName'"
    az logicapp  deployment source config-zip `
                    --resource-group $resourceGroupName `
                    --name $logicAppName `
                    --src $zipFile

    # if deployment succeded clean up
    # otherwise leave files for inspection
    if($LASTEXITCODE -eq 0) {
        Clear-PublishFiles $publishPath $zipFile
    }
}