using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolutionForTestResultAnalysis.Logic.DataModel
{
    public class TestCase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("ExecutionTime")]
        public int ExecutionTimeSeconds { get; set; }
        public DateTime TimeStamp { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TestResult TestResult { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {ExecutionTimeSeconds}, {TimeStamp}, {TestResult}";
        }
    }
}
