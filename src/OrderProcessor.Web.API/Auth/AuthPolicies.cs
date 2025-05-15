namespace OrderProcessor.Web.API.Auth
{
    public enum AuthPolicies
    {
        RequireAdminOrCustomerRole,
        RequireCustomerRole,
        RequireAdministratorRole
    }
}
