namespace TipSplitter.Domain;

public sealed record TipSplitResult(decimal TipAmount, decimal Total, IReadOnlyList<decimal> PerPerson);

public static class TipSplitCalculator
{
    public static TipSplitResult Split(decimal billAmount, decimal tipPercent, int peopleCount)
    {
        if (peopleCount <= 0)
        {
            throw new TipSplitValidationException("Anzahl Personen muss mindestens 1 sein.");
        }

        if (billAmount < 0)
        {
            throw new TipSplitValidationException("Rechnungsbetrag darf nicht negativ sein.");
        }

        if (tipPercent < 0)
        {
            throw new TipSplitValidationException("Trinkgeld-Prozentsatz darf nicht negativ sein.");
        }

        var tipAmount = Math.Round(billAmount * tipPercent / 100m, 2, MidpointRounding.AwayFromZero);
        var total = billAmount + tipAmount;
        var totalCents = Math.Round(total * 100m, 0, MidpointRounding.AwayFromZero);
        var baseCentsPerPerson = Math.Floor(totalCents / peopleCount);
        var remainderCents = totalCents - baseCentsPerPerson * peopleCount;

        var perPerson = Enumerable.Repeat(baseCentsPerPerson / 100m, peopleCount).ToList();
        perPerson[0] += remainderCents / 100m;

        return new TipSplitResult(tipAmount, total, perPerson);
    }
}
