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
}
