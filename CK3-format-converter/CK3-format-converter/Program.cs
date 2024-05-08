//using System;
//using System.IO;
//using System.Collections.Generic;

//class Program
//{
//    static void Main(string[] args)
//    {
//        Console.WriteLine("Enter the path to the first directory:");
//        string dir1 = Console.ReadLine();
//        Console.WriteLine("Enter the path to the second directory:");
//        string dir2 = Console.ReadLine();
//        Console.WriteLine("Enter the path to the third directory where to copy files:");
//        string dir3 = Console.ReadLine();

//        if (!Directory.Exists(dir1) || !Directory.Exists(dir2))
//        {
//            Console.WriteLine("One or both directories do not exist.");
//            return;
//        }

//        // Create third directory if it does not exist
//        Directory.CreateDirectory(dir3);

//        try
//        {
//            Dictionary<string, string> fileMap = BuildFileMap(dir2);
//            var filesDir1 = Directory.GetFiles(dir1, "*.*", SearchOption.AllDirectories);

//            foreach (var file in filesDir1)
//            {
//                string fileName = Path.GetFileName(file);
//                if (fileMap.ContainsKey(fileName))
//                {
//                    string relativePath = fileMap[fileName];
//                    string targetPath = Path.Combine(dir3, relativePath);

//                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
//                    File.Copy(file, targetPath, true);

//                    Console.WriteLine($"Copied {file} to {targetPath}");
//                }
//                else
//                {
//                    Console.WriteLine($"No matching file found in {dir2} for {file}");
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("An error occurred: " + ex.Message);
//        }

//        Console.WriteLine("Operation completed. Press any key to exit.");
//        Console.ReadKey();
//    }

//    // Build a map of filenames to their relative paths within the directory
//    static Dictionary<string, string> BuildFileMap(string directory)
//    {
//        var result = new Dictionary<string, string>();
//        var allFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

//        foreach (var file in allFiles)
//        {
//            string fileName = Path.GetFileName(file);
//            string relativePath = Path.GetRelativePath(directory, file);
//            if (!result.ContainsKey(fileName))
//            {
//                result.Add(fileName, relativePath);
//            }
//            else
//            {
//                Console.WriteLine($"Duplicate file name found: {fileName} at {relativePath}. Only the first occurrence will be mapped.");
//            }
//        }

//        return result;
//    }
//}



using System.Text;
using System.Text.RegularExpressions;

Console.WriteLine("Enter the path to the directory containing YAML files:");
string directoryPath = Console.ReadLine();

// Remove any double quotes that might encase the directory path
directoryPath = directoryPath.Trim('"');

if (!Directory.Exists(directoryPath))
{
    Console.WriteLine("Directory does not exist.");
    return;
}


//var files3 = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
//foreach (var file in files3)
//{
//    string[] lines = File.ReadAllLines(file);
//    bool hasProblem = false;
//    for (int i = 0; i < lines.Length; i++)
//    {
//        if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].StartsWith(" ") && !lines[i].StartsWith("\t") &&
//            !lines[i].StartsWith("#") && !IsLanguageFirstLine(lines[i]))
//        {
//            lines[i] = "  " + lines[i];
//            string relativePath = Path.GetRelativePath(directoryPath, file);
//            if (!hasProblem)
//            {
//                hasProblem = true;
//                Console.WriteLine($"File: {relativePath}");
//            }

//            Console.WriteLine($"    Line {i + 1}: {lines[i]}");
//        }

//    }

//    File.WriteAllLines(file, lines);
//}

//Console.WriteLine("Files processed. Changes logged to editLog.txt");
//Console.WriteLine("Press any key to exit.");
//Console.ReadKey();

//// Recursively get all files in the directory and subdirectories
//var files1 = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
//Console.WriteLine("Scanning files...");
//bool found1 = false;

//foreach (var file in files1)
//{
//    if (Contains4spaces(file))
//    {
//        Console.WriteLine(file);
//        found1 = true;
//    }
//}

//if (!found1)
//{
//    Console.WriteLine("No files containing Cyrillic letters were found.");
//}

//return;


Console.WriteLine("Select operation mode:");
Console.WriteLine("1: Correct format (increase indentation, remove numbers - prepare for Crowdin)");
Console.WriteLine("2: Revert format (decrease indentation - prepare for game)");
Console.WriteLine("3: Copy all cyrillic files (to load only translated files)");
Console.WriteLine("4: Copy all localization source files (to get all strings from game)");
string mode = Console.ReadLine();

if (mode != "1" && mode != "2" && mode != "3" && mode != "4")
{
    Console.WriteLine("Invalid mode selected. Exiting...");
    return;
}

