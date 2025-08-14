using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Spells;
using StardewValley.Buffs;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.GameData.Objects;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Internal;
using StardewValley.Projectiles;
using static StardewValley.Menus.CharacterCustomization;
using StardewValley.Objects;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;

namespace ArsVenefici.Framework.Skill
{
    public class SpellPartSkillManager
    {
        private ModEntry modEntry;

        private Dictionary<string, SpellPartSkill> spellPartSkills = new Dictionary<string, SpellPartSkill>();
        private Dictionary<string, SpellPartSkill> contentPackSpellPartSkills = new Dictionary<string, SpellPartSkill>();

        public bool dictionariesPoplulated = false;

        public SpellPartSkillManager(ModEntry modEntry)
        {
            this.modEntry = modEntry;
        }

        public void AddSpellPartSkills(List<ObjectHolder<SpellPartSkill>> list)
        {
            foreach (ObjectHolder<SpellPartSkill> holder in list)
            {
                AddSpellPartSkill(holder.Get());
            }
        }

        public void ContentPackAddSpellPartSkills(List<ObjectHolder<SpellPartSkill>> list)
        {
            foreach (ObjectHolder<SpellPartSkill> holder in list)
            {
                ContentPackAddSpellPartSkill(holder.Get());
            }
        }

        public void AddSpellPartSkill(SpellPartSkill skill)
        {
            if (!spellPartSkills.ContainsKey(skill.GetId()))
                spellPartSkills.Add(skill.GetId(), skill);
        }

        public void ContentPackAddSpellPartSkill(SpellPartSkill skill)
        {
            if (!contentPackSpellPartSkills.ContainsKey(skill.GetId()))
                contentPackSpellPartSkills.Add(skill.GetId(), skill);
        }

        public Dictionary<string, SpellPartSkill> GetSpellPartSkills()
        {
            return spellPartSkills;
        }

        public Dictionary<string, SpellPartSkill> GetContentPackSpellPartSkills()
        {
            return contentPackSpellPartSkills;
        }
    }
}
