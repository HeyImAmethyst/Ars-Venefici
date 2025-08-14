using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Spells.Registry;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace ArsVenefici.Framework.Commands
{
    internal class SpellPartsCommand : Command
    {

        public SpellPartsCommand(ModEntry modEntry) : base(modEntry)
        {

        }

        public void LearnSpellPart(string command, string[] args)
        {
            if(args.Length > 0 && args[0] != null)
            {
                string value = args[0];

                if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
                {
                    if (modEntry.spellPartSkillManager.GetSpellPartSkills().ContainsKey(value) || modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().ContainsKey(value))
                    {
                        var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

                        string spellPartNameText = modEntry.Helper.Translation.Get($"spellpart.{value}.name");

                        helper.Learn(modEntry, Game1.player, value);

                        modEntry.Monitor.Log($"You learned the spell part {spellPartNameText}!", LogLevel.Info);
                    }
                    else
                    {
                        modEntry.Monitor.Log($"The name {value} is not a valid spell part id!", LogLevel.Info);
                    }
                }
            }
        }

        public void LearnAllSpellParts(string command, string[] args)
        {
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

            helper.LearnAll(modEntry, Game1.player);

            modEntry.Monitor.Log($"You learned all the spell parts!", LogLevel.Info);
        }

        public void ForgetSpellPart(string command, string[] args)
        {
            if (args.Length > 0 && args[0] != null)
            {
                string value = args[0];

                if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
                {
                    if (modEntry.spellPartSkillManager.GetSpellPartSkills().ContainsKey(value) || modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().ContainsKey(value))
                    {
                        var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

                        string spellPartNameText = modEntry.Helper.Translation.Get($"spellpart.{value}.name");

                        helper.Forget(modEntry, Game1.player, value);

                        modEntry.Monitor.Log($"You forgot the spell part {spellPartNameText}!", LogLevel.Info);
                    }
                    else
                    {
                        modEntry.Monitor.Log($"The name {value} is not a valid spell part id!", LogLevel.Info);
                    }
                }
            }
        }

        public void ForgetAllSpellParts(string command, string[] args)
        {
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();
            helper.ForgetAll(modEntry, Game1.player);

            modEntry.Monitor.Log($"You forgot all the spell parts!", LogLevel.Info);
        }

        public void KnowsSpellPart(string command, string[] args)
        {
            if (args.Length > 0 && args[0] != null)
            {
                string value = args[0];

                if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
                {
                    if (modEntry.spellPartSkillManager.GetSpellPartSkills().ContainsKey(value) || modEntry.spellPartSkillManager.GetContentPackSpellPartSkills().ContainsKey(value))
                    {
                        var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper();

                        string spellPartNameText = modEntry.Helper.Translation.Get($"spellpart.{value}.name");

                        if (helper.Knows(modEntry, Game1.player, value))
                        {
                            modEntry.Monitor.Log($"You know the spell part {spellPartNameText}!", LogLevel.Info);
                        }
                        else
                        {
                            modEntry.Monitor.Log($"You do not know the spell part {spellPartNameText}!", LogLevel.Info);
                        }
                    }
                    else
                    {
                        modEntry.Monitor.Log($"The name {value} is not a valid spell part id!", LogLevel.Info);
                    }
                }
            }
        }
    }
}
