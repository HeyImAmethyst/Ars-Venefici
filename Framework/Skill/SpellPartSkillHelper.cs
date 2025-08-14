using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using SpaceCore;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Mods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace ArsVenefici.Framework.Skill
{
    public class SpellPartSkillHelper : ISpellPartSkillHelper
    {

        private static SpellPartSkillHelper INSTANCE = new SpellPartSkillHelper();

        // <summary>The prefix added to mod data keys.</summary>
        private const string Prefix = "HeyImAmethyst.ArsVenifici";

        /// <summary>The data key for the player's known spell parts.</summary>
        private const string KnownSpellPartsKey = Prefix + "/KnownSpellPartSkills";

        /// <summary>The player's learned knowlege.</summary>
        public IDictionary<string, SpellPartSkill> KnownSpellPartSkills { get; set; }

        private SpellPartSkillHelper()
        {
            KnownSpellPartSkills = new Dictionary<string, SpellPartSkill>();
        }

        public static SpellPartSkillHelper Instance()
        {
            return INSTANCE;
        }

        public bool Knows(ModEntry modEntry, Farmer player, string skillID)
        {
            return KnownSpellPartSkills.Keys.Contains(skillID);
        }

        public bool Knows(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            return KnownSpellPartSkills.Values.Contains(skill);
        }

        public bool CanLearn(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            bool canLearn = true;

            if (!skill.Cost().Any())
            {
                canLearn = true;
            }
            else
            {
                foreach (Item item in skill.Cost().Keys)
                {
                    if (player.Items.CountId(item.QualifiedItemId) < skill.Cost()[item])
                    {
                        canLearn = false;
                    }
                }
            }

            return canLearn && ((skill.Parents().Any() && skill.Parents().All(value => KnownSpellPartSkills.Values.Contains(value))) || !skill.Parents().Any());
        }

        public void Learn(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            if (!KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                KnownSpellPartSkills.Add(skill.GetId(), skill);

            SyncToPlayer(player);
            UpdateIfNeeded(modEntry, player);
        }

        public void Learn(ModEntry modEntry, Farmer player, string spellPartId)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (skill.GetId().Equals(spellPartId) && !KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                        KnownSpellPartSkills.Add(spellPartId, skill);
                }

                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (skill.GetId().Equals(spellPartId) && !KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                        KnownSpellPartSkills.Add(spellPartId, skill);
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        public void LearnAll(ModEntry modEntry, Farmer player)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (!KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                        KnownSpellPartSkills.Add(spellPartSkill.GetId(), spellPartSkill);

                    //Learn(modEntry, player, spellPartSkill);
                }

                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (!KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                        KnownSpellPartSkills.Add(spellPartSkill.GetId(), spellPartSkill);

                    //Learn(modEntry, player, spellPartSkill);
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        public void Forget(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            if(KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                KnownSpellPartSkills.Remove(skill.GetId());

            SyncToPlayer(player);
            UpdateIfNeeded(modEntry, player);
        }

        public void Forget(ModEntry modEntry, Farmer player, string spellPartId)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (skill.GetId().Equals(spellPartId) && KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                        KnownSpellPartSkills.Remove(spellPartId);
                }

                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (skill.GetId().Equals(spellPartId) && KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                        KnownSpellPartSkills.Remove(spellPartId);
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        public void ForgetAll(ModEntry modEntry, Farmer player)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                        KnownSpellPartSkills.Remove(spellPartSkill.GetId());

                    //Forget(modEntry, player, spellPartSkill);
                }

                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                        KnownSpellPartSkills.Remove(spellPartSkill.GetId());

                    //Forget(modEntry, player, spellPartSkill);
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        public IDictionary<string, SpellPartSkill> GetKnownSpellPartSkills(ModEntry modEntry, Farmer player)
        {
            return this.KnownSpellPartSkills;
        }

        public void UpdateIfNeeded(ModEntry modEntry, Farmer player)
        {
            this.KnownSpellPartSkills = player.modData.GetCustom(KnownSpellPartsKey, parse => this.ParseKnownSpellPartSkills(modEntry, parse), suppressError: false) ?? new Dictionary<string, SpellPartSkill>();
        }

        private void SyncToPlayer(Farmer player)
        {
            player.modData.SetCustom(KnownSpellPartsKey, KnownSpellPartSkills.Values, serialize: this.SerializeSpellPartSkills);
        }

        /// <summary>Parse serialized known spell part skills.</summary>
        /// <param name="raw">The raw serialized string.</param>
        private IDictionary<string, SpellPartSkill> ParseKnownSpellPartSkills(ModEntry modEntry, string raw)
        {
            Dictionary<string, SpellPartSkill> spells = new();

            if (raw != null)
            {
                foreach (SpellPartSkill spell in this.ParseSpellPartSkill(modEntry, raw))
                {
                    //spells[spell.GetId()] = spell;

                    spells.Add(spell.GetId(), spell);
                }
            }

            return spells;
        }

        /// <summary>Parse a serialized spell part skill list.</summary>
        /// <param name="raw">The raw serialized string.</param>
        private List<SpellPartSkill> ParseSpellPartSkill(ModEntry modEntry, string raw)
        {
            List<SpellPartSkill> spellPartSkill = new();

            if (string.IsNullOrWhiteSpace(raw))
                return spellPartSkill;

            foreach (string rawSpellPart in raw.Split(','))
            {
                if (string.IsNullOrWhiteSpace(rawSpellPart))
                    spellPartSkill.Add(null);
                else
                {
                    if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
                    {
                        modEntry.spellPartSkillManager.GetSpellPartSkills().TryGetValue(rawSpellPart, out SpellPartSkill spellPart);
                        modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().TryGetValue(rawSpellPart, out SpellPartSkill contentPackSpellPart);

                        if (spellPart != null)
                            spellPartSkill.Add(spellPart);

                        if (contentPackSpellPart != null)
                            spellPartSkill.Add(contentPackSpellPart);
                    }
                }
            }

            return spellPartSkill;
        }

        /// <summary>Serialize spell part skill for storage.</summary>
        /// <param name="spells">The spell part skills to serialize.</param>
        private string SerializeSpellPartSkills(IEnumerable<SpellPartSkill> spellPartSkills)
        {
            return string.Join(",", spellPartSkills.Select(spellPartSkill => spellPartSkill != null ? $"{spellPartSkill.GetId()}" : "")).TrimEnd(',');
        }
    }
}
