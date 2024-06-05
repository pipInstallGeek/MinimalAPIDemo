
using System.Net;
using MagicVilla_CouponAPI.Model;
using MagicVilla_CouponAPI.Model.DTO;
using MagicVilla_CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_CouponAPI.Endpoints
{
    public static class AuthEnpoints
    {
        public static void ConfigureAuthEnpoints(this WebApplication app)
        {
            app.MapPost("/api/login", Login).WithName("Login").Accepts<LoginRequestDTO>("application/json")
                .Produces<APIResponse>(200)
                .Produces(400);
          
            app.MapPost("/api/register", Register).WithName("Register").Accepts<RegistrationRequestDTO>("application/json")
               .Produces<APIResponse>(200)
               .Produces(400);
        }


        private static async Task<IResult> Login(IAuthRepository _authRepo , [FromBody] LoginRequestDTO model)
        {
            APIResponse response = new() { isSuccess = false, StatusCode = HttpStatusCode.BadRequest};
            var loginResponse = await _authRepo.Login(model);

            if (loginResponse == null) {

                response.ErrorsMessages.Add("Username or password is incorrect");
                return Results.BadRequest(response);
            }

            response.Result = loginResponse;
            response.isSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);    
        }

        private static async Task<IResult> Register(IAuthRepository _authRepo, [FromBody] RegistrationRequestDTO model)
        {
            APIResponse response = new() { isSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            bool ifUserNameIsUnique = _authRepo.IsUniqueUser(model.UserName);
            if (!ifUserNameIsUnique)
            {
                response.ErrorsMessages.Add("Username or password is incorrect");
                return Results.BadRequest(response);

            }
            var registerResponse = await _authRepo.Register(model);
            if (registerResponse == null || string.IsNullOrEmpty(registerResponse.UserName)){
                return Results.BadRequest(response);
            }
            response.isSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        
        }
    }
}
