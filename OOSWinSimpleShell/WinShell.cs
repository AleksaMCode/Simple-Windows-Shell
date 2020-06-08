using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOSWinSimpleShell.UserSQLManager;

namespace OOSWinSimpleShell
{
    public class WinShell
    {
        private string CurrentLocation { get; set; } = null;
        private readonly string prompt = "> ";
        private readonly string wrongCommand = "The command or its signature is wrong.";
        private string CurrentDir { get; set; } = null;
        private string AliasCurrentDir { get; set; } = null;
        private enum commandsChoice { where, go, create, list, print, find, findDat, logout, exit, clear, alias, addUser, rmUser, error };
        /// <summary>
        /// command name, min. num of options/arguments, max. number of options/arguments
        /// </summary>
        private readonly List<Tuple<string, int, int>> commands = new List<Tuple<string, int, int>>
        {
            Tuple.Create("where",1,1),
            Tuple.Create("go",2,2),
            Tuple.Create("create",2,3),
            Tuple.Create("list",1,2),
            Tuple.Create("print",2,2),
            Tuple.Create("find",3,3),
            Tuple.Create("findDat",3,3),
            Tuple.Create("logout",1,1),
            Tuple.Create("exit",1,1),
            Tuple.Create("clear",1,1),
            Tuple.Create("alias", 1,2),
            Tuple.Create("addUser",2,2),
            Tuple.Create("rmUser",2,2)
        };

        public void WelcomeScreen()
        {
            Console.WriteLine("Windows OOS Simple Shell\n");
            LoginScreen();
        }

        private string PasswordEnter()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            return pass;
        }

