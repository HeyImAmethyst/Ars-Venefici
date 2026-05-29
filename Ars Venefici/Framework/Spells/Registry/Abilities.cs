using ArsVenefici.Framework.API.ability;
using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using SpaceShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Registry
{
    public class Abilities
    {
        public static ObjectRegister<Ability> ABILITIES = ObjectRegister<Ability>.Create(ModEntry.ArsVenificiModId);

        public static ObjectHolder<Ability> FIRE_RESISTANCE = ABILITIES.Register(new ObjectHolder<Ability>(new Ability("fire_resistance", Affinities.FIRE.Get(), 0.01, 1.0)));
        public static ObjectHolder<Ability> RESISTANCE = ABILITIES.Register(new ObjectHolder<Ability>(new Ability("resistance", Affinities.EARTH.Get(), 0.01, 1.0)));
        public static ObjectHolder<Ability> SMITE = ABILITIES.Register(new ObjectHolder<Ability>(new Ability("smite", Affinities.LIFE.Get(), 0.01, 1.0)));
        public static ObjectHolder<Ability> MAGIC_DAMAGE = ABILITIES.Register(new ObjectHolder<Ability>(new Ability("magic_damage", Affinities.ARCANE.Get(), 0.5, 1.0)));
        public static ObjectHolder<Ability> HEALTH_REGENERATION = ABILITIES.Register(new ObjectHolder<Ability>(new Ability("health_regeneration", Affinities.LIFE.Get(), 0.6, 1.0)));
        public static ObjectHolder<Ability> STAMINA_REGENERATION = ABILITIES.Register(new ObjectHolder<Ability>(new Ability("stamina_damage", Affinities.LIFE.Get(), 0.6, 1.0)));
    
    }
}
