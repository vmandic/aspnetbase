# AspNetBase - an ASP.NET Core application bootstrap template

This project serves as a bootstrap template for new Microsoft ASP.NET Core 2.0+ web projects. It includes a layered architecture approach which splits the code in four organizational layers: Common, Core, Infrastructure and Presentation.

- **Common layer** handles common and shared business concernes such as utility and helper code.

- **Core layer** handles core business logic such as specifying business contratcs as interfaces, their implementation as providers and other business wise logic such as dependency management through a composition project.

- **Infrastructure** layer handles all persistance and storage concerns such as database connections, migrations and data access or file storage access.

- **Presentation layer** handles exposing other layers through a web or service application interface, also handles all of the configuration logic needed for startuping a web or api application thus represents the system entry point.

A separate task scheduling service (implemented with `Hangfire`) is planned to be included as a separate web api app in the presentation layer also.

In general each app will query for its own views (if needed) and JSON on its own domain. The Task runner app should be invokable / accessible to all the other three presentation layer apps.

## Features

- [ ] request throtlling (on all Presentation apps)
- [ ] ASP.NET Core localization initialized with a default Resource files (MVC and SPA)
- [ ] Web API app #1 for SPA (serves static content built with Webpack, and data through JSON endpoints)
  - [ ] auth through bearer JWT
  - [ ] Swagger installed
- [ ] Web (Service) API app #2 for external access
  - [ ] auth through bearer JWT
  - [ ] Swagger installed
- [ ] Web Api app #3 for Task Scheduling via Hangfire
  - [ ] auths with existing schemes from other apps
  - [ ] uses default ~/hangfire route
  - [ ] Tasks are implemented under a dedicated project in Core layer
- [ ] ? (overengineerning?) MediatR set up with base Response and Request and Handlers
  - [ ] ? `CQRS` (as opposed to Uow and DbSet Data Access from Core/Provider layer) over MediatR
- [ ] Feature Folders instead default MVC
- [ ] Quick and efficent "Forms over Data" in MVC with Razor and Razor Pages, validation, model state validation etc.
  - [ ] has example pages and controls set up
  - [ ] uses a templated approach for constructing grid controls that are highly customizable (DataTables.net, JQGrid, sth else?) in terms of search, filtering, paging etc.
- [ ] `FluentValidation` for all validation purposes
- [ ] default Email service through a Gmail SMTP (designed to have an adaptive provider throgh a common interface)
- [ ] SMS service (provider adaptable as email)
- [ ] Error / unhandled exception catching with `ElmahCore`, logs to database by default, does not send emails by default, uses default [app]/elmah route
- [x] static file serving, https, hsts
- [x] IoC with Microsoft Dependency Service
  - [x] dedicated composition project
  - [x] IoC injection through attributes
  - [x] central registration of attributed injected services
- [x] Logging implemented with Serilog
  - [x] sink to log file output
  - [x] sink to console output
- [x] MVC app for Razor and Pages, controllers serve both HTML views and JSON
  - [x] auth through cookies
  - [ ] ? base Razor PageModel
  - [ ] base controller
- [x] Entity Framework Core, DbContext, simple UoW
  - [x] dedicated db migrations project
  - [x] supports code first database migrations
  - [x] uses an `IEntityBase` interface on all CF entities
  - [x] handles configuration in DbContext
  - [x] has `DesignTimeDbContext` implemented for db migrations
  - [x] dedicated project for handling database seeding, runs migrations and seed on WebApp startup
- [x] ASP.NET Identity implemented with using default `UserManager`, `RoleManager`, `SignInManager`, logic moved from Razor pages to separate Core handling services designed to be invoked in Presentation Razor pages
  - [x] uses Identity default options, can be configured in extension methods for Identity configuration under web app startup.cs
  - [x] implements custom `IdentityUser`, `IdentityRole`, `IdentityUserRole`, `IdentityUserLogin`, `IdentityUserClaim`, `IdentityUserToken` and `IdentityRoleClaim`
  - [x] uses extedend `AppUserClaimsFactory` which can enhance existing default User Claims saved to user auth cookie or JWT, enhanced with unique ID GUID
  - [x] uses the EF Core implemented UserStore and RoleStore
  - [x] user registration
  - [x] external login register (if providers added, none by default)
  - [x] confirm email
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
  - [ ] ? token issuer and refresher (when not using cookies)
  - [x] login with 2fa (if configured)
  - [x] login with recovery code (if 2fa auth device not available, if configured)
  - [x] external login (if configured)
  - [x] forgot password
  - [x] reset password

## Solution Structure / Projects

- [x] src
  - Common
    - [x] Utils
  - Core
    - [x] Composition
    - [x] Contracts
    - [x] Models
    - [x] Providers
    - [ ] Tasks
  - Infrastructure
    - [x] DataAccess
    - [x] DbMigrations
    - [ ] DbSeeder
  - Presentation
    - [x] Server (TODO: rename to WebApp)
    - [ ] WebService (as external webapi)
    - [ ] ? Spa (serves SPA if not from WebApp)
    - [ ] TaskRunner (via webapi /w Hangfire, accessible only to the previous three apps, auths through cookie or JWT)
- [ ] tests
  - [ ] unit
  - [ ] integration
    - [ ] db (teardown per test or tests, in memory DB provider)
    - [ ] WebApp (TestServer)
    - [ ] WebService (TestServer)
    - [ ] TaskRunner (TestServer)
  - [ ] e2e (Selenium)
