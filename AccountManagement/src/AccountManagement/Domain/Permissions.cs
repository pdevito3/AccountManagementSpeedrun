namespace AccountManagement.Domain;

using System.Reflection;

public static class Permissions
{
    // Permissions marker - do not delete this comment
    public const string CanDeleteUserAccount = nameof(CanDeleteUserAccount);
    public const string CanUpdateUserAccount = nameof(CanUpdateUserAccount);
    public const string CanAddUserAccount = nameof(CanAddUserAccount);
    public const string CanReadBankingSystem = nameof(CanReadBankingSystem);
    public const string CanDeleteUser = nameof(CanDeleteUser);
    public const string CanUpdateUser = nameof(CanUpdateUser);
    public const string CanAddUser = nameof(CanAddUser);
    public const string CanReadUsers = nameof(CanReadUsers);
    public const string CanDeleteRolePermission = nameof(CanDeleteRolePermission);
    public const string CanUpdateRolePermission = nameof(CanUpdateRolePermission);
    public const string CanAddRolePermission = nameof(CanAddRolePermission);
    public const string CanReadRolePermissions = nameof(CanReadRolePermissions);
    public const string CanRemoveUserRole = nameof(CanRemoveUserRole);
    public const string CanAddUserRole = nameof(CanAddUserRole);
    public const string CanGetRoles = nameof(CanGetRoles);
    
    public static List<string> List()
    {
        return typeof(Permissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(x => (string)x.GetRawConstantValue())
            .ToList();
    }
}
