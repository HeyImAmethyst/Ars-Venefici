using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class EtherealTouch : AbstractShape
    {
        public EtherealTouch() : base()
        {
           
        }

        public override string GetId()
        {
            return "ethereal_touch";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();
            //return helper.Invoke(modEntry, spell, caster, level, helper.Trace(modEntry, (Character)caster.entity, level, helper.GetModifiedStat(1f, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index), true, true), ticksUsed, index, awardXp);
            return helper.Invoke(modEntry, spell, caster, level, helper.Trace(modEntry, (Character)caster.entity, level, 1, true, true), ticksUsed, index, awardXp);
        }

        public override bool NeedsToComeFirst()
        {
            return true;
        }

        public override float ManaCost()
        {
            return 3;
        }
    }
}
