using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SERVICIOS
{
    // Encriptacion reversible (AES-256, modo CBC) para datos que hay que poder volver a leer,
    // como el domicilio del cliente. A diferencia de Encriptacion.GenerarHash, se puede desencriptar.
    // Formato guardado: "AES$" + Base64( IV de 16 bytes + texto cifrado ). El IV es distinto en cada encriptacion.
    public class EncriptacionReversible
    {
        private const string Prefijo_235JB = "AES$";
        private const string Frase_235JB = "CAFECCINO-235JB-clave-de-domicilio";

        // La clave de 256 bits sale del SHA-256 de la frase.
        private static byte[] ObtenerClave_235JB()
        {
            using (SHA256 sha_235JB = SHA256.Create())
                return sha_235JB.ComputeHash(Encoding.UTF8.GetBytes(Frase_235JB));
        }

        public bool EstaEncriptado_235JB(string texto_235JB)
        {
            return !string.IsNullOrEmpty(texto_235JB) && texto_235JB.StartsWith(Prefijo_235JB, StringComparison.Ordinal);
        }

        public string Encriptar_235JB(string textoPlano_235JB)
        {
            if (string.IsNullOrEmpty(textoPlano_235JB)) return textoPlano_235JB;

            using (Aes aes_235JB = Aes.Create())
            {
                aes_235JB.Key = ObtenerClave_235JB();
                aes_235JB.Mode = CipherMode.CBC;
                aes_235JB.Padding = PaddingMode.PKCS7;
                aes_235JB.GenerateIV();

                byte[] plano_235JB = Encoding.UTF8.GetBytes(textoPlano_235JB);
                byte[] cifrado_235JB;
                using (ICryptoTransform cifrador_235JB = aes_235JB.CreateEncryptor())
                    cifrado_235JB = cifrador_235JB.TransformFinalBlock(plano_235JB, 0, plano_235JB.Length);

                byte[] todo_235JB = new byte[aes_235JB.IV.Length + cifrado_235JB.Length];
                Buffer.BlockCopy(aes_235JB.IV, 0, todo_235JB, 0, aes_235JB.IV.Length);
                Buffer.BlockCopy(cifrado_235JB, 0, todo_235JB, aes_235JB.IV.Length, cifrado_235JB.Length);
                return Prefijo_235JB + Convert.ToBase64String(todo_235JB);
            }
        }

        // Si el texto no esta encriptado (dato anterior a la encriptacion) lo devuelve tal cual.
        public string Desencriptar_235JB(string textoCifrado_235JB)
        {
            if (!EstaEncriptado_235JB(textoCifrado_235JB)) return textoCifrado_235JB;

            try
            {
                byte[] todo_235JB = Convert.FromBase64String(textoCifrado_235JB.Substring(Prefijo_235JB.Length));
                using (Aes aes_235JB = Aes.Create())
                {
                    aes_235JB.Key = ObtenerClave_235JB();
                    aes_235JB.Mode = CipherMode.CBC;
                    aes_235JB.Padding = PaddingMode.PKCS7;

                    byte[] iv_235JB = new byte[16];
                    Buffer.BlockCopy(todo_235JB, 0, iv_235JB, 0, 16);
                    aes_235JB.IV = iv_235JB;

                    using (ICryptoTransform descifrador_235JB = aes_235JB.CreateDecryptor())
                    {
                        byte[] plano_235JB = descifrador_235JB.TransformFinalBlock(todo_235JB, 16, todo_235JB.Length - 16);
                        return Encoding.UTF8.GetString(plano_235JB);
                    }
                }
            }
            catch (Exception ex_235JB) when (ex_235JB is CryptographicException || ex_235JB is FormatException || ex_235JB is ArgumentException)
            {
                return textoCifrado_235JB;
            }
        }
    }
}
