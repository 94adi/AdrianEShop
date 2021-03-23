using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomAttribute
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class CustomEmailValidatorAttribute : ValidationAttribute
    {
        private const string _emailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public CustomEmailValidatorAttribute()
        {

        }

        public string GetErrorMessage() => "Custom message: Please enter a valid email address";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isEmailValid = Regex.IsMatch(value.ToString(), _emailRegex , RegexOptions.IgnoreCase);
            if (isEmailValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
    
}
