using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.Services.Identity.API.Extensions
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert identity result error to string.
        /// </summary>
        /// <param name="inputString">Input string.</param>
        /// <param name="result">Result.</param>
        /// <returns>Result error.</returns>
        public static string ConvertIdentityResultErrorToString(this string inputString, IdentityResult result)
        {
            var errorList = result.Errors.Select(error => error.Code).ToList();

            var resultError = string.Empty;
            resultError += string.Join(',', errorList);
            return resultError;
        }
    }
}
