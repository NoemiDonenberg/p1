
//using System.CommandLine;
//using System.CommandLine.Invocation;

//var languageOption = new Option<string>("--language", "Programming language");
//var bundleOption = new Option<FileInfo>("--output", "File path and name");
//var bundleCommand = new Command("bundle", "Bundle code file to a single file");
//bundleCommand.AddOption(bundleOption);
//bundleCommand.SetHandler((output) =>
//{
//    File.Create(output.FullName);
//    Console.WriteLine("File was created");
//}, bundleOption);
//var rootCommnd = new RootCommand("Root command for file bundler CLA");
//rootCommnd.AddCommand(bundleCommand);
//rootCommnd.InvokeAsync(args);
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var bundleOption = new Option<string>("--output", "File path and name");

        var languageOption = new Option<string>("--language", "Programming language to include in the bundle");
        languageOption.IsRequired = true;
        var bundleCommand = new Command("bundle", "Bundle code files to a single file");
        bundleCommand.AddOption(bundleOption);
        bundleCommand.AddOption(languageOption);
        bundleCommand.Handler = CommandHandler.Create<string, string, string>((output, directoryPath, language) =>
        {

            BundleFiles(output, directoryPath, language);
        });
        //bundleCommand.SetHandler((output) =>
        //{
        //    File.Create(output.FullName);
        //    Console.WriteLine("File was created");
        //}, bundleOption);
        var rootCommand = new RootCommand("Root command for file Bundler CLI");
        rootCommand.AddCommand(bundleCommand);

        rootCommand.InvokeAsync(args).Wait();
    }

    static void BundleFiles(string output, string directoryPath, string language)
    {
        // המקור של הקובץ
        string sourceComment = $"// Source: {Path.Combine(directoryPath, output)}";

        string fullPath;

        if (Path.IsPathRooted(output))
        {
            // אם הקלט הוא נתיב מלא
            fullPath = output;
        }
        else
        {
            // אם הקלט הוא שם קובץ בלבד
            fullPath = Path.Combine(directoryPath, output);
        }

        // פתיחת קובץ עם אפשרות להוסיף ולכתוב
        using (StreamWriter sw = File.AppendText(fullPath))
        {
            // הוספת הערה לקובץ
            sw.WriteLine(sourceComment);
            sw.WriteLine(); // שורה ריקה לסיום הערה
        }
    }
}