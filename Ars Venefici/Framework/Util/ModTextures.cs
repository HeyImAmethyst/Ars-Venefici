using ItemExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class ModTextures
    {
        //Mana bar
        public static Texture2D MANA_BG;
        public static Texture2D MANA_FG;

        public static Texture2D AOE;
        public static Texture2D BEAM;
        public static Texture2D TOUCH;
        public static Texture2D PROJECTILE;

        public static Texture2D ELEMENT_PARTICLE_TEXTURE;

        public static Texture2D JUNIMO;


        public static void LoadAssets(IModHelper helper)
        {
            MANA_BG = helper.ModContent.Load<Texture2D>("assets/farmer/manabg.png");

            Color manaCol = new Color(0, 48, 255);
            MANA_BG = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            MANA_BG.SetData(new[] { manaCol });

            AOE = helper.ModContent.Load<Texture2D>("assets/aoe/aoe.png");
            BEAM = helper.ModContent.Load<Texture2D>("assets/beam/beam.png");
            TOUCH = helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");
            PROJECTILE = helper.ModContent.Load<Texture2D>("assets/projectile/projectile.png");

            ELEMENT_PARTICLE_TEXTURE = helper.ModContent.Load<Texture2D>("assets/particle/damage_effect.png");

            JUNIMO = Game1.content.Load<Texture2D>("Characters\\Junimo");
        }
    }
}