if (mode == "3")
{
    try
    {
        string newRootDirectory = Path.Combine(Path.GetDirectoryName(directoryPath), "cyrillic_" + Path.GetFileName(directoryPath));
        Directory.CreateDirectory(newRootDirectory);

        // Recursively get all files in the directory and subdirectories
        var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
        Console.WriteLine("Scanning files...");
        bool found = false;
        List<string> cyrillicFiles = new List<string>(files.Length);

        foreach (var file in files)
        {
            if (ContainsCyrillic(file))
            {
                Console.WriteLine(file);
                cyrillicFiles.Add(file);
                CopyFileToNewDirectory(file, directoryPath, newRootDirectory);
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No files containing Cyrillic letters were found.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }

    Console.WriteLine("Operation completed. Press any key to exit.");
    Console.ReadKey();
    return;
}

if (mode == "4")
{
    string newRootDirectory = Path.Combine(Path.GetDirectoryName(directoryPath), "for_translation_" + Path.GetFileName(directoryPath));
    Directory.CreateDirectory(newRootDirectory);

    // Recursively get all files in the directory and subdirectories
    var files = Directory.GetFiles(directoryPath, "*.yml", SearchOption.AllDirectories);
    Console.WriteLine("Scanning files...");
    bool found = false;

    foreach (var file in files)
    {
        string changedPath = file;
        if (Contains(file, ref changedPath))
        {
            Console.WriteLine(file);
            CopyFileForTranslation(file, changedPath, directoryPath, newRootDirectory);
            found = true;
        }
    }


    if (!found)
    {
        Console.WriteLine("No files containing Cyrillic letters were found.");
    }

    Console.WriteLine("Operation completed. Press any key to exit.");
    Console.ReadKey();
    return;
}


// Creating a root corrected directory at the same level as the input directory
string correctedRootDirectory = Path.Combine(Directory.GetParent(directoryPath).FullName, (mode == "1" ? "corrected_" : "reverted_") + Path.GetFileName(directoryPath));
Directory.CreateDirectory(correctedRootDirectory);

// Get all YAML files in the directory and subdirectories
string[] allFilePaths = Directory.GetFiles(directoryPath, "*.yml", SearchOption.AllDirectories);
string[] yamlPaths = Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories);
Array.Resize(ref allFilePaths, allFilePaths.Length + yamlPaths.Length);
yamlPaths.CopyTo(allFilePaths, allFilePaths.Length - yamlPaths.Length);

if (allFilePaths.Length == 0)
{
    Console.WriteLine("No YAML files found in the directory.");
    return;
}

foreach (var filePath in allFilePaths)
{
    Console.WriteLine($"Processing file: {filePath}");

    // Calculate new file path by replacing original directory path with the corrected root directory path
    string newFilePath = filePath.Replace(directoryPath, correctedRootDirectory);
    string directoryForNewFile = Path.GetDirectoryName(newFilePath);
    Directory.CreateDirectory(directoryForNewFile);  // Ensure the directory structure exists

    string processedContent = mode == "1" ? IncreaseIndentationByLine(filePath, directoryPath) : DecreaseIndentation(filePath);
    File.WriteAllText(newFilePath, processedContent, new UTF8Encoding(true));
    Console.WriteLine($"{(mode == "1" ? "Corrected" : "Reverted")} file saved as {newFilePath}");
}

Console.WriteLine("All files processed. Press any key to exit.");
Console.ReadKey();

static string IncreaseIndentation(string filePath)
{
    string content = File.ReadAllText(filePath);
    string indentedContent = Regex.Replace(content, @"^( +)", m => new string(' ', m.Groups[1].Value.Length * 2), RegexOptions.Multiline);


    // Remove numbers and colons immediately following the keys
    string cleanedContent = Regex.Replace(indentedContent, @"([\w\-]+):(\d+)\s+\""", "$1: \"");

    string cleanOldTranslations = Regex.Replace(cleanedContent, @"^( *)(\"")(\#)", "$1$3", RegexOptions.Multiline);
    return cleanOldTranslations;
}

static string IncreaseIndentationByLine(string filePath, string directoryPath)
{
    string[] lines = File.ReadAllLines(filePath);
    bool hasProblem = false;
    string relativePath = Path.GetRelativePath(directoryPath, filePath);

    string logFile = Path.Combine(Directory.GetParent(directoryPath).FullName, "log_" + Path.GetFileName(directoryPath) + ".txt");

    using (StreamWriter logWriter = new StreamWriter(logFile, true)) // Open the log file to append
    {

        for (int i = 0; i < lines.Length; i++)
        {
            // Replaces 1 space with 2 on the start of the line
            lines[i] = Regex.Replace(lines[i], @"^( +)", "  ");

            // Removes numbers at the end of a key
            lines[i] = Regex.Replace(lines[i], @"([\w\-]+):(\d+)\s+\""", "$1: \"");

            // Removes " before #
            lines[i] = Regex.Replace(lines[i], @"^( *)(\"")(\#)", "$1$3");


            //lines[i] = Regex.Replace(lines[i], @"\""", ""); // Remove all double quotes
            //lines[i] = Regex.Replace(lines[i], @"[^\w\s\p{P}]", ""); // Remove any non-word, non-whitespace, non-punctuation character



            var line = lines[i].Trim();
            // Pass lines with no " in the end
            if (!string.IsNullOrWhiteSpace(lines[i]) &&
                !line.StartsWith("#") && !IsLanguageFirstLine(lines[i]) && !lines[i].EndsWith("\""))
            {
                // Adds " to the end
                //lines[i] = lines[i] + "\"";


                if (!hasProblem)
                {
                    hasProblem = true;
                    Log($"File: {relativePath}", logWriter);
                }

                Log($"    Not handled - Line {i + 1}: {lines[i]}", logWriter);
            }

            // Remove non sense lines
            if (!string.IsNullOrWhiteSpace(lines[i]) &&
                (lines[i] == ":\"\"" ||
                 lines[i] == "\"\"|\"\"" ||
                 lines[i] == "\"\";\"\"" ||
                 lines[i] == "\"\".\"\"" ||
                 lines[i] == "\"\",\"\"" ||
                 (lines[i].Contains("\"\"|\"\"|\"\"") ||
                 lines[i].Contains("\"\",\"\",\"\"") ||
                 lines[i].Contains("\"\".\"\".\"\""))))
            {
                if (!hasProblem)
                {
                    hasProblem = true;
                    Log($"File: {relativePath}", logWriter);
                }

                Log($"    To remove - Line {i + 1}: {lines[i]}", logWriter);

                //lines[i] = Environment.NewLine;

            }

            // New logic to add indentation if the line does not start with space, tab, or specific strings
            if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].StartsWith(" ") && !lines[i].StartsWith("\t") &&
                !lines[i].StartsWith("#") && !IsLanguageFirstLine(lines[i]))
            {
                // Final logic
                lines[i] = "  " + lines[i];


                if (!hasProblem)
                {
                    hasProblem = true;
                    Log($"File: {relativePath}", logWriter);
                }

                Log($"    Incorrect space - Line {i + 1}: {lines[i]}", logWriter);
            }
        }
    }

    return string.Join(Environment.NewLine, lines);
}

static bool IsLanguageFirstLine(string line)
{
    line = line.TrimEnd();
    return line.Equals("l_english:") ||
           line.Equals("l_french:") ||
           line.Equals("l_german:") ||
           line.Equals("l_spanish:") ||
           line.Equals("l_simp_chinese:") ||
           line.Equals("l_russian:") ||
           line.Equals("l_korean:");
}

static string DecreaseIndentation(string filePath)
{
    string content = File.ReadAllText(filePath);
    return Regex.Replace(content, @"^( +)", m => new string(' ', (int)Math.Ceiling(m.Groups[1].Value.Length / 2.0)), RegexOptions.Multiline);
}

static bool ContainsCyrillic(string filePath)
{
    try
    {
        string content = File.ReadAllText(filePath);
        return Regex.IsMatch(content, @"\p{IsCyrillic}");
    }
    catch
    {
        // Ignore files that cannot be read
        return false;
    }
}

static bool Contains4spaces(string filePath)
{
    try
    {
        string content = File.ReadAllText(filePath);
        return content.Contains("    ");
    }
    catch
    {
        // Ignore files that cannot be read
        return false;
    }
}

static void CopyFileToNewDirectory(string filePath, string originalRoot, string newRoot)
{
    string relativePath = Path.GetRelativePath(originalRoot, filePath);
    string newFilePath = Path.Combine(newRoot, relativePath);
    string newFileDir = Path.GetDirectoryName(newFilePath);

    if (!Directory.Exists(newFileDir))
    {
        Directory.CreateDirectory(newFileDir);
    }

    File.Copy(filePath, newFilePath, true);
    Console.WriteLine($"Copied to: {newFilePath}");
}


static void CopyFileForTranslation(string filePath, string changedFilePath, string originalRoot, string newRoot)
{
    string relativePath = Path.GetRelativePath(originalRoot, changedFilePath);
    string newFilePath = Path.Combine(newRoot, relativePath);
    string newFileDir = Path.GetDirectoryName(newFilePath);

    if (!Directory.Exists(newFileDir))
    {
        Directory.CreateDirectory(newFileDir);
    }

    File.Copy(filePath, newFilePath, true);
    Console.WriteLine($"Copied to: {newFilePath}");
}

bool Contains(string s, ref string changedFilePath)
{
    if (s.Contains("l_english") || s.Contains("languages.yml"))
    {
        changedFilePath = changedFilePath.Replace("game\\common", String.Empty);
        changedFilePath = changedFilePath.Replace("game\\localization", String.Empty);
        changedFilePath = changedFilePath.Replace("clausewitz\\localization", String.Empty);
        changedFilePath = changedFilePath.Replace("jomini\\localization", String.Empty);
        changedFilePath = changedFilePath.Replace("jomini\\", String.Empty);
        return true;
    }

    return false;
}

static void Log(string line, StreamWriter file)
{
    Console.WriteLine(line);
    file.WriteLine(line);
}