# Todo

Todo web app built with [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0),
[ReactJS](https://reactjs.org/) and [SQL Server](https://www.microsoft.com/en-us/sql-server)

<p align="center">
 <a href="#technologies">Technologies</a> •
 <a href="#getting-started">Getting started</a> •
 <a href="#run-with-docker">Run with docker</a> •
</p>

## Technologies

-   [Docker](https://www.docker.com/): Platform to build, run, and share applications with containers.
-   Backend
    -   [ASP NET Core](https://docs.microsoft.com/en-us/aspnet/core/): Cross-platform framework to build web apps
    -   [SQL Server](https://www.microsoft.com/en-us/sql-server): Microsoft Relational Database
    -   [Dapper](https://dapper-tutorial.net/dapper): Micro ORM
    -   Tests
        -   [xUnit](https://xunit.net/): Unit testing tool for the .NET
        -   [TestContainers](https://dotnet.testcontainers.org/): Testcontainers for .NET is a library to support tests with throwaway instances of Docker containers for all compatible .NET Standard versions
-   Frontend
    -   [ReactJS](https://reactjs.org/): JavaScript library for building user interfaces
    -   [Typescript](https://www.typescriptlang.org/): Typed superset of JavaScript
    -   [React Router DOM](https://reacttraining.com/react-router/web/guides/quick-start): Declarative routing for React

# Getting started

### Clone

Clone this repository

```
git clone https://github.com/tmezups/ReactAppToDo.git
```

Then, change to repository folder

```
cd ReactAppToDo
```

### Run with docker

Requires

-   [Docker](https://docs.docker.com/get-docker/)
-   [Docker compose](https://docs.docker.com/compose/install/)

Run the following command to start database, backend and frontend

```
docker-compose up
```

The app will be available at `http://localhost:8080`

To stop the application run

```
docker-compose down
```

