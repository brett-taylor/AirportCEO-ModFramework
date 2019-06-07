using System;
using System.Linq;
using ACMF.ModHelper.ModPrefabs;
using System.Collections.Generic;
using System.Reflection;

namespace ACMF.ModHelper.PatchTime
{
    internal static class PatchTimeManager
    {
        internal static void Initialise()
        {
            List<Assembly> assembliesToCheck = new List<Assembly>();
            assembliesToCheck.Add(Assembly.GetExecutingAssembly());
            assembliesToCheck.AddRange(ModLoader.ModLoader.ModsLoaded.Select(t => t.Value.Assembly));

            IEnumerable<Type> patchTimeTypes = assembliesToCheck.SelectMany(t => t.GetTypes()).
                Where(t => t.IsSubclassOf(typeof(PatchableClass)) && t.IsClass && !t.IsAbstract);

            Utilities.Logger.Print($"Found {patchTimeTypes.Count()} Patch Time Types.");
            foreach(Type type in patchTimeTypes)
            {
                if (Activator.CreateInstance(type) is PatchableClass patchableClass)
                {
                    try
                    {
                        patchableClass.Patch();
                    }
                    catch (Exception e)
                    {
                        Utilities.Logger.Error($"{patchableClass} Failed To Patch: {e.ToString()}");
                    }
                }
            }
        }
    }
}
