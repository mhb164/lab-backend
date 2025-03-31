namespace TypicalAuth.Mapping;

public static partial class MappingExtentions
{
    public static UserInfo ToUserInfo(this User modelItem, bool readOnly)
       => new UserInfo(
           username: modelItem.Username,
           firstname: modelItem.Firstname,
           lastname: modelItem.Lastname,
           locallyAvailable: modelItem.LocallyAvailable,
           readOnly: readOnly,
           ldapAccounts: modelItem.LdapAccounts.Select(x => $"{x.Username}.{x.Domain}").ToList(),
           emails: modelItem.Emails.Select(x => new UserInfoEmail(x.Email, x.Verified)).ToList());


}
