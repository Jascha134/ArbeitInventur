using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.UserInterface
{
    public class PasswortHashing
    {
        // Erzeugt ein Hash für ein Passwort unter Verwendung von PBKDF2
        public static string ErzeugePasswortHash(string passwort)
        {
            // Erzeuge ein 16-Byte-Salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Erzeuge den Hash mit PBKDF2 und 10000 Iterationen
            var pbkdf2 = new Rfc2898DeriveBytes(passwort, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20); // 20-Byte-Hash erzeugen

            // Salt und Hash zusammenführen
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Rückgabe als Base64-String
            return Convert.ToBase64String(hashBytes);
        }

        // Überprüft, ob ein eingegebenes Passwort mit dem gespeicherten Hash übereinstimmt
        public static bool UeberpruefePasswort(string passwort, string gespeicherterHash)
        {
            // Entpacke Salt und Hash aus dem gespeicherten Hash
            byte[] hashBytes = Convert.FromBase64String(gespeicherterHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Erzeuge einen neuen Hash des eingegebenen Passworts mit dem gespeicherten Salt
            var pbkdf2 = new Rfc2898DeriveBytes(passwort, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Vergleiche die Hashes byteweise
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false; // Passwort ist falsch
                }
            }

            return true; // Passwort stimmt überein
        }
    }
}
