using Microsoft.AspNetCore.Mvc;


namespace back.Application.Common;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess
            ? new NoContentResult()
            : new BadRequestObjectResult(result.Error);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess 
            ? (IActionResult)new OkObjectResult(result.Value) 
            : new BadRequestObjectResult(result.Error);
    }

    public static IActionResult ToActionResultCreated<T>(
        this Result<T> result, 
        string actionName, 
        Func<T, object> routeValues)
    {
        return result.IsSuccess 
            ? (IActionResult)new CreatedAtActionResult(actionName, null, routeValues(result.Value), result.Value)
            : new BadRequestObjectResult(result.Error);
    }
}