using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Buffs
{
    public class Buffs
    {
        ModEntry modEntry;

        public Buff growSickNess;
        public Buff hasteBuff;
        public Buff regenerationBuff;
        public Buff manaRegenerationBuff;
        public Buff shieldBuff;

        public Buffs(ModEntry modEntry)
        {
            this.modEntry = modEntry;

            InitializeBuffs();
        }

        public void InitializeBuffs()
        {

            growSickNess = new Buff(
                id: "HeyImAmethyst.ArsVenifici_GrowSickness",
                displayName: modEntry.Helper.Translation.Get("debuff.grow_sickness.name"),
                iconTexture: Game1.buffsIcons, //modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/BuffsIcons.png"),
                iconSheetIndex: 25,
                duration: 84_000 // 84 seconds
            );

            hasteBuff = new Buff(
                id: "HeyImAmethyst.ArsVenifici_Haste",
                displayName: modEntry.Helper.Translation.Get("spellpart.haste.name"),
                iconTexture: Game1.buffsIcons, //modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/BuffsIcons.png"),
                iconSheetIndex: 9,
                duration: 30_000, // 30 seconds
                effects: new BuffEffects()
                {
                    Speed = { 2 } // shortcut for buff.Speed.Value = 10
                }
            );

            regenerationBuff = new Buff(
                id: "HeyImAmethyst.ArsVenifici_HealthRegeneration",
                displayName: modEntry.Helper.Translation.Get("spellpart.regeneration.name"),
                iconTexture: Game1.buffsIcons, //modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/BuffsIcons.png"),
                duration: 30_000 // 30 seconds
            );

            manaRegenerationBuff = new Buff(
                id: "HeyImAmethyst.ArsVenifici_ManaRegeneration",
                displayName: modEntry.Helper.Translation.Get("buff.mana_regeneration.name"),
                iconTexture: Game1.buffsIcons, //modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/BuffsIcons.png"),
                duration: 30_000 // 30 seconds
            );

            shieldBuff = new Buff(
                id: "HeyImAmethyst.ArsVenifici_Shield",
                displayName: modEntry.Helper.Translation.Get("spellpart.shield.name"),
                iconTexture: Game1.buffsIcons, //modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/BuffsIcons.png"),
                iconSheetIndex: 10, //34
                duration: 45_000 // 30 seconds
            );
        }
    }
}
