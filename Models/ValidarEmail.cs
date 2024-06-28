using System;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class ValidarEmail : ValidationAttribute
    {
        private readonly string _allowedDomain;

        public ValidarEmail(string allowedDomain)
        {
            _allowedDomain = allowedDomain;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string email = value.ToString();
                if (email.EndsWith(_allowedDomain))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"O e-mail deve terminar com {_allowedDomain}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
