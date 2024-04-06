using System;
using crypto.projects; // import the namespace where KeysManagement is located

namespace crypto.projects
{
    class Program
    {
        static void Main()
        {
            string jsonFilePath = @"C:\Users\danal\source\repos\profu\crypto.projects\";
            HandleKeys handleKeys = new HandleKeys(); // Instance of KeysManagement for key management
            Decompressor decompress = new Decompressor(); // Instance of Decompressor for decompression and signature verification

            while (true)
            {
                Console.WriteLine("\nKeys Management Menu:");
                Console.WriteLine("1. Generate RSA key pair");
                Console.WriteLine("2. Generate Signature (compresses the signature, public key, and message)");
                Console.WriteLine("3. Decompress and verify signature");
                Console.WriteLine("4. Exit");

                Console.Write("Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        handleKeys.GenerateKeys(); // Generates a new RSA key pair
                        break;
                    case "2":
                        handleKeys.DisplayKeys(jsonFilePath); // Displays existing keys and allows message signing
                        break;
                    case "3":
                        decompress.Decompress(jsonFilePath); // Decompresses and verifies a signature
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
