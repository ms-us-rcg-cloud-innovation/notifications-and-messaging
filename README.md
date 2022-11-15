# About The Project

In this repository you'll find assets demonstrating the different messaging capabilities offered by Azure.  The asset is designed in such a way that it attempts to demonstrate each resources capabilities for a specific use case

## Contents

In this project we will deploy:

- Infrastructure for sending notifications and messages
- Services powered by Azure Functions
- .NET Maui Android app for receiving notifications and messages

## Prerequisites

- Azure Subscription
- Firebase project
  - [Getting started with Firebase](https://cloud.google.com/firestore/docs/client/get-firebase)
- Android Device or Emulator
  - [Android Emulator Setup](https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/)

## Project Components

- Infrastructure
  - Terraform Definitions
    - Notificaiont Hub
    - Azure Functions
      - App. Service Plan
      - Storage Account

- Services
  - Azure Functions
    - Notification Hub
      - Register Device
      - Deregister Device
      - Send Notification

- Clients
  - Maui Android App

- CI/CD
  - Azure Functions deployment workflow

## Services Selection

Use the decistion tree below to determine the type of implementation you need. Do you require async. or sync messaging? Do you need notifications instead?

![services-decision-tree](docs/media/decision-tree.png)

## Setup

### Notifications using `Notificatin Hub`

This setup will enable Push Notifications to mobile devices using `Notification Hub` as the broker to Firebase.  The Azur resources used will be `Notification Hub` and `Azure Functions`.  For it all to work you will need to make sure you have a project configured in Firebase (see step 4).

1. Infrastructure  
  1.1 Use the provided [Notification Hub Demo defintion](infrastructure/terraform/notification-hub-demo/main.tf)  
  1.2 Use Azure CLI  
    - [Notification Hub](https://learn.microsoft.com/en-us/azure/notification-hubs/create-notification-hub-azure-cli)
    - [Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cin-process#create-supporting-azure-resources-for-your-function)
2. Deploy backend services using [`deploy-functions` workfow](./.github/workflows/deploy-functions.yaml)
3. Add a `static class` at the root of the Maui project called `Local.Constants.cs`  
  3.1 Replace content with code below. Update values wit your function endpoint and token  

  ```cs
    internal class Local_Constants
    {
        public const string NH_REGISTRATION_UPSERT_ENDPOINT = "<function-endpoint-sans-token>";
        public const string NH_REGISTRATION_FUNC_TOKEN = "<function-token>";
    }
  ```

4. Configure Maui Firebase integration  
  3.1 [Import `google-services.json`](https://learn.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm#add-the-google-services-json-file) to the `Platforms/Android` folder

> _Note_
>
> To run backend services locally add the following values to your `local.settings.json` file in the Functions project
>
> ```json
> "NOTIFICATION_HUB_NAME": "<notification-hub-name>",
> "NOTIFICATION_HUB_CS": "<ManagementApiAccessSignature-access-policy-connection-string>"
> ```

## Architecture

### Notification Hub

![notification-hub-arch](docs/media/notification-hub.jpg)

## References

- [Azure Notification Hubs documentation](https://learn.microsoft.com/en-us/azure/notification-hubs/)
- [Create Azure Notification Hub using CLI](https://learn.microsoft.com/en-us/azure/notification-hubs/create-notification-hub-azure-cli)
- [Enterprise push architecture guiadance](https://learn.microsoft.com/en-us/azure/notification-hubs/notification-hubs-enterprise-push-notification-architecture)
- [Choose between Azure messaging services - Event Grid, Event Hubs, Service bus](https://learn.microsoft.com/en-us/azure/event-grid/compare-messaging-services)