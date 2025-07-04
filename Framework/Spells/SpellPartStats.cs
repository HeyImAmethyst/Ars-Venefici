﻿using ArsVenefici.Framework.API.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells
{
    public class SpellPartStats : ISpellPartStat
    {

        public SpellPartStatType spellPartStatType;
        private string id;

        public SpellPartStats(SpellPartStatType type)
        {
            spellPartStatType = type;
            id = spellPartStatType.ToString().ToLower();
        }

        public string GetId()
        {
            return id;
        }

        public void SetSpellPartStatType(SpellPartStatType type)
        {
            spellPartStatType = type;
        }
    }

    public enum SpellPartStatType
    {
        BOUNCE,
        DAMAGE,
        DURATION,
        HEALING,
        PIERCING,
        POWER,
        MINING_TIER,
        RANGE,
        SPEED
    }
}
