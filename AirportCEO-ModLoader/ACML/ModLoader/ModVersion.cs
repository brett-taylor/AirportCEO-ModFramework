using System.Text.RegularExpressions;

namespace ACML.ModLoader
{
    public class ModVersion
    {
        public static readonly ModVersion Default = new ModVersion(1, 0, 0);
        public static readonly Regex ModVersionRegEx = new Regex(@"[0-9]+.[0-9]+.[0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }

        public ModVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public static ModVersion Parse(string version)
        {
            if (ModVersionRegEx.IsMatch(version))
            {
                string[] split = version.Split('.');
                int.TryParse(split[0], out int major);
                int.TryParse(split[1], out int minor);
                int.TryParse(split[2], out int patch);
                return new ModVersion(major, minor, patch);
            }

            return Default;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}
