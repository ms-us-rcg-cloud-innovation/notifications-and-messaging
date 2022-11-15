# About The Project

In this repository you'll find assets demonstrating the different messaging capabilities offered by Azure.  The asset is designed in such a way that it attempts to cover the most use cases.

## Contents

---

In this project we will deploy:

- Infrastructure for sending messages to desired PNS (firebase in this case)
- APIs powered by Azure Functions to send messages, register devices, and deregister devices

A local .NET Maui Android project for receiving notifications

## Prerequisites

---

- Azure Subscription
- Firebase project
  - [Getting started with Firebase](https://cloud.google.com/firestore/docs/client/get-firebase)
- Android Device or Emulator
  - [Android Emulator Setup](https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/)

## Setup

1. Infrastructure  
  1.1 Use the provided [terraform definitions](./infrastructure/terraform/main.tf)  
  1.2 Use Azure CLI commands
2. Add a `static class` at the root of the Maui project called `Local.Constants.cs`  
  2.1 Replace content with code below -- using your function endpoint and token  

  ```cs
    internal class Local_Constants
    {
        public const string REGISTRATION_UPSERT_ENDPOINT = "<function-endpoint-sans-token>";
        public const string REGISTRATION_FUNC_TOKEN = "<function-token>";
    }

  ```

3. Configure Maui Firebase integration  
  3.1 [Import `google-services.json`](https://learn.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm#add-the-google-services-json-file) to the `Platforms/Android` folder

## Architecture

---

![notification-hub-arch](docs/media/notification-hub.jpg)

## Project Components

- Infrastructure
  - Terraform Definitions
    - Notificaiont Hub
    - Azure Functions
      - App. Service Plan
      - Storage Account

- Services
  - Azure Functions
    - Register Device
    - Deregister Device
    - Send Notification

- Clients
  - Maui App.
    - Android Platform

- CI/CD
  - Azure Functions deployment workflow
  - Infrastructure deployment workflow -- Terraform

## References

- [Azure Notification Hubs documentation](https://learn.microsoft.com/en-us/azure/notification-hubs/)
- [Create Azure Notification Hub using CLI](https://learn.microsoft.com/en-us/azure/notification-hubs/create-notification-hub-azure-cli)
- [Enterprise push architecture guiadance](https://learn.microsoft.com/en-us/azure/notification-hubs/notification-hubs-enterprise-push-notification-architecture)