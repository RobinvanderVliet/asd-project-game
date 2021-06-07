using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Creature
{
    [ExcludeFromCodeCoverage]
    public class SuperUgly
    {
        private static readonly char _separator = Path.DirectorySeparatorChar;
        private static readonly string _currentDirectory =
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..{_separator}..{_separator}..{_separator}"));

        private static readonly string _base_path = $"{_currentDirectory}..{_separator}ASD-Game{_separator}";
        public static readonly string MONSTER_PATH = $"{_base_path}resource{_separator}npc{_separator}monster.cfg";
    }
}
