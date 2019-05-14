using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;
using System.IO;

namespace ACMLInstaller
{
    public class Patcher
    {
        public static void Patch(string dllDirectory, string acmlDLL, string patchCallType, string patchCallMethod, string targetCallType, string targetCallMethod, 
            string copyExtension, int PATCH_INTO_INSTRUCTION_NUMBER)
        {
            string newDLLDirectory = Path.Combine(Path.GetDirectoryName(dllDirectory), Path.GetFileNameWithoutExtension(dllDirectory)) + copyExtension;
            File.Move(dllDirectory, newDLLDirectory);
            File.Copy(acmlDLL, Path.Combine(Path.GetDirectoryName(dllDirectory), Path.GetFileName(acmlDLL)));

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
