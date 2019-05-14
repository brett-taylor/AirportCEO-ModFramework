using System;
using System.IO;

namespace ACML.Installer
{
    public class Program
    {
        private static readonly string AIRPORT_CEO_EXECUTABLE_FILE_NAME = "Airport CEO.exe";
        private static readonly string EXAMPLE_DIRECTORY = "C:\\Steam Library\\steamapps\\common\\Airport CEO";
        private static readonly string DLL_DIRECTORY = Path.Combine("Airport CEO_Data", Path.Combine("Managed", "Assembly-CSharp.dll"));
        
        public static void Main()
        {
            string directory = GetInputDirectory();
            if (VerifyExecutableDirectory(directory) == true)
            {
                Console.WriteLine("Found executable. Attempting to now find Assembly Assembly-CSharp");
                if (VerifyDLLDirectory(directory) == true)
                {
                    Console.WriteLine("Found DLL. Attempting to patch");
                    string dll = Path.Combine(directory, DLL_DIRECTORY);
                    // Patch
                }
            }
            Console.ReadKey();
        }

        private static string GetInputDirectory()
        {
            Console.WriteLine($"Please input the directory to your \"{AIRPORT_CEO_EXECUTABLE_FILE_NAME}\" file.");
            Console.WriteLine($"You may find it in a similar location to \"{EXAMPLE_DIRECTORY}\".");
            Console.Write("Please enter it now: ");
            return Console.ReadLine();
        }

        private static bool VerifyExecutableDirectory(string directory)
        {
            bool result = File.Exists(Path.Combine(directory, AIRPORT_CEO_EXECUTABLE_FILE_NAME));
            if (result == false)
                Console.WriteLine($"Did not find the Airport CEO.exe file there. It must be named \"{AIRPORT_CEO_EXECUTABLE_FILE_NAME}\"");

            return result;
        }

        private static bool VerifyDLLDirectory(string directory)
        {
            bool result = File.Exists(Path.Combine(directory, DLL_DIRECTORY));
            if (result == false)
                Console.WriteLine($"Did not find the DLL at \"{Path.Combine(directory, DLL_DIRECTORY)}\".");

            return result;
        }
    }
}
