# Funfair Airlines - In progress..

Funfair Airlines is a project that aims to create an airline platform where users can design their own airplanes, make flight reservations, and book seats. The architecture of the project is based on Domain-Driven Design (DDD) and Event-Driven Architecture (EDA). It leverages various Azure components, such as Azure Service Bus (ASB) and MSSQL, and the infrastructure is provisioned using Terraform. The configuration details can be found in the `funfair-azure-init` file.

## Business logic

All assumptions and rules are available on [Miro](https://miro.com/app/board/uXjVMJnevlI=/?share_link_id=614941052400) 

## Architecture

The Funfair Airlines project follows the principles of Domain-Driven Design (DDD) and Event-Driven Architecture (EDA). This architectural approach focuses on modeling the business domain and utilizing events to enable communication and react to changes in the system.

The project relies on several Azure components, including:

- .NET 7.0: The app environment
- MSSQL: The chosen database to store airplane data, flight reservations, and seat bookings.
- Docker: Containerization
- Azure: Cloud provider

## Configuration

The project's infrastructure is managed using Terraform, and the configuration can be found in the `funfair-azure-init` file. This file contains the necessary Terraform code to provision and configure the Azure resources required by the Funfair Airlines project.

