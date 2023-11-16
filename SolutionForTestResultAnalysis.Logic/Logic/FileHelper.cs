using System.Text.Json;

namespace SolutionForTestResultAnalysis.Logic.Logic
{
    /// <summary>
    /// Helper which contains File related operations
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Reads json file based on the path to the file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A string representation of the json file content</returns>
        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Validates if the file exist or not
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True or false, on dependence of the file existance</returns>
        public static bool IsValidFilePath(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Validates if the path exist or not
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True or false, on dependence of the path existance</returns>
        public static bool IsValidPath(string path)
        {
            return Path.Exists(path);
        }

        /// <summary>
        /// Validates json structure
        /// </summary>
        /// <param name="json"></param>
        /// <returns>True or false on dependence of the json being valid or not</returns>
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

        /// <summary>
        /// Writes the specified content in a file that will be in the specified path
        /// </summary>
        /// <param name="path">Path to create the file</param>
        /// <param name="content">Content to write in the file</param>
        public static void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
