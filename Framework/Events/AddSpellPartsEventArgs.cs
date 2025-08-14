using ArsVenefici.Framework.API.Spell;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Events
{
    public class AddSpellPartsEventArgs : EventArgs
    {
        ISpellPart part;

        public AddSpellPartsEventArgs(ISpellPart part)
        {
            this.part = part;
        }
    }
}
