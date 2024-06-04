using FluentValidation;
using MagicVilla_CouponAPI.Model.DTO;

namespace MagicVilla_CouponAPI.Validations
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation() {

            RuleFor(model => model.Id).GreaterThan(0);
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(mode => mode.Percent).InclusiveBetween(1,100);
        }
    }
}
