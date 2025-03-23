using System.Text.Json;
using ProgressVisualiserApi.Database;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Tests.Helpers
{
    public static class Utilities
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static void InitializeDbForTests(ProgressVisualiserApiContext context)
        {
            context.Users.AddRange(GetUsersSeedingData());
            context.Metrics.AddRange(GetMetricSeedingData());
            context.MetricData.AddRange(GetMetricDataSeedingData());
            context.SaveChanges();
        }

        public static List<T> DeserializeResponse<T>(string responseString)
        {
            return JsonSerializer.Deserialize<List<T>>(responseString, jsonSerializerOptions) ?? [];
        }

        public static List<User> GetUsersSeedingData()
        {
            return
            [
                new User
                {
                    Id = 1,
                    Username = "user1",
                    CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
                }
            ];
        }

        public static List<Metric> GetMetricSeedingData()
        {
            return
            [
                new Metric
                {
                    Id = 1,
                    Name = "metric1",
                    Description = "description1",
                    Unit = "unit1",
                    CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UserId = 1
                }
            ];
        }

        public static List<MetricData> GetMetricDataSeedingData()
        {
            return
            [
                new MetricData
                {
                    Id = 1,
                    Value = 1.0,
                    MetricId = 1,
                    RecordedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
                }
            ];
        }
    }
}