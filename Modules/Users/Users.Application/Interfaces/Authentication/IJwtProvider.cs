using Users.Core.Enums;
using Users.Core.Models;

namespace Users.Application.Interfaces.Authentication;

public interface IJwtProvider
{
    string Generate(User user, IEnumerable<Role> roles);
}