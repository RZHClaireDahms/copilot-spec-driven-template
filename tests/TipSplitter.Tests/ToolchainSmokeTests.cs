using Shouldly;

namespace TipSplitter.Tests;

public class ToolchainSmokeTests
{
    [Fact]
    public void Toolchain_Should_RunWithShouldlyAssertions()
    {
        true.ShouldBeTrue();
    }
}
