# message-board-backend
This application is the backend for a practice project. The application is a message board which allows users to register and login, to create and view topics and write messages to discuss the topics with other users. The project includes a pipeline which builds and deploys the app to azure. The app api can be found hosted on Azure at https://jason-message-board-backend.azurewebsites.net

[The frontend of the project is located here](https://github.com/solita-jasona/practice-message-board-frontend)

## Tech Stack
C# 
ASP.NET 6
Azure SQL server

## Requirements 
- .NET SDK v6.0.301

## Project setup
Clone the repo, then install dependencies:
```
dotnet restore
```
The default database connection is set in the appsettings.json file. If server set up is required, the initial set up can be done with the following command:
```
dotnet ef database update
```