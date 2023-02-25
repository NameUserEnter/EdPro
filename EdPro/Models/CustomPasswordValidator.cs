using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EdPro.Models
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        public int RequiredLength { get; set; } // минимальная длина

        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (String.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Мінімальна довжина пароля {RequiredLength}"
                });
            }
            string pattern = "^[0-9]+$";

            if (!Regex.IsMatch(password, pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль повинен бути тільки з цифр"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
            IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}