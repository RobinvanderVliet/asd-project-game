using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ASD_project.Creature
{
    [ExcludeFromCodeCoverage]
    public class SuperUgly
    {
        private static readonly char _seperator = Path.DirectorySeparatorChar;
        private static readonly string _currentDirectory =
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..{_seperator}..{_seperator}..{_seperator}"));

        private static readonly string _base_path = $"{_currentDirectory}..{_seperator}ASD-Game{_seperator}";
        public static readonly string MONSTER_PATH = $"{_base_path}resource{_seperator}npc{_seperator}monster.cfg";
    }
}
