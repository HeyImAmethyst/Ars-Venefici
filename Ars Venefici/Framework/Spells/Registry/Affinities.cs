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
    public class Affinities
    {
        public static ObjectRegister<Affinity> AFFINITIES = ObjectRegister<Affinity>.Create(ModEntry.ArsVenificiModId);

        public static ObjectHolder<Affinity> NONE = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.NONE).SetColor(Color.White).SetDirectOpposite(Affinity.NONE).SetCastSound(ModSounds.CAST_NONE).Build()));
        public static ObjectHolder<Affinity> EARTH = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.EARTH).SetColor(Color.Peru).SetDirectOpposite(Affinity.AIR).SetCastSound(ModSounds.CAST_EARTH).AddMajorOpposites(Affinity.WATER, Affinity.ARCANE, Affinity.LIFE, Affinity.LIGHTNING).AddMinorOpposites(Affinity.NATURE, Affinity.FIRE).Build()));
        public static ObjectHolder<Affinity> WATER = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.WATER).SetColor(new Color(159, 185, 255, 230)).SetDirectOpposite(Affinity.FIRE).SetCastSound(ModSounds.CAST_WATER).AddMajorOpposites(Affinity.LIGHTNING, Affinity.EARTH, Affinity.ARCANE, Affinity.DARKNESS).AddMinorOpposites(Affinity.AIR, Affinity.ICE).Build()));
        public static ObjectHolder<Affinity> AIR = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.AIR).SetColor(Color.White).SetDirectOpposite(Affinity.EARTH).SetCastSound(ModSounds.CAST_AIR).AddMajorOpposites(Affinity.NATURE, Affinity.FIRE, Affinity.ICE, Affinity.DARKNESS).AddMinorOpposites(Affinity.WATER, Affinity.ARCANE).Build()));
        public static ObjectHolder<Affinity> FIRE = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.FIRE).SetColor(new Color(255, 133, 0, 230)).SetDirectOpposite(Affinity.WATER).SetCastSound(ModSounds.CAST_FIRE).AddMajorOpposites(Affinity.AIR, Affinity.ICE, Affinity.NATURE, Affinity.LIFE).AddMinorOpposites(Affinity.EARTH, Affinity.LIGHTNING).Build()));
        public static ObjectHolder<Affinity> NATURE = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.NATURE).SetColor(new Color(137, 252, 80, 230)).SetDirectOpposite(Affinity.ARCANE).SetCastSound(ModSounds.CAST_NATURE).AddMajorOpposites(Affinity.AIR, Affinity.DARKNESS, Affinity.LIGHTNING, Affinity.FIRE).AddMinorOpposites(Affinity.LIFE, Affinity.EARTH).Build()));
        public static ObjectHolder<Affinity> ICE = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.ICE).SetColor(new Color(201, 238, 255, 230)).SetDirectOpposite(Affinity.LIGHTNING).SetCastSound(ModSounds.CAST_ICE).AddMajorOpposites(Affinity.LIFE, Affinity.FIRE, Affinity.AIR, Affinity.ARCANE).AddMinorOpposites(Affinity.WATER, Affinity.DARKNESS).Build()));
        public static ObjectHolder<Affinity> LIGHTNING = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.LIGHTNING).SetColor(Color.PaleGoldenrod).SetDirectOpposite(Affinity.ICE).SetCastSound(ModSounds.CAST_LIGHTNING).AddMajorOpposites(Affinity.WATER, Affinity.DARKNESS, Affinity.NATURE, Affinity.EARTH).AddMinorOpposites(Affinity.LIFE, Affinity.FIRE).Build()));
        public static ObjectHolder<Affinity> LIFE = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.LIFE).SetColor(new Color(255, 45, 104, 230)).SetDirectOpposite(Affinity.DARKNESS).SetCastSound(ModSounds.CAST_LIFE).AddMajorOpposites(Affinity.ARCANE, Affinity.ICE, Affinity.FIRE, Affinity.EARTH).AddMinorOpposites(Affinity.NATURE, Affinity.LIGHTNING).Build()));
        public static ObjectHolder<Affinity> ARCANE = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.ARCANE).SetColor(new Color(0, 248, 255, 230)).SetDirectOpposite(Affinity.NATURE).SetCastSound(ModSounds.CAST_ARCANE).AddMajorOpposites(Affinity.LIFE, Affinity.EARTH, Affinity.WATER, Affinity.ICE).AddMinorOpposites(Affinity.AIR, Affinity.DARKNESS).Build()));
        public static ObjectHolder<Affinity> DARKNESS = AFFINITIES.Register(new ObjectHolder<Affinity>(Affinity.CreateBuilder().SetId(Affinity.DARKNESS).SetColor(new Color(103, 51, 125, 230)).SetDirectOpposite(Affinity.LIFE).SetCastSound(ModSounds.CAST_DARKNESS).AddMajorOpposites(Affinity.NATURE, Affinity.LIGHTNING, Affinity.WATER, Affinity.AIR).AddMinorOpposites(Affinity.ARCANE, Affinity.ICE).Build()));
    }
}
