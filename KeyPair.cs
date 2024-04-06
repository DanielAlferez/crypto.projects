namespace crypto.projects
{
    public class KeyPair
    {
        public string KeyId { get; set; } // Represents the identifier of a key pair.
        public string PrivateKey { get; set; } // Stores the private key in base64 format.
        public string PublicKey { get; set; } // Stores the public key in base64 format.
        public RSAParametersJson Parameters { get; set; } // Stores RSA parameters in JSON format.
    }

    public class RSAParametersJson
    {
        public byte[] D { get; set; } // Stores the private exponent.
        public byte[] DP { get; set; } // Stores the private exponent modulated by (P-1).
        public byte[] DQ { get; set; } // Stores the private exponent modulated by (Q-1).
        public byte[] Exponent { get; set; } // Stores the public exponent.
        public byte[] InverseQ { get; set; } // Stores the inverse of Q modulo P.
        public byte[] Modulus { get; set; } // Stores the modulus.
        public byte[] P { get; set; } // Stores the first factor of the modulus.
        public byte[] Q { get; set; } // Stores the second factor of the modulus.
    }
}
