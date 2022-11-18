using Assets._Scripts.Network.Enum;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Assets._Scripts.Network.Message
{
    class RegisterMessage : BaseMessage
    {
        public string username;
        public string passwordHash;

        public RegisterMessage(string dataUsername, string dataPassword) : base(MessageType.Register)
        {
            username = dataUsername;
            passwordHash = GetHashString(dataPassword);
        }

        private byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
