using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.CustomObjects
{
    public class PassableTorch : Torch
    {
        public override bool isPassable()
        {
            return true;
        }
    }
}
