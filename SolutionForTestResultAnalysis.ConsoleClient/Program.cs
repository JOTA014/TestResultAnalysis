using SolutionForTestResultAnalysis.Logic.Logic;
using System.Reflection.Metadata;

namespace SolutionForTestResultAnalysis
{
    internal class Program
    {
        const string PartialRetryText = ", press any key to retry";
        static void Main(string[] args)
        {
            Execute();
        }

        private static void Execute()
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

        private static void WaitAndClear()
        {
            Console.ReadKey();
            Console.Clear();
        }

        private static string GetInputFilePath()
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

        static string GetInputFromConsole()
        {
            Console.WriteLine("Enter the json in one line");
            return Console.ReadLine();
        }

        private static string ProcessFileAndShowResults(string json, string outputPath)
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

        private static void PrintMetrics(TestRunAnalyzer processor)
        {
            var metrics = processor.GetType()
                .GetProperties()
                .Select(p => $"{GetSpacedText(p.Name)}: {p.GetValue(processor)}");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Test metrics:");
            Console.WriteLine(string.Join(Environment.NewLine, metrics));
            Console.WriteLine(Environment.NewLine);
        }

        private static string GetOutputPath()
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

        private static string GetOutputPathFromUserInput()
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

        private static int ProcessInput()
        {
            Console.WriteLine("Choose the numeric option you want to execute, then press enter");
            var input = Console.ReadLine();
            var parsedOption = ValidateOption(input);
            return parsedOption;
        }

        static void PrintOptions()
        {
            var array = (ExecutionOptions[])Enum.GetValues(typeof(ExecutionOptions));
            var options = string.Join(Environment.NewLine, array.Select(x => $"- {(int)x} - {GetSpacedText(x.ToString())}"));
            Console.WriteLine(options);
        }

        static string GetSpacedText(string text)
        {
            return string.Join("", text.Select((x, i) => char.IsUpper(x) && i != 0 ? $" {x}" : x.ToString()));
        }

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