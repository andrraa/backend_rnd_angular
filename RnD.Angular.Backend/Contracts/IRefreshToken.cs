namespace RnD.Angular.Backend.Contracts
{
    public interface IRefreshToken
    {
        string GenerateToken(string username);
    }
}
