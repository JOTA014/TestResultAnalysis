using SolutionForTestResultAnalysis.Logic.DataModel;
using System.Text.Json;

namespace SolutionForTestResultAnalysis.Logic.Logic
{
    public class TestRunAnalyzer
    {
        public int TotalNumerOfTestCasesExecuted { get; set; }
        public int NumberOfTestCasesPassed { get; set; }
        public int NumberOfTestCasesFailed { get; set; }
        public double AverageExecutionTimeForAllTestCases { get; set; }
        public int MinimunExecutionTimeAmongAllTestCases { get; set; }
        public int MaximumExecutionTimeAmongAllTestCases { get; set; }

        public string ProcessFile(string json, string outputPath)
        {
            var ranTestCases = DeserializeTestRunResults(json);
            SetMetrics(ranTestCases);
            var csvText = ToCSVText(ranTestCases);
            var outputFile = GetFilePath(outputPath);
            FileHelper.WriteFile(outputFile, csvText);
            return outputFile;
        }

        private string GetFilePath(string outputPath) => Path.Combine(outputPath, $"TestResults-{DateTime.UtcNow.ToString("M-dd-yyyy h-mm-ss tt")}.csv");

        private void SetMetrics(IEnumerable<TestCase> testCaseList)
        {
            TotalNumerOfTestCasesExecuted = testCaseList.Count();
            NumberOfTestCasesPassed = testCaseList.Count(t => t.TestResult == TestResult.Pass);
            NumberOfTestCasesFailed = testCaseList.Count(t => t.TestResult == TestResult.Fail);
            AverageExecutionTimeForAllTestCases = testCaseList.Average(t => t.ExecutionTimeSeconds);
            MinimunExecutionTimeAmongAllTestCases = testCaseList.Min(t => t.ExecutionTimeSeconds);
            MaximumExecutionTimeAmongAllTestCases = testCaseList.Max(t => t.ExecutionTimeSeconds);
        }

        private IEnumerable<TestCase> DeserializeTestRunResults(string testRunResult)
        {
            return JsonSerializer.Deserialize<IEnumerable<TestCase>>(testRunResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private string ToCSVText(IEnumerable<TestCase> testCaseList)
        {
            var propertyNames = GetPropertyNames(testCaseList);
            if (!propertyNames.Any())
                return string.Empty;

            var header = string.Join(",", propertyNames);
            var body = string.Join(Environment.NewLine, testCaseList.Select(x => x.ToString()));

            return string.Join(Environment.NewLine, header, body);
        }

        private IEnumerable<string> GetPropertyNames(IEnumerable<TestCase> testCaseList)
        {
            return testCaseList.FirstOrDefault()?
                            .GetType()
                            .GetProperties()
                            .Select(p => p.Name) ?? Enumerable.Empty<string>();
        }
    }
}