        private void LoginScreen()
        {
            bool superuser = false;
            while (true) // exit only if command exit has been executed
            {
                while (true)
                {
                    Console.Write(Environment.UserName + " login: ");
                    string username = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = PasswordEnter();

                    if (LoginControl.Login(username, password, DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss")) == true)
                    {
                        if (username.Equals("root"))
                        {
                            superuser = true;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nLogin incorrect");
                    }
                }

                AliasCurrentDir = CurrentDir = Directory.GetCurrentDirectory();
                if (Shell(superuser) == true)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        private bool CommandCheck(string[] tokens, string currentCommand, ref commandsChoice selectedCommand, bool superuser)
        {
            int i = 0;
            foreach (var command in commands)
            {
                if (currentCommand.Equals(commands[i++].Item1))
                {
                    if ((currentCommand.Equals("addUser") || currentCommand.Equals("rmUser")) && superuser == false)
                    {
                        Console.WriteLine("You don't have root privileges.");
                        return false;
                    }

                    if (tokens.Length > command.Item3)
                    {
                        break;
                    }
                    else
                    {
                        if ((tokens.Length == command.Item2) || (tokens.Length == command.Item3))
                        {
                            selectedCommand = (commandsChoice)(--i);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            if (selectedCommand == commandsChoice.error)
            {
                Console.WriteLine(wrongCommand);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void FileExists(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("File {0} already exists.", path);
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    File.Create(path);
                }
            }
        }

        private void FindFile(string path, string filename)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (filename.Equals(Path.GetFileName(file)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(path + "\\");
                        Console.ResetColor();
                        Console.Write(filename + "\n");
                    }
                }
                foreach (string newDir in Directory.GetDirectories(path))
                {
                    FindFile(newDir, filename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DirectorySearch(string dir, string three)
        {
            try
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    //if(string.IsNullOrWhiteSpace(Path.GetExtension(file)))
                    Console.WriteLine(three + Path.GetFileName(file));
                }
                foreach (string newDir in Directory.GetDirectories(dir))
                {
                    Console.Write(three);
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Path.GetFileName(newDir) + "\n");
                    Console.ResetColor();

                    DirectorySearch(newDir, three + "\t");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SetFileNameAndPath(ref string[] tokens, ref string path, ref string fileName)
        {
            if (tokens.Length == 3 && tokens[2].Contains("\\"))
            {
                path = tokens[2].Substring(0, tokens[2].LastIndexOf("\\"));
                fileName = tokens[2].Split('\\').Last();
            }
            else if (tokens.Length == 2 && tokens[1].Contains("\\"))
            {
                path = tokens[1].Substring(0, tokens[1].LastIndexOf("\\"));
                fileName = tokens[1].Split('\\').Last();
            }
        }

        private void Mkdir(string fileName, string path, string[] tokens)
        {
            if (tokens.Length == 3)
            {
                if (tokens[1].Equals("-d"))
                {
                    if (Directory.Exists(tokens[2]))
                    {
                        Console.WriteLine("Directory {0} already exists.", tokens[2]);
                    }
                    else
                    {
                        if (path != null)
                        {
                            if (Directory.Exists(path))
                            {
                                Directory.CreateDirectory(tokens[2]);
                            }
                            else
                            {
                                Console.WriteLine("Path {0} is not valid.", path);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(tokens[2]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The option '{0}' is not a valid command option.", tokens[1]);
                }
            }
            else
            {
                if (path != null)
                {
                    if (Directory.Exists(path))
                    {
                        FileExists(tokens[1]);
                    }
                    else
                    {
                        Console.WriteLine("Path {0} is not valid.", path);
                    }
                }
                else
                {
                    FileExists(tokens[1]);
                }
            }
        }

        private bool Shell(bool superuser)
        {
            while (true)
            {
                string currentCommand = "";
                Console.Write(AliasCurrentDir + prompt);
                string[] tokens = Console.ReadLine().Split(' ');
                currentCommand = tokens[0];
                commandsChoice selectedCommand = commandsChoice.error;

                if (CommandCheck(tokens, currentCommand, ref selectedCommand, superuser) == false)
                {
                    continue;
                }
                else
                {
                    switch (selectedCommand)
                    {
                        case commandsChoice.logout:
                            {
                                return false;
                            }
                        case commandsChoice.exit:
                            {
                                return true;
                            }
                        case commandsChoice.clear:
                            {
                                Console.Clear();
                                break;  //or continue
                            }
                        case commandsChoice.where:
                            {
                                Console.WriteLine(CurrentDir);
                                break;
                            }
                        case commandsChoice.addUser:
                            {
                                UserDatabase db = new UserDatabase(@"C:\Users\Aleksa\source\repos\OOSWinSimpleShell\OOSWinSimpleShell\Users.db");
                                if (db.GetUser(tokens[1]) != null)
                                {
                                    Console.WriteLine("Username '{0}' already exists.", tokens[1]);
                                    break;
                                }

                                Console.Write("Password:");
                                db.AddUser(tokens[1], PasswordEnter());
                                Console.WriteLine();
                                break;
                            }
                        case commandsChoice.rmUser:
                            {
                                UserDatabase db = new UserDatabase(@"C:\Users\Aleksa\source\repos\OOSWinSimpleShell\OOSWinSimpleShell\Users.db");
                                if (db.GetUser(tokens[1]) == null)
                                {
                                    Console.WriteLine("User '{0}' doesn't exist.", tokens[1]);
                                    break;
                                }

                                Console.Write("sudo password: ");

                                if (db.GetUser("root").IsPasswordValid(PasswordEnter()))
                                {
                                    db.RemoveUser(tokens[1]);
                                }
                                else
                                {
                                    Console.WriteLine("\nWrong sudo password.");
                                }
                                Console.WriteLine();
                                break;
                            }
                        case commandsChoice.go:
                            {
                                if (tokens[1].Equals(".."))
                                {
                                    if (AliasCurrentDir == CurrentDir && CurrentDir.Contains("\\"))
                                    {
                                        AliasCurrentDir = CurrentDir = CurrentDir.Substring(0, CurrentDir.LastIndexOf("\\"));
                                    }
                                    else if (CurrentDir.Contains("\\"))
                                    {
                                        CurrentDir = CurrentDir.Substring(0, CurrentDir.LastIndexOf("\\"));
                                    }
                                }
                                else
                                {
                                    tokens[1] = tokens[1].Trim();
                                    if ((Directory.Exists(tokens[1]) && tokens[1].Contains("\\")) || Directory.Exists(CurrentDir + "\\" + tokens[1]))
                                    {
                                        if (AliasCurrentDir == CurrentDir && tokens[1].Contains("\\"))
                                        {
                                            AliasCurrentDir = CurrentDir = tokens[1];
                                        }
                                        else if (AliasCurrentDir != CurrentDir && tokens[1].Contains("\\"))
                                        {
                                            CurrentDir = tokens[1];
                                        }
                                        else if (AliasCurrentDir == CurrentDir && !tokens[1].Contains("\\"))
                                        {
                                            AliasCurrentDir = CurrentDir += "\\" + tokens[1];
                                        }
                                        else
                                        {
                                            CurrentDir += "\\" + tokens[1];
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Path is not valid.");
                                    }
                                }
                                break;
                            }
                        case commandsChoice.alias:
                            {
                                if (tokens.Length == 2)
                                {
                                    AliasCurrentDir = tokens[1];
                                }
                                else
                                {
                                    AliasCurrentDir = CurrentDir;
                                }
                                break;
                            }
                        case commandsChoice.create:
                            {
                                string fileName = null;
                                string path = null;

                                SetFileNameAndPath(ref tokens, ref path, ref fileName); // help method
                                Mkdir(fileName, path, tokens);

                                break;
                            }
                        case commandsChoice.list:
                            {
                                if (tokens.Length == 2)
                                {
                                    tokens[1] = tokens[1].Trim();
                                    if (Directory.Exists(tokens[1]))
                                    {
                                        DirectorySearch(tokens[1], "\t");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Path is not valid.");
                                    }
                                }
                                else
                                {
                                    DirectorySearch(CurrentDir, "\t");
                                }
                                break;
                            }
                        case commandsChoice.findDat:
                            {
                                tokens[2] = tokens[2].Trim();
                                if (Directory.Exists(tokens[2]))
                                {
                                    FindFile(tokens[2], tokens[1]);
                                }
                                else
                                {
                                    Console.WriteLine("Path is not valid.");
                                }
                                break;
                            }
                        case commandsChoice.print:
                            {
                                if (File.Exists(CurrentDir + "\\" + tokens[1]))
                                {
                                    string extension = Path.GetExtension(tokens[1]);
                                    if (extension == String.Empty)
                                    {
                                        Console.WriteLine("File {0} doesn't have extension information.", CurrentDir);
                                    }
                                    else if (extension.Contains("."))
                                    {
                                        extension = extension.TrimStart('.');
                                        if (extension.Equals("txt"))
                                        {
                                            Console.Write(File.ReadAllText(CurrentDir + "\\" + tokens[1]));
                                        }
                                        else
                                        {
                                            Console.WriteLine("File '{0}' is not a text file.", CurrentDir + tokens[1]);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("File doesn't have an extension.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("File '{0}' doesn't exist in path {1}.", tokens[1], CurrentDir);
                                }
                                break;
                            }
                        case commandsChoice.find:
                            {
                                if (tokens[1].StartsWith("\"") && tokens[1].EndsWith("\"") && tokens[1].Length > 2)
                                {
                                    if (File.Exists(CurrentDir + "\\" + tokens[2]))
                                    {
                                        tokens[1] = tokens[1].Substring(1, tokens[1].Length - 2);


                                        var lines = File.ReadLines(CurrentDir + "\\" + tokens[2]);
                                        int lineNmb = 1;
                                        foreach (var line in lines)
                                        {
                                            if (line.Contains(tokens[1]))
                                            {
                                                Console.Write(lineNmb + ":");
                                                Console.Write(line.Substring(0, line.IndexOf(tokens[1])));
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write(tokens[1]);
                                                Console.ResetColor();
                                                Console.WriteLine(line.Substring(line.IndexOf(tokens[1]) + tokens[1].Length));
                                                break;
                                            }
                                            lineNmb++;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("File '{0}' doesn't exist in path {1}.", tokens[2], CurrentDir);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Text option doesn't have the correct signature.");
                                }
                                break;
                            }
                    }
                }
            }
        }
    }
}