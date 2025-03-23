using ProgressVisualiserApi.Database.Models;
using ProgressVisualiserApi.Tests.Helpers;

namespace ProgressVisualiserApi.Tests.IntegrationTests;

public class ProgressVisualiserApi_MetricTests(ProgressVisualiserApiWebApplicationFactory factory) :
    IClassFixture<ProgressVisualiserApiWebApplicationFactory>
{
    private readonly ProgressVisualiserApiWebApplicationFactory _factory = factory;

    [Fact]
    public async Task GetMetrics_ReturnsSeededMetrics()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/metrics", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var responseJson =  Utilities.DeserializeResponse<Metric>(responseString);
        var expectedJson = Utilities.GetMetricSeedingData();
        Assert.Equivalent(expectedJson, responseJson);
    }
}