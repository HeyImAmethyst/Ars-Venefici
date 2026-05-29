using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class Self : AbstractShape
    {
        public override HashSet<Affinity> GetAffinities()
        {
            return new HashSet<Affinity>();
        }

        public override Dictionary<Affinity, float> GetAffinityShifts()
        {
            return new Dictionary<Affinity, float>();
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, HitResult target, int ticksUsed, int index, bool awardXp)
        {
            //Game1.addHUDMessage(new HUDMessage("Invoking Spell Part " + GetId(), 3));

            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            return SpellHelper.Instance().Invoke(modEntry,spell, caster, gameLocation, new CharacterHitResult(caster.entity as Character), ticksUsed, index, awardXp);
        }

        public override bool NeedsPrecedingShape()
        {
            return false;
        }

        public override bool NeedsToComeFirst()
        {
            return true;
        }

        public override string GetId()
        {
            return "self";
        }

        public override float ManaCost()
        {
            return 0.5f;
        }
    }
}
