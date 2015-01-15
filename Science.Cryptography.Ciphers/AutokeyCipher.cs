﻿using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Autokey cipher.
    /// </summary>
    public class AutokeyCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public AutokeyCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        public string Encrypt(string plaintext, string key)
        {
            string autokey = key + plaintext;

            char[] result = new char[plaintext.Length];
            int charCounter = 0;

            autokey = autokey.ToUpperInvariant();

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOf(plaintext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                    result[i] = (this.Charset[(idx + this.Charset.IndexOf(autokey[charCounter++])) % this.Charset.Length]).ToSameCaseAs(plaintext[i]);
                else
                    result[i] = plaintext[i];
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, string key)
        {
            char[] result = new char[ciphertext.Length];

            // create storages
            char[] plaintextLettersOnly = new char[ciphertext.Length];
            int plaintextOffset = 0;

            int keyOffset = 0;

            int i;

            // construct the initial part of the plaintext
            for (i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOf(ciphertext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                {
                    result[i] = (this.Charset[Mod(idx - this.Charset.IndexOf(key[plaintextOffset]), this.Charset.Length)]).ToSameCaseAs(ciphertext[i]);
                    plaintextLettersOnly[plaintextOffset] = Char.ToUpperInvariant(result[i]);
                    plaintextOffset++;

                    if (plaintextOffset == key.Length)
                        break;
                }
                else
                    result[i] = ciphertext[i];
            }

            // decipher the remaining message
            for (i++; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOf(ciphertext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                {
                    result[i] = (this.Charset[Mod(idx - this.Charset.IndexOf(plaintextLettersOnly[plaintextOffset - key.Length]), this.Charset.Length)]).ToSameCaseAs(ciphertext[i]);
                    plaintextLettersOnly[plaintextOffset] = Char.ToUpperInvariant(result[i]);
                    plaintextOffset++;

                    if (keyOffset == key.Length)
                        break;
                }
                else
                    result[i] = ciphertext[i];
            }

            return new String(result);
        }

        internal int Mod(int a, int b)
        {
            return a >= 0 ? a % b : (b + a) % b;
        }
    }
}
