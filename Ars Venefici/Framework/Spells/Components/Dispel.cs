using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Dispel : AbstractComponent
    {
        public override string GetId()
        {
            return "dispel";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            var api = modEntry.arsVeneficiAPILoader.GetAPI();
            var helper = api.GetSpellHelper();

            int radius = (int)helper.GetModifiedStat(5, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, target, index) * 4;

            if (target.GetCharacter() is Farmer farmer)
            {
                //List<Buff> effects = new List<Buff>();

                //foreach (Buff buff in farmer.buffs.AppliedBuffs.Values)
                //{
                //    effects.Add(buff);
                //}

                //foreach (Buff effect in effects)
                //{
                //    farmer.buffs.Remove(effect.id);
                //}

                farmer.buffs.Clear();

                return farmer.buffs.AppliedBuffs.Count() == 0 ? new SpellCastResult(SpellCastResultType.EFFECT_FAILED) : new SpellCastResult(SpellCastResultType.SUCCESS);
            }

            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override float ManaCost()
        {
            return 35;
        }
    }
}
