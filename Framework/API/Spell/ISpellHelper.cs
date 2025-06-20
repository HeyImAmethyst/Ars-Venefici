﻿using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using StardewValley;
using Microsoft.Xna.Framework;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using static ArsVenefici.ModConfig;

namespace ArsVenefici.Framework.API.Spell
{
    public interface ISpellHelper
    {
        public void SetSpell(SpellBook spellBook, ISpell spell);

        Character GetPointedCharacter(Character entity, Vector2 from, Vector2 to, double range);

        HitResult Trace(ModEntry modEntry, Character entity, GameLocation level, double range, bool entities, bool mouseCursor);

        List<HitResult> TraceCone(ModEntry modEntry, Character entity, GameLocation level, int range);

        /// <summary>
        /// Get the stat value modified by the modifiers.
        /// </summary>
        /// <param name="baseValue">The base value for the stat.</param>
        /// <param name="stat">The stat that is modified.</param>
        /// <param name="spell">The spell that the part belongs to.</param>
        /// <param name="caster">The entity casting the spell.</param>
        /// <param name="target">The target of the spell cast.</param>
        /// <param name="componentIndex">The 1 based index of the currently invoked part.</param>
        /// <returns>The modified value of the stat.</returns>
        float GetModifiedStat(float baseValue, ISpellPartStat stat, List<ISpellModifier> modifiers, ISpell spell, IEntity caster, HitResult target, int componentIndex);

        void CastSpell(Farmer farmer, ModKeyBinds modKeyBinds);

        /// <summary>
        /// Casts the spell.
        /// </summary>
        /// <param name="spell">The spell to cast.</param>
        /// <param name="caster">The entity casting the spell.</param>
        /// <param name="level">The level the spell is cast in.</param>
        /// <param name="target">The target of the spell cast.</param>
        /// <param name="castingTicks">How long the spell has already been cast.</param>
        /// <param name="index">The 1 based index of the currently invoked part.</param>
        /// <param name="awardXp"The magic xp awarded for casting this spell.></param>
        /// <returns>A SpellCastResult that represents the spell casting outcome.</returns>
        SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, HitResult target, int castingTicks, int index, bool awardXp);

        public void NextShapeGroup(ISpell spell);

        public void PrevShapeGroup(ISpell spell);
    }
}
