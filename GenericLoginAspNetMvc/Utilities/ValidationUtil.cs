using GenericLoginAspNetMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericLoginAspNetMvc.Utilities
{
    // Validation has been placed into a single God Class for simplicity
    // Actual Implementation of Validation may differ depending on coding pattern implemented
    public static class ValidationUtil
    {

        public static void UserRepository_CreateUser(string firstName, string lastName, string username, string password, string accountType)
        {
            string err_message = string.Empty;
            bool hasFailedValidation = false;

            if(!Enum.TryParse<AccountTypeEnum>(accountType, out AccountTypeEnum _blank))
            {
                hasFailedValidation = true;
                err_message += "Please Select Valid Account Type";
            }

            // I could probably do this with regex but i'll leave that for later

            if (string.IsNullOrEmpty(firstName))
            {
                hasFailedValidation = true;
                err_message += "Please input First Name!, \n";
            }

            if (string.IsNullOrEmpty(lastName))
            {
                hasFailedValidation = true;
                err_message += "Please input Last Name!, \n";
            }

            if (string.IsNullOrEmpty(username))
            {
                hasFailedValidation = true;
                err_message += "Please input Username!, \n";    
            }

            if (string.IsNullOrEmpty(password))
            {
                hasFailedValidation = true;
                err_message += "Please input Password!, \n";

                throw new Exception(err_message);
            }

            if (!password.Any(x => char.IsDigit(x)))
            {
                hasFailedValidation = true;
                err_message += "Password must contain at least one (1) number, \n";
            }

            if (!password.Any(x => char.IsUpper(x)))
            {
                hasFailedValidation = true;
                err_message += "Password must contain at least one (1) upper-case letter, \n";
            }

            if (!password.Any(x => char.IsLower(x)))
            {
                hasFailedValidation = true;
                err_message += "Password must contain at least one (1) lower-case letter, \n";
            }

            if (password.Length < 6)
            {
                hasFailedValidation = true;
                err_message += "Password must be at least 6 characters long";
            }

            if (hasFailedValidation)
                throw new Exception(err_message);
        }
        public static void UserRepository_AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new Exception("Authentication Failed");
        }
    }
}
