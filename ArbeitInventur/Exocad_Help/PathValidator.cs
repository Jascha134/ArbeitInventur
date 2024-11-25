using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.Exocad_Help
{
    public static class PathValidator
    {
        public static bool ArePathsValid()
        {
            string exocadConstructionsPath = Properties.Settings.Default.ExocadConstructions;
            string dentalCADPath = Properties.Settings.Default.ExocaddentalCAD;

            return IsPathValid(exocadConstructionsPath) && IsPathValid(dentalCADPath);
        }

        public static bool IsPathValid(string path)
        {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }
    }
}
