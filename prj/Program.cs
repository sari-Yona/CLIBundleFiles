    using System.CommandLine;
    using System;
    using System.IO;
    using System.Reflection;
    using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;


string[] fileExtensions = new string[] { ".sql", ".java", ".js", ".c++", ".c#" };
string[] fileCodes = new string[] { "sql", "java", "js", "c++", "c#" };

var bundleCommand = new Command("bundle", "Bundle code files for a single page");

var authorOption = new Option<string>("--author", "write author name");
bundleCommand.AddOption(authorOption);
authorOption.AddAlias("-a");


var linesOption = new Option<bool>("--empty-lines", "remove empty lines");
bundleCommand.AddOption(linesOption);
linesOption.AddAlias("-e");


var noteOption = new Option<bool>("--note", "writing files paths");
bundleCommand.AddOption(noteOption);
noteOption.AddAlias("-n");



var sortOption = new Option<string>("--sort", getDefaultValue: () => "abc", "sort the files").FromAmong("abc","code");
    bundleCommand.AddOption(sortOption);
sortOption.AddAlias("-s");


var languageOption = new Option<string>("--language", "which language code included"){ IsRequired = true }.FromAmong(
                "c#",
                "c++",
                "js",
                "java",
                "sql",
                "all");
    ;
    bundleCommand.AddOption(languageOption);
languageOption.AddAlias("-l");


var bundleOption = new Option<FileInfo>("--output", "File path and name") {IsRequired=true  };
    bundleCommand.AddOption(bundleOption);
bundleOption.AddAlias("-o");



