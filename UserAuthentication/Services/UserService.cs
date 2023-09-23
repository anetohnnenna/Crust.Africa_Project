using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuthentication.Entities;
using UserAuthentication.Interface;
using UserAuthentication.Models;

namespace UserAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<RegistrationTable> _userReg;
        private readonly IConfiguration _configuration;
        private readonly IRepository<LoginTable> _loginDetails;

        public UserService(IRepository<RegistrationTable> userReg, IConfiguration configuration, IRepository<LoginTable> loginDetails)
        {
            _userReg = userReg;
            _configuration = configuration;
            _loginDetails = loginDetails;
        }
        public async Task<GenericApiResponse<RegistrationTable>> UserReg(RegistrationTable reg)
        {
            GenericApiResponse<RegistrationTable> result = new GenericApiResponse<RegistrationTable>();
            try
            {
                Encryption encypt = new Encryption();
                //check if the user is existing
                RegistrationTable details = await _userReg.FetchRecord(m => m.Email == reg.Email);

                //if it returns no record, then it means the user is a not existing
                if (details == null)
                {
                    reg.Password = encypt.ConvertToEncrypt(reg.Password);
                    reg.DateCreated = DateTime.Now;
                    //string bb = encypt.ConvertToDecrypt(reg.Password);
                    await _userReg.Add(reg);
                    result.ResponseCode = ResponseCodes.Success;
                    result.ResponseDescription = "You have registered suceessfully, kindly login to proceed. Thank you.";

                }
                else
                {
                    result.ResponseDescription = "You have registered before, kindly login to proceed. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<RegistrationTable>> LoginUser(RegistrationTable reg)
        {
            GenericApiResponse<RegistrationTable> result = new GenericApiResponse<RegistrationTable>();
            try
            {
                //check if the user is existing
                RegistrationTable details = await _userReg.FetchRecord(m => m.Email == reg.Email);
                string bb = encrypt.ConvertToDecrypt(details.Password);
                //if it returns no record, then it means the user is a not existing
                if (details != null && encrypt.ConvertToDecrypt(details.Password) == reg.Password)
                {
                   
                    result.ResponseCode = ResponseCodes.Success;
                }
                else
                {
                    result.ResponseDescription = "You have not registered before, kindly register before you login. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;
                }
            
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task LoginDetails(LoginTable res)
        {
            GenericApiResponse<LoginTable> result = new GenericApiResponse<LoginTable>();
            try
            {
                
                await _loginDetails.Add(res);
        
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            
        }
    }
}
