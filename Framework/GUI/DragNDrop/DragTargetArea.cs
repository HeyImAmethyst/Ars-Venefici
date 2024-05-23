using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.DragNDrop
{
    public abstract class DragTargetArea<T> : DragArea<T>
    {
        protected int maxElements;

        public DragTargetArea(Rectangle bounds, int maxElements, string name) : base(bounds, name)
        {
            this.maxElements = maxElements;
        }

        public DragTargetArea(Rectangle bounds, int maxElements, string name, string lable) : base(bounds, name, lable)
        {
            this.maxElements = maxElements;
        }

        public override bool CanDrop(T draggable, int mouseX, int mouseY)
        {
            return CanStore();
        }

        public virtual bool CanStore()
        {
            return maxElements > contents.Count();
        }
    }
}
