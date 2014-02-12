using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var aes = new SimpleAES();

            var text = "abc";
            var text_encrypted = aes.Encrypt(text);
            var text_decrypted = aes.Decrypt(text_encrypted);

            Console.WriteLine("Clear text: " + text);
            Console.WriteLine("Encrypted text: " + text_encrypted);
            Console.WriteLine("Decrypted text: " + text_decrypted);

            Console.ReadKey();
        }
    }
}
