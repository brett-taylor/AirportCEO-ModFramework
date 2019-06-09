using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ACMF.ModHelper.PatchTime
{
    internal static class PatchTimeManager
    {
        internal static void Initialise()
        {
            List<Assembly> assembliesToCheck = new List<Assembly>();
            assembliesToCheck.AddRange(ModLoader.ModLoader.ModsLoaded.Select(t => t.Value.Assembly));
            assembliesToCheck.Add(Assembly.GetExecutingAssembly());

            List<Tuple<Type, MethodInfo>> patchMethods = assembliesToCheck.SelectMany(t => t.GetTypes())
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(PatchTimeMethod), true).Length > 0 && m.ReflectedType.IsClass && !m.ReflectedType.IsAbstract)
                      .Select(m => new Tuple<Type, MethodInfo>(m.ReflectedType, m))
                      .ToList();

            Utilities.Logger.Print($"Found {patchMethods.Count()} Patch Methods.");
            foreach(Tuple<Type, MethodInfo> tuple in patchMethods)
            {
                try
                {
                    if (tuple.Item2.IsStatic)
                        tuple.Item2.Invoke(null, null);
                    else
                        tuple.Item2.Invoke(Activator.CreateInstance(tuple.Item1), null);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error($"{tuple.Item1} {tuple.Item2} || Failed To Patch: {e.ToString()}");
                }
            }
        }
    }
}
