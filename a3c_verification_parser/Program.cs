using System;
using System.IO;
using System.Reflection;

namespace a3c_verification_parser
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "A3C Verification Parser by SzwedzikPL";
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directory = new DirectoryInfo(currentDirectory);
            var directoryFiles = directory.GetFiles("*.pbo");
            string modName = "";

            if (directoryFiles.Length > 0)
            {
                Console.WriteLine("Wykryto {0} pbo", directoryFiles.Length);
                Console.WriteLine("Podaj nazwę moda (małe litery np. pam a3ap a3mp itp).");
                modName = Console.ReadLine();

                if (modName == "")
                {
                    while (modName == "")
                    {
                        Console.WriteLine("Błędna nazwa, podaj jeszcze raz.");
                        modName = Console.ReadLine();
                    }
                }

                Console.WriteLine("Tworzę plik {0}.txt...", modName);

                //prepare data
                string verificationData = "[";
                int foreachCounter = 0;
                foreach (var directoryFile in directoryFiles)
                {
                    if (foreachCounter > 0) {verificationData += ",";}
                    verificationData += "[\"" + directoryFile.Name + "\"," + directoryFile.Length + "]";
                    Console.WriteLine("Dodaję plik {0} o wadze {1} bajtów", directoryFile.Name, directoryFile.Length);
                    foreachCounter++;
                }
                verificationData += "]";

                //delete file if exists
                var filePath = currentDirectory + "\\" + modName + ".txt";
                if (File.Exists(filePath)) {File.Delete(filePath);}

                //create new file & save data
                using (StreamWriter outputFile = new StreamWriter(currentDirectory + @"\" + modName + ".txt"))
                {
                    outputFile.WriteLine(verificationData);
                }

                Console.WriteLine("");
                Console.WriteLine("Plik utworzony pomyślnie.");
                Console.WriteLine("Wrzuć go do ftp_strony/repo/versions w momencie wydania aktualizacji.");
            }
            else
            {
                Console.WriteLine("Nie wykryto żadnych plików pbo.");
                Console.WriteLine("Umieść program w folderze z plikami pbo i uruchom go ponownie!");
            }

            Console.WriteLine("");
            Console.WriteLine("Wciśnij dowolny przycisk aby zakończyć...");
            Console.ReadKey();
        }
    }
}