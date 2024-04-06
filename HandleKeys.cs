using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace crypto.projects
{
    public class HandleKeys
    {
        private List<KeyPair> keyPairs;
        private Signing signingHandler;

        public HandleKeys()
        {
            keyPairs = new List<KeyPair>();
            signingHandler = new Signing();
        }

        public void GenerateKeys()
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    RSAParameters privateKey = rsa.ExportParameters(true);
                    RSAParameters publicKey = rsa.ExportParameters(false);
                    RSAParameters rsaParams = rsa.ExportParameters(true);
                    int nextIndex = keyPairs.Count;

                    keyPairs.Add(new KeyPair
                    {
                        KeyId = nextIndex.ToString(),
                        PrivateKey = Convert.ToBase64String(privateKey.D),
                        PublicKey = Convert.ToBase64String(publicKey.Modulus),
                        Parameters = new RSAParametersJson
                        {
                            D = rsaParams.D,
                            DP = rsaParams.DP,
                            DQ = rsaParams.DQ,
                            Exponent = rsaParams.Exponent,
                            InverseQ = rsaParams.InverseQ,
                            Modulus = rsaParams.Modulus,
                            P = rsaParams.P,
                            Q = rsaParams.Q
                        }
                    });

                    Console.WriteLine("Key pair generated and added.");
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine($"Cryptographic error: {e.Message}");
            }
        }

        public void DisplayKeys(string path)
        {
            Console.WriteLine("\nExisting Keys:");
            foreach (var keyPair in keyPairs)
            {
                Console.WriteLine($"Key ID: {keyPair.KeyId}");
                Console.WriteLine($"Private Key: {keyPair.PrivateKey}");
                Console.WriteLine($"Public Key: {keyPair.PublicKey}");
                Console.WriteLine();
            }

            Console.Write("Enter the ID of the key pair you want to select: ");
            string selectedId = Console.ReadLine();

            KeyPair selectedPair = keyPairs.Find(pair => pair.KeyId == selectedId);
            if (selectedPair != null)
            {
                signingHandler.Compress(selectedPair, path);
            }
            else
            {
                Console.WriteLine("Invalid key ID. No operation performed.");
            }
        }
    }
}
