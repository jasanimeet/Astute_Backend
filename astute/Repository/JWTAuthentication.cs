using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace astute.Repository
{
    public class JWTAuthentication : IJWTAuthentication
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public JWTAuthentication(IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            AstuteDbContext dbContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public string Generate_Jwt_Token(Employee_Master employee_Master)
        {   
            var clims = new[]
            {
                new Claim("user_Id", employee_Master.Employee_Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtToken:Issuer"]!,
                audience: _configuration["JwtToken:Audience"],
                claims: clims,
                expires: DateTime.UtcNow.AddMinutes(120),
                //signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public int? Validate_Jwt_Token(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtToken:SecretKey"]!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "user_Id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch (Exception ex)
            {
                // return null if validation fails
                if (ex.Message.Contains("The token is expired."))
                {
                    return -1;
                }
                return null;
            }
        }
        public async Task<int> Insert_Update_Employee_JWT_Token(Employee_JWT_Token employee_JWT_Token)
        {   
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);

            var token_Id = new SqlParameter("@Token_Id", employee_JWT_Token.Token_Id);
            var employee_Id = employee_JWT_Token.Employee_Id > 0 ? new SqlParameter("@Employee_Id", employee_JWT_Token.Employee_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            var iP_Address = !string.IsNullOrEmpty(ip_Address) ? new SqlParameter("@IP_Address", ip_Address) : new SqlParameter("@IP_Address", DBNull.Value);
            var token = !string.IsNullOrEmpty(employee_JWT_Token.Token) ? new SqlParameter("@Token", employee_JWT_Token.Token) : new SqlParameter("@Token", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Employee_JWT_Token_Insert_Update @Token_Id, @Employee_Id, @IP_Address, @Token", token_Id, employee_Id, iP_Address, token));

            return result;
        }
        public async Task<Employee_JWT_Token> Get_Employee_JWT_Token(int employee_Id)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var _employee_Id = employee_Id > 0 ? new SqlParameter("@Employee_Id", employee_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            var _ip_Address = !string.IsNullOrEmpty(ip_Address) ? new SqlParameter("@IP_Address", ip_Address) : new SqlParameter("@IP_Address", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Employee_JWT_Token
                            .FromSqlRaw(@"exec Employee_JWT_Token_Select @Employee_Id, @IP_Address", _employee_Id, _ip_Address)
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        public async Task<int> Delete_Employee_JWT_Token(int user_Id)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var employee_Id = user_Id > 0 ? new SqlParameter("@Employee_Id", user_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            var iP_Address = !string.IsNullOrEmpty(ip_Address) ? new SqlParameter("@IP_Address", ip_Address) : new SqlParameter("@IP_Address", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Employee_JWT_Token_Delete @Employee_Id, @IP_Address", employee_Id, iP_Address));

            return result;
        }
        #endregion
    }
}
