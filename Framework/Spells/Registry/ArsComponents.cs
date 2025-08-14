using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Registry
{
    public class ArsComponents
    {
        public static ObjectRegister<AbstractComponent> COMPONENTS = ObjectRegister<AbstractComponent>.Create(ModEntry.ArsVenificiModId);

        public static ObjectHolder<AbstractComponent> DIG = RegisterComponent(new ObjectHolder<AbstractComponent>(new Dig(ModEntry.INSTANCE)));
        public static ObjectHolder<AbstractComponent> PLOW = RegisterComponent(new ObjectHolder<AbstractComponent>(new Plow(ModEntry.INSTANCE)));
        public static ObjectHolder<AbstractComponent> GROW = RegisterComponent(new ObjectHolder<AbstractComponent>(new Grow()));
        public static ObjectHolder<AbstractComponent> HARVEST = RegisterComponent(new ObjectHolder<AbstractComponent>(new Harvest()));
        public static ObjectHolder<AbstractComponent> CREATE_WATER = RegisterComponent(new ObjectHolder<AbstractComponent>(new CreateWater()));
        public static ObjectHolder<AbstractComponent> EXPLOSION = RegisterComponent(new ObjectHolder<AbstractComponent>(new Explosion()));
        public static ObjectHolder<AbstractComponent> BLINK = RegisterComponent(new ObjectHolder<AbstractComponent>(new Blink()));
        public static ObjectHolder<AbstractComponent> LIGHT = RegisterComponent(new ObjectHolder<AbstractComponent>(new Light(ModEntry.INSTANCE.Helper.Multiplayer.GetNewID)));
        
        public static ObjectHolder<AbstractComponent> HEAL = RegisterComponent(new ObjectHolder<AbstractComponent>(new Heal(ModEntry.INSTANCE)));
        public static ObjectHolder<AbstractComponent> LIFEDRAIN = RegisterComponent(new ObjectHolder<AbstractComponent>(new LifeDrain()));
        public static ObjectHolder<AbstractComponent> LIFETAP = RegisterComponent(new ObjectHolder<AbstractComponent>(new LifeTap()));
        
        public static ObjectHolder<AbstractComponent> PHYSICALDAMAGE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Damage("physical_damage", 25, ComponentDamageType.Physical, () => 8.0 * (Game1.player.CombatLevel + 1))));
        public static ObjectHolder<AbstractComponent> MAGICDAMAGE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Damage("magic_damage", 25, ComponentDamageType.Magic, () => 8.0 * (Game1.player.CombatLevel + 1))));
        public static ObjectHolder<AbstractComponent> FROSTDAMAGE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Damage("frost_damage", 25, ComponentDamageType.Frost, () => 8.0 * (Game1.player.CombatLevel + 1))));
        public static ObjectHolder<AbstractComponent> LIGHTNINGDAMAGE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Damage("lightning_damage", 55, ComponentDamageType.Lightning, () => 20.0 * (Game1.player.CombatLevel + 1))));
        public static ObjectHolder<AbstractComponent> FIREDAMAGE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Damage("fire_damage", 25, ComponentDamageType.Fire, () => 10.0 * (Game1.player.CombatLevel + 1))));
        
        public static ObjectHolder<AbstractComponent> HASTE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Effect("haste", 30, ModEntry.INSTANCE.buffs.hasteBuff)));
        public static ObjectHolder<AbstractComponent> REGENERATION = RegisterComponent(new ObjectHolder<AbstractComponent>(new Effect("regeneration", 30, ModEntry.INSTANCE.buffs.regenerationBuff)));
        public static ObjectHolder<AbstractComponent> MANA_REGENERATION = RegisterComponent(new ObjectHolder<AbstractComponent>(new Effect("mana_regeneration", 30, ModEntry.INSTANCE.buffs.manaRegenerationBuff)));
        public static ObjectHolder<AbstractComponent> DISPEL = RegisterComponent(new ObjectHolder<AbstractComponent>(new Dispel()));
        
        public static ObjectHolder<AbstractComponent> FORGE = RegisterComponent(new ObjectHolder<AbstractComponent>(new Forge(ModEntry.INSTANCE)));
        public static ObjectHolder<AbstractComponent> SHIELD = RegisterComponent(new ObjectHolder<AbstractComponent>(new Shield()));
        public static ObjectHolder<AbstractComponent> SUMMON = RegisterComponent(new ObjectHolder<AbstractComponent>(new Summon()));

        public static ObjectHolder<AbstractComponent> RegisterComponent(ObjectHolder<AbstractComponent> obj)
        {
            ObjectHolder<AbstractComponent> toReturn = COMPONENTS.Register(obj);
            ModEntry.INSTANCE.Monitor.Log("Registered Component " + obj.Get().GetId(), StardewModdingAPI.LogLevel.Info);
            return toReturn;
        }
    }
}
