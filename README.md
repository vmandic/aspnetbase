# AspNetBase - a ASP.NET Core bootstrapper

This project serves as a bootstrapping project for new Microsoft ASP.NET Core web application projects.

## Features

The project serves as a base or template which includes the following features:

-   static files enabled and SPA path setup
-   DI and IoC (DryIoC?) bootstrapped
-   Logging bootstrapped
-   MediatR set up with base Response and Request and Handlers
-   Feature Folders isntead default MVC
-   MVC app for Razor and Pages
-   Web API app #1 for SPA
-   Web API app #2 for external access
-   Entity Framework Core bootstrapped
-   ASP.NET Identity implemented with core Business Logic: UserManager, RoleManager, SignInManager...

    -   implements IdentityUser, IdentityRole, IdentityUserRole, IdentityUserLogin, IdentityUserClaim, IdentityUserToken and IdentityRoleClaim
    -   uses the EF Core implemented UserStore and RoleStore

## ASP.NET Identity features (bootstraps the DB also)

-   user registration
-   external login register (if configured)
-   confirm email (if configured)
-   user account and auth management

    -   update user account data
    -   change password
    -   set password (new account from external login)
    -   enable and disable authentication (2fa)
    -   download and delete personal data (GDPR)
    -   manage external logins
    -   generate recovery codes

-   user login (Cookie & Bearer auth schemes)

    -   login with username & password
    -   token issuer and refresher (when not using cookies)
    -   login with 2fa (if configured)
    -   login with recovery code (if 2fa auth device not available, if configured)
    -   external login (if configured)

-   forgot password

    -   reset password

-   supports code first database migrations

## Solution Structure / Projects

TBA