bundleCommand.SetHandler((output, language,note,sort,line,author) =>
    {
    //יצירת קובץ
    try
    {
        using (System.IO.File.Create(output.FullName))
        {
            Console.WriteLine("File was created");
        }

    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("Error: File path was invalid");
    }


    string folderPath = Environment.CurrentDirectory;
    //העתקת הקבצים בפועל
    try
    {
            List<string> txtFilesList = new List<string>();
            string[] txtFiles;
            //איסוף כל קבצי הקוד מהתיקיה
            if (language.Equals("all"))
            {
                foreach (string extension in fileExtensions)
                {
                    string[] files = Directory.GetFiles(folderPath, $"*{extension}");
                    txtFilesList.AddRange(files);
                }
                // ריצה על כל התיקיות שבקובץ אם הן לא בין/דיבג  
                string[] directories = Directory.GetDirectories(folderPath);
                foreach (string directory in directories)
                {
                    if (Path.GetFileName(directory) != "Debug" && Path.GetFileName(directory) != "bin")
                        foreach (string extension in fileExtensions)
                        {
                            string[] files = Directory.GetFiles(directory, $"*{extension}");
                            txtFilesList.AddRange(files);
                        }
                }
                txtFiles = txtFilesList.ToArray();
                // all מיון עפ שפת קוד רלוונטי רק אם המשתמש הקיש
                if (sort == "code")
                    txtFiles = txtFiles.OrderBy(f => Path.GetExtension(f)).ToArray();

            }
            //איסוף קבצים בסיומת ספציפית
            else
            {
                string[] files = Directory.GetFiles(folderPath, "*." + language);
                txtFilesList.AddRange(files);
                string[] directories = Directory.GetDirectories(folderPath);
                foreach (string directory in directories)
                {
                    if (Path.GetFileName(directory) != "Debug" && Path.GetFileName(directory) != "bin")
                    {
                        files = Directory.GetFiles(directory, "*." + language);
                        txtFilesList.AddRange(files);
                    }

                }
                txtFiles= txtFilesList.ToArray();
            }
            // מיון ע"פ סדר א"ב של שמות קוד
            if(string.IsNullOrEmpty(sort) || sort == "abc")
                txtFiles = txtFiles.OrderBy(f => Path.GetFileName(f)).ToArray();
            //אם לא נמצאו קבצים- מדפיס שגיאה למשתמש
            if (txtFiles.Length == 0)
            Console.WriteLine("Error: no documents found with this ending");
            //העתקת הקבצים בפועל- ריצה על מערך הקבצים שנוצר
            foreach (string txtFile in txtFiles)
            {
                //חילוץ שם קובץ היעד
                string destinationFilePath = Path.Combine(output.FullName);
                //שמירת הטקסט להעתקה בקובץ סטרינג
                string textToAppend = System.IO.File.ReadAllText(txtFile);
                //העתקה
                System.IO.File.AppendAllText(destinationFilePath, textToAppend);
                //אפשרות הוספת מקור
                if (note)
                {
                    string path = "\n file name: " + Path.GetFileName(txtFile)+"\n file path:"+txtFile;
                    System.IO.File.AppendAllText(destinationFilePath, path);
                }
            }
        }
        //תפיסת שגיאה במהלך העתקה
        catch
        {
            Console.WriteLine("Error: copy was failed");
        }

        //מחיקת שורות ריקות
        if (line) 
        { 
            try
            {
                string[] lines = System.IO.File.ReadAllLines(output.FullName);
                List<string> nonEmptyLines = new List<string>();

                foreach (string l in lines)
                {
                    if (!string.IsNullOrWhiteSpace(l))
                    {
                        nonEmptyLines.Add(l.Trim());
                    }
                }

                System.IO.File.WriteAllLines(output.FullName, nonEmptyLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to remove empty lines - " + ex.Message);
            }

        
        }
        if(author!=null)
        {
          try  
            {
                string destinationFilePath = Path.Combine(output.FullName);
                System.IO.File.AppendAllText(destinationFilePath, author);
            }
            catch { }
        }
    },bundleOption,languageOption,noteOption,sortOption,linesOption,authorOption);

    var rspCommand = new Command("rsp", "create-rsp for bundle code files for a single page");
    

rspCommand.SetHandler(() => {
        Console.WriteLine("Please enter the full path for the new file or just his name:");
        string output = Console.ReadLine();
        bool right = false; string language = "";
        while (!right) 
        {
            Console.WriteLine("Please enter the code language that you want to include to the bundle file. if you want all the files in the folder press: 'all'");
            language = Console.ReadLine();
            foreach (string option in fileCodes)
            {
                if (language.Equals(option))
                {
                    right = true;
                    break;
                }
            }
            if(language =="all")
                right=true;
            if(right==false)
            {
                Console.WriteLine("Invalid input. Please enter all or a code language");
            }
        }

        
        right=false; string note="";
        while (!right)
        {
            Console.WriteLine("Are you want to include the files paths? enter 'yes' or 'no'");
            note = Console.ReadLine();
            if (note == "no" || note == "yes")
            {
                right = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter either 'yes' or 'no'.");
            }
        }
        right = false; string sort = "";
        while (!right)
        {
            Console.WriteLine("Which sorting method will used? press 'abc' or 'code'");
            sort = Console.ReadLine();
            if(sort =="abc"||sort=="code")
                right=true;
            else 
            {
                Console.WriteLine("Invalid input. Please enter either 'abc' or 'code'");
            }
        }
        
        right=false;string line=""; 
        while (!right)
        {
            Console.WriteLine("Are you want to remove the empty lines? enter 'yes' or 'no'");
            line = Console.ReadLine();
            if (line == "no" || line == "yes")
            {
                right = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter either 'yes' or 'no'.");
            }
        }
        Console.WriteLine("Pleas write the author name:");
        string author = Console.ReadLine();

        string cmdln = "bundle \n --output " + output + "\n  --language " + language + " ";
        if (note == "yes")
            cmdln += "\n --note ";
        cmdln += "\n --sort " + sort;
        if (line == "yes")
            cmdln += "\n  --empty-lines ";
        cmdln += "\n --author " + author;
        //יצירת קובץ מסוג rsp
        string currentDirectory = Directory.GetCurrentDirectory();
        string FileName = "response_file.rsp";
        string FilePath = Path.Combine(currentDirectory, FileName);
        using (StreamWriter writer = System.IO.File.CreateText(FilePath))
        {
            writer.WriteLine(cmdln);
        }
        Console.WriteLine("response file was created in your path! Now you can run this token: 'bdl @response_file.rsp");

    });


var rootCommand = new RootCommand("Root Command for file bundle CLI");
    rootCommand.AddCommand(bundleCommand);
    rootCommand.AddCommand(rspCommand);
    rootCommand.InvokeAsync(args);


