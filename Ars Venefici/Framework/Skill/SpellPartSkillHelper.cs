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

        /// <summary>
        /// Checks if the farmer knows a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="skillID">The spell part skill id</param>
        /// <returns>True or false</returns>
        public bool Knows(ModEntry modEntry, Farmer player, string skillID)
        {
            return KnownSpellPartSkills.Keys.Contains(skillID);
        }

        /// <summary>
        /// Checks if the farmer knows a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="skill">The spell part skill object</param>
        /// <returns>True or false</returns>
        public bool Knows(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            return KnownSpellPartSkills.Values.Contains(skill);
        }

        /// <summary>
        /// Checks if the farmer can learn a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="skill">The spell part skill object</param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a player learn a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="skill">The spell part skill object</param>
        public void Learn(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            if(skill != null)
            {
                if (!KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                    KnownSpellPartSkills.Add(skill.GetId(), skill);
            }

            SyncToPlayer(player);
            UpdateIfNeeded(modEntry, player);
        }

        /// <summary>
        /// Makes a player learn a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="spellPartId">The spell part skill id</param>
        public void Learn(ModEntry modEntry, Farmer player, string spellPartId)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (skill != null)
                    {
                        if (skill.GetId().Equals(spellPartId) && !KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                            KnownSpellPartSkills.Add(spellPartId, skill);
                    }
                }

                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (skill != null)
                    {
                        if (skill.GetId().Equals(spellPartId) && !KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                            KnownSpellPartSkills.Add(spellPartId, skill);
                    }
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        /// <summary>
        /// Learns all the spell part skills
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        public void LearnAll(ModEntry modEntry, Farmer player)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (spellPartSkill != null)
                    {
                        if (!KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                            KnownSpellPartSkills.Add(spellPartSkill.GetId(), spellPartSkill);
                    }

                    //Learn(modEntry, player, spellPartSkill);
                }

                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (spellPartSkill != null)
                    {
                        if (!KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                            KnownSpellPartSkills.Add(spellPartSkill.GetId(), spellPartSkill);
                    }

                    //Learn(modEntry, player, spellPartSkill);
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        /// <summary>
        /// Makes a player forget a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="skill">The spell part skill object</param>
        public void Forget(ModEntry modEntry, Farmer player, SpellPartSkill skill)
        {
            if (skill != null)
            {
                if (KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                    KnownSpellPartSkills.Remove(skill.GetId());
            }

            SyncToPlayer(player);
            UpdateIfNeeded(modEntry, player);
        }

        /// <summary>
        /// Makes a player forget a spell part skill
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        /// <param name="spellPartId">The spell part skill id</param>
        public void Forget(ModEntry modEntry, Farmer player, string spellPartId)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if(skill != null)
                    {
                        if (skill.GetId().Equals(spellPartId) && KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                            KnownSpellPartSkills.Remove(spellPartId);
                    }
                    
                }

                foreach (SpellPartSkill skill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if (skill != null)
                    {
                        if (skill.GetId().Equals(spellPartId) && KnownSpellPartSkills.Keys.Contains(skill.GetId()))
                            KnownSpellPartSkills.Remove(spellPartId);
                    }
                }

                SyncToPlayer(player);
                UpdateIfNeeded(modEntry, player);
            }
        }

        /// <summary>
        /// Forgets all the spell part skills
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        public void ForgetAll(ModEntry modEntry, Farmer player)
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetSpellPartSkills().Values)
                {
                    if (spellPartSkill != null)
                    {
                        if (KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                            KnownSpellPartSkills.Remove(spellPartSkill.GetId());
                    }
                    
                    //Forget(modEntry, player, spellPartSkill);
                }

                foreach (SpellPartSkill spellPartSkill in modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().Values)
                {
                    if(spellPartSkill != null)
                    {
                        if (KnownSpellPartSkills.Keys.Contains(spellPartSkill.GetId()))
                            KnownSpellPartSkills.Remove(spellPartSkill.GetId());
                    }

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

        /// <summary>
        /// Updates the KnownSpellPartSkills to what is in the player mod data
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        public void UpdateIfNeeded(ModEntry modEntry, Farmer player)
        {
            this.KnownSpellPartSkills = player.modData.GetCustom(KnownSpellPartsKey, parse => this.ParseKnownSpellPartSkills(modEntry, parse), suppressError: false) ?? new Dictionary<string, SpellPartSkill>();
        }

        /// <summary>
        /// Updates the player's mod data to what is in KnownSpellPartSkills
        /// </summary>
        /// <param name="player"></param>
        private void SyncToPlayer(Farmer player)
        {
            player.modData.SetCustom(KnownSpellPartsKey, KnownSpellPartSkills.Values, serialize: this.SerializeSpellPartSkills);
        }

        /// <summary>Parse serialized known spell part skills.</summary>
        /// <param name="raw">The raw serialized string.</param>
        private IDictionary<string, SpellPartSkill> ParseKnownSpellPartSkills(ModEntry modEntry, string raw)
        {
            Dictionary<string, SpellPartSkill> spellParts = new();

            if (raw != null)
            {
                foreach (SpellPartSkill spellPartSkill in this.ParseSpellPartSkill(modEntry, raw))
                {
                    //spells[spell.GetId()] = spell;

                    if(spellPartSkill != null)
                    {
                        spellParts.Add(spellPartSkill.GetId(), spellPartSkill);
                    }
                }
            }

            return spellParts;
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
