using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Extensions
{
    public static class ListToActionResult
    {
        public static IActionResult ToActionResult<T>(this List<T> data) => new OkObjectResult(data);
        public static Task<IActionResult> ToActionResult<T>(this Task<List<T>> data) =>
            data.Map(ToActionResult);
    }
}
