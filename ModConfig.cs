using Microsoft.Xna.Framework;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici
{
    public class ModConfig
    {
        /// <summary>The button to open the spell book menu.</summary>
        public SButton OpenSpellBookButton { get; set; }

        public SButton MoveCurrentSpellLable { get; set; } = SButton.OemTilde;

        /// <summary>The pixel position at which to draw the spell lable, relative to the top-left corner of the screen.</summary>
        public Point Position { get; set; } = new(10, 10);

        public SButton NextSpellButton { get; set; } = SButton.X;
        public SButton PreviousSpellButton { get; set; } = SButton.Z;

        public SButton CastSpellButton { get; set; } = SButton.Q;

        public ModConfig()
        {
            this.OpenSpellBookButton = SButton.B;
        }
    }
}
