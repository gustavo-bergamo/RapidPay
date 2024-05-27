# RapidPay code challenge

This project is a challenge proposed by RapidPay organization.
DO NOT use this code on production environment

## Main features

- Authenticate user
- Create creditcard for logged user
- Create a purchase from pre-defined products
- Get creditcard balance

## Security

- Non authenticated users cannot access endpoints
- Creditcard should be stored encrypted and cannot be recovered without a mask
- Not authorized users cannot create creditcards
- Creditcard balance just can be seen by the creditcard owner

## Tech Stack

This solution has been implemented in .NET 8 with Microsoft Identity using Visual Studio 2022, SQL Server database, Entity Framework Core ORM.

### Third party libraries

- Swagger
  - Facilitates the evaluator to excute API calls
- Automapper
  - Map Request/Responses objects into database Entities
- MediatR
  - To help mediator pattern implementation

## Run Project

### Pre-requisits

- .NET 8 sdk
- SQL Server

### Step-by-step

- Make sure you SQL Server instance is accepting windows authentication
- Open to file `\RapidPay.API\appsettings.json` and change the connection string according your needs
- Open command prompt on solution folder where you checkout it
- Make sure you have EFTools installed on your system running: `dotnet tool install --global dotnet-ef`
- If you already have it, run the command `dotnet tool update --global dotnet-ef` to update it
- Run command `dotnet ef database update` to create database via ef core migrations
- Check on your database instance if a database named `rapidpay-local` has been created.
- Run the following commands to build and compile the applicatin
  -- `dotnet restore`
  -- `dotnet build -c Release`
  -- `dotnet publish -c Release -o 'release'`
- Go to created release folder `cd release`
- Run command to start application `dotnet RapidPay.API.dll`

# Test application

- Click [here](https://localhost:5000/) to access the application running locally
- You can use demo user to get access JWT token calling POST Authentication endpoint with following credentials:
  - User: `demo@rapidpay.com`
  - Password: `demo`
- On response field, copy the content of Token property
- Click on Authorize button (top right) and add the following value: `Bearer {jwt-token}`
  - Where jwt-token is token copied from previous step
- Now you have 30 minutes to test application before this token to be expired
