using System;

namespace ACMF.ModHelper.EnumPatcher
{
    public partial class EnumCache<T> : IEnumCache where T : Enum
    {
        private static EnumCache<T> InternalInstance = null;
        public static EnumCache<T> Instance
        {
            get
            {
                if (InternalInstance == null)
                    InternalInstance = LoadOrCreateInstance<T>();

                return InternalInstance;
            }
        }
    }
}
