using Shouldly;
using TipSplitter.Application;
using TipSplitter.Domain;

namespace TipSplitter.Tests.Application;

public class TipSplitServiceTests
{
    [Fact]
    public void Split_ValidInput_ReturnsResponseMatchingDomainResult()
    {
        var response = TipSplitService.Split(new TipSplitRequest(Amount: 87.50m, TipPercent: 10m, People: 3));

        response.TotalTip.ShouldBe(8.75m);
        response.PerPerson.ShouldBe(new[] { 32.09m, 32.08m, 32.08m });
    }

    [Fact]
    public void Split_InvalidPeopleCount_PropagatesDomainException()
    {
        var exception = Should.Throw<TipSplitValidationException>(
            () => TipSplitService.Split(new TipSplitRequest(Amount: 100m, TipPercent: 0m, People: 0)));

        exception.Message.ShouldBe("Anzahl Personen muss mindestens 1 sein.");
    }
}
