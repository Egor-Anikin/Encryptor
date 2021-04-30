using System;
using System.Collections.Generic;
using System.Text;

namespace Encryptor.PageModels
{
    public class Cipher
    {
        const string alphabetRu = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        const string alphabetEn = "abcdefghijklmnopqrstuvwxyz";

        private static bool ChecKey(string textKey, out string key, out string alphabet)
        {
            key = textKey;
            string[] ar = { ",", ".", "!", "?", "-", " ", "\t", "\n" };
            for (int i = 0; i < ar.Length; i++)
            {
                key = key.Replace(ar[i], "");
            }

            if (alphabetRu.IndexOf(Char.ToLower(key[0])) != -1)
            {
                alphabet = alphabetRu;
            }
            else
            {
                alphabet = alphabetEn;
            }

            for (int i = 0; i < key.Length; i++)
            {
                if(alphabet.IndexOf(Char.ToLower(key[i])) == -1)
                {
                    return false;
                }
            }
            return true;

        }

        public static string Encrypt(string text, string textKey, bool encrypting)
        {
            string result = "";
            int keyPrt = 0;
            if(!ChecKey(textKey, out string key, out string alphabet))
            {
                return null;
            }

            for (int i = 0; i < text.Length; i++)
            {
                int letterIndex = alphabet.IndexOf(Char.ToLower(text[i]));
                if (letterIndex < 0)
                {
                    result += text[i].ToString();
                }
                else
                {
                    int keyIndex = alphabet.IndexOf(Char.ToLower(key[keyPrt])); // +1
                    string c = alphabet[(alphabet.Length + letterIndex + ((encrypting ? 1 : -1) * keyIndex)) % alphabet.Length].ToString();
                    result += Char.IsUpper(text[i]) ? c.ToUpper() : c;
                    keyPrt++;
                    if (keyPrt == key.Length)
                    {
                        keyPrt = 0;
                    }
                }
            }

            return result;
        }
    }
}
