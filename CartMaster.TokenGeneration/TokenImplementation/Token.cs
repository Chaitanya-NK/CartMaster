using CartMaster.Static;
using CartMaster.TokenGeneration.Models;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8604

namespace CartMaster.TokenGeneration.TokenImplementation
{
    public class Token : IToken
    {
        public string CreateToken(TokenModel tokenModel)
        {
            if(tokenModel == null)
            {
                throw new ArgumentNullException(nameof(tokenModel));
            }


            var claims = new List<Claim>
            {
                new Claim(StaticToken.UserID, tokenModel.UserID),
                new Claim(StaticToken.Username, tokenModel.UserName),
                new Claim(StaticToken.FirstName, tokenModel.FirstName),
                new Claim(StaticToken.LastName, tokenModel.LastName),
                new Claim(StaticToken.Email, tokenModel.Email),
                new Claim(StaticToken.RoleID, tokenModel.RoleID),
                new Claim(StaticToken.RoleName, tokenModel.RoleName),
                new Claim(StaticToken.CartID, tokenModel.CartID)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticToken.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var utcNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            var token = new JwtSecurityToken(StaticToken.JwtIssuer,
                                             StaticToken.JwtAudience,
                                             claims,
                                             expires: DateTime.Now.AddMinutes(StaticToken.JwtTokenExpiryMinutes),
                                             signingCredentials: credentials,
                                             notBefore: utcNow
                                             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenModel ReadToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticToken.JwtKey));
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = StaticToken.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = StaticToken.JwtAudience,
                };

                handler.InboundClaimTypeMap.Clear();
                SecurityToken securityToken;
                var identity = handler.ValidateToken(token, validations, out securityToken).Identity as ClaimsIdentity;
                if (identity == null || identity.Claims == null)
                {
                    return null;
                }
                var user = new TokenModel();
                foreach(var item in identity.Claims)
                {
                    switch (item.Type)
                    {
                        case StaticToken.UserID:
                            user.UserID = item.Value;
                            break;
                        case StaticToken.Username:
                            user.UserName = item.Value;
                            break;
                        case StaticToken.FirstName:
                            user.FirstName = item.Value;
                            break;
                        case StaticToken.LastName:
                            user.LastName = item.Value;
                            break;
                        case StaticToken.Email:
                            user.Email = item.Value;
                            break;
                        case StaticToken.RoleID:
                            user.RoleID = item.Value;
                            break;
                        case StaticToken.RoleName:
                            user.RoleName = item.Value;
                            break;
                        case StaticToken.CartID:
                            user.CartID = item.Value;
                            break;
                    }
                }
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
