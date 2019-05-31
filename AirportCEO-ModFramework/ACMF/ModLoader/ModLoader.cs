using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ACMF.ModLoader.Attributes;
using UnityEngine;

namespace ACMF.ModLoader
{
    public static class ModLoader
    {
        public static Dictionary<string, Mod> ModsFound { get; private set; }
        public static Dictionary<string, Mod> ModsLoaded { get; private set; }

        public static void Initialise()
        {
            ModsFound = new Dictionary<string, Mod>();
            ModsLoaded = new Dictionary<string, Mod>();

            string modPath = GetModPath();
            if (Directory.Exists(modPath) == false)
            {
                Utilities.Logger.Print($"Failed to located ModPath at: {GetModPath()}");
                return;
            }

            Utilities.Logger.Print($"Located ModPath: {GetModPath()}");
            FileInfo[] allDlls = new DirectoryInfo(modPath).GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (FileInfo dll in allDlls)
            {
                AddToFoundModsIfACMLMod(dll.ToString());
            }

            foreach (Mod mod in ModsFound.Values)
            {
                mod.CalculateIfShouldLoad();
                if (mod.ShouldLoad())
                    LoadMod(mod);
                else
                    Utilities.Logger.Print($"{mod.ModInfo.Name} did not load because {mod.ModLoadFailure}");
            }
        }

        public static string GetModPath()
        {
            return SystemInfo.operatingSystem.ToLower().Contains("windows") ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Apoapsis Studios/Airport CEO/Mods" :
                SystemInfo.operatingSystem.ToLower().Contains("mac") ? Application.persistentDataPath + "/Mods" : "";
        }

        private static void AddToFoundModsIfACMLMod(string dllLocation)
        {
            Utilities.Logger.Print($"Attempting to load dll: {dllLocation}");

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllLocation);
                Type entryPointClass = assembly.ManifestModule.GetTypes().First((x) => x.GetCustomAttributes(typeof(ACMFMod), true).Length > 0);
                if (entryPointClass == null)
                    return;

                MethodInfo entryPointMethod = entryPointClass.GetMethods().First((x) => x.GetCustomAttributes(typeof(ACMFModEntryPoint), true).Length > 0);
                if (entryPointMethod == null)
                    return;

                ACMFMod acmlMod = (ACMFMod)entryPointClass.GetCustomAttributes(typeof(ACMFMod), true).FirstOrDefault();
                ModsFound.Add(acmlMod.ID, new Mod(acmlMod, assembly, entryPointMethod));
                Utilities.Logger.Print($"Found Mod: {acmlMod.Name}");
            }
            catch
            {

            }
        }

        private static void LoadMod(Mod mod)
        {
            Utilities.Logger.Print($"Executing entry point of mod: {mod.ModInfo.Name}");
            try
            {
                ModsLoaded.Add(mod.ModInfo.ID, mod);
                mod.EntryPoint.Invoke(null, new object[] { mod });
                Utilities.Logger.Print($"Sucessfully executed entry point of mod: {mod.ModInfo.Name}");
            }
            catch (Exception e)
            {
                if (ModsLoaded.ContainsKey(mod.ModInfo.ID))
                    ModsLoaded.Remove(mod.ModInfo.ID);

                Utilities.Logger.Print($"Failed to execute entry point of mod: {mod.ModInfo.Name}");
                Utilities.Logger.Print(e.ToString());
            }
        }
    }
}
