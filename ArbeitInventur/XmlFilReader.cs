using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ArbeitInventur
{
    public class XmlFilReader
    {
        static string xmlFilePath = Properties.Settings.Default.XMLSettinsPath;
        static string settingsFilePath = "settings.txt";
        public static string selectPathTemplate = "";
        static string Status;

        public static bool UpdateTemplate(string newFilenameTemplate, string newPathTemplate)
        {
            if (string.IsNullOrWhiteSpace(xmlFilePath) || !File.Exists(xmlFilePath))
            {
                MessageBox.Show("XML-Datei wurde nicht gefunden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);  // Direkt laden, ohne FileStream

                XmlNode filenameNode = xmlDoc.SelectSingleNode("//FilenameTemplate");
                XmlNode pathNode = xmlDoc.SelectSingleNode("//PathTemplate");

                if (filenameNode == null || pathNode == null)
                {
                    MessageBox.Show("Ungültige XML-Struktur: Knoten fehlen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                selectPathTemplate = filenameNode.InnerText;
                string currentPathTemplate = pathNode.InnerText;

                bool updated = false;
                if (selectPathTemplate != newFilenameTemplate)
                {
                    filenameNode.InnerText = newFilenameTemplate;
                    updated = true;
                }

                if (currentPathTemplate != newPathTemplate)
                {
                    pathNode.InnerText = newPathTemplate;
                    updated = true;
                }

                if (updated)
                {
                    xmlDoc.Save(xmlFilePath);
                    MessageBox.Show("Dateinamensformat erfolgreich aktualisiert.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren der XML-Datei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public static void LoadSettings()
        {
            try
            {
                // Falls settings.txt nicht existiert, erstelle sie mit einem Standardpfad
                if (!File.Exists(settingsFilePath))
                {
                    MessageBox.Show("Einstellungsdatei nicht gefunden! Eine neue wird erstellt.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CreateDefaultSettingsFile();
                }

                // Falls XML-Datei nicht existiert, erstelle eine neue
                if (!File.Exists(xmlFilePath))
                {
                    MessageBox.Show("XML-Datei nicht gefunden. Eine neue wird erstellt.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // XML-Datei laden
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);

                // FilenameTemplate auslesen
                XmlNode filenameNode = xmlDoc.SelectSingleNode("//FilenameTemplate");
                if (filenameNode != null)
                {
                    selectPathTemplate = filenameNode.InnerText;
                }
                else
                {
                    MessageBox.Show("Der Knoten 'FilenameTemplate' wurde in der XML-Datei nicht gefunden. Standardwert wird gesetzt.", "Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    selectPathTemplate = "%d_%n-%s";
                }
            }
            catch (XmlException)
            {
                MessageBox.Show("Fehler beim Laden der XML-Datei. Die Datei ist möglicherweise beschädigt. Eine neue wird erstellt.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Einstellungen: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void CreateDefaultSettingsFile()
        {
            string defaultXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");
            File.WriteAllText(settingsFilePath, defaultXmlPath);
        }
    }

}
