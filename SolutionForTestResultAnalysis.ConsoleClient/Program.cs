using SolutionForTestResultAnalysis.Logic.Logic;

namespace SolutionForTestResultAnalysis
{
    internal class Program
    {
        const string PartialRetryText = ", press any key to retry";
        static void Main(string[] args)
        {
            Execute();
        }

        /// <summary>
        /// Executes the whole flow inside a never ending loop
        /// </summary>
        static void Execute()
        {
            while (true)
            {
                Console.WriteLine("Test Result Analysis - Code Challenge");
                PrintOptions();
                var option = ProcessInput();
                if(option == 0)
                {
                    WaitAndClear();
                    continue;
                }

                string jsonText = "";
                if ((ExecutionOptions)option == ExecutionOptions.ExecuteFromFile)
                {
                    var path = GetInputFilePath();
                    if (path is null)
                    {
                        WaitAndClear();
                        continue;
                    }
                    jsonText = FileHelper.ReadJsonFile(path);
                }
                else if ((ExecutionOptions)option == ExecutionOptions.ExecuteFromInputText)
                {
                    jsonText = GetInputFromConsole();
                }

                var outputPath = GetOutputPath();

                var result = ProcessFileAndShowResults(jsonText, outputPath);
                if(result is null)
                {
                    WaitAndClear();
                    continue;
                }
                else
                {
                    Console.WriteLine($"The following file was created: {result}");
                    Console.WriteLine("Please press any key to restart");
                    WaitAndClear();
                    continue;
                }
            }
        }

        /// <summary>
        /// Utility method to avoid repeating these 2 lines
        /// </summary>
        static void WaitAndClear()
        {
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Gets the input file (json) path when the user picks ExecuteFromFile option 
        /// </summary>
        /// <returns>A string representation of the json path</returns>
        static string GetInputFilePath()
        {
            Console.WriteLine("Enter the file's path:");
            var path = Console.ReadLine();

            if(!FileHelper.IsValidFilePath(path))
            {
                Console.WriteLine($"The specified path either is not valid or the file does not exist{PartialRetryText}");
                return null;
            }

            return path;
        }

        /// <summary>
        /// Gets the json file directly from the console, always the input is in a single line
        /// </summary>
        /// <returns>A string representation of the input json</returns>
        static string GetInputFromConsole()
        {
            Console.WriteLine("Enter the json in one line");
            return Console.ReadLine();
        }

        /// <summary>
        /// Instanciate TestRunAnalyzer which contains the actual file processing
        /// </summary>
        /// <param name="json">string representation of the input json</param>
        /// <param name="outputPath">Path where the final CSV file will be located</param>
        /// <returns>Returns the CSV filepath including the filename</returns>
        static string ProcessFileAndShowResults(string json, string outputPath)
        {
            var processor = new TestRunAnalyzer();
            if(!FileHelper.IsValidJson(json))
            {
                Console.WriteLine($"The specified json input is not valid{PartialRetryText}");
                return null;
            }
            var result = processor.ProcessFile(json, outputPath);
            PrintMetrics(processor);
            return result;
        }

        /// <summary>
        /// Prints all the metrics calculared in ProcessFileAndShowResults method
        /// </summary>
        /// <param name="processor"></param>
        static void PrintMetrics(TestRunAnalyzer processor)
        {
            var metrics = processor.GetType()
                .GetProperties()
                .Select(p => $"{GetSpacedText(p.Name)}: {p.GetValue(processor)}");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Test metrics:");
            Console.WriteLine(string.Join(Environment.NewLine, metrics));
            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Gets the output path to locate the final CSV. Allows to select from a default path
        /// inside the running directory of this program, or a user input path
        /// </summary>
        /// <returns>A string path where the CSV file will be located</returns>
        static string GetOutputPath()
        {
            string outputPath;
            do
            {
                Console.WriteLine("Do you want to use the default path to locate the output file? (Y/N)");

                outputPath = Console.ReadLine() switch
                {
                    "Y" => Directory.GetCurrentDirectory(),
                    "N" => GetOutputPathFromUserInput(),
                    _ => InvalidSelection()
                };
            } while (outputPath is null);

            string InvalidSelection()
            {
                Console.WriteLine($"The selected option is not valid, lets try again");

                return null;
            }

            return outputPath;
        }

        /// <summary>
        /// Gets the final CSV location directly from the user input
        /// </summary>
        /// <returns>A string path where the CVS will be located</returns>
        static string GetOutputPathFromUserInput()
        {
            Console.WriteLine("Please enter the path to save the output file");
            var userInput = Console.ReadLine();
            if(FileHelper.IsValidPath(userInput))
            {
                return userInput;
            }
            else
            {
                Console.WriteLine($"The specified path is not valid{PartialRetryText}");
                return null;
            }
        }

        /// <summary>
        /// Gets and validates user input. Specifically at the "select option" momment when the application is just starting
        /// </summary>
        /// <returns></returns>
        static int ProcessInput()
        {
            Console.WriteLine("Choose the numeric option you want to execute, then press enter");
            var input = Console.ReadLine();
            var parsedOption = ValidateOption(input);
            return parsedOption;
        }

        /// <summary>
        /// Auxiliar method to print all options inside the ExecutionOptions enum
        /// </summary>
        static void PrintOptions()
        {
            var array = (ExecutionOptions[])Enum.GetValues(typeof(ExecutionOptions));
            var options = string.Join(Environment.NewLine, array.Select(x => $"- {(int)x} - {GetSpacedText(x.ToString())}"));
            Console.WriteLine(options);
        }

        /// <summary>
        /// Auxiliar method to Pascal property names separated by space every time there is a capital letter
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A string, space separated, property name</returns>
        static string GetSpacedText(string text)
        {
            return string.Join("", text.Select((x, i) => char.IsUpper(x) && i != 0 ? $" {x}" : x.ToString()));
        }

        /// <summary>
        /// Validates ExecutionOptions
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        static int ValidateOption(string option)
        {
            var isNumber = int.TryParse(option, out int numericOption);
            if (!isNumber)
            {
                Console.WriteLine($"Only numeric options allowed{PartialRetryText}");
                return 0;
            }

            if (!Enum.IsDefined(typeof(ExecutionOptions), numericOption))
            {
                Console.WriteLine($"The option you just entered is either non implemented or not allowed{PartialRetryText}");
                return 0;
            }

            return numericOption;
        }
    }
}