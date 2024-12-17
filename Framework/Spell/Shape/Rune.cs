using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;
using Microsoft.Xna.Framework;
using xTile.Tiles;
using StardewValley.ItemTypeDefinitions;

namespace ArsVenefici.Framework.Spell.Shape
{
    public class Rune : AbstractShape
    {
        public Rune() : base()
        {

        }

        public override string GetId()
        {
            return "rune";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var helper = SpellHelper.Instance();

            Vector2 tilePos = hit.GetLocation();

            if (!level.objects.ContainsKey(tilePos))
            {
                RuneObject runeObject = new RuneObject(modEntry, level,hit, index, spell, tilePos);

                runeObject.Fragility = 1;
                runeObject.Category = -24;
                runeObject.Type = "Crafting";

                level.objects.Add(tilePos, runeObject);

                return new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override bool NeedsPrecedingShape()
        {
            return true;
        }

        public override bool IsEndShape()
        {
            return true;
        }

        public override int ManaCost()
        {
            return 1;
        }
    }
}
