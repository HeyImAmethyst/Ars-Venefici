using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Registry
{
    public class ArsModifiers
    {
        public static ObjectRegister<AbstractModifier> MODIFIERS = ObjectRegister<AbstractModifier>.Create(ModEntry.ArsVenificiModId);

        public static ObjectHolder<AbstractModifier> DAMAGE = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("damage", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.DAMAGE), DefaultSpellPartStatModifier.Add(5f))));
        public static ObjectHolder<AbstractModifier> RANGE = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("range", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.RANGE), DefaultSpellPartStatModifier.Add(1f))));
        public static ObjectHolder<AbstractModifier> BOUNCE = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("bounce", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.BOUNCE), DefaultSpellPartStatModifier.Add(2f))));
        public static ObjectHolder<AbstractModifier> PIERCING = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("piercing", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.PIERCING), DefaultSpellPartStatModifier.COUNTING)));
        public static ObjectHolder<AbstractModifier> VELOCITY = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("velocity", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.SPEED), DefaultSpellPartStatModifier.AddMultipliedBase(0.5f))));
        public static ObjectHolder<AbstractModifier> HEALING = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("healing", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.HEALING), DefaultSpellPartStatModifier.Multiply(2f))));
        public static ObjectHolder<AbstractModifier> DURATION = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("duration", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.DURATION), DefaultSpellPartStatModifier.Multiply(2f))));
        public static ObjectHolder<AbstractModifier> MINING_POWER = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("mining_power", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.MINING_TIER), DefaultSpellPartStatModifier.Add(1f))));
        public static ObjectHolder<AbstractModifier> EFFECT_POWER = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("effect_power", 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.POWER), DefaultSpellPartStatModifier.Multiply(2f))));

        private static ObjectHolder<AbstractModifier> RegisterModifier(ObjectHolder<AbstractModifier> obj)
        {
            ObjectHolder<AbstractModifier> toReturn = MODIFIERS.Register(obj);
            return toReturn;
        }
    }
}
