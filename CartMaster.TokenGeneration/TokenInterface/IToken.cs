using CartMaster.TokenGeneration.Models;

namespace CartMaster.TokenGeneration.TokenInterface
{
    public interface IToken
    {
        public string CreateToken(TokenModel tokenModel);
        public TokenModel ReadToken(string token);
    }
}
