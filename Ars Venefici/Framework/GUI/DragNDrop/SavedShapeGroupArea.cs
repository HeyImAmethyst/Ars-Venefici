using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.DragNDrop
{
    public class SavedShapeGroupArea<T>
    {
        protected List<T> contents = new List<T>();

        public virtual List<T> GetAll()
        {
            return contents;
        }

        public virtual void SetAll(List<T> list)
        {
            contents = list;
        }

        public virtual void SetAll(int index, List<T> list)
        {
            contents = list;
        }
    }
}
