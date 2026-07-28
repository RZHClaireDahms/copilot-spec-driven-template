namespace TipSplitter.Domain;

public sealed record TipSplitResult(decimal TipAmount, decimal Total, IReadOnlyList<decimal> PerPerson);

public static class TipSplitCalculator
{
    public static TipSplitResult Split(decimal billAmount, decimal tipPercent, int peopleCount)
    {
        var tipAmount = Math.Round(billAmount * tipPercent / 100m, 2, MidpointRounding.AwayFromZero);
        var total = billAmount + tipAmount;
        var perPersonAmount = Math.Round(total / peopleCount, 2, MidpointRounding.AwayFromZero);
        var perPerson = Enumerable.Repeat(perPersonAmount, peopleCount).ToList();

        return new TipSplitResult(tipAmount, total, perPerson);
    }
}
