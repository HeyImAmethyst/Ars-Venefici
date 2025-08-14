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
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class Cone : AbstractShape
    {
        public Cone() : base(new SpellPartStats(SpellPartStatType.RANGE))
        {

        }

        public override string GetId()
        {
            return "cone";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            int range = (int)helper.GetModifiedStat(1, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index);

            int coneRange = range;

            if (range == 1) coneRange = 4;
            if (range == 2) coneRange = 6;
            if (range == 3) coneRange = 8;
            if (range == 4) coneRange = 10;

            ModEntry.INSTANCE.Monitor.Log("coneRange:" + coneRange, StardewModdingAPI.LogLevel.Info);

            List <HitResult> hitResults = helper.TraceCone(modEntry, (Character)caster.entity, level, coneRange);

            foreach (HitResult hitResult in hitResults)
            {
                helper.Invoke(modEntry, spell, caster, level, hitResult, ticksUsed, index, awardXp);
            }

            return new SpellCastResult(SpellCastResultType.SUCCESS);
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
