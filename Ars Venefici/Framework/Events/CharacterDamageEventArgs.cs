using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Events
{
    public class CharacterDamageEventArgs : EventArgs
    {
        public Character character;

        public CharacterDamageEventArgs(Character character)
        {
            this.character = character; 
        }
    }
}
