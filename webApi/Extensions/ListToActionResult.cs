using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Extensions
{
    public static class ListToActionResult
    {
        public static IActionResult ToActionResult<T>(this IEnumerable<T> data) =>
             new OkObjectResult(data);
        public static Task<IActionResult> ToActionResult<T>(this Task<IEnumerable<T>> data) =>
            data.Map(ToActionResult);
    }
}
