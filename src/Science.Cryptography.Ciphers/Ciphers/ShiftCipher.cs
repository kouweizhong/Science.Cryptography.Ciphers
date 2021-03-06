﻿using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Shift cipher.
    /// </summary>
    [Export("Shift", typeof(IKeyedCipher<>))]
    public class ShiftCipher : IKeyedCipher<int>, ISupportsCustomCharset
    {
        public ShiftCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        /// <summary>
        /// Creates Julius Caesar's cipher.
        /// </summary>
        public static ICipher CreateCaesar(string charset = Charsets.English)
        {
            return new ShiftCipher(charset).Pin(WellKnownShiftCipherKeys.Caesar);
        }

        /// <summary>
        /// Creates a ROT-13 cipher.
        /// </summary>
        public static ICipher CreateRot13(string charset = Charsets.English)
        {
            return new ShiftCipher(charset).Pin(WellKnownShiftCipherKeys.Rot13);
        }


        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string Crypt(string text, int key)
        {
            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(text[i]);

                result[i] = idx != -1
                    ? this.Charset.At(idx + key).ToSameCaseAs(text[i])
                    : text[i]
                ;
            }

            return new String(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext, int key)
        {
            return this.Crypt(plaintext, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext, int key)
        {
            return this.Crypt(ciphertext, -key);
        }
    }
}
