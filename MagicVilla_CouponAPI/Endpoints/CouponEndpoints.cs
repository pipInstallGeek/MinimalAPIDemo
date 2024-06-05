using AutoMapper;
using FluentValidation;
using MagicVilla_CouponAPI.Model.DTO;
using MagicVilla_CouponAPI.Model;
using MagicVilla_CouponAPI.Repository;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_CouponAPI.Endpoints
{
    public static class CouponEndpoints
    {
        public static void ConfigureCouponEndoints(this WebApplication app)
        {
            //   Get All Coupons
            app.MapGet("/api/coupon",GetAllcoupons).WithName("GetCoupons").Produces<APIResponse>(200)
              ;

            //   Get Coupon By Id 
            app.MapGet("/api/coupon/{id:int}", GetCoupon).WithName("GetCoupon").Produces<APIResponse>(200)
                .AddEndpointFilter(async (context, next) =>
            {
                var id = context.GetArgument<int>(1);
                if (id == 0)
                {
                    return Results.BadRequest("Cannot have 0 in id");
                }
                return await next(context);
            });


            //   Add A Coupon 
            app.MapPost("/api/coupon", CreateCoupon).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);



            app.MapPut("/api/coupon", UpdateCoupon).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400);

            app.MapDelete("/api/coupon/{id:int}", DeleteCoupon);
        }

        private async static Task<IResult> GetAllcoupons(ICouponRepository _couponRepo, ILogger<Program> _logger)
        {
            APIResponse response = new();

            _logger.Log(LogLevel.Information, "Getting All Coupons");

            response.Result = await _couponRepo.GetAllAsync();
            response.isSuccess = true;
            response.StatusCode = HttpStatusCode.OK;


            return Results.Ok(response);
        }

        private async static Task<IResult> GetCoupon(ICouponRepository _couponRepo, int id)
        {
            APIResponse response = new();
            response.Result = await _couponRepo.GetAsync(id);
            response.isSuccess = true;
            response.StatusCode = HttpStatusCode.OK;


            return Results.Ok(response);
        }
        [Authorize]
        private async static Task<IResult> CreateCoupon(ICouponRepository _couponRepo, IMapper _mapper, IValidator<CouponCreateDTO> _validation, [FromBody] CouponCreateDTO coupon_C_DTO)
        {
            APIResponse response = new() { isSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            //var validationResult = _validation.ValidateAsync(coupon_C_DTO).GetAwaiter().GetResult();    
            // Or make the method  Async 

            var validationResult = await _validation.ValidateAsync(coupon_C_DTO);

            if (!validationResult.IsValid)
            {
                response.ErrorsMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(response);

            }
            if (await _couponRepo.GetAsync(coupon_C_DTO.Name.ToLower()) != null)
            {
                response.ErrorsMessages.Add("Coupon Name Already exists");
                return Results.BadRequest(response);
            }

            // Mapping CouponCreateDTO with Coupon manually :( 
            /*    Coupon coupon = new()
                {
                    Name = coupon_C_DTO.Name,
                    Percent = coupon_C_DTO.Percent,
                    IsActive = coupon_C_DTO.IsActive
                };*/


            // Mapping CouponCreateDto to Coupon using AutoMapper :) 
            Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);

            await _couponRepo.CreateAsync(coupon);
            await _couponRepo.SaveAsync();
            CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);
            
            
            //return Results.Created($"/api/coupon/{coupon.Id}", coupon);

            //return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, couponDTO );

            response.Result = couponDTO;
            response.isSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return Results.Ok(response);
        }
        [Authorize]
        private async static Task<IResult> UpdateCoupon(ICouponRepository _couponRepo, IMapper _mapper, IValidator<CouponUpdateDTO> _validation, [FromBody] CouponUpdateDTO coupon_U_DTO)
        
        {
            APIResponse response = new() { isSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            var validationResult = await _validation.ValidateAsync(coupon_U_DTO);
            if (!validationResult.IsValid)
            {

                response.ErrorsMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(response);

            }

            await _couponRepo.UpdateAsync(_mapper.Map<Coupon>(coupon_U_DTO));
            await _couponRepo.SaveAsync();

            response.Result = _mapper.Map<CouponDTO>(await _couponRepo.GetAsync(coupon_U_DTO.Id));
            response.isSuccess = true;
            response.StatusCode = HttpStatusCode.OK;


            return Results.Ok(response);
        }
        [Authorize]
        private async static Task<IResult> DeleteCoupon(ICouponRepository _couponRepo, int id)
        {
            APIResponse response = new() { isSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            Coupon coupon = await _couponRepo.GetAsync(id);
            if (coupon != null)
            {

                await _couponRepo.RemoveAsync(coupon);
                await _couponRepo.SaveAsync();
                response.isSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
                return Results.Ok(response);
            }
            else
            {
                response.ErrorsMessages.Add("Invalid Id");
                return Results.BadRequest(response);
            }

        }
    }
    
}
