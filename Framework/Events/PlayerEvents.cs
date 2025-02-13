using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.Spell.Buffs;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Events
{
    public class PlayerEvents
    {
        ModEntry modEntryInstance;

        public PlayerEvents(ModEntry modEntry)
        {
            modEntryInstance = modEntry;
        }

        public void OnItemEaten(object sender, EventArgs args)
        {
            if (Game1.player != null)
            {
                if (Game1.player.itemToEat == null)
                {
                    modEntryInstance.Monitor.Log("No item eaten for the item eat event?!?", LogLevel.Error);
                    return;
                }

                if (Game1.player.itemToEat != null)
                {

                    if (Game1.objectData != null)
                    {
                        if (Game1.objectData.TryGetValue(Game1.player.itemToEat.ItemId, out var data))
                        {
                            if (data != null)
                            {
                                if (data.CustomFields != null)
                                {
                                    if (data.CustomFields.TryGetValue($"{ModEntry.ArsVenificiContentPatcherId}/Mana", out string manaValue))
                                    {
                                        if (int.TryParse(manaValue, out int value))
                                            Game1.player.AddMana(value);
                                    }
                                }
                            }
                        }
                    }

                    if (Game1.player.itemToEat.QualifiedItemId.Equals("(O)" + ModEntry.ArsVenificiContentPatcherId + "_Mana_Martini") || Game1.player.itemToEat.QualifiedItemId.Equals("(O)" + ModEntry.ArsVenificiContentPatcherId + "_Mana_Juice"))
                    {
                        Buff manaRegenBuff = modEntryInstance.buffs.manaRegenerationBuff;

                        Game1.player.applyBuff(
                            new Buff
                            (
                                id: manaRegenBuff.id,
                                displayName: manaRegenBuff.displayName,
                                iconTexture: modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/icon/buff/mana_regeneration.png"),
                                iconSheetIndex: manaRegenBuff.iconSheetIndex, //34
                                duration: manaRegenBuff.millisecondsDuration * 2,
                                effects: manaRegenBuff.effects
                            )
                        );
                    }
                }
            }
        }

        public void OnWarped(object sender, WarpedEventArgs e)
        {
            if (!e.IsLocalPlayer)
                return;

            string eventSong = "WizardSong";
            string eventViewPort;
            string eventActors;
            string eventAttributes = "skippable/ignoreCollisions farmer";
            string eventMove;

            if (modEntryInstance.isSVEInstalled && e.NewLocation.Name == "Custom_WizardBasement")
            {
                eventViewPort = "15 7";
                eventActors = "Wizard 16 5 0 farmer 16 11 0";
                eventAttributes = "skippable/ignoreCollisions farmer";
                eventMove = "move farmer 0 -4 0/pause 500/faceDirection Wizard 1/faceDirection Wizard 2";

                WizardEvent(eventSong, eventViewPort, eventActors, eventAttributes, eventMove, e.NewLocation);
            }
            else if (!modEntryInstance.isSVEInstalled && e.NewLocation.Name == "WizardHouse")
            {
                eventViewPort = "0 5";
                eventActors = "Wizard 8 6 0 farmer 8 15 0";
                eventAttributes = "skippable/ignoreCollisions farmer";
                eventMove = "move farmer 0 -8 0/pause 500/faceDirection Wizard 1/faceDirection Wizard 2";

                WizardEvent(eventSong, eventViewPort, eventActors, eventAttributes, eventMove, e.NewLocation);
            }
        }

        private void WizardEvent(string eventSong, string eventViewPort, string eventActors, string eventAttributes, string eventMove, GameLocation gameLocation)
        {
            if (!modEntryInstance.farmerMagicHelper.LearnedWizardy && Game1.player.friendshipData.TryGetValue("Wizard", out Friendship wizardFriendship) && wizardFriendship.Points >= 1000)
            {

                string magicAltarRecipe = $"{ModEntry.ArsVenificiContentPatcherId}_Magic_Altar";
                CraftingRecipe craftingRecipe = new CraftingRecipe(magicAltarRecipe);

                //addCraftingRecipe

                string eventWizardDialogue1 = "pause 1500/speak Wizard \"{0}\"/pause 1000";
                eventWizardDialogue1 = string.Format(eventWizardDialogue1, modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_1"));

                string eventWizardDialogue2 = "speak Wizard \"{0}\"/pause 1000/speak Wizard \"{1}\"/pause 1000/jump farmer/pause 1000";
                eventWizardDialogue2 = string.Format(eventWizardDialogue2,
                    modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_2"),
                    modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_3")
                );

                string eventWizardDialogue3 = "speak Wizard \"{0}\"/pause 1000/playSound reward/message \"{1}\"/pause 1500";
                eventWizardDialogue3 = string.Format(eventWizardDialogue3,
                    modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_4"),
                    modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_1")
                );

                string eventWizardDialogue4 = "speak Wizard \"{0}#$b#{1}#$b#{2}#$b#{3}\"/pause 1500/playSound reward/message \"{4}\"/pause 1500/speak Wizard \"{5}#$b#{6}\"/pause 1000/message \"{7}\"/pause 1000/message \"{8}\"/pause 1000/message \"{9}\"/pause 1000/message \"{10}\"";

                eventWizardDialogue4 = string.Format(eventWizardDialogue4,
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_5"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_6"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_7"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_8"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_2"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_9"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_10"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_3"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_4"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_5"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_6")
                 );

                string eventWizardDialogue5 = "speak Wizard \"{0}#$b#{1}#$b#{2}#$b#{3}#$b#{4}\"/pause 1500/message \"{5}\"/pause 1500/speak Wizard \"{6}\"/message \"{7}\"/pause 1000/message \"{8}\"/speak Wizard \"{9}\"";

                eventWizardDialogue5 = string.Format(eventWizardDialogue5,
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_11"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_12"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_13"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_14"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_15"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_7"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_16"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_8"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_9"),
                     modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_17")
                 );

                string eventWizardAboveHeadDialogue = "showFrame Wizard 16/textAboveHead Wizard \"{0}\"";

                eventWizardAboveHeadDialogue = string.Format(eventWizardAboveHeadDialogue,
                    modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_abovehead")
                );

                string eventEnd = "pause 750/fade 750/end";

                string eventStr = string.Join(
                    "/",
                    eventSong,
                    eventViewPort,
                    eventActors,
                    eventAttributes,
                    eventMove,
                    eventWizardDialogue1,
                    eventWizardDialogue2,
                    eventWizardDialogue3,
                    eventWizardDialogue4,
                    eventWizardDialogue5,
                    eventWizardAboveHeadDialogue,
                    eventEnd
                );


                //e.NewLocation.currentEvent = new Event(eventStr, ModEntry.LearnedMagicEventId);
                gameLocation.currentEvent = new Event(eventStr, null, modEntryInstance.farmerMagicHelper.LearnedWizardryEventId.ToString());

                Game1.eventUp = true;
                Game1.displayHUD = false;
                Game1.player.CanMove = false;
                Game1.player.showNotCarrying();

                Game1.player.AddCustomSkillExperience(FarmerMagicHelper.Skill, FarmerMagicHelper.Skill.ExperienceCurve[0]);
                modEntryInstance.farmerMagicHelper.FixManaPoolIfNeeded(Game1.player, overrideWizardryLevel: 1); // let player start using magic immediately
                Game1.player.eventsSeen.Add(modEntryInstance.farmerMagicHelper.LearnedWizardryEventId.ToString());

                if (!Game1.player.craftingRecipes.Keys.Contains(magicAltarRecipe))
                    Game1.player.craftingRecipes.Add(magicAltarRecipe, 0);

                if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Arcane_Compound"))
                    Game1.player.craftingRecipes.Add(ModEntry.ArsVenificiContentPatcherId + "_Arcane_Compound", 0);

                if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Purified_Vinteum_Dust"))
                    Game1.player.craftingRecipes.Add(ModEntry.ArsVenificiContentPatcherId + "_Purified_Vinteum_Dust", 0);

                if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Blank_Rune"))
                    Game1.player.craftingRecipes.Add(ModEntry.ArsVenificiContentPatcherId + "_Blank_Rune", 0);
            }
        }
    }
}
