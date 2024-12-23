using ArsVenefici.Framework.GameSave;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Commands
{
    public class Commands
    {

        ModEntry modEntryInstance;

        ToggleWizardryCommand toggleWizardryCommand;
        SpellPartsCommand spellPartsCommand;

        public Commands(ModEntry modEntry)
        {
            modEntryInstance = modEntry;

            toggleWizardryCommand = new ToggleWizardryCommand(modEntryInstance);
            spellPartsCommand = new SpellPartsCommand(modEntryInstance);
        }

        public void AddCommands()
        {
            AddCommand("player_togglewizardry", "Toggles the player's the ability to cast spells.\n\nUsage: player_togglewizardry <value>\n- value: true or false.", toggleWizardryCommand.ToggleWizardry);

            AddCommand("player_learnspellpart", "Allows the player to learn a spell part.\n\nUsage: player_learnspellpart <value>\n- value: the id of the spell part.", spellPartsCommand.LearnSpellPart);
            AddCommand("player_forgetspellpart", "Allows the player to forget a spell part.\n\nUsage: player_forgetspellpart <value>\n- value: the id of the spell part.", spellPartsCommand.ForgetSpellPart);

            AddCommand("player_learnallspellparts", "Allows the player to learn all spell parts.\n\nUsage: player_learnallspellparts", spellPartsCommand.LearnAllSpellParts);
            AddCommand("player_forgetallspellparts", "Allows the player to forget all spell parts.\n\nUsage: player_forgetallspellparts", spellPartsCommand.ForgetAllSpellParts);

            AddCommand("player_knowsspellpart", "Checks if a player knows a spell part.\n\nUsage: player_knowsspellpart <value>\n- value: the id of the spell part.", spellPartsCommand.KnowsSpellPart);

            AddCommand("save_manapointsperlevel", "Sets the amount of mana players have per Wizardry level.\n\nUsage: save_manapointsperlevel <value>\n- value: the amount of mana points per Wizardry level",
                (string command, string[] args) =>
                {
                    int value;

                    if (args.Length > 0 && args[0] != null && int.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.ManaPointsPerLevel = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });

            AddCommand("save_manaregenrate", "Sets the mana regen rate of players.\n\nUsage: save_manaregenrate <value>\n- value: the rate of mana regen rate of players",
                (string command, string[] args) =>
                {
                    int value;

                    if (args.Length > 0 && args[0] != null && int.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.ManaRegenRate = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });

            AddCommand("save_enableinfinitemana", "Toggles infinate mana.\n\nUsage: save_enableinfinitemana <value>\n- value: true or false",
                (string command, string[] args) =>
                {
                    bool value;

                    if (args.Length > 0 && args[0] != null && bool.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.InfiniteMana = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });

            AddCommand("save_enablegrowsickness", "Toggles the grow sickness debuff.\n\nUsage: save_enablegrowsickness <value>\n- value: true or false",
                (string command, string[] args) =>
                {
                    bool value;

                    if (args.Length > 0 && args[0] != null && bool.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.EnableGrowSickness = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });

            AddCommand("save_growsicknessdurationmillisecondslessthanlevelsix", "Sets the grow sickness debuff duration for Wizardry levels less than level 6.\n\nUsage: save_growsicknessdurationmillisecondslessthanlevelsix <value>\n- value: the duration in milliseconds",
                (string command, string[] args) =>
                {
                    int value;

                    if (args.Length > 0 && args[0] != null && int.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.GrowSicknessDurationMillisecondsLessThanLevelSix = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });

            AddCommand("save_growsicknessdurationmillisecondsgreaterthanorequaltolevelsix", "Sets the grow sickness debuff duration for Wizardry levels greater than or equal to level 6.\n\nUsage: save_growsicknessdurationmillisecondsgreaterthanorequaltolevelsix <value>\n- value: the duration in milliseconds",
                (string command, string[] args) =>
                {
                    int value;

                    if (args.Length > 0 && args[0] != null && int.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelSix = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });

            AddCommand("save_growsicknessdurationmillisecondsgreaterthanorequaltoleveleight", "Sets the grow sickness debuff duration for Wizardry levels greater than or equal to level 8.\n\nUsage: save_growsicknessdurationmillisecondsgreaterthanorequaltoleveleight <value>\n- value: the duration in milliseconds",
            (string command, string[] args) =>
            {
                int value;

                if (args.Length > 0 && args[0] != null && int.TryParse(args[0], out value))
                {
                    if (!Game1.IsMasterGame)
                    {
                        modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                        return;
                    }

                    modEntryInstance.ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelEight = value;

                    modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                    modEntryInstance.Helper.Multiplayer.SendMessage(
                        new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                        ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                }
            });

            AddCommand("save_growsicknessdurationmillisecondsgreaterthanorequaltolevelten", "Sets the grow sickness debuff duration for Wizardry levels greater than or equal to level 10.\n\nUsage: save_growsicknessdurationmillisecondsgreaterthanorequaltolevelten <value>\n- value: the duration in milliseconds",
                (string command, string[] args) =>
                {
                    int value;

                    if (args.Length > 0 && args[0] != null && int.TryParse(args[0], out value))
                    {
                        if (!Game1.IsMasterGame)
                        {
                            modEntryInstance.Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        modEntryInstance.ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelTen = value;

                        modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);

                        modEntryInstance.Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
                    }
                });
        }

        public void AddCommand(string commandName, string commandDescription, Action<string, string[]> callback)
        {
            modEntryInstance.Helper.ConsoleCommands.Add(commandName, commandDescription, callback);
        }
    }
}
