using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class MutableKeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public MutableKeyValuePair (TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public MutableKeyValuePair(TKey key, ref TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
