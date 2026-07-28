using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using TipSplitter.Api.Controllers;

namespace TipSplitter.Tests.Api;

public class SplitEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SplitEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ValidRequest_Returns200WithExpectedBody()
    {
        var response = await _client.PostAsJsonAsync("/split", new SplitRequest(Amount: 87.5m, TipPercent: 10m, People: 3));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<SplitResponse>();
        body.ShouldNotBeNull();
        body.TotalTip.ShouldBe(8.75m);
        body.PerPerson.ShouldBe(new[] { 32.09m, 32.08m, 32.08m });
    }

    [Fact]
    public async Task Post_ZeroPeople_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/split", new SplitRequest(Amount: 100m, TipPercent: 0m, People: 0));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_NegativeAmount_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/split", new SplitRequest(Amount: -1m, TipPercent: 0m, People: 3));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_NegativeTipPercent_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/split", new SplitRequest(Amount: 100m, TipPercent: -5m, People: 3));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_MissingField_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/split", new { tipPercent = 10m, people = 3 });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_ValidRequest_RespondsWithinOneSecond()
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await _client.PostAsJsonAsync("/split", new SplitRequest(Amount: 87.5m, TipPercent: 10m, People: 3));

        stopwatch.Stop();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        stopwatch.Elapsed.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }
}
