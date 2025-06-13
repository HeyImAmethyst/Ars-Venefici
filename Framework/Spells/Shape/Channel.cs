using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class Channel : AbstractShape
    {
        public override string GetId()
        {
            return "channel";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();
            return helper.Invoke(modEntry, spell, caster, level, helper.Trace(modEntry, (Character)caster.entity, level, 1, true, false), ticksUsed, index, awardXp);
        }

        public override bool IsContinuous()
        {
            return true;
        }

        public override float ManaCost()
        {
            return 0.5f;
        }
    }
}
