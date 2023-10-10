---
title: Notification Hubs
description: This asset contains samples, guidance, and selection help for the many azure notification and messaging resources
author: RCG
date: 11/15/2022
---

## About

In this repository you'll find assets demonstrating the different messaging capabilities offered by Azure.  The asset is designed in such a way that it attempts to demonstrate each resources capabilities for a specific use case leveraging several Azure technologies to achieve a lean communication pathway.

## Contents

In this project we will deploy:

- Infrastructure for sending notifications and messages
- Services powered by Azure Functions
- .NET Maui Android app for receiving notifications and messages

## Prerequisites

- Azure Subscription
- Visual Studio
- Firebase project
  - [Getting started with Firebase](https://cloud.google.com/firestore/docs/client/get-firebase)
- Android Device or Emulator using API 29+
  - [Android Emulator Setup](https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/)
- Terraform

## Project Components

- Infrastructure
  - Terraform Definitions
    - Notificaiont Hub
    - Azure Functions on Linux
    - Azure Communication Services
    - Email Communication Services
    - Event Grid
    - Logic App. Standard
    - Service Bus
    - Storage Account

- Clients
  - Maui Android App

- CI/CD
  - Azure Functions deployment workflow

## Services Selection

Use the decistion tree below to determine the type of implementation you need. Do you require async. or sync messaging? Do you need notifications instead?

![services-decision-tree](docs/media/decision-tree.png)

## Architectures

### [Notification Hubs](/docs/notification-hubs/nh-readme.md)

![notification-hubs](docs/media/notification-hub.jpg)

### [Azure Communication Services](/docs/azure-communication-services/acs-readme.md)

![acs](docs/media/acs-email-transmit-serverless.jpg)