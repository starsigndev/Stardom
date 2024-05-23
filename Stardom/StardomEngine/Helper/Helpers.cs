using System;
using System.Collections.Generic;
using System.Data.HashFunction.MurmurHash;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Helper
{
    public class Helpers
    {
        public static IMurmurHash3 murmurHash3 = MurmurHash3Factory.Instance.Create();

        public static int GenerateHash(string name)
        {

            byte[] hashBytes = murmurHash3.ComputeHash(Encoding.UTF8.GetBytes(name)).Hash;
            // Take the first 4 bytes of the hash to create an int value
            int hash = BitConverter.ToInt32(hashBytes, 0);
            return hash;

        }

        public static bool ListSame<T>(List<T> a,List<T> b) where T : class
        {
            if(a.Count!= b.Count) return false;

            int idx = 0;
            foreach(var i1 in a)
            {
                var i2 = b[idx];
                if (i1 != i2) return false;
                //if (i1 is b[idx]) return false;
                idx++;
            }

            return true;

        }

    }
}
