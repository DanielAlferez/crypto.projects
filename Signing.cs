using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace crypto.projects
{
    public class Signing
    {
        private int FileNumber = 1; // Counter for naming compressed files

        public Signing()
        {
        }

        public void Compress(KeyPair selectedPair, string filePath)
        {
            try
            {
                if (selectedPair == null)
                    throw new ArgumentNullException(nameof(selectedPair), "No key pair selected.");

                Console.Write("Enter the message you want to sign: ");
                string message = Console.ReadLine();

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(selectedPair.Parameters.ToRSAParameters());

                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                    string messagePath = Path.Combine(filePath, "Message.txt");
                    File.WriteAllText(messagePath, message);

                    string publicKeyPath = Path.Combine(filePath, "PublicKey.txt");
                    File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));

                    byte[] signature = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                    string signaturePath = Path.Combine(filePath, "Signature.txt");
                    File.WriteAllBytes(signaturePath, signature);

                    string zipFileName = $"CompressedValues{FileNumber}.zip";
                    while (File.Exists(Path.Combine(filePath, zipFileName)))
                    {
                        FileNumber++;
                        zipFileName = $"CompressedValues{FileNumber}.zip";
                    }

                    using (ZipArchive zip = ZipFile.Open(Path.Combine(filePath, zipFileName), ZipArchiveMode.Create))
                    {
                        zip.CreateEntryFromFile(messagePath, "Message.txt");
                        zip.CreateEntryFromFile(publicKeyPath, "PublicKey.txt");
                        zip.CreateEntryFromFile(signaturePath, "Signature.txt");
                    }

                    Console.WriteLine($"Files compressed successfully as {zipFileName}.");

                    File.Delete(messagePath);
                    File.Delete(publicKeyPath);
                    File.Delete(signaturePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public static class RSAParametersExtensions
    {
        public static RSAParameters ToRSAParameters(this RSAParametersJson rsaParamsJson)
        {
            return new RSAParameters
            {
                D = rsaParamsJson.D,
                DP = rsaParamsJson.DP,
                DQ = rsaParamsJson.DQ,
                Exponent = rsaParamsJson.Exponent,
                InverseQ = rsaParamsJson.InverseQ,
                Modulus = rsaParamsJson.Modulus,
                P = rsaParamsJson.P,
                Q = rsaParamsJson.Q
            };
        }
    }
}
