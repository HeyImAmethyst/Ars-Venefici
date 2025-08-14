using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class ObjectHolder<T>
    {
        private T obj;

        public ObjectHolder(T obj)
        {
            this.obj = obj;
        }

        public T Get() 
        { 
            return obj; 
        }
    }
}
