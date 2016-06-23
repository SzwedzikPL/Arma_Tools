using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace RPTLogParser
{
    class Program
    {
        //test
        static void Main(string[] args)
        {
            var directory = new DirectoryInfo("./");
            var directoryFiles = directory.GetFiles("*.rpt");
            if (directoryFiles.Length > 0)
            {
                var files = directoryFiles.OrderByDescending(f => f.LastWriteTime);
                foreach (FileInfo file in files)
                {
                    string fullFilename = file.FullName;
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fullFilename);
                    if (!(fileNameWithoutExt.Contains("_parsed")))
                    {
                        string fileExtension = Path.GetExtension(fullFilename);
                        string fileDir = Path.GetDirectoryName(fullFilename);
                        List<string> newFileLines = new List<string>();

                        using (Stream stream = File.Open(fullFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader streamReader = new StreamReader(stream))
                            {
                                string line;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    if (line.Length > 9)
                                    {
                                        string newLine = line.Remove(0, 9);
                                        newFileLines.Add(newLine);
                                    }
                                }
                            }
                        }

                        string newFileName = fileNameWithoutExt + "_parsed" + fileExtension;
                        string newFile = fileDir + @"\" + newFileName;

                        //Write new config
                        if (File.Exists(newFile))
                        {
                            File.Delete(newFile);
                        }
                        using (StreamWriter outputFile = new StreamWriter(newFile))
                        {
                            newFileLines.ToArray();
                            foreach (string line in newFileLines)
                            {
                                string lineString = line;
                                outputFile.WriteLine(lineString);
                            }
                        }
                    }
                }
            }
        }
    }
}
