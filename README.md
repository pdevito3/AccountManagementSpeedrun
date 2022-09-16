# BankingSystemApi

This project was created with [Craftsman](https://github.com/pdevito3/craftsman).

## Getting Started
### Docker Setup
1. Run `docker-compose up --build` from your `.sln` directory to spin up your database(s) and other supporting
   infrastructure depending on your configuration (e.g. Keycloak, Jaeger, etc.).

### Keycloak Auth Server
1. If using a Keycloak auth server, you'll need to use the scaffolded Pulumi setup or configure it manually (new realm, client, etc).
   1. [Install the pulumi CLI](https://www.pulumi.com/docs/get-started/)
   1. `cd` to your the `AccountManagementKeycloak` project directory
   1. Run `pulumi up` to start the scaffolding process
   1. Create a new stack by pressing `Enter` when prompted and then typing the name of the stack (e.g. `dev`). Alternatively
      you can use the `pulumi stack init` command to make a new stack first.
      > Note: The stack name must match the extension on your yaml config file (e.g. `Pulumi.dev.yaml`) would have a stack of `dev`.
   1. Select yes to apply the configuration to your local Keycloak instance.
   1. Navigate to keycloak client at `localhost:6734/auth` and login with `admin` for username and password to view config (if you want).

### Api
To use the api:
1. Make sure you have the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) installed
2. Make sure you have entity framework installed: `dotnet tool install --global dotnet-ef`
3. Apply migrations
   1. Confirm your environment (`ASPNETCORE_ENVIRONMENT`) is set to `Development` using
      `$Env:ASPNETCORE_ENVIRONMENT = "Development"` for powershell or `export ASPNETCORE_ENVIRONMENT=Development` for bash.
   2. `cd` to the boundary project root (e.g. `cd AccountManagement/src/AccountManagement`)
   3. Run `dotnet ef database update` to apply your migrations.
4. From the `AccountManagement/src/AccountManagement` directory, run the api: `dotnet run`
5. Go to  `https://localhost:5375/swagger/index.html` to use swagger
6. To hit protected endpoints, login with `alice` for username and password or register a new user.
  1. Note that the first user you log in as will be made a super admin and other users won't be able to perform actions unless you give them a super admin role or a user role. If the latter you'll need to add permissions to the `rolepermissions` table.

## Tests
> **Note** 
> The value object broke a most of the associated functional tests and a unit test. TODO investigation

## Running Integration Tests
Make sure you have docker installed to run integration tests.

> ⏳ If you don't have the database image pulled down to your machine, they will take some time on the first run.

### Troubleshooting
- If your entity has foreign keys, you might need to adjust some of your tests after scaffolding to accomodate them.

## Running Migrations
To create a new migration, make sure your environment is set to `Development`:

### Powershell
```powershell
$Env:ASPNETCORE_ENVIRONMENT = "Development"
```

### Bash
```bash
export ASPNETCORE_ENVIRONMENT=Development
```

Then run the following:

```shell
cd YourBoundedContextName/src/YourBoundedContextName
dotnet ef migrations add "MigrationDescription"
```

To apply your migrations to your local db, make sure your database is running in docker run the following:

```bash
cd YourBoundedContextName/src/YourBoundedContextName
dotnet ef database update
```
