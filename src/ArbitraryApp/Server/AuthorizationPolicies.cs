namespace ArbitraryApp.Server
{
    /// <summary>
    /// Wrapper class that contains all the authorization policies available in this application.
    /// </summary>
    public static class AuthorizationPolicies
    {
        public const string AssignmentToAdminRoleRequired = nameof(AssignmentToAdminRoleRequired);
    }
}
