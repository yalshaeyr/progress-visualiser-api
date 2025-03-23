using ProgressVisualiserApi.Database.Models;
using ProgressVisualiserApi.Tests.Helpers;

namespace ProgressVisualiserApi.Tests.IntegrationTests;

public class ProgressVisualiserApi_MetricDataTests(ProgressVisualiserApiWebApplicationFactory factory) :
    IClassFixture<ProgressVisualiserApiWebApplicationFactory>
{
    private readonly ProgressVisualiserApiWebApplicationFactory _factory = factory;

    [Fact]
    public async Task GetMetricData_ReturnsSeededMetricData()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/metricdata", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var responseJson = Utilities.DeserializeResponse<MetricData>(responseString);
        var expectedJson = Utilities.GetMetricDataSeedingData();
        Assert.Equivalent(expectedJson, responseJson);
    }
}
