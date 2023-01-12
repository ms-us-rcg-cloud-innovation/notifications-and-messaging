# This powershell scripts is used to extend capabilities from the azapi provider.
# Currently enough information isn't being returned when creating resources
# to properly setup all downstream resources. We need either connection string for our
# functions to connect to ACS. Finally, to properly send emails via an SDK a from e-mail
# must be provided. The from email in this instance is on the ACS-Domain resource.  Using
# az cli we query the object by its id and resource type to retrieve this information.  
# Finally a custom object is generated with all the required properties. This object 
# is converted to JSON and written to stdout for the terraform external data resource 
# that calls this script


# read input JSON from stdin -- as per terraform docs
$jsonInput =  [Console]::In.ReadLine() | ConvertFrom-Json

$acs_name       = $jsonInput.acs_name
$resource_group = $jsonInput.resource_group
$acs_domain_id  = $jsonInput.domainEmailResourceId
$from_email     = $jsonInput.from_email

# get details on domain requested by id
$domainEmail = (az resource show --ids $acs_domain_id -o json | ConvertFrom-Json)

# get access keys for ACS resource
$accessKeys = (az communication list-key --name $acs_name --resource-group $resource_group -o json | ConvertFrom-Json -Depth 12)

# build object with all the properties we need to bubble up to the terraform data resource
$result = @{
    fromDomain                = $domainEmail.properties.fromSenderDomain
    domainUser                = $from_email
    fromEmail                 = $from_email + "@" + $domainEmail.properties.fromSenderDomain
    primaryKey                = $accessKeys.primaryKey
    secondaryKey              = $accessKeys.secondaryKey
    primaryConnectionString   = $accessKeys.primaryConnectionString
    secondaryConnectionString = $accessKeys.secondaryConnectionString
}

# the external data resource expects content in JSON -- before finalizing 
# execution convert the result object to json and send to stdout
ConvertTo-Json -InputObject $result