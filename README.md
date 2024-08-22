# dotnet-webapi-integration-testing-with-testcontainers

[![License](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://github.com/rufer7/dotnet-webapi-integration-testing-with-testcontainers/blob/main/LICENSE)

## About

This repository contains the source code of my presentation at [.NET Day Switzerland 2024](https://sessionize.com/app/speaker/session/674028).

- **Title**: Mastering Integration Testing for .NET Web APIs with WebApplicationFactory and TestContainers
- **Teaser**: Reduce the fear of making changes to the code to a minimum and at the same time detect bugs early in the development process. This can be achieved by implementing integration tests. With the help of the WebApplicationFactory and the use of the Testcontainers library, .NET Web APIs including common 3rd party dependencies like SQL database, Redis cache and many more can be tested end-to-end and including authorization. In this session you will learn how to do this.

## Getting Started

### Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/)
- .NET 8 SDK
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Azure tenant with permissions to create app registrations

### Running the Application / Tests

1. Clone this GitHub repository
1. Open the solution (`src\ArbitraryApp.sln`) in Visual Studio or Visual Studio Code
1. Create app registration in azure tenant (see [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app))
1. Update the `appsettings.json` file in the `ArbitraryApp.Server` project with the app registration details

   IMPORTANT: store `ClientSecret` in managed user secrets

1. Run the application (`ArbitraryApp.Server`) / the tests (`ArbitraryApp.IntegrationTests`) in Visual Studio or Visual Studio Code

## Credits

- [Blazor.BFF.AzureAD.Template](https://github.com/damienbod/Blazor.BFF.AzureAD.Template)

## Useful Links

- [Integration tests in ASP.NET Core (Microsoft docs)](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0)
- [Testcontainers](https://testcontainers.com/)
- [[HOWTO] Implement integration tests for ASP.NET Core Web API with AntiForgery token validation](https://blog.rufer.be/2023/11/13/howto-implement-integration-tests-for-asp-net-core-web-api-with-antiforgery-token-validation/)
- [[FollowUp] Using Testcontainers in integration tests for ASP.NET Core Web API](https://blog.rufer.be/2023/11/29/followup-using-testcontainers-in-integration-tests-for-asp-net-core-web-api/)
- [[NoBrainer] Avoid HTTPS redirection warnings in integration test logs](https://blog.rufer.be/2023/06/21/nobrainer-avoid-https-redirection-warnings-in-integration-test-logs/)
