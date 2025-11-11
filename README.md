# Star Wars Fleet Intel

## Project Overview
This will be a .NET backend that consumes SWAPI data and serves it to Angular frontend, showcasing Clean Architecture, design patterns, and modern .NET practices.

## Setup Instructions
you can skip the backend setup and just run the frontend because the backend project is deployed on a server
but if you choose to setup the backend too, then don't forget to change the apiURL in 
environment.development.ts from 'http://swapi.runasp.net/api' to https://localhost:7205/api

### Backend Setup 
1. **Clone the repository**
```bash
git clone https://github.com/not-midozayn/StarWarsFleetIntel.git
cd backend
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Run tests** 
```bash
dotnet test
```

4. **Run the API**
```bash
cd StarWarsFleetIntel.API
dotnet run
```
The API will be available at `https://localhost:7205` (or the port shown in console).

### Frontend Setup

1. **Navigate to frontend directory**
```bash
cd frontend-angular
```

1. **Install dependencies**
```bash
npm install
```

1. **Run tests**
```bash
ng test
# or
npm test
```

1. **Start development server**
```bash
ng serve
# or
npm start
```

1. **Access the application**
```
Open your browser and navigate to `http://localhost:4200`
```


### Why Clean Architecture?

1. **Separation of Concerns**: Clear boundaries between business logic, infrastructure, and presentation layers
2. **Testability**: Business logic is isolated and easily testable without external dependencies
3. **Independence**: Core business logic doesn't depend on frameworks, databases, or UI
4. **Maintainability**: Changes in one layer don't ripple through the entire application
5. **Scalability**: Easy to add new features without affecting existing code
6. **Industry Standard**: Widely adopted architecture pattern in enterprise applications, making the codebase familiar to other developers and aligning with modern job market demands
   
## Key Features

### Clean Architecture
- 4-layer architecture with clear separation of concerns
- Domain-driven design principles

### Design Patterns
- **Chain of Responsibility** for validation pipelines
- **Strategy Pattern** for currency conversion
- **Decorator Pattern** for dynamic feature addition (ship modifications in our case)
- **Factory Pattern** for object creation
- **Repository Pattern** for data access abstraction
(SWAPI is a REST API, thats why the ISwapiClient interface acts as a repository abstraction.)
-**Result Pattern** - Error Handling
Consistent error handling with Result<T> wrapper for success/failure states.


### Modern .NET Practices
- **MediatR** for CQRS implementation
- **FluentValidation** for request validation
- **AutoMapper** for object mapping
- **Serilog** for structured logging with correlation IDs
- **Polly** for resilience (retry, circuit breaker)
- **OpenTelemetry** for distributed tracing

### Error Handling
- Global exception middleware
- Consistent error response format
- Proper HTTP status codes
- Detailed logging

### Cross-Cutting Concerns
- Correlation ID tracking across requests
- Structured logging with BeginScope
- Caching with IMemoryCache
- CORS configuration for frontends


## Structure

```
.
├── backend/    # .NET API
├── frontend-angular/   # Angular Web App
└── frontend-flutter/     # Flutter frontend-flutter App
```
## Phase 1: Backend Structure Setup

### Step 1.1: Create Solution Structure
```bash
cd backend
dotnet new sln -n StarWarsFleetIntel

# Create projects
dotnet new webapi -n StarWarsFleetIntel.API
dotnet new classlib -n StarWarsFleetIntel.Application
dotnet new classlib -n StarWarsFleetIntel.Domain
dotnet new classlib -n StarWarsFleetIntel.Infrastructure
dotnet new xunit -n StarWarsFleetIntel.Tests

# Add projects to solution
dotnet sln add StarWarsFleetIntel.API/StarWarsFleetIntel.API.csproj
dotnet sln add StarWarsFleetIntel.Application/StarWarsFleetIntel.Application.csproj
dotnet sln add StarWarsFleetIntel.Domain/StarWarsFleetIntel.Domain.csproj
dotnet sln add StarWarsFleetIntel.Infrastructure/StarWarsFleetIntel.Infrastructure.csproj
dotnet sln add StarWarsFleetIntel.Tests/StarWarsFleetIntel.Tests.csproj

# Set up project references
cd StarWarsFleetIntel.API
dotnet add reference ../StarWarsFleetIntel.Application/StarWarsFleetIntel.Application.csproj
dotnet add reference ../StarWarsFleetIntel.Infrastructure/StarWarsFleetIntel.Infrastructure.csproj

cd ../StarWarsFleetIntel.Application
dotnet add reference ../StarWarsFleetIntel.Domain/StarWarsFleetIntel.Domain.csproj

cd ../StarWarsFleetIntel.Infrastructure
dotnet add reference ../StarWarsFleetIntel.Domain/StarWarsFleetIntel.Domain.csproj
dotnet add reference ../StarWarsFleetIntel.Application/StarWarsFleetIntel.Application.csproj

