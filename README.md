Overview
ABCRetail is a cloud-based retail management web application built with ASP.NET Core MVC and deployed to Azure App Service.
The system integrates several Azure Storage services to handle different business needs:
Blob Storage → Stores multimedia (e.g., product images, documents).
Queue Storage → Manages asynchronous transaction & inventory messages.
File Storage → Stores logs and files using Azure File Share.
This project was developed to demonstrate integration with Azure services and deployment to a live Azure environment.

Features
User authentication with Microsoft Entra ID (Azure AD).
Upload and retrieve multimedia from Azure Blob Storage.
Store and peek at messages in Azure Queue Storage for transactions/inventory.
Save log files in Azure File Share, retrievable via the web app.

Technologies Used
ASP.NET Core MVC 8.0
Azure Storage SDK:
Azure.Storage.Blobs
Azure.Storage.Queues
Azure.Storage.Files.Shares
Microsoft Entra ID Authentication

To Login
Run Visual Studio
Run the project on VS
Use the details:
Username - admin
password - ChangeMe123!
