namespace Laboratory.Backend.Extensions;

public static class AuthPermissions
{
    public static AuthMetadata Public => AuthMetadata.Public;
    public static AuthMetadata TokenIsEnough => AuthMetadata.TokenIsEnough;

    public static readonly string Domain_Laboratory = "lab";

    public static readonly string Scope_ProductTypes = "product_types";

    public static readonly string Permission_View = "view";
    public static readonly string Permission_Add = "add";
    public static readonly string Permission_Edit = "edit";
    public static readonly string Permission_Remove = "remove";

    /// <summary>
    /// Domain should be match to "lab"
    /// </summary>
    public static readonly AuthMetadata DomainSpecific = TokenIsEnough.NewWith(domain: Domain_Laboratory);

    public static readonly AuthMetadata ProductTypes = DomainSpecific.NewWith(scope: Scope_ProductTypes, permission: Permission_View);
    public static readonly AuthMetadata ProductTypesAdd = DomainSpecific.NewWith(permission: Permission_Add);
    public static readonly AuthMetadata ProductTypesEdit = DomainSpecific.NewWith(permission: Permission_Edit);
    public static readonly AuthMetadata ProductTypesRemove = DomainSpecific.NewWith(permission: Permission_Remove);

}
