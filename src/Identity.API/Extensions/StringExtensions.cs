using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.Services.Identity.API.Extensions
{
    public static class StringExtensions
    {
        public static string ConverIdentityResultErrorToString(this string inputString, IdentityResult result)
        {
            var errorList = new List<string>();
            foreach (var error in result.Errors)
            {
                errorList.Add(error.Code);
            }

            var resultError = string.Empty;
            resultError += string.Join(',', errorList);
            return resultError;
        }
    }
}
