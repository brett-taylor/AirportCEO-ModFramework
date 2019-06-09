using System;

namespace ACMF.ModHelper.PatchTime
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PatchTimeMethod : Attribute
    {
    }
}
