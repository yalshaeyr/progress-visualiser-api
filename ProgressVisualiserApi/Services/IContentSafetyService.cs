namespace ProgressVisualiserApi.Services
{
    public interface IContentSafetyService
    {
        /// <summary>
        /// Analyzes the provided content and determines if it is safe.
        /// </summary>
        /// <param name="content">The content to analyze.</param>
        /// <returns>A boolean indicating whether the content is safe.</returns>
        Task<bool> IsContentSafeAsync(string content);
    }
}