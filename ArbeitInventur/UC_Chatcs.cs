using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class UC_Chatcs : UserControl
    {
        private string jsonDateiPfad = "C:\\Users\\jakov\\Desktop\\Impla\\chatMessages.json"; // Pfad zur JSON-Datei auf dem Server
        private JsonDateiÜberwacher jsonÜberwacher;
        private List<ChatMessage> chatMessages = new List<ChatMessage>();
        private System.Timers.Timer debounceTimer;
        private string User;
        public UC_Chatcs(Benutzer benutzer)
        {
            InitializeComponent();
            // JSON-Überwachung starten
            User = benutzer.Name;
            lb_Benutzer.Text = benutzer.Name + " :";
            jsonÜberwacher = new JsonDateiÜberwacher(jsonDateiPfad);
            jsonÜberwacher.DateiGeändert += JsonDateiWurdeGeändert;

            // Timer initialisieren
            debounceTimer = new System.Timers.Timer(500); // 500 ms Verzögerung
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += DebounceTimerElapsed;

            // Nachrichten laden und anzeigen
            LadeNachrichtenUndAnzeigen();

            txt_Chat.KeyDown += txt_Chat_KeyDown;
        }
    
        private void button1_Click(object sender, EventArgs e)
        {

            // Nachricht erstellen
            var neueNachricht = new ChatMessage
            {
                Sender = User, // Oder ein anderer Identifier
                Message = txt_Chat.Text,
                Timestamp = DateTime.Now
            };


            // Nachricht zur Liste hinzufügen
            chatMessages.Add(neueNachricht);

            // Nachrichten in der JSON-Datei speichern
            SpeichereNachrichten();

            // Eingabefeld leeren
            txt_Chat.Clear();
        }
        private void LadeNachrichtenUndAnzeigen()
        {
            // Laden der Nachrichten aus der JSON-Datei
            if (File.Exists(jsonDateiPfad))
            {
                try
                {
                    string jsonContent = File.ReadAllText(jsonDateiPfad);
                    chatMessages = JsonConvert.DeserializeObject<List<ChatMessage>>(jsonContent) ?? new List<ChatMessage>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Laden der Nachrichten: " + ex.Message);
                }
            }

            // Nachrichten in der ListBox anzeigen
            ListBoxChat.Items.Clear();
            foreach (var message in chatMessages)
            {
                ListBoxChat.Items.Add($"[{message.Timestamp}] {message.Sender}: {message.Message}");
            }
        }

        private void SpeichereNachrichten()
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(chatMessages, Formatting.Indented);
                File.WriteAllText(jsonDateiPfad, jsonContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern der Nachricht: " + ex.Message);
            }
        }

        private void JsonDateiWurdeGeändert()
        {
            // Wenn der FileSystemWatcher eine Änderung erkennt, den Timer neu starten
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void DebounceTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    LadeNachrichtenUndAnzeigen();
                }));
            }
            else
            {
                LadeNachrichtenUndAnzeigen();
            }
        }

        private void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Überwachung stoppen, wenn das Formular geschlossen wird
            jsonÜberwacher.StopÜberwachung();
        }

        private void txt_Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Verhindere das typische "Pling"-Geräusch, wenn Enter gedrückt wird
                e.SuppressKeyPress = true;

                // Methode zum Senden der Nachricht aufrufen
                button1_Click(sender, e);
            }
        }
    }
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
