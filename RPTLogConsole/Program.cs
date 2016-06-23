using System;
using System.Linq;
using System.IO;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        public static string watchFile = "";
        public static string consoleTitle = "Obserwator RPT by SzwedzikPL [Arma 3]";
        public static int consoleLineCounter = 0;
        public static string watchType = "n";
        public static string[] watchTypes = { "n", "s" };
        public static string watchPath = "C:\\Users\\SzwedzikPL\\AppData\\Local\\Arma 3";

        public static void Main()
        {
            Console.Title = consoleTitle;
            Console.WriteLine("Typ obserwacji: n - normalna / s - serwerowa");
            watchType = Console.ReadLine();

            if (watchTypes.Contains(watchType))
            {
                while (!watchTypes.Contains(watchType))
                {
                    Console.WriteLine("Błędny typ, podaj jeszcze raz.");
                    watchType = Console.ReadLine();
                }
            }
            if (watchType == "s")
            {
                watchPath = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Arma 3\\DEDYK";
                consoleTitle = "Obserwator RPT by SzwedzikPL [Arma 3 Server]";
            }
            FindNewestRPT();
            int sleepCounter = 0;
            while (true)
            {
                ReadRPTLogs();
                sleepCounter += 500;
                if (sleepCounter == 2000)
                {
                    FindNewestRPT();
                    sleepCounter = 0;
                }
                Thread.Sleep(500);
            }
        }
        public static void FindNewestRPT()
        {
            var directory = new DirectoryInfo(watchPath);
            var directoryFiles = directory.GetFiles("*.rpt");
            if (directoryFiles.Length > 0)
            {
                var newestFile = directoryFiles.OrderByDescending(f => f.LastWriteTime).First();
                if (newestFile.Name != watchFile)
                {
                    watchFile = newestFile.Name;
                    Console.Title = consoleTitle + " :: " + watchFile;
                    consoleLineCounter = 0;
                    Console.Clear();
                }
            }
            else
            {
                watchFile = "";
                Console.Clear();
                Console.Title = consoleTitle + " :: Brak pliku RPT";
                Console.WriteLine("Brak pliku RPT");
            }
        }
        public static void ReadRPTLogs()
        {
            if (watchFile != "")
            {
                string filePath = watchPath + "\\" + watchFile;
                if (File.Exists(filePath))
                {
                    using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            int lineCounter = 0;
                            string line;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                lineCounter++;
                                if (lineCounter > consoleLineCounter)
                                {
                                    if (line.Length > 9)
                                    {
                                        string consoleLine;
                                        consoleLine = line.Remove(0, 9);
                                        if (consoleLine.Length > 12)
                                        {
                                            if (consoleLine.ToLower().Contains("info"))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine(consoleLine);
                                                Console.ResetColor();
                                            }
                                            else if (consoleLine.ToLower().Contains("warning"))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine(consoleLine);
                                                Console.ResetColor();
                                            }
                                            else if (consoleLine.ToLower().Contains("error"))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine(consoleLine);
                                                Console.ResetColor();
                                            }
                                            else
                                            {
                                                Console.WriteLine(consoleLine);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(consoleLine);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(line);
                                    }
                                    consoleLineCounter++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}