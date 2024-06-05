using MagicVilla_CouponAPI.Model.DTO;

namespace MagicVilla_CouponAPI.Repository
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register (RegistrationRequestDTO registrationRequestDTO);
    }
}
