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
                AddToFoundModsIfACMLMod(dll.ToString());

            List<Tuple<string, List<string>>> attemptToLoadMods = new List<Tuple<string, List<string>>>();

            foreach (Mod mod in ModsFound.Values)
            {
                if (mod.IsOnCorrectVersionOFACMF() == false)
                {
                    mod.ModLoadFailure = ModLoadFailure.REQUIRES_NEWER_VERSION_OF_ACMF;
                    string message = $"{mod.ModInfo.Name} requires a newer version of ACMF (requires {mod.ModInfo.RequiredACMFVersion}";
                    Utilities.Logger.Error(message);
                    ModHelper.DialogPopup.DialogManager.QueueMessagePanel(message);
                }
                else
                    attemptToLoadMods.Add(new Tuple<string, List<string>>(mod.ModInfo.ID, mod.ModInfo.RequiredMods));
            }

            GenerateModLoadOrder(attemptToLoadMods, out Queue<string> loadOrder, out List<string> failedMods);
            if (failedMods.Count != 0)
            {
                Utilities.Logger.Error("Some mods failed to load:");
                foreach(string failedMod in failedMods)
                {
                    Utilities.Logger.Error($"   {failedMod} failed to load as the following mods were missing: {ModsFound[failedMod].MissingDependencies()}.");
                    foreach(string requiredMod in ModsFound[failedMod].GetMissingDependencies())
                        ModHelper.DialogPopup.DialogManager.QueueMessagePanel($"{failedMod} requires the mod {requiredMod}");
                }
            }

            Utilities.Logger.Print("Mod load order decided:");
            foreach (string loadMod in loadOrder)
                Utilities.Logger.Print($"   {loadMod}");

            while (loadOrder.Count != 0)
            {
                string modToLoad = loadOrder.Dequeue();
                ModsFound.TryGetValue(modToLoad, out Mod mod);
                if (mod != null)
                    LoadMod(mod);
            }
        }

        public static Queue<string> GenerateModLoadOrder(List<Tuple<string, List<string>>> modsToLoad)
        {
            Queue<string> loadOrder = new Queue<string>();

            bool changed = false;
            do
            {
                changed = false;
                foreach (Tuple<string, List<string>> mod in modsToLoad)
                {
                    if (!loadOrder.Contains(mod.Item1))
                    {
                        mod.Item2.RemoveAll(x => loadOrder.Contains(x));
                        if (mod.Item2.Count == 0)
                        {
                            loadOrder.Enqueue(mod.Item1);
                            changed = true;
                        }
                    }
                }
            } while (changed);

            return loadOrder;
        }

        public static void GenerateModLoadOrder(List<Tuple<string, List<string>>> modsToLoad, out Queue<string> modLoadOrder, out List<string> modsFailedToLoad)
        {
            modLoadOrder = GenerateModLoadOrder(modsToLoad);
            modsFailedToLoad = new List<string>();
            foreach (string mod in ModsFound.Keys)
                if (modLoadOrder.Contains(mod) == false)
                    modsFailedToLoad.Add(mod);
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

                Utilities.Logger.Error($"Failed to execute entry point of mod: {mod.ModInfo.Name}");
                Utilities.Logger.Error(e.ToString());
            }
        }
    }
}
