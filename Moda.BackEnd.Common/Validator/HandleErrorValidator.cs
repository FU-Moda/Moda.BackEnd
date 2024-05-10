using FluentValidation.Results;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Common.Validator
{
    public class HandleErrorValidator
    {
        public AppActionResult HandleError(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errorMessage = new List<string>();
                foreach (var error in result.Errors) errorMessage.Add(error.ErrorMessage);
                return new AppActionResult
                {
                    IsSuccess = false,
                    Messages = errorMessage,
                    Result = null
                };
            }

            return new AppActionResult();
        }
    }
}
