using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesManagers
{
    sealed class EncryptionUtility
    {
        //To prevent from to prevent instances from being created with "new EncryptionUtility()"
        private EncryptionUtility()
        {

        }

        //Do not change
        private static string _key = "N0Bllwft7IWnKJ7WnsLbuw==";
        private static string _algorithmName = "RC2";

        internal static string EncryptData(string data)
        {
            byte[] ClearData = Encoding.ASCII.GetBytes(data);
            SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create(_algorithmName);
            MemoryStream Target = new MemoryStream();
            Algorithm.Key = Convert.FromBase64String(_key);
            Algorithm.GenerateIV();
            Target.Write(Algorithm.IV, 0, Algorithm.IV.Length);
            CryptoStream cs = new CryptoStream(Target, Algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(ClearData, 0, ClearData.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(Target.ToArray());
        }

        internal static string DecryptData(string data)
        {
            byte[] bytes = Convert.FromBase64String(Convert.ToString(data).Trim());
            SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create(_algorithmName);
            Algorithm.Key = Convert.FromBase64String(_key);
            MemoryStream Target = new MemoryStream();
            int ReadPos = 0;
            byte[] IV = new byte[Algorithm.IV.Length];
            Array.Copy(bytes, IV, IV.Length);
            Algorithm.IV = IV;
            ReadPos += Algorithm.IV.Length;
            CryptoStream cs = new CryptoStream(Target, Algorithm.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, ReadPos, bytes.Length - ReadPos);
            cs.FlushFinalBlock();
            return Encoding.ASCII.GetString(Target.ToArray());
        }
    }
}
