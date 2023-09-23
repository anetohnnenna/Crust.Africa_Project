using UserAuthentication.Entities;

namespace UserAuthentication.Interface
{
    public interface IUserService
    {
        Task<GenericApiResponse<RegistrationTable>> UserReg(RegistrationTable reg);
        Task<GenericApiResponse<RegistrationTable>> LoginUser(RegistrationTable reg);
        Task LoginDetails(LoginTable res);
    }
}
