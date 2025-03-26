namespace TypicalAuth.Ldap;

public static class LdapClient
{
    public static bool Validate(string username, string password, string domainName)
    {
        try
        {
            using (var connection = CreateConnection(username, password, domainName))
                return connection != null;
        }
        catch
        {
            return false;
        }
    }

    private static LdapConnection CreateConnection(string username, string password, string domainName = null)
    {
        if (string.IsNullOrWhiteSpace(domainName))
            return null;

        var domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domainName));
        var domainController = domain.FindDomainController();
        var credentials = new NetworkCredential(username, password, domain.Name);
        var serverId = new LdapDirectoryIdentifier(domainController.Name);
        var connection = new LdapConnection(serverId, credentials);
        connection.Bind();
        return connection;
    }

    public static UserData GetData(string username, string password, string domainName)
    {
        try
        {
            using (var connection = CreateConnection(username, password, domainName))
            {
                var userAttributes = GetUserAttributes(connection, username);
                return new UserData(userAttributes);
            }
        }
        catch
        {
            return null;
        }
    }

    private static Dictionary<string, string> GetUserAttributes(LdapConnection connection, string username)
    {
        var searchFilter = $"(&(objectClass=user)(sAMAccountName={username}))";
        var request = new SearchRequest(null, "(objectClass=*)", SearchScope.Base, "defaultNamingContext");
        var response = (SearchResponse)connection.SendRequest(request);

        if (response.Entries.Count == 0)
            throw new Exception("Failed to retrieve the default naming context.");

        string baseDn = response.Entries[0].Attributes["defaultNamingContext"][0].ToString();
        //Console.WriteLine($"Base DN Found: {baseDn}");

        var searchRequest = new SearchRequest(baseDn, searchFilter, SearchScope.Subtree, null); // NULL retrieves all attributes
        var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

        if (searchResponse.Entries.Count > 0)
        {
            var userAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (string attributeName in searchResponse.Entries[0].Attributes.AttributeNames)
            {
                var attributeValue = searchResponse.Entries[0].Attributes[attributeName][0].ToString();
                userAttributes.Add(attributeName, attributeValue);
            }

            return userAttributes; // Return all attributes
        }

        throw new Exception("No result!");
    }
}