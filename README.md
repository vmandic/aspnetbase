# AspNetBase - A monolithic clean layered ASPNET Core template project

This project will serve as a bootstrap template for new Microsoft ASP.NET Core 2.0+ (monolithic enterprise) web projects. It includes a (clean) layered architecture approach which splits the code in four organizational layers: Common, Core, Infrastructure and Presentation.

- **Common layer** handles common and shared business concernes such as utility and helper code.

- **Core layer** handles core business logic such as specifying business contratcs as interfaces, their implementation as providers and other business wise logic such as dependency management through a composition project.

- **Infrastructure** layer handles all persistance and storage concerns such as database connections, migrations and data access or file storage access.

- **Presentation layer** handles exposing other layers through a web or service application interface, also handles all of the configuration logic needed for startuping a web or api application thus represents the system entry point.

A separate task scheduling service (implemented with `Hangfire`) is planned to be included as a separate web api app in the presentation layer also.

In general each app will query for its own views (if needed) and JSON on its own domain. The Task runner app should be invokable / accessible to all the other presentation layer apps. SPA should hanlde complex UI/UX scenarios where MVC should be always favor for all of the "forms over data" scenarios which is a common pattern in enterprise applications.

## Features

- [x] MVC ASP.NET Core app (serves standard Razor views and Razor pages, one should favor this over SPA, controllers should not favor AJAX)
  - [x] ASP.NET Core localization initialized with a default Resource, file in `en-US` lang and culture, also supported for testing `hr-HR` is added
  - [x] uses a global / injected `@Localizer` in Razor files
  - [x] localized with single SharedResources.resx
  - [x] initialized through Startup's `ConfigureServices()` and `Configure()` methods where all logic is extracted to extensions methods
  - [x] static files are bundled and minified with `gulp.js`
  - [x] uses `Bootstrap 4` for the default html layout, bundled in.
  - [x] comes with jquery, jquery-validation and jquery-validation-unobtrusive to support ASPNET's `<form>` model validation with property attributes
  - [x] has AMD JS modules enabled with `require.js`, has a dedicated `require.config.js` file
  - [x] has `TypeScript@3.333` enabled for static JavaScript source which is hooked up with `require.js` module loader, TS is configured for most strict and restrictive style, tslib.js import helpers enabled
  - [x] uses [mvc6 grid](http://mvc6-grid.azurewebsites.net) library (which has fantastic docs and examples) for quick Razor grid / list construction
  - [x] default auth through ASPNET Identity cookies
  - [x] base Razor PageModel, base MVC controller, though no valuable logic in them
  - [x] default user and roles management simple UI implemented, more details below under ASPNET Identity implementation details
  - [x] current (logged in) user account settings management UI
- [x] React/TS SPA WebAPI (serves static content and data through JSON endpoints (AJAX only controllers), develop complex UI's here when not handy in MVC app)
  - [x] client is built with webpack, TypeScript and React
  - [ ] client auths with API through bearer JWT
  - [ ] Swagger API enabled
- [ ] WebAPI app for Task Scheduling (here you long running jobs, should not act as dedicated que service) via Hangfire service
  - [ ] auths with MVC app cookie and/or SPA app JWT bearer
  - [ ] uses default `~/hangfire` route
  - [ ] Tasks are implemented under a dedicated project in Core layer
- [x] uses SQLite by default setting for quicker testing and development, but can switch over to MSSQL
- [ ] EF auditing (with `Audit.NET` or some other library?) to log all DB changes in a controlled manner
- [x] a business service "layer" design is used. There are two projects in Core layer per presentation application. One for interfaces (`Core.AppName.Contracts`) and one for implementation (`Core.AppName.Providers`). This can be extended to support CQRS (over `MediatR` e.g.) if desired but this is the minimal effort taken to make it layered, clean and organized to result in maintainable codebase
- [x] centralized project settings, i.e. application settings setup from config file and other sources (invoked in `ConfigureServices()`) in Core layer. Settings injected as singletons, not using IOptions pattern but rather a Locator pattern if not availalbe through DI (during Startup.cs init)
- [x] default Email service through a Gmail SMTP e.g.
- [x] Error / unhandled exception catching with `ElmahCore`, logs to XML log files by default, uses default `~/elmah` route, files saved under `~/wwwroot`
- [x] static file serving (~/wwwroot, which has a generated dist directory, by gulp and TypeScript), https, hsts
- [x] IoC with Microsoft Dependency Service
  - [x] dedicated composition project in Core layer
  - [x] IoC injection through attributes, allows injection style specification like `LifetimeScope`, contract type, injection type like instant (default), func, lazy, or all styles
  - [x] central registration of attributed (uses `[RegisterDependency(...)]` attribute on services to track types for DI) injected services, this is a quick way to inject services without using the `services.Add*()` syntax
- [x] Logging enhanced with Serilog
  - [x] application bootstrapping process is logged, how long does DI and config settings process take and which types are registered and how
  - [x] sink to log rolling file output
  - [x] sink to colorized console output
  - [x] a HTTP Context enhancer is added to trace all HTTP requests and details
- [x] Entity Framework Core, `IdentityDbContext`, simple UoW with `AfterCommit` event
  - [x] dedicated db migrations project, presetted for Sqlite, can be toggled via configuration options to use MSSQL
  - [x] dedicated db seed project, seeders (dedicated seed classes) registered to IoC, initial seeders for test users and roles are added
  - [x] uses EF code first database migrations
  - [x] uses an `IEntityBase` interface on all CF entities, contract requires `int ID` and `guid Uid` properties
  - [x] handles entity configuration in `AppDbContext` which inherits from `IdentityDbContext`
  - [x] has `DesignTimeDbContext` implemented for db migrations
  - [x] dedicated project for database initialization, runs migrations and seed on App startup
- [x] ASP.NET Identity implemented with using default `UserManager`, `RoleManager`, `SignInManager`, logic moved from autogenerated Razor pages to separate Core handling services designed to be invoked in Presentation Razor pages
  - [x] uses Identity default options, can be configured in extension methods for Identity configuration under web app startup.cs
  - [x] implements custom `IdentityUser`, `IdentityRole`, `IdentityUserRole`, `IdentityUserLogin`, `IdentityUserClaim`, `IdentityUserToken` and `IdentityRoleClaim` with App* prefix, resulting in models like `AppUser`, `AppRole` etc
  - [x] uses extedend `AppUserClaimsPrincipalFactory` inheriting from `UserCalimsPrincipalFactory<AppUser, AppRole>` which can enhance existing default User Claims saved to user auth cookie or JWT, currently enhanced with unique ID GUID
  - [x] supports default Authorization for Identity with the default `AuthroizeAttribute`, supports Role Authorization through signed in user's role claim check (uses default framework's `User.IsInRole()` method)
  - [x] uses the EF Core implemented UserStore and RoleStore
  - [x] user registration
  - [x] external login register (if providers added, none by default)
  - [x] confirm email
    - [x] email service for sending confirmtion emails
  - [x] user account and auth management
  - [x] update user account data
  - [x] change password
  - [x] set password (new account from external login)
  - [x] enable and disable authentication (2fa)
  - [x] download and delete personal data (GDPR)
  - [x] manage external logins
  - [x] generate recovery codes
  - [x] user login (Cookie auth schemes)
  - [x] login with username & password
  - [x] login with 2fa (if configured)
  - [x] login with recovery code (if 2fa auth device not available, if configured)
  - [x] external login (if configured)
  - [x] forgot password
    - [x] email service for sending confirmtion emails
  - [x] reset password
    - [x] email service for sending confirmtion emails
  - [x] admininstrator user/role management section
    - [x] manage users and their roles: add (i.e. register by admin), edit (roles and basic info)

## Solution Structure / Projects

- [x] src
  - Common
    - [x] Utils
  - Core
    - [x] Composition
    - [x] Contracts
    - [x] App.Models (FormModels, ViewModels, Dtos)
    - [ ] SPA.Models (FormModels, ViewModels, Dtos)
    - [x] Providers (implements Contracts)
    - [ ] Tasks (accessed by TaskRunner)
    - [x] Settings (appsettings.json injection as singletons)
  - Infrastructure
    - [x] DataAccess
    - [x] DbMigrations
    - [x] DbSeeds
    - [X] DbInitializer (runs DbMigrations and DbSeeds)
  - Presentation
    - [x] App (core, entry application, MVC favoring Razor)
    - [ ] Spa (serves dedicated SPA which is webpack bundled with npm into App's ~/wwwroot, needs separate host to serve JSON endpoints)
    - [ ] TaskRunner (via webapi /w Hangfire, accessible only to the previous three apps, auths through cookie or JWT)
- [x] tests (with xUnit and custom category traits)
  - [x] base project (custom traits and helpers)
  - [x] unit (5%, started)
  - [ ] integration
    - [ ] Db (teardown per test or tests, SQLite file DB provider)
    - [ ] App (TestServer)
    - [ ] Spa (TestServer)
    - [ ] TaskRunner (TestServer)
  - [ ] e2e (very probably Selenium C#, or just maybe Puppeteer)

## Features to take in consideration

- [ ] a renaming utility to rename all of the AspNetBase strings to a new project name
- [ ] dockerize, add docker-compose to lunch presentation apps, possibly test with mssql container also
- [ ] External API as Presentation layer app (web service, web api) for domain external access of third party services
  - [ ] auths through bearer JWT, client ID and client secret
  - [ ] Swagger enabled API
- [ ] request throtlling (configurable, on Presentation apps)
- [ ] `FluentValidation` for all validation purposes
- [ ] SMS service, a `Twilio` provider implementation
- [ ] JWT token issuer and refresher (when not using Identity cookies) side by side with Identity's authentication in App
- [ ] *Feature Folders* instead of default MVC, Razor pages solves this though, plus Areas help out a bit
- [ ] `MediatR` mediator communication pattern / layer set up with base Response and Request and Handlers
  - [ ]  `CQRS` for data access (as opposed to Uow and DbSet Data Access from Core/Provider layer) over MediatR
- [ ]  add IdentityServer presentation app for central authentication purposes and to enable an OAuth protocol for the external API with valid clients and grants

## Why (another template)

This is a dear brainchild of mine. One which I started thinking about like every new developer has at the start of a carrer when first confronted with a large enterprise app. And then asking myself: "How would I plan this out?". Basically that's it. So, I just started out one day writting this piece by piece. I guess it is a never ending project.

## Contribution

Please open issues for any ideas or change requests you might have. Thank you.
