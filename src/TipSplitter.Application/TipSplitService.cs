using TipSplitter.Domain;

namespace TipSplitter.Application;

public sealed record TipSplitRequest(decimal Amount, decimal TipPercent, int People);

public sealed record TipSplitResponse(IReadOnlyList<decimal> PerPerson, decimal TotalTip);

public static class TipSplitService
{
    public static TipSplitResponse Split(TipSplitRequest request)
    {
        var result = TipSplitCalculator.Split(request.Amount, request.TipPercent, request.People);

        return new TipSplitResponse(result.PerPerson, result.TipAmount);
    }
}
