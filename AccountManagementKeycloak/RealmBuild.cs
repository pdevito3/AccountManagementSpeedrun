namespace AccountManagementKeycloak;

using AccountManagementKeycloak.Extensions;
using AccountManagementKeycloak.Factories;
using Pulumi;
using Pulumi.Keycloak;
using Pulumi.Keycloak.Inputs;

class RealmBuild : Stack
{
    public RealmBuild()
    {
        var realm = new Realm("DevRealm-realm", new RealmArgs
        {
            RealmName = "DevRealm",
            RegistrationAllowed = true,
            ResetPasswordAllowed = true,
            RememberMe = true,
            EditUsernameAllowed = true
        });
        var accountmanagementScope = ScopeFactory.CreateScope(realm.Id, "account_management");
        
        var accountManagementPostmanMachineClient = ClientFactory.CreateClientCredentialsFlowClient(realm.Id,
            "account_management.postman.machine", 
            "974d6f71-d41b-4601-9a7a-a33081f84682", 
            "AccountManagement Postman Machine",
            "https://oauth.pstmn.io");
        accountManagementPostmanMachineClient.ExtendDefaultScopes(accountmanagementScope.Name);
        accountManagementPostmanMachineClient.AddAudienceMapper("account_management");
        
        var accountManagementPostmanCodeClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "account_management.postman.code", 
            "974d6f71-d41b-4601-9a7a-a33081f84680", 
            "AccountManagement Postman Code",
            "https://oauth.pstmn.io",
            redirectUris: null,
            webOrigins: null
            );
        accountManagementPostmanCodeClient.ExtendDefaultScopes(accountmanagementScope.Name);
        accountManagementPostmanCodeClient.AddAudienceMapper("account_management");
        
        var accountManagementSwaggerClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "account_management.swagger", 
            "974d6f71-d41b-4601-9a7a-a33081f80687", 
            "AccountManagement Swagger",
            "https://localhost:5375",
            redirectUris: null,
            webOrigins: null
            );
        accountManagementSwaggerClient.ExtendDefaultScopes(accountmanagementScope.Name);
        accountManagementSwaggerClient.AddAudienceMapper("account_management");
        
        var bob = new User("bob", new UserArgs
        {
            RealmId = realm.Id,
            Username = "bob",
            Enabled = true,
            Email = "bob@domain.com",
            FirstName = "Smith",
            LastName = "Bobson",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "bob",
                Temporary = true,
            },
        });

        var alice = new User("alice", new UserArgs
        {
            RealmId = realm.Id,
            Username = "alice",
            Enabled = true,
            Email = "alice@domain.com",
            FirstName = "Alice",
            LastName = "Smith",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "alice",
                Temporary = true,
            },
        });
    }
}