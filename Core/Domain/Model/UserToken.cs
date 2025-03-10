using Backend.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model;

public class UserToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required AuthType Type { get; set; }
    public required string Username { get; set; }
    public required DateTime Time { get; set; }
    public required string Description { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefereshedAt { get; set; }
}
