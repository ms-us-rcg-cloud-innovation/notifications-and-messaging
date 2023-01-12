uniquify              = true
resource_group_name   = "acs-demo-rg"
location              = "eastus2"
acs_name              = "acs-demo"
from_email            = "acs_demo_dnr"
function_app_name     = "acs-send-emails-fa"
logic_app_name        = "acs-email-events-la"
storage_account_name  = "acsdemomsftsa"
service_bus_namespace = "acs-sb-demo"
event_grid_topic_name = "acs-events-topic"

send_email_queue       = "send-email"
email_status_queue     = "email-status-events"
engagement_event_queue = "email-engagement-update-events"

emails_table            = "sendEmails"
email_status_table      = "emailStatus"
engagement_events_table = "engagementEvents"
