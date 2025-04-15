using Azure;
using Azure.AI.ContentSafety;

namespace ProgressVisualiserApi.Services
{
    public class ContentSafetyService(IConfiguration configuration, ILogger<ContentSafetyService> logger)
        : IContentSafetyService
    {
        private readonly string _endpoint = configuration["AzureContentSafety:Endpoint"] ?? "";
        private readonly string _key = configuration["AzureContentSafety:Key"] ?? "";

        private readonly ILogger<ContentSafetyService> _logger = logger;

        public async Task<bool> IsContentSafeAsync(string content)
        {
            ContentSafetyClient client = new(new Uri(_endpoint), new AzureKeyCredential(_key));

            var request = new AnalyzeTextOptions(content);
            Response<AnalyzeTextResult> response;

            try 
            {
                response = await client.AnalyzeTextAsync(request);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error analysing text: {Content}, Error: {Message}", content, ex.Message);
                return false;
            }
            
            for (int i = 0; i < response.Value.CategoriesAnalysis.Count; i++)
            {
                // see https://learn.microsoft.com/en-us/azure/ai-services/content-safety/concepts/harm-categories?tabs=definitions#text-content
                // for severity definitions
                if (response.Value.CategoriesAnalysis[i].Severity >= 2)
                {
                    _logger.Log(
                        LogLevel.Information, 
                        "Analysed text: {Content}, Category: {Category}, Severity: {Severity}", 
                        content, response.Value.CategoriesAnalysis[i].Category, response.Value.CategoriesAnalysis[i].Severity
                    );
                    return false;
                }
            }
            
            return true;
        }

    }
}