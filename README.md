# Teashop Backend

WebAPI application working as a backend in Teashop application environment.

## Technologies used

- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) - base software framework
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1) - base backend framework
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM framework
- [MediatR](https://github.com/jbogard/MediatR) - mediator implementation library
- [FluentAssertions](https://fluentvalidation.net/) - validation library
- [xUnit](https://xunit.net/) - testing library
- Moq - mocking library for testing
- FluentAssertions - additional assertion methods for testing
- [Docker](https://www.docker.com/) - containerization

## Usage

This application is a part of Teashop application environment. To see the details of setting up the entire environment, go to [Teashop Ops](https://github.com/lukaszslazyk/teashop-ops) repository.

The application requires a connection to SQL Express database. In order to make the application run correctly, you need to provide a connection string in appsettings.json.

### Local

For the easiest way of running the application, it is advised to open it in Visual Studio and run using its built-in launcher.

If you need to run it using command line, you can use the following .NET Core CLI commands in project directory:
```
dotnet restore
dotnet publish
cd Teashop.Backend\bin\Debug\netcoreapp3.1\publish
dotnet Teashop.Backend.dll
```

The application will run on: [http://localhost:5000](http://localhost:5000)

### Docker container

To setup the application in docker container, first build the container image by running the following command in project directory:
```
docker build -t teashop_backend .
```

Then to run the container:
```
docker run -p 5000:80 teashop_backend
```

## Notes

- The application's architecture is mostly based on ideas and mechanisms from Clean Architecture Solution Template repository: [https://github.com/jasontaylordev/CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture). It follows Clean Architecture principles with Command and Query Responsibility Segregation. I strongly recommend watching an awesome [presentation](https://www.youtube.com/watch?v=dK4Yb6-LxAk) by Jason Taylor, the author of the repository, explaining ideas behind it in detail.