using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ArmaConfigParser
{
    class Program
    {
        static void ParseFile(string file, int maxClassLevel, bool whitelistEnabled, List<string> whitelist)
        {
            List<string> newConfigLines = new List<string>();
            List<string> baseClassNames = new List<string>();
            List<int> classCount = new List<int>();
            int baseClasses = -1;
            string actualBaseClass = "";
            string configDir = Path.GetDirectoryName(file);
            string configName = Path.GetFileName(file);
            string configNameWithoutExt = Path.GetFileNameWithoutExtension(file);
            string configExtension = Path.GetExtension(file);

            //Read config
            using (Stream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    int lineCounter = 0;
                    int classLevel = 0;
                    int nextLineClassLevel = 0;
                    string line;

                    Console.WriteLine("\n===================================================\n");
                    Console.WriteLine("Wczytuję {0}\n", file);
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        lineCounter++;
                        classLevel = nextLineClassLevel;
                        string configLine = line.Trim();
                        //Clear comments
                        bool addLine = false;
                        bool hasClass = false;
                        if (configLine.Contains("//"))
                        {
                            int commentIndex = configLine.IndexOf("//");
                            if (commentIndex != 0)
                            {
                                int commentLength = configLine.Length - commentIndex;
                                configLine = configLine.Remove(commentIndex, commentLength);
                            }
                            else
                            {
                                configLine = "";
                            }
                        }
                        //Check classes
                        if (configLine.ToLower().Contains("class "))
                        {
                            if (configLine.ToLower().Trim().IndexOf("class ") == 0)
                            {
                                hasClass = true;
                            }
                        }

                        if (hasClass)
                        {
                            addLine = true;
                            if (!(configLine.ToLower().Contains(";")))
                            {
                                nextLineClassLevel += 1;
                                if (!(configLine.ToLower().Contains("{")))
                                {
                                    configLine += " {";
                                }
                            }
                            if (classLevel == 0)
                            {
                                string baseClassName = configLine.Remove(configLine.ToLower().IndexOf("class "), 6);
                                if (baseClassName.ToLower().Contains("{"))
                                {
                                    baseClassName = baseClassName.Remove(baseClassName.ToLower().IndexOf("{"), 1).Trim();
                                }
                                baseClasses += 1;
                                classCount.Add(0);
                                baseClassNames.Add(baseClassName);
                                actualBaseClass = baseClassName;
                            }
                            else if (classLevel == 1 && classLevel <= maxClassLevel)
                            {
                                classCount[baseClasses] += 1;
                            };
                        }
                        else
                        {
                            //Check class closed
                            if (classLevel > 0 && configLine.Trim().Contains("};"))
                            {
                                if (configLine.Trim().Substring(0, 2) == "};")
                                {
                                    addLine = true;
                                    nextLineClassLevel -= 1;
                                    classLevel -= 1;
                                }
                            }
                        }

                        if (classLevel > maxClassLevel)
                        {
                            addLine = false;
                        }
                        if (whitelistEnabled)
                        {
                            if (!(whitelist.Any(str => str.ToLower().Contains(actualBaseClass.ToLower()))))
                            {
                                addLine = false;
                            }
                        }

                        if (addLine && configLine.Trim() != "")
                        {
                            //Add space tabs
                            if (classLevel > 0)
                            {
                                string tab = "";
                                for (int i = 1; i <= classLevel; i++)
                                {
                                    tab += "    ";
                                }
                                configLine = tab + configLine;
                            }
                            newConfigLines.Add(configLine);
                        }
                    }
                }
            }
            baseClassNames.ToArray();
            int baseClassCount = 0;
            foreach (string baseClass in baseClassNames)
            {
                bool addLog = true;
                if (whitelistEnabled)
                {
                    if (!(whitelist.Any(str => str.ToLower().Contains(baseClass.ToLower()))))
                    {
                        addLog = false;
                    }
                }
                if (addLog)
                {
                    Console.Write("Klas ");
                    Console.ForegroundColor = System.ConsoleColor.Yellow;
                    Console.Write(baseClass);
                    Console.ResetColor();
                    Console.Write(": ");
                    Console.ForegroundColor = System.ConsoleColor.Green;
                    Console.Write(classCount[baseClassCount]);
                    Console.ResetColor();
                    Console.WriteLine();
                    //Console.WriteLine("Klas {0}: {1}", baseClass, classCount[baseClassCount]);
                }
                baseClassCount += 1;
            }

            //===============================================================

            string newConfigName = configNameWithoutExt + "_parsed" + configExtension;
            string newConfig = configDir + @"\" + newConfigName;

            //Write new config
            if (File.Exists(newConfig))
            {
                File.Delete(newConfig);
            }
            using (StreamWriter outputFile = new StreamWriter(newConfig))
            {
                int lineIndex = 0;
                int nextIgnoreLine = -1;
                newConfigLines.ToArray();

                foreach (string line in newConfigLines)
                {
                    string lineString = line;
                    if (nextIgnoreLine != lineIndex)
                    {
                        if (!(lineString.ToLower().Contains(";")) && newConfigLines[lineIndex + 1].Trim() == "};")
                        {
                            lineString += "};";
                            nextIgnoreLine = lineIndex + 1;
                        }
                        outputFile.WriteLine(lineString);
                    }
                    lineIndex++;
                }
            }
        }

        static List<string> SearchFile(string dir, string fileName)
        {
            List<string> filesList = new List<string>();
            List<string> filesCollected = new List<string>();

            //Check dirs inside
            foreach (string d in Directory.GetDirectories(dir, "*"))
            {
                var curDir = new DirectoryInfo(d);
                var curDirConfigs = curDir.GetFiles(fileName);
                if (curDirConfigs.Length > 0)
                {
                    foreach (FileInfo curDirConfig in curDirConfigs)
                    {
                        filesList.Add(curDirConfig.FullName); 
                    }
                }
                filesCollected = SearchFile(d, fileName);
            }

            List<string> files = new List<string>();
            files.AddRange(filesCollected);
            files.AddRange(filesList);
            return files;
        }


        static void Main(string[] args)
        {
            //Title
            Console.Title = "Parser configów ArmA by SzwedzikPL";

            //Default params
            string filePath = "";
            bool scanRecursively = false;
            int maxClassLevel = 1;
            bool whitelistEnabled = false;
            List<string> whitelist = new List<string>();

            //===============================================================

            //Load arguments
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
                //maxclasslevel
                if (arg.ToLower().Contains("-maxclasslevel="))
                {
                    int paramVal = 0;
                    string param = arg.Remove(0, 15);
                    if(int.TryParse(param, out paramVal))
                    {
                        maxClassLevel = paramVal;
                    }
                }

                //whiteList
                if (arg.ToLower().Contains("-whitelist="))
                {
                    whitelistEnabled = true;
                    string param = arg.Remove(0, 11);
                    string[] splitter = new string[] { ";" };
                    string[] paramVals = param.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string paramVal in paramVals)
                    {
                        whitelist.Add(paramVal);
                    }
                }

                //path
                if (arg.ToLower().Contains("-path="))
                {
                    string param = arg.Remove(0, 6);
                    if(param != "")
                    {
                        filePath = param;
                    }
                }

                //scan recursively
                if (arg.ToLower().Contains("-r"))
                {
                    scanRecursively = true;
                }

            }

            //===============================================================

            string fileName = "config.cpp";

            if(filePath == "")
            {
                filePath = Path.GetFullPath(@"./");
            }

            //Load config
            if (scanRecursively)
            {
                //Recursive check
                List<string> allConfigsList = SearchFile(filePath, fileName);

                //Check current dir
                string curDirFile = Path.GetFullPath(filePath + @"\" + fileName);
                if (File.Exists(curDirFile))
                {
                    allConfigsList.Add(curDirFile);
                }

                string[] allConfigs = allConfigsList.ToArray();
                foreach (string configFile in allConfigs)
                {
                    ParseFile(configFile, maxClassLevel, whitelistEnabled, whitelist);
                }
            }
            else
            {
                //Simple check
                string configFile = Path.GetFullPath(filePath + @"\" + fileName); 
                if (File.Exists(configFile))
                {
                    ParseFile(configFile, maxClassLevel, whitelistEnabled, whitelist);
                }
                else
                {
                    Console.WriteLine("Nie znaleziono pliku config.cpp...");
                }
            }
            Console.WriteLine("\n===================================================\n");
            Console.WriteLine("Naciśnij enter aby zakończyć...");
            Console.Read();
        }
    }
}
