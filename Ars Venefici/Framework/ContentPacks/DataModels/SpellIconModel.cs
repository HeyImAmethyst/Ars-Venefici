using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.ContentPacks.DataModels
{
    /// <summary>
    /// Class used to add spell part packs
    /// </summary>
    public class SpellIconModel
    {
        // The list of spell icons
        public Texture2D[] Textures;

        // The mod name where the spell icons came from
        public string ModName;
    }
}
