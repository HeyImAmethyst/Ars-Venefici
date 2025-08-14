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
        
        public static ObjectHolder<AbstractModifier> DAMAGE = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("damage", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.DAMAGE), DefaultSpellPartStatModifier.Add(5f))));
        public static ObjectHolder<AbstractModifier> RANGE = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("range", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.RANGE), DefaultSpellPartStatModifier.Add(1f))));
        public static ObjectHolder<AbstractModifier> BOUNCE = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("bounce", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.BOUNCE), DefaultSpellPartStatModifier.Add(2f))));
        public static ObjectHolder<AbstractModifier> PIERCING = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("piercing", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.PIERCING), DefaultSpellPartStatModifier.COUNTING)));
        public static ObjectHolder<AbstractModifier> VELOCITY = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("velocity", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.SPEED), DefaultSpellPartStatModifier.AddMultipliedBase(0.5f))));
        public static ObjectHolder<AbstractModifier> HEALING = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("healing", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.HEALING), DefaultSpellPartStatModifier.Multiply(2f))));
        public static ObjectHolder<AbstractModifier> DURATION = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("duration", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.DURATION), DefaultSpellPartStatModifier.Multiply(2f))));
        public static ObjectHolder<AbstractModifier> MINING_POWER = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("mining_power", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.MINING_TIER), DefaultSpellPartStatModifier.Add(1f))));
        public static ObjectHolder<AbstractModifier> EFFECT_POWER = RegisterModifier(new ObjectHolder<AbstractModifier>(new GenericSpellModifier("effect_power", ModEntry.INSTANCE.Helper, 1.25f).AddStatModifier(new SpellPartStats(SpellPartStatType.POWER), DefaultSpellPartStatModifier.Multiply(2f))));

        public static ObjectHolder<AbstractModifier> RegisterModifier(ObjectHolder<AbstractModifier> obj)
        {
            ObjectHolder<AbstractModifier> toReturn = MODIFIERS.Register(obj);
            ModEntry.INSTANCE.Monitor.Log("Registered Modifier " + obj.Get().GetId(), StardewModdingAPI.LogLevel.Info);
            return toReturn;
        }
    }
}
