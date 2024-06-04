using FluentValidation;
using MagicVilla_CouponAPI.Model.DTO;

namespace MagicVilla_CouponAPI.Validations
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation() {

            RuleFor(model => model.Name).NotEmpty();
            RuleFor(mode => mode.Percent).InclusiveBetween(1,100);
        }
    }
}
