# DEPI_Graduation_Project

A car-sharing system built with ASP.NET Core (Web API + MVC) and (HTML, CSS, JS).
It allows Admins, Car Owners, and Participants to register, manage cars, and share rides securely.

## Features

- Role-based access: Admin, CarOwner, Participant

- User authentication with BCrypt + JWT

- Car listing, booking, and approval

- RESTful API + MVC frontend

## Setup
```bash
git clone https://github.com/rahmataysier/DEPI_Graduation_Project.git
cd DEPI_Graduation_Project
dotnet restore
```
Edit the connection string in appsettings.json, then run:
```bash
dotnet ef database update --project CarShareDAL
dotnet run --project CarShareAPI
```