cd ../StarWarsFleetIntel.Tests
dotnet add reference ../StarWarsFleetIntel.API/StarWarsFleetIntel.API.csproj
dotnet add reference ../StarWarsFleetIntel.Application/StarWarsFleetIntel.Application.csproj
dotnet add reference ../StarWarsFleetIntel.Infrastructure/StarWarsFleetIntel.Infrastructure.csproj
```

### Step 1.2: Install Required NuGet Packages

**API Project:**
```bash
cd StarWarsFleetIntel.API
dotnet add package MediatR
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package FluentValidation.AspNetCore
dotnet add package AutoMapper
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Swashbuckle.AspNetCore
dotnet add package OpenTelemetry.Exporter.Console
dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Instrumentation.AspNetCore
dotnet add package OpenTelemetry.Instrumentation.Http
```

**Application Project:**
```bash
cd ../StarWarsFleetIntel.Application
dotnet add package MediatR
dotnet add package AutoMapper
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
dotnet add package Microsoft.Extensions.Logging.Abstractions
```

**Infrastructure Project:**
```bash
cd ../StarWarsFleetIntel.Infrastructure
dotnet add package Microsoft.Extensions.Http.Polly
dotnet add package Polly
dotnet add package Microsoft.Extensions.Caching.Memory
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
```

**Test Project:**
```bash
cd ../StarWarsFleetIntel.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package AutoFixture
dotnet add package AutoFixture.Xunit2
dotnet add package Bogus
dotnet add package Microsoft.Extensions.Logging.Abstractions
```
## Phase 2: Domain Layer

### Step 2.1: Create Domain Entities
**Domain/Entities/Starship.cs:**
**Domain/Entities/PreFlightCheckResult.cs:**

### Step 2.2: Create Domain Enums

**Domain/Enums/Currency.cs:**
**Domain/Enums/ModificationType.cs:**

## Phase 3: Application Layer

### Step 3.1: Create Common Infrastructure
**Application/Common/Wrappers/Result.cs (Result Pattern):**
**Application/Common/Wrappers/PaginatedResult.cs:**

### Step 3.2: Create Application Interfaces
**Application/Interfaces/ISwapiClient.cs:**
**Application/Interfaces/IPreFlightCheckHandler.cs (Chain of Responsibility):**
**Application/Interfaces/ICurrencyConverter.cs (Strategy Pattern):**
**Application/Interfaces/IStarshipDecoratorFactory.cs (Factory Pattern):**
**Application/Interfaces/IStarshipDecorator.cs:**



### Step 3.3: Create CQRS GetStarshipById Query (MediatR)
**Application/Features/Starships/Queries/GetStarshipById/GetStarshipByIdQuery.cs:**
**Application/Features/Starships/Queries/GetStarshipById/GetStarshipByIdQueryValidator.cs:**
**Application/Features/Starships/Queries/GetStarshipById/GetStarshipByIdQueryHandler.cs:**

### Step 3.4: Create CQRS GetStarships Query (MediatR)
**Application/Features/Starships/Queries/GetStarships/GetStarshipsQuery.cs:**
**Application/Features/Starships/Queries/GetStarships/GetStarshipsQueryValidator.cs:**
**Application/Features/Starships/Queries/GetStarships/GetStarshipsHandler.cs:**

### Step 3.5: Create a  GetStarshipResponse for both queries
**Application/Features/Starships/Queries/GetStarshipResponse.cs:**

### Step 3.6: Create AutoMapper Profiles
**Application/Mappings/StarshipMapping/StarShipMappingProfile.cs:**

**Application/Mappings/StarshipMapping/Queries/PreFlightChecksMapping.cs:**
**Application/Mappings/StarshipMapping/Queries/StarshipMapping.cs:**


### Step 3.7: Extentions (Add MediatR, AutoMapper, Validators Settings)
**Application/DependencyInjection.cs:**



## Phase 4: Infrastructure Layer

### Step 4.1: Implement Typed HTTP Client
**Infrastructure/ExternalServices/SwapiClient.cs:**

### Step 4.2: Implement Chain of Responsibility Pattern
**Infrastructure/PreFlightChecks/BasePreFlightCheckHandler.cs:**
**Infrastructure/PreFlightChecks/CrewCapacityCheckHandler.cs:**
**Infrastructure/PreFlightChecks/HyperdriveCheckHandler.cs:**
**Infrastructure/PreFlightChecks/ConsumablesCheckHandler.cs:**

### Step 4.3: Implement Strategy Pattern
**Infrastructure/Services/CurrencyConverter.cs:**
**Infrastructure/Services/CurrencyStrategies/ICurrencyConversionStrategy.cs:**
**Infrastructure/Services/CurrencyStrategies/ImperialCreditsStrategy.cs:**
**Infrastructure/Services/CurrencyStrategies/GalacticCreditsStrategy.cs:**
**Infrastructure/Services/CurrencyStrategies/PeggatsCreditsStrategy.cs:**
**Infrastructure/Services/CurrencyStrategies/TrugutsCreditsStrategy.cs:**
**Infrastructure/Services/CurrencyStrategies/WupiupiCreditsStrategy.cs:**

### Step 4.4: Implement Decorator Pattern
**Infrastructure/Decorators/StarshipDecoratorFactory.cs (Decorator Pattern):**
**Infrastructure/Decorators/WeaponUpgradeDecorator.cs:**
**Infrastructure/Decorators/ArmorPlatingDecorator.cs:**
**Infrastructure/Decorators/ShieldEnhancementDecorator.cs:**
**Infrastructure/Decorators/StealthSystemDecorator.cs:**
**Infrastructure/Decorators/EngineBoostDecorator.cs:**

### Step 4.5: Configurations (Cache Settings + Swapi Settings)
**Infrastructure/Configuration/CacheSettings:**
**Infrastructure/Configuration/SwapiSettings.cs:**

### Step 4.5: Configurations (Cache Settings + Swapi Settings)
**Infrastructure/DependencyInjection.cs:**


## Phase 5: API Layer

### Step 5.1: Configure Program.cs with dependancies
**API/Program.cs:**

### Step 5.2: Create Middleware
**API/Middleware/ExceptionHandlingMiddleware.cs:**
**API/Middleware/CorrelationIdMiddleware.cs:**

### Step 5.3: Create Controllers
**API/Controllers/StarshipsController.cs:**

### Step 5.4: Configure appsettings.json
**API/appsettings.json:**

### Step 5.5: Configurations (Cors Settings)
**API/Configuration/CorsSettings.cs:**

### Step 5.6: Extentions (Cors Settings)
**API/Extentions/ServiceCollectionExtensions.cs:**
**API/Extentions/WebApplicationExtensions.cs:**



---

## Phase 6.0 : frontend-angular (Angular)
### Project Structure (7-1 SASS Pattern)

```
src/
├── app/
│   ├── core/
│   │   ├── interceptors/
│   │   │   ├── correlation-id-interceptor.ts
│   │   │   ├── error-handler-interceptor.ts
│   │   │   └── loading-interceptor.ts
│   │   ├── models/
│   │   │   ├── response.models.ts
│   │   │   └── starship.model.ts
│   │   ├── services/
│   │   │   ├── loading.ts
│   │   │   ├── starship-api.ts
│   │   │   └── toast.ts
│   │   └── stores/
│   │       └── starship.store.ts
│   ├── features/
│   │   ├── starship-details-component/
│   │   └── starship-list-component/
│   │       ├── components/
│   │       │   └── starship-card-component/
│   │       ├── starship-list-component.html
│   │       ├── starship-list-component.scss
│   │       └── starship-list-component.ts
│   ├── layout/
│   └── shared/
│       └── components/
│           ├── loading-spinner-component/
│           ├── pagination-component/
│           ├── search-bar-component/
│           └── toast-component/
├── environments/
│   └── environment.development.ts
│
└── styles/
    ├── abstracts/
    │   ├── _variables.scss
    │   ├── _index.scss
    │   ├── _mixins.scss
    │   └── _functions.scss
    ├── base/
    │   ├── _reset.scss
    │   └── _typography.scss
    │   └── _index.scss
    ├── components/
    │   ├── _buttons.scss
    │   └── _cards.scss
    ├── layout/
    │   ├── _header.scss
    │   └── _grid.scss
    ├── pages/
    │   ├── _list.scss
    │   └── _detail.scss
    ├── themes/
    │   └── _default.scss
    ├── vendors/
    └── main.scss
