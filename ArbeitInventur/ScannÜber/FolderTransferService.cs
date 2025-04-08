using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.ScannÜber
{
    /// <summary>
    /// Kopiert den Inhalt eines Ordners (inklusive Unterordner) in einen Zielordner.
    /// </summary>
    public class FolderTransferService
    {
        public void CopyFolder(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Quellordner nicht gefunden: {sourceDir}");
            }

            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(directory));
                CopyFolder(directory, destSubDir);
            }
        }
    }
}
