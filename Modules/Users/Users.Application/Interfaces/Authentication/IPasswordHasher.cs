namespace Users.Application.Interfaces.Authentication;

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashedPassword);
}