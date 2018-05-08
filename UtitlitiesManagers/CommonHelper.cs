using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesManagers
{
    public class CommonHelper
    {
        public string EncryptData(object DataforEcryption)
        {
            return EncryptionUtility.EncryptData(Convert.ToString(DataforEcryption));
        }

        public string DecryptData(Object DataforEcryption)
        {
            return EncryptionUtility.DecryptData(Convert.ToString(DataforEcryption).Trim());
        }

        //Function to get random number
        //private static readonly Random getrandom = new Random();
        public long GetRandomNumber(int min, int max)
        {
            Random getrandom = new Random();
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }
    }
}
