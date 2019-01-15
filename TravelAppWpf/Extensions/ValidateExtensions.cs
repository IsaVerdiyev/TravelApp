using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.Extensions
{
    public static class ValidateExtensions
    {
        public static string Validate(this ObservableObject vm, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name in validation extension", propertyName);
            }

            string error = string.Empty;
            var value = vm.GetType().GetProperty(propertyName).GetValue(vm);
            var results = new List<ValidationResult>(1);
            var result = Validator.TryValidateProperty(value, new ValidationContext(vm, null, null) { MemberName = propertyName }, results);
            if (!result)
            {
                var validationResult = results.First();
                error = validationResult.ErrorMessage;
            }
            return error;
        }
    }
}
