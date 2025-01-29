using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Spell;
using ArsVenefici.Framework.Util;
using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArsVenefici.ModConfig;
using Microsoft.Xna.Framework;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.FarmerPlayer;

namespace ArsVenefici.Framework.Events
{
    public class ButtonEvents
    {
        ModEntry modEntryInstance;
        public ModKeyBinds modKeyBinds;

        StringBuilder tutorialTextString;

        public int spellKeyHoldTime = 0;

        public ButtonEvents(ModEntry modEntry)
        {
            modEntryInstance = modEntry;

            tutorialTextString = new StringBuilder();
            tutorialTextString.Clear();
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (Game1.activeClickableMenu != null)
                return;

            if (!modEntryInstance.farmerMagicHelper.LearnedWizardy)
                return;

            Farmer farmer = Game1.player;
            SpellBook spellBook = farmer.GetSpellBook();

            var spellHelper = SpellHelper.Instance();
            SpellPartSkillHelper knowlegeHelper = SpellPartSkillHelper.Instance();

            if (Context.IsPlayerFree)
            {
                var input = modEntryInstance.Helper.Input;

                //Swap keybinds

                if (Game1.input.GetKeyboardState().GetPressedKeys().Length != 0)
                {
                    modKeyBinds = modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds;
                }
                else
                {
                    modKeyBinds = modEntryInstance.Config.controllerKeyBinds.modKeyBinds;
                }

                //Open spell book

                if (modKeyBinds.OpenSpellBookButtons.JustPressed())
                {
                    Game1.activeClickableMenu = new SpellBookMenu(modEntryInstance);
                }

                //Toggle spell casting mode

                if (modKeyBinds.SpellToggles.JustPressed())
                {
                    FarmerMagicHelper.SpellCastingMode = !FarmerMagicHelper.SpellCastingMode;
                }

                //Move spell lable

                if (modKeyBinds.MoveSpellLabelButtons.JustPressed())
                {
                    modEntryInstance.Config.Position = new Point((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y);
                    modEntryInstance.Helper.WriteConfig(modEntryInstance.Config);
                }

                //Cycle spells

                if (modKeyBinds.NextSpellButtons.JustPressed())
                {
                    spellBook.SetCurrentSpellIndex(ValidateSpellIndex(spellBook.GetCurrentSpellIndex() + 1));

                    spellBook.TurnToSpell();
                    spellBook.SaveSpellBook(modEntryInstance);
                }

                if (modKeyBinds.PreviousSpellButtons.JustPressed())
                {
                    spellBook.SetCurrentSpellIndex(ValidateSpellIndex(spellBook.GetCurrentSpellIndex() - 1));

                    spellBook.TurnToSpell();
                    spellBook.SaveSpellBook(modEntryInstance);
                }

                //Cycle spell shape groups

                if (modKeyBinds.NextShapeGroupButtons.JustPressed())
                {
                    spellHelper.NextShapeGroup(spellBook.GetCurrentSpell());
                }

                if (modKeyBinds.PreviousShapeGroupButtons.JustPressed())
                {
                    spellHelper.PrevShapeGroup(spellBook.GetCurrentSpell());
                }

                //Cast spell

                if (FarmerMagicHelper.SpellCastingMode)
                {
                    CastSpell(spellHelper, spellBook, farmer);
                }

                //Show tutorial text

                if (modEntryInstance.farmerMagicHelper.LearnedWizardy && modKeyBinds.OpenTutorialTextButtons.JustPressed())
                {
                    ShowTutorialText();
                }

                //Display magic altar menu

                if (modEntryInstance.farmerMagicHelper.LearnedWizardy && (input.GetState(SButton.MouseRight) == SButtonState.Pressed || input.GetState(SButton.ControllerA) == SButtonState.Pressed))
                {
                    DisplayMagicAltarMenu();
                }
            }
        }

        private void ShowTutorialText()
        {
            tutorialTextString.Clear();

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_5") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_7") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_9") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_10") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_3") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_4") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_5") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_6") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_11") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_12") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_13") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_14") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_15") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_7") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.wizard_dialogue_16") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_8") + "^");

            tutorialTextString.AppendLine("^");

            tutorialTextString.AppendLine(modEntryInstance.Helper.Translation.Get("event.learn_wizardry.message_9") + "^");

            Game1.activeClickableMenu = new LetterViewerMenu(tutorialTextString.ToString());
        }

        private void DisplayMagicAltarMenu()
        {
            Vector2 toolLocationTile = Utils.AbsolutePosToTilePos(Utility.clampToTile(Game1.player.GetToolLocation(true)));
            Vector2 toolPixel = toolLocationTile * Game1.tileSize + new Vector2(Game1.tileSize / 2f); // center of tile

            if (Game1.player.currentLocation.objects.TryGetValue(toolLocationTile, out StardewValley.Object obj))
            {
                Game1.player.lastClick = toolPixel;

                //string s = ModEntry.JsonAssetsApi.GetBigCraftableId("Magic Altar");
                string s = $"(BC){ModEntry.ArsVenificiContentPatcherId}_Magic_Altar";

                //if (obj.QualifiedItemId.Equals("(O){{spacechase0.JsonAssets/BigCraftable: Magic Altar}}"))

                //craftingAltarTokenString.UpdateContext();
                //string value = craftingAltarTokenString.Value;

                if (obj.QualifiedItemId.Equals(s))
                {
                    obj.readyForHarvest.Value = true;

                    if (obj.checkForAction(Game1.player, true))
                    {
                        Game1.activeClickableMenu = new MagicAltarMenu(modEntryInstance);
                    }
                }
            }
        }

        private void CastSpell(SpellHelper spellHelper, SpellBook spellBook, Farmer farmer)
        {
            if (modKeyBinds.CastSpellButtons.JustPressed())
                spellHelper.CastSpell(farmer, modKeyBinds);

            if (modKeyBinds.CastSpellPage1.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(0);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage2.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(1);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage3.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(2);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage4.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(3);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage5.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(4);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage6.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(5);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage7.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(6);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage8.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(7);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage9.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(8);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellPage10.JustPressed())
            {
                spellBook.SetCurrentSpellIndex(9);
                spellBook.TurnToSpell();
                spellBook.SaveSpellBook(modEntryInstance);
                spellHelper.CastSpell(farmer, modKeyBinds);
            }

            if (modKeyBinds.CastSpellButtons.IsDown())
            {
                spellKeyHoldTime++;
            }

            if (!modKeyBinds.CastSpellButtons.IsDown())
            {
                spellKeyHoldTime = 0;
            }
        }

        public int ValidateSpellIndex(int index)
        {

            if (index < 0)
            {
                index = 9;
            }
            else if (index >= 10)
            {
                index = 0;
            }

            return index;
        }
    }
}
