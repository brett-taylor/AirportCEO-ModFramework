using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ACMFInstaller
{
    public static class Installer
    {
        private static readonly string EXAMPLE_DIRECTORY = "C:\\Steam Library\\steamapps\\common\\Airport CEO";
        private static readonly string AIRPORT_CEO_EXECUTABLE_FILE_NAME = "Airport CEO.exe";
        private static readonly string MANAGED_DIRECTORY = Path.Combine("Airport CEO_Data", "Managed");
        private static readonly string DLL_DIRECTORY = Path.Combine(MANAGED_DIRECTORY, "Assembly-CSharp.dll");

        private static readonly string REQUIRMENTS_LOCATION = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ACMFInstallerRequirements");
        private static readonly string REQUIRMENTS_COPY_TO_LOCATION = "ACMF";

        private static readonly string BACKUP_DLL_EXTESION = "_original.dll";

        private static readonly string ACMF_DLL_NEW_LOCATION = Path.Combine(Path.Combine(MANAGED_DIRECTORY, REQUIRMENTS_COPY_TO_LOCATION), "ACMF.dll");
        private static readonly int PATCH_INTO_INSTRUCTION_NUMBER = 2;
        private static readonly string PATCH_CALL_TYPE = "MainMenuWorldController";
        private static readonly string PATCH_CALL_METHOD = "Awake";
        private static readonly string TARGET_CALL_TYPE = "ACMF.ACMF";
        private static readonly string TARGET_CALL_METHOD = "Entry";

        public static void Main()
        {
            string airportCEODirectory = GetInputDirectory();
            if (VerifyExecutableDirectory(airportCEODirectory) == true)
            {
                Console.WriteLine("Found executable. Attempting to now find Assembly Assembly-CSharp");
                if (VerifyDLLDirectory(airportCEODirectory) == true)
                {
                    Console.WriteLine("Found DLL. Attempting to patch");
                    string managed = Path.Combine(airportCEODirectory, MANAGED_DIRECTORY);
                    string dll = Path.Combine(airportCEODirectory, DLL_DIRECTORY);
                    try
                    {
                        CopyRequirments(REQUIRMENTS_LOCATION, Path.Combine(managed, REQUIRMENTS_COPY_TO_LOCATION));
                        Console.WriteLine("Patch Starting.");
                        string backupDLL = Path.Combine(Path.GetDirectoryName(dll), Path.GetFileNameWithoutExtension(dll)) + BACKUP_DLL_EXTESION;
                        ReplaceOriginalWithBackupIfItExists(dll, backupDLL);
                        CreateBackup(dll, backupDLL);
                        Patch(dll, backupDLL, Path.Combine(airportCEODirectory, ACMF_DLL_NEW_LOCATION));
                        Console.WriteLine("");
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

        private static void CopyRequirments(string from, string to)
        {
            Console.WriteLine("");
            Console.WriteLine($"Copying required files over.");
            Console.WriteLine($"Copying from: {from}");
            Console.WriteLine($"Copying to: {to}");

            if (File.Exists(to))
                Directory.Delete(to, true);

            Directory.CreateDirectory(to);
            foreach (string newPath in Directory.GetFiles(from, "*.*", SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine($"Copying: {newPath}");
                File.Copy(newPath, newPath.Replace(from, to), true);
            }

            Console.WriteLine("Done copying files.");
        }

        private static void ReplaceOriginalWithBackupIfItExists(string dll, string backupDLL)
        {
            if (File.Exists(backupDLL) == false)
                return;

            if (File.Exists(dll) == true)
                File.Delete(dll);

            Console.WriteLine($"Replaced original dll with backed up dll.");
            File.Copy(backupDLL, dll);
        }

        private static void CreateBackup(string dll, string backupDLL)
        {
            Console.WriteLine("");
            Console.WriteLine($"Creating backup of Assembly-CSharp.dll.");

            Console.WriteLine($"Directory: {backupDLL}.");
            if (File.Exists(backupDLL))
                File.Delete(backupDLL);

            File.Copy(dll, backupDLL);
            Console.WriteLine("Done.");
        }

        private static void Patch(string dll, string dllcopy, string acmfDLL)
        {
            Console.WriteLine("");
            Console.WriteLine($"Patching DLL now.");
            Console.WriteLine($"DLL {dll}");
            Console.WriteLine($"DLLCOPY {dllcopy}");
            Console.WriteLine($"ACMF {acmfDLL}");

            AssemblyDefinition patchAssembly = AssemblyDefinition.ReadAssembly(dllcopy);
            MethodDefinition patchMethod = patchAssembly.MainModule.GetType(PATCH_CALL_TYPE).Methods.First((x) => x.Name == PATCH_CALL_METHOD);
            ILProcessor patchIL = patchMethod.Body.GetILProcessor();

            AssemblyDefinition targetAssembly = AssemblyDefinition.ReadAssembly(acmfDLL);
            MethodReference tagetMethod = targetAssembly.MainModule.GetType(TARGET_CALL_TYPE).Methods.First(x => x.Name == TARGET_CALL_METHOD);

            patchIL.InsertBefore(patchMethod.Body.Instructions[PATCH_INTO_INSTRUCTION_NUMBER], Instruction.Create(OpCodes.Call, patchMethod.Module.ImportReference(tagetMethod)));
            patchAssembly.Write(dll);
            patchAssembly.Dispose();
            targetAssembly.Dispose();
            Console.WriteLine("Done.");
        }
    }
}
