using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace crypto.projects
{
    public class Decompressor
    {
        public void Decompress(string filePath)
        {
            try
            {
                Console.Write("Enter the name of the compressed file with (.zip): ");
                string zipFilePath = Console.ReadLine();
                string fullPath = filePath + zipFilePath;

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("The specified file does not exist.");
                    return;
                }

                string extractPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExtractedFiles");
                ZipFile.ExtractToDirectory(fullPath, extractPath);

                string message = File.ReadAllText(Path.Combine(extractPath, "Message.txt"));
                string publicKeyXml = File.ReadAllText(Path.Combine(extractPath, "PublicKey.txt"));
                byte[] signature = File.ReadAllBytes(Path.Combine(extractPath, "Signature.txt"));

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKeyXml);

                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                    bool verified = rsa.VerifyData(messageBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    if (verified)
                    {
                        Console.WriteLine("The signature is valid.");
                    }
                    else
                    {
                        Console.WriteLine("The signature is invalid.");
                    }
                }

                Directory.Delete(extractPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
