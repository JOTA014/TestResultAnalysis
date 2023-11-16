using System.Text.Json;

namespace SolutionForTestResultAnalysis.Logic.Logic
{
    public static class FileHelper
    {
        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static bool IsValidFilePath(string path)
        {
            return File.Exists(path);
        }
        public static bool IsValidPath(string path)
        {
            return Path.Exists(path);
        }

        public static bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
