using RnD.Angular.Backend.Contracts;
using RnD.Angular.Backend.Models;
using System.Security.Cryptography;

namespace RnD.Angular.Backend.Repositories
{
    public class RefreshTokenRepositories : IRefreshToken
    {
        private readonly LearnDbContext _context;

        public RefreshTokenRepositories(LearnDbContext learnDbContext)
        {
            _context = learnDbContext;
        }

        public string GenerateToken(string username)
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                string refreshToken = Convert.ToBase64String(randomNumber);

                var _user = _context.TblRefreshtokens.FirstOrDefault(opt => opt.UserId == username);

                if (_user != null)
                {
                    _user.RefreshToken = refreshToken;
                    _context.SaveChanges();
                }
                else
                {
                    RefreshTokenModel refreshTokenModel = new RefreshTokenModel()
                    {
                        UserId = username,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken = refreshToken,
                        IsActive = true
                    };
                }
                return refreshToken;
            }
        }
    }
}
