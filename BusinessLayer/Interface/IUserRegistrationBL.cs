﻿using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserRegistrationBL
    {
        public bool RegisterUser(RegistrationDTO registrationDTO);
        public string LoginUser(LoginDTO loginDTO);
        public bool ForgotPasswordBL(ForgotPasswordDTO forgotPasswordDTO);
        public bool ResetPasswordBL(ResetPasswordDTO resetPasswordDTO);

    }
}
