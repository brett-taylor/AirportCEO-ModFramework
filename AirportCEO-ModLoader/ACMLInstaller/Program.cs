using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;

namespace ACMLInstaller
{
    public class Program
    {
        private static readonly string AIRPORT_CEO_EXECUTABLE_FILE_NAME = "Airport CEO.exe";
        private static readonly string EXAMPLE_DIRECTORY = "C:\\Steam Library\\steamapps\\common\\Airport CEO";
        private static readonly string DLL_DIRECTORY = Path.Combine("Airport CEO_Data", Path.Combine("Managed", "Assembly-CSharp.dll"));
        private static readonly string ACML_DLL = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ACML.dll");
        private static readonly string HARMONY_DLL = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "0Harmony.dll");
        private static readonly string PATCH_CALL_TYPE = "MainMenuUI";
        private static readonly string PATCH_CALL_METHOD = "Awake";
        private static readonly string TARGET_CALL_TYPE = "ACML.AirportCEOModLoader";
        private static readonly string TARGET_CALL_METHOD = "Entry";
        private static readonly string ORIGINAL_DLL_EXTESION = "_original.dll";
        private static readonly int PATCH_INTO_INSTRUCTION_NUMBER = 2;
        private static readonly string ACMH_FOLDER_NAME = "ACMH";
        private static readonly string ACMH_ASSETS_FOLDER_NAME = "ACMHAssets";

        public static void Main()
        {
            string directory = GetInputDirectory();
            if (VerifyExecutableDirectory(directory) == true)
            {
                Console.WriteLine("Found executable. Attempting to now find Assembly Assembly-CSharp");
                if (VerifyDLLDirectory(directory) == true)
                {
                    Console.WriteLine("Found DLL. Attempting to patch");
                    Console.WriteLine(" ");
                    string dll = Path.Combine(directory, DLL_DIRECTORY);
                    try
                    {
                        Console.WriteLine("Copying AirportCEO Modding Helper.");
                        CreateOrUpdateACMHDirectory(directory);
                        Console.WriteLine("Copied AirportCEO Modding Helper.");
                        Console.WriteLine("");
                        Console.WriteLine("Patch Starting.");
                        Patch(dll, ACML_DLL, HARMONY_DLL, PATCH_CALL_TYPE, PATCH_CALL_METHOD, TARGET_CALL_TYPE, TARGET_CALL_METHOD, ORIGINAL_DLL_EXTESION, PATCH_INTO_INSTRUCTION_NUMBER);
                        Console.WriteLine("Patch successful.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Patch failed");
                        Console.WriteLine(e.ToString());
                    }
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

        private static void CreateOrUpdateACMHDirectory(string executableDirectory)
        {
            string acmhDirectory = Path.Combine(executableDirectory, ACMH_FOLDER_NAME);
            Console.WriteLine($"Checking to see if directory exists: {acmhDirectory}");
            if (Directory.Exists(acmhDirectory) == false)
            {
                Directory.CreateDirectory(acmhDirectory);
            }

            string acmhAssetsDirectory = Path.Combine(acmhDirectory, ACMH_ASSETS_FOLDER_NAME);
            Console.WriteLine($"Attempting to delete: {acmhAssetsDirectory}");
            if (Directory.Exists(acmhAssetsDirectory) == true)
            {
                foreach (string filePath in Directory.GetFiles(acmhAssetsDirectory))
                {
                    Console.WriteLine($"Attempting to delete file: {filePath}");
                    File.Delete(filePath);
                }
                Directory.Delete(acmhAssetsDirectory);
            }

            Console.WriteLine($"Attempting to create directory {acmhAssetsDirectory}");
            Directory.CreateDirectory(acmhAssetsDirectory);
            foreach (string filePath in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), ACMH_ASSETS_FOLDER_NAME)))
            {
                Console.WriteLine($"Attempting to copy file {filePath}");
                File.Copy(filePath, Path.Combine(acmhAssetsDirectory, Path.GetFileName(filePath)));
            }
        }

        public static void Patch(string dllDirectory, string acmlDLL, string harmonyDLL, string patchCallType, string patchCallMethod, string targetCallType, string targetCallMethod,
            string copyExtension, int PATCH_INTO_INSTRUCTION_NUMBER)
        {
            string newDLLDirectory = Path.Combine(Path.GetDirectoryName(dllDirectory), Path.GetFileNameWithoutExtension(dllDirectory)) + copyExtension;
            string newACMLDLLDirectory = Path.Combine(Path.GetDirectoryName(dllDirectory), Path.GetFileName(acmlDLL));
            string newHarmonyDLLDirectory = Path.Combine(Path.GetDirectoryName(dllDirectory), Path.GetFileName(harmonyDLL));

            Console.WriteLine($"DLL: {newDLLDirectory}");
            if (File.Exists(newDLLDirectory) == true)
            {
                File.Delete(newDLLDirectory);
            }

            Console.WriteLine($"ACML: {newACMLDLLDirectory}");
            if (File.Exists(newACMLDLLDirectory) == true)
            {
                File.Delete(newACMLDLLDirectory);
            }

            Console.WriteLine($"Harmony: {newHarmonyDLLDirectory}");
            if (File.Exists(newHarmonyDLLDirectory) == true)
            {
                File.Delete(newHarmonyDLLDirectory);
            }

            File.Move(dllDirectory, newDLLDirectory);
            File.Copy(acmlDLL, newACMLDLLDirectory);
            File.Copy(harmonyDLL, newHarmonyDLLDirectory);

            AssemblyDefinition patchAssembly = AssemblyDefinition.ReadAssembly(newDLLDirectory);
            MethodDefinition patchMethod = patchAssembly.MainModule.GetType(patchCallType).Methods.First((x) => x.Name == patchCallMethod);
            ILProcessor patchIL = patchMethod.Body.GetILProcessor();

            AssemblyDefinition targetAssembly = AssemblyDefinition.ReadAssembly(acmlDLL);
            MethodReference tagetMethod = targetAssembly.MainModule.GetType(targetCallType).Methods.First(x => x.Name == targetCallMethod);

            patchIL.InsertBefore(patchMethod.Body.Instructions[PATCH_INTO_INSTRUCTION_NUMBER], Instruction.Create(OpCodes.Call, patchMethod.Module.ImportReference(tagetMethod)));
            patchAssembly.Write(dllDirectory);
            patchAssembly.Dispose();
            targetAssembly.Dispose();
        }
    }
}
