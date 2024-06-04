using AutoMapper;
using MagicVilla_CouponAPI.Model;
using MagicVilla_CouponAPI.Model.DTO;

namespace MagicVilla_CouponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() { 
        
            CreateMap<Coupon, CouponCreateDTO >().ReverseMap();
            CreateMap<Coupon, CouponDTO >().ReverseMap();
            CreateMap<Coupon, CouponUpdateDTO>().ReverseMap();
        }

    }
}