```

# Angular CLI Commands to Generate Project Structure

## Core - Interceptors
```bash
ng generate interceptor core/interceptors/correlation-id
ng generate interceptor core/interceptors/error-handler
ng generate interceptor core/interceptors/loading
```

## Core - Models
```bash
# Models are typically created manually as interfaces/classes
# Create files manually:
# - src/app/core/models/response.models.ts
# - src/app/core/models/starship.model.ts
```

## Core - Services
```bash
ng generate service core/services/loading
ng generate service core/services/starship-api
ng generate service core/services/toast
```

## Core - Stores
```bash
# Stores are typically created manually
# Create file manually:
# - src/app/core/stores/starship.store.ts
# - src/app/core/stores/starship.store.spec.ts
```

## Features - Starship List
```bash
ng generate component features/starship-list-component
ng generate component features/starship-list-component/components/starship-card-component
```

## Features - Starship Details
```bash
ng generate component features/starship-details-component
```

## Features - Layout
```bash
ng generate component features/layout
```

## Shared - Components
```bash
ng generate component shared/components/loading-spinner-component
ng generate component shared/components/pagination-component
ng generate component shared/components/search-bar-component
ng generate component shared/components/toast-component
```

## Notes
- All `ng generate` commands automatically create `.spec.ts` test files
- Models and stores need to be created manually with their test files
- Interceptors are created with `ng generate interceptor` and include test files
