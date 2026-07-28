using Microsoft.AspNetCore.Mvc;
using TipSplitter.Application;
using TipSplitter.Domain;

namespace TipSplitter.Api.Controllers;

public sealed record SplitRequest(decimal? Amount, decimal? TipPercent, int? People);

public sealed record SplitResponse(IReadOnlyList<decimal> PerPerson, decimal TotalTip);

[ApiController]
[Route("split")]
public sealed class SplitController : ControllerBase
{
    [HttpPost]
    public ActionResult<SplitResponse> Post(SplitRequest request)
    {
        if (request.Amount is null || request.TipPercent is null || request.People is null)
        {
            return Problem(detail: "Amount, tipPercent and people are required.", statusCode: StatusCodes.Status400BadRequest);
        }

        try
        {
            var result = TipSplitService.Split(new TipSplitRequest(request.Amount.Value, request.TipPercent.Value, request.People.Value));

            return Ok(new SplitResponse(result.PerPerson, result.TotalTip));
        }
        catch (TipSplitValidationException exception)
        {
            return Problem(detail: exception.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
