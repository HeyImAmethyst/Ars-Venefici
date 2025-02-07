﻿using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Shape
{
    public class Beam : AbstractShape
    {
        public Beam() : base()
        {

        }

        public override string GetId()
        {
            return "beam";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var helper = SpellHelper.Instance();
            //return helper.Invoke(modEntry, spell, caster, level, helper.Trace(modEntry, (Character)caster.entity, level, helper.GetModifiedStat(1f, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index), true, true), ticksUsed, index, awardXp);
            return helper.Invoke(modEntry, spell, caster, level, helper.Trace(modEntry, (Character)caster.entity, level, 4, true, false), ticksUsed, index, awardXp);
        }

        public override bool IsContinuous()
        {
            return true;
        }

        public override bool NeedsPrecedingShape()
        {
            return false;
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
