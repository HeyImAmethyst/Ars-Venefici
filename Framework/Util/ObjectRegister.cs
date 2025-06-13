using ArsVenefici.Framework.Spells.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class ObjectRegister<T>
    {
        protected static string modID;
        protected List<ObjectHolder<T>> list;

        public ObjectRegister(string id)
        {
            modID = id;
            list = new List<ObjectHolder<T>>();
        }

        public static ObjectRegister<T> Create(string id)
        {
            return new ObjectRegister<T>(id);
        }

        public ObjectHolder<T> Register(ObjectHolder<T> obj)
        {
            list.Add(obj);
            return obj;
        }

        public List<ObjectHolder<T>> GetObjectList()
        {
            return list;
        }
    }
}
