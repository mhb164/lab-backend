namespace Laboratory.Backend.Auth.Ldap;

public class LdapService
{
    private readonly Dictionary<string, DomainUrl> _domainUrls = new Dictionary<string, DomainUrl>(StringComparer.OrdinalIgnoreCase);

    public void AddDomain(string domain, string url)
        => AddDomain(new DomainUrl(domain, url));

    public void AddDomain(DomainUrl ldapDomainUrl)
    {
        if (_domainUrls.ContainsKey(ldapDomainUrl.Domain))
            throw new InvalidOperationException($"{ldapDomainUrl.Domain} already added!");

        _domainUrls.Add(ldapDomainUrl.Domain, ldapDomainUrl);
    }

    private string GetUrl(string domain)
    {
        if (!_domainUrls.TryGetValue(domain, out var ldapDomainUrl))
            return string.Empty;

        return ldapDomainUrl.Url;
    }


    public bool Validate(string domain, string username, string password)
    {
        try
        {
            using (var connection = CreateConnection(domain, username, password))
                return true;
        }
        catch
        {
            return false;
        }
    }

    public UserData GetData(string domain, string username, string password)
    {
        return new UserData(GetAllData(domain, username, password));
    }

    public Dictionary<string, string> GetAllData(string domain, string username, string password)
    {
        var searchFilter = $"(&(objectClass=user)(sAMAccountName={username}))";

        try
        {
            using (var connection = CreateConnection(domain, username, password))
            {
                // 1️⃣ Get the Default Naming Context (Base DN) dynamically
                var request = new SearchRequest(null, "(objectClass=*)", SearchScope.Base, "defaultNamingContext");
                var response = (SearchResponse)connection.SendRequest(request);

                if (response.Entries.Count == 0)
                    throw new Exception("Failed to retrieve the default naming context.");

                string baseDn = response.Entries[0].Attributes["defaultNamingContext"][0].ToString();
                Console.WriteLine($"Base DN Found: {baseDn}");

                // 2️⃣ Perform a search for the user
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
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    private LdapConnection CreateConnection(string domain, string username, string password)
    {
        var url = GetUrl(domain);
        if (string.IsNullOrWhiteSpace(domain))
            return null;

        var credentials = new NetworkCredential(username, password, domain);
        var serverId = new LdapDirectoryIdentifier(url);

        var connection = new LdapConnection(serverId, credentials);
        connection.Bind();
        return connection;
    }
}