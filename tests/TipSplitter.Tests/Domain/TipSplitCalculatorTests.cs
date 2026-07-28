using Shouldly;
using TipSplitter.Domain;

namespace TipSplitter.Tests.Domain;

public class TipSplitCalculatorTests
{
    [Fact]
    public void Split_100Bill_0PercentTip_4People_ReturnsEqualShares()
    {
        var result = TipSplitCalculator.Split(billAmount: 100m, tipPercent: 0m, peopleCount: 4);

        result.TipAmount.ShouldBe(0m);
        result.Total.ShouldBe(100m);
        result.PerPerson.ShouldBe(new[] { 25m, 25m, 25m, 25m });
    }

    [Fact]
    public void Split_100Bill_10PercentTip_4People_ReturnsEqualSharesIncludingTip()
    {
        var result = TipSplitCalculator.Split(billAmount: 100m, tipPercent: 10m, peopleCount: 4);

        result.TipAmount.ShouldBe(10m);
        result.Total.ShouldBe(110m);
        result.PerPerson.ShouldBe(new[] { 27.50m, 27.50m, 27.50m, 27.50m });
    }

    [Fact]
    public void Split_100Bill_0PercentTip_3People_AssignsRemainderCentToFirstPerson()
    {
        var result = TipSplitCalculator.Split(billAmount: 100m, tipPercent: 0m, peopleCount: 3);

        result.PerPerson.ShouldBe(new[] { 33.34m, 33.33m, 33.33m });
        result.PerPerson.Sum().ShouldBe(100m);
    }

    [Fact]
    public void Split_100Bill_0PercentTip_7People_AssignsAllRemainderCentsToFirstPerson()
    {
        var result = TipSplitCalculator.Split(billAmount: 100m, tipPercent: 0m, peopleCount: 7);

        result.PerPerson.ShouldBe(new[] { 14.32m, 14.28m, 14.28m, 14.28m, 14.28m, 14.28m, 14.28m });
        result.PerPerson.Sum().ShouldBe(100m);
    }

    [Fact]
    public void Split_87_50Bill_10PercentTip_3People_ReturnsExpectedShares()
    {
        var result = TipSplitCalculator.Split(billAmount: 87.50m, tipPercent: 10m, peopleCount: 3);

        result.Total.ShouldBe(96.25m);
        result.PerPerson.ShouldBe(new[] { 32.09m, 32.08m, 32.08m });
        result.PerPerson.Sum().ShouldBe(result.Total);
    }

    [Fact]
    public void Split_0Bill_15PercentTip_3People_ReturnsAllZeroWithoutException()
    {
        var result = TipSplitCalculator.Split(billAmount: 0m, tipPercent: 15m, peopleCount: 3);

        result.TipAmount.ShouldBe(0m);
        result.Total.ShouldBe(0m);
        result.PerPerson.ShouldBe(new[] { 0m, 0m, 0m });
    }

    [Fact]
    public void Split_0People_ThrowsDomainException()
    {
        var exception = Should.Throw<TipSplitValidationException>(
            () => TipSplitCalculator.Split(billAmount: 100m, tipPercent: 0m, peopleCount: 0));

        exception.Message.ShouldBe("Anzahl Personen muss mindestens 1 sein.");
    }

    [Fact]
    public void Split_NegativePeople_ThrowsDomainException()
    {
        var exception = Should.Throw<TipSplitValidationException>(
            () => TipSplitCalculator.Split(billAmount: 100m, tipPercent: 0m, peopleCount: -1));

        exception.Message.ShouldBe("Anzahl Personen muss mindestens 1 sein.");
    }

    [Fact]
    public void Split_NegativeBillAmount_ThrowsDomainException()
    {
        var exception = Should.Throw<TipSplitValidationException>(
            () => TipSplitCalculator.Split(billAmount: -0.01m, tipPercent: 0m, peopleCount: 3));

        exception.Message.ShouldBe("Rechnungsbetrag darf nicht negativ sein.");
    }

    [Fact]
    public void Split_NegativeTipPercent_ThrowsDomainException()
    {
        var exception = Should.Throw<TipSplitValidationException>(
            () => TipSplitCalculator.Split(billAmount: 100m, tipPercent: -0.01m, peopleCount: 3));

        exception.Message.ShouldBe("Trinkgeld-Prozentsatz darf nicht negativ sein.");
    }
}
