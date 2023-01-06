---
title: Azure Communication Services - Email
description: This document describes how to deploy an instance of Azure Communication Service with Email Services and how to handle email status events and engagements.  
author: RCG
date: 12/30/2022
---

# About

The primary resource we are demonstrating in this demo is Azure Communication Services Email (ACSe); which is capabable of high transactional, bulk, and marketing emails to enable Application-to-Person (A2P) use cases.  Along with ACSe we will use the several other Azure resource to simulate an automated email transmission system. The following resources are utlized:

- Azure Communicaiton Services
- Event Grid System Topic
- Service Bus
- Logic Apps
- Function Apps
- Table Storage

## Architecture

Figure 1.0 illustrates the architecture of the messaging system. There are two external connections the system works with; Microsoft Teams and PowerApps.  The Microsoft Teams connection is used to post messages to a channel to inform users of a failed email deliery, this can be expanded to include more details based on the orignal email data stored in the table and data retrieved from the email status event.  An 