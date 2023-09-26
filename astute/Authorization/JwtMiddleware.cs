using astute.CoreServices;
using astute.Repository;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace astute.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtMiddleware(RequestDelegate requestDelegate,
            IHttpContextAccessor httpContextAccessor)
        {
            _requestDelegate = requestDelegate;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context, IJWTAuthentication userService, IJWTAuthentication jWTAuthentication)
        {
            // Extract the JWT token from the Authorization header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Validate the JWT token
            var loginUserId = jWTAuthentication.Validate_Jwt_Token(token);

            if (loginUserId != null)
            {
                var ipAddress = await CoreService.GetIP_Address(_httpContextAccessor);
                // Fetch user information based on the validated token
                var employee = await userService.Get_Employee_JWT_Token(loginUserId.Value);

                if (employee != null)
                {
                    // Get the client's IP address
                    

                    // Check if the token and IP address match the user's stored values
                    if (employee.Token.Equals(token) && employee.IP_Address.Equals(ipAddress))
                    {
                        // Attach user information to the context on successful validation
                        context.Items["User"] = employee;
                    }
                    else
                    {
                        // Token or IP address doesn't match, return Unauthorized status
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }
                else
                {
                    // User not found, return Unauthorized status
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            // Continue processing the request pipeline
            await _requestDelegate(context);
        }
    }
}
