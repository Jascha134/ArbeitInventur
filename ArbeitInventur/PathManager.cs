using System;
using System.IO;

namespace ArbeitInventur
{
    /// <summary>
    /// Centralized management of file system paths used throughout the application.
    /// </summary>
    public class PathManager
    {
        public string DataJsonPath { get; private set; }
        public string ExocadConstructionsPath { get; private set; }
        public string ExocadDentalCadPath { get; private set; }
        public string LocalScanFolderPath { get; private set; }
        public string ServerTargetFolderPath { get; private set; }

        public PathManager()
        {
            LoadFromSettings();
        }

        private void LoadFromSettings()
        {
            DataJsonPath = Properties.Settings.Default.DataJSON ?? @"C:\ArbeitInventur\Data";
            ExocadConstructionsPath = Properties.Settings.Default.ExocadConstructions ?? @"C:\Exocad\Constructions";
            ExocadDentalCadPath = Properties.Settings.Default.ExocaddentalCAD ?? @"C:\Exocad\DentalCAD";
            LocalScanFolderPath = Properties.Settings.Default.LocalScanFolder ?? @"C:\ArbeitInventur\ScanFolder";
            ServerTargetFolderPath = Properties.Settings.Default.ServerTargetFolder ?? @"\\Server\ArbeitInventur\Target";
        }

        public bool AreAllPathsValid()
        {
            return ValidatePath(DataJsonPath) && ValidatePath(ExocadConstructionsPath) &&
                   ValidatePath(ExocadDentalCadPath) && ValidatePath(LocalScanFolderPath) &&
                   ValidatePath(ServerTargetFolderPath);
        }

        private bool ValidatePath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists(path);
        }

        public void UpdatePath(string propertyName, string newPath)
        {
            if (string.IsNullOrWhiteSpace(newPath) || !Directory.Exists(newPath))
                throw new ArgumentException($"Invalid path: {newPath}");

            switch (propertyName)
            {
                case nameof(DataJsonPath):
                    DataJsonPath = newPath;
                    Properties.Settings.Default.DataJSON = newPath;
                    break;
                case nameof(ExocadConstructionsPath):
                    ExocadConstructionsPath = newPath;
                    Properties.Settings.Default.ExocadConstructions = newPath;
                    break;
                case nameof(ExocadDentalCadPath):
                    ExocadDentalCadPath = newPath;
                    Properties.Settings.Default.ExocaddentalCAD = newPath;
                    break;
                case nameof(LocalScanFolderPath):
                    LocalScanFolderPath = newPath;
                    Properties.Settings.Default.LocalScanFolder = newPath;
                    break;
                case nameof(ServerTargetFolderPath):
                    ServerTargetFolderPath = newPath;
                    Properties.Settings.Default.ServerTargetFolder = newPath;
                    break;
                default:
                    throw new ArgumentException($"Unknown path property: {propertyName}");
            }
            Properties.Settings.Default.Save();
        }
    }
}