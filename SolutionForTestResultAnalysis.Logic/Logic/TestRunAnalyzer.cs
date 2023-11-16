using SolutionForTestResultAnalysis.Logic.DataModel;
using System.Text.Json;

namespace SolutionForTestResultAnalysis.Logic.Logic
{
    /// <summary>
    /// This class contains the json processor methods to get the metric and execute the new file saving
    /// </summary>
    public class TestRunAnalyzer
    {
        #region metrics
        public int TotalNumerOfTestCasesExecuted { get; set; }
        public int NumberOfTestCasesPassed { get; set; }
        public int NumberOfTestCasesFailed { get; set; }
        public double AverageExecutionTimeForAllTestCases { get; set; }
        public int MinimunExecutionTimeAmongAllTestCases { get; set; }
        public int MaximumExecutionTimeAmongAllTestCases { get; set; }
        #endregion

        /// <summary>
        /// Exposes the actual flow outside of thhis class
        /// </summary>
        /// <param name="json">string representation of the json file content</param>
        /// <param name="outputPath">Path where the final CSV file will be created</param>
        /// <returns>Path where the CSV file got created</returns>
        public string ProcessFile(string json, string outputPath)
        {
            var ranTestCases = DeserializeTestRunResults(json);
            SetMetrics(ranTestCases);
            var csvText = ToCSVText(ranTestCases);
            var outputFile = GetFilePath(outputPath);
            FileHelper.WriteFile(outputFile, csvText);
            return outputFile;
        }

        /// <summary>
        /// Combines the output path for the CSV file with a filename that contains the current date and time in UTC
        /// </summary>
        /// <param name="outputPath">The output path for the CSV file</param>
        /// <returns>Output file for the CVS file including the file name</returns>
        private string GetFilePath(string outputPath) => Path.Combine(outputPath, $"TestResults-{DateTime.UtcNow.ToString("M-dd-yyyy h-mm-ss tt")}.csv");

        /// <summary>
        /// Calculates the metrics and sets their values into public properties
        /// </summary>
        /// <param name="testCaseList">List of the testcases deseralized from the json file</param>
        private void SetMetrics(IEnumerable<TestCase> testCaseList)
        {
            TotalNumerOfTestCasesExecuted = testCaseList.Count();
            NumberOfTestCasesPassed = testCaseList.Count(t => t.TestResult == TestResult.Pass);
            NumberOfTestCasesFailed = testCaseList.Count(t => t.TestResult == TestResult.Fail);
            AverageExecutionTimeForAllTestCases = testCaseList.Average(t => t.ExecutionTimeSeconds);
            MinimunExecutionTimeAmongAllTestCases = testCaseList.Min(t => t.ExecutionTimeSeconds);
            MaximumExecutionTimeAmongAllTestCases = testCaseList.Max(t => t.ExecutionTimeSeconds);
        }

        /// <summary>
        /// Deserializes de json file
        /// </summary>
        /// <param name="testRunResult">string representation of the json, containing the test run results</param>
        /// <returns>IEnumerable of the test cases contained in the json file</returns>
        private IEnumerable<TestCase> DeserializeTestRunResults(string testRunResult)
        {
            return JsonSerializer.Deserialize<IEnumerable<TestCase>>(testRunResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Builds the csv file content based on the deserialized list of test case results
        /// </summary>
        /// <param name="testCaseList"></param>
        /// <returns></returns>
        private string ToCSVText(IEnumerable<TestCase> testCaseList)
        {
            var propertyNames = GetPropertyNames(testCaseList);
            if (!propertyNames.Any())
                return string.Empty;

            var header = string.Join(",", propertyNames);
            var body = string.Join(Environment.NewLine, testCaseList.Select(x => x.ToString()));

            return string.Join(Environment.NewLine, header, body);
        }

        /// <summary>
        /// Gets a list of the property names of the test case class
        /// </summary>
        /// <param name="testCaseList"></param>
        /// <returns></returns>
        private IEnumerable<string> GetPropertyNames(IEnumerable<TestCase> testCaseList)
        {
            return testCaseList.FirstOrDefault()?
                            .GetType()
                            .GetProperties()
                            .Select(p => p.Name) ?? Enumerable.Empty<string>();
        }
    }
}
