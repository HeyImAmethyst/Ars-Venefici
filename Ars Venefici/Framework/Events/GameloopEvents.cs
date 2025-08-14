using ArsVenefici.Framework.GameSave;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Util;
using SpaceShared.APIs;
using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.Spells.Effects;
using ArsVenefici.Framework.Spells;
using SpaceCore;
using static ArsVenefici.ModConfig;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.API;
using ItemExtensions;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Events
{
    public class GameloopEvents
    {
        ModEntry modEntryInstance;


        public GameloopEvents(ModEntry modEntry) 
        {
            modEntryInstance = modEntry;
        }

        /// <summary>
        /// Event that is called when the game launches.
        /// </summary>
        /// <param name="sender"> The object</param>
        /// <param name="e"> The Game Launched Event argument</param>
        /// <remarks> This is used to load the content packs</remarks>
        public void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // Read The Content Packs
            modEntryInstance.PackHelper.ReadContentPacks();

            var configMenu = modEntryInstance.Helper.ModRegistry.GetGenericModConfigMenuApi(modEntryInstance.Monitor);

            if (configMenu != null)
            {
                configMenu.Register(
                    mod: modEntryInstance.ModManifest,
                    reset: () => modEntryInstance.Config = new ModConfig(),
                    save: () => modEntryInstance.Helper.WriteConfig(modEntryInstance.Config)
                );

                configMenu.AddNumberOption(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.position_x.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.position_x.tooltip"),
                    getValue: () => modEntryInstance.Config.Position.X,
                    setValue: value => modEntryInstance.Config.Position = new(value, modEntryInstance.Config.Position.Y)
                );

                configMenu.AddNumberOption(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.position_y.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.position_y.tooltip"),
                    getValue: () => modEntryInstance.Config.Position.Y,
                    setValue: value => modEntryInstance.Config.Position = new(modEntryInstance.Config.Position.X, value)
                );

                //Keyboard Controlls Config UI

                configMenu.AddSectionTitle(
                    mod: modEntryInstance.ModManifest,
                    text: () => modEntryInstance.Helper.Translation.Get("config.keyboard_controlls_section_title.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.keyboard_controlls_section_title.tooltip")
                );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.open_spellbook_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.open_spellbook_key.tooltip"),
                    getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.OpenSpellBookButtons,
                    setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.OpenSpellBookButtons = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.spell_toggle_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.spell_toggle_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.SpellToggles,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.SpellToggles = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.move_spell_label_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.move_spell_label_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.MoveSpellLabelButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.MoveSpellLabelButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.next_spell_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.next_spell_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.NextSpellButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.NextSpellButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.next_shape_group_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.next_shape_group_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.NextShapeGroupButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.NextShapeGroupButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.previous_spell_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.previous_spell_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.PreviousSpellButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.PreviousSpellButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.previous_shape_group_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.previous_shape_group_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.PreviousShapeGroupButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.PreviousShapeGroupButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellButtons = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.open_tutorial_text_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.open_tutorial_text_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.OpenTutorialTextButtons,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.OpenTutorialTextButtons = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_1_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_1_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage1,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage1 = value
                );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_2_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_2_key.tooltip"),
                    getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage2,
                    setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage2 = value
                );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_3_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_3_key.tooltip"),
                    getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage3,
                    setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage3 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_4_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_4_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage4,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage4 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_5_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_5_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage5,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage5 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_6_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_6_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage6,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage6 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_7_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_7_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage7,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage7 = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_8_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_8_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage8,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage8 = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_9_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_9_key.tooltip"),
                     getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage9,
                     setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage9 = value
                 );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_10_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_10_key.tooltip"),
                    getValue: () => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage10,
                    setValue: value => modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds.CastSpellPage10 = value
                );

                //Controller Controlls Config UI

                configMenu.AddSectionTitle(
                    mod: modEntryInstance.ModManifest,
                    text: () => modEntryInstance.Helper.Translation.Get("config.controller_controlls_section_title.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.controller_controlls_section_title.tooltip")
                );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.open_spellbook_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.open_spellbook_key.tooltip"),
                    getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.OpenSpellBookButtons,
                    setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.OpenSpellBookButtons = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.spell_toggle_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.spell_toggle_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.SpellToggles,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.SpellToggles = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.move_spell_label_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.move_spell_label_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.MoveSpellLabelButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.MoveSpellLabelButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.next_spell_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.next_spell_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.NextSpellButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.NextSpellButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.next_shape_group_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.next_shape_group_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.NextShapeGroupButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.NextShapeGroupButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.previous_spell_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.previous_spell_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.PreviousSpellButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.PreviousSpellButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.previous_shape_group_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.previous_shape_group_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.PreviousShapeGroupButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.PreviousShapeGroupButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.open_tutorial_text_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.open_tutorial_text_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.OpenTutorialTextButtons,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.OpenTutorialTextButtons = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_1_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_1_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage1,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage1 = value
                );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_2_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_2_key.tooltip"),
                    getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage2,
                    setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage2 = value
                );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_3_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_3_key.tooltip"),
                    getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage3,
                    setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage3 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_4_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_4_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage4,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage4 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_5_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_5_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage5,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage5 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_6_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_6_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage6,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage6 = value
                );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_7_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_7_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage7,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage7 = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_8_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_8_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage8,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage8 = value
                 );

                configMenu.AddKeybindList(
                     mod: modEntryInstance.ModManifest,
                     name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_9_key.name"),
                     tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_9_key.tooltip"),
                     getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage9,
                     setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage9 = value
                 );

                configMenu.AddKeybindList(
                    mod: modEntryInstance.ModManifest,
                    name: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_10_key.name"),
                    tooltip: () => modEntryInstance.Helper.Translation.Get("config.cast_spell_10_key.tooltip"),
                    getValue: () => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage10,
                    setValue: value => modEntryInstance.Config.controllerKeyBinds.modKeyBinds.CastSpellPage10 = value
                );
            }

            // hook Mana Bar
            {
                var manaBar = modEntryInstance.Helper.ModRegistry.GetApi<IManaBarApi>("spacechase0.ManaBar");

                if (manaBar == null)
                {
                    modEntryInstance.Monitor.Log("No mana bar API???", LogLevel.Error);
                    return;
                }

                ModEntry.ManaBarApi = manaBar;
            }

            // hook Content Patcher
            {
                var api = modEntryInstance.Helper.ModRegistry.GetApi<ContentPatcher.IContentPatcherAPI>("Pathoschild.ContentPatcher");
                ModEntry.ContentPatcherApi = api;
            }

            //hook Item Extensions
            {
                if (modEntryInstance.isItemExtensionsInstalled)
                {
                    var api = modEntryInstance.Helper.ModRegistry.GetApi<Framework.ModAPIs.ItemExtensions.IApi>("mistyspring.ItemExtensions");
                    ModEntry.ItemExtensionsApi = api;
                }
            }

            //modEntryInstance.spellPartManager.PopluateDictionary();
            //modEntryInstance.spellPartSkillManager.PopluateDictionary();

            modEntryInstance.spellPartEvents.InvokeOnAddSpellParts();
            modEntryInstance.spellPartEvents.InvokeOnAddSpellPartSkills();
        }

        public void OnSaveCreating(object sender, SaveCreatingEventArgs e)
        {
            if (modEntryInstance.ModSaveData == null)
                modEntryInstance.ModSaveData = new();
        }

        public void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ReadModSaveData();

            if (Context.IsWorldReady)
            {
                //modEntryInstance.spellPartSkillManager = new SpellPartSkillManager(modEntryInstance);
                modEntryInstance.spellPartIconManager.PoplulateSprites();
                modEntryInstance.arsVeneficiAPILoader.GetAPI().GetSpellPartSkillHelper().UpdateIfNeeded(modEntryInstance, Game1.player);
            }
        }

        private void ReadModSaveData()
        {
            if (!Game1.IsMasterGame)
                return;
            try
            {
                modEntryInstance.ModSaveData = modEntryInstance.Helper.Data.ReadSaveData<ModSaveData>(ModEntry.SAVEDATA);

                if (modEntryInstance.ModSaveData == null)
                    modEntryInstance.ModSaveData = new();
                //else
                //    modEntryInstance.ModSaveData.ResetValues();

                modEntryInstance.Helper.Data.WriteSaveData(ModEntry.SAVEDATA, modEntryInstance.ModSaveData);
            }
            catch (InvalidOperationException)
            {
                modEntryInstance.Monitor.Log($"Failed to read existing save data, previous settings lost.", LogLevel.Warn);
                modEntryInstance.ModSaveData = new();
            }
        }

        /// <summary>
        /// Event that is called when a save is loaded.
        /// </summary> 
        /// <param name="sender"> The object</param>
        /// <param name="e"> The Save Loaded Event arguement</param>
        public void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            var api = modEntryInstance.arsVeneficiAPILoader.GetAPI();

            if (Context.IsWorldReady)
            {
                if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree)
                    return;

                if (api.GetMagicHelper().LearnedWizardy(Game1.player))
                {
                    string magicAltarRecipe = $"{ModEntry.ArsVenificiContentPatcherId}_Magic_Altar";

                    CraftingRecipe craftingRecipe = new CraftingRecipe(magicAltarRecipe);

                    if (!Game1.player.craftingRecipes.Keys.Contains(magicAltarRecipe))
                        Game1.player.craftingRecipes.Add(magicAltarRecipe, 0);

                    if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Arcane_Compound"))
                        Game1.player.craftingRecipes.Add(ModEntry.ArsVenificiContentPatcherId + "_Arcane_Compound", 0);

                    if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Purified_Vinteum_Dust"))
                        Game1.player.craftingRecipes.Add(ModEntry.ArsVenificiContentPatcherId + "_Purified_Vinteum_Dust", 0);

                    if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Blank_Rune"))
                        Game1.player.craftingRecipes.Add(ModEntry.ArsVenificiContentPatcherId + "_Blank_Rune", 0);

                    if (!Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Mana_Cake"))
                        Game1.player.mailForTomorrow.Add("ArsVenefici.Mail.ManaManagement");

                    if (Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Mana_Cake") && Game1.player.mailForTomorrow.Contains("ArsVenefici.Mail.ManaManagement"))
                        Game1.player.mailForTomorrow.Remove("ArsVenefici.Mail.ManaManagement");

                    if (Game1.player.mailbox.Contains("ArsVenefici.Mail.ManaManagement") && Game1.player.craftingRecipes.Keys.Contains(ModEntry.ArsVenificiContentPatcherId + "_Mana_Cake"))
                        Game1.player.mailbox.Remove("ArsVenefici.Mail.ManaManagement");

                    //Game1.player.mailForTomorrow.Add("ArsVenefici.Mail.ManaManagement");

                    modEntryInstance.farmerMagicHelper.FixManaPoolIfNeeded(Game1.player, Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill));
                }

                SpellBook spellBook = Game1.player.GetSpellBook();

                if (spellBook != null)
                {
                    string filePath = Path.Combine(modEntryInstance.Helper.DirectoryPath + "/Saves", $"{Constants.SaveFolderName}_spellbook_data.json");

                    if (File.Exists(filePath))
                    {
                        spellBook.SyncSpellBook(modEntryInstance);
                    }

                    spellBook.CreateSpells(modEntryInstance);
                    spellBook.TurnToSpell();

                    if (Game1.player.HasCustomProfession(ArsVeneficiSkill.ManaEfficiencyProfession))
                    {
                        spellBook.SetManaCostReductionAmount(ArsVeneficiSkill.ManaEfficiencyProfession.GetValue<int>());
                    }
                    else if (Game1.player.HasCustomProfession(ArsVeneficiSkill.ManaEfficiency2Profession))
                    {
                        spellBook.SetManaCostReductionAmount(ArsVeneficiSkill.ManaEfficiency2Profession.GetValue<int>());
                    }
                    else
                    {
                        spellBook.SetManaCostReductionAmount(1);
                    }

                    spellBook.SaveSpellBook(modEntryInstance);
                }

                if (Game1.player.GetCustomSkillLevel(FarmerMagicHelper.Skill) >= 6)
                {
                    modEntryInstance.dailyTracker.SetMaxDailyGrowCastCount(int.MaxValue);
                }
                else
                {
                    modEntryInstance.dailyTracker.SetMaxDailyGrowCastCount(2);
                }

                modEntryInstance.dailyTracker.SetDailyGrowCastCount(0);
            }
        }

        public void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            var api = modEntryInstance.arsVeneficiAPILoader.GetAPI();

            if (Game1.activeClickableMenu == null || !Game1.eventUp || Context.IsPlayerFree)
                modEntryInstance.dailyTracker.Update(modEntryInstance, e, Game1.currentLocation);

            if (Context.IsWorldReady)
            {
                var spellHelper = api.GetSpellHelper();
                Farmer farmer = Game1.player;
                ISpell spell = farmer.GetSpellBook().GetCurrentSpell();

                //modEntryInstance.Monitor.Log(spellKeyHoldTime.ToString(), LogLevel.Info);
                //modEntryInstance.Monitor.Log(spell.IsContinuous().ToString(), LogLevel.Info);


                if (spell.IsContinuous() && modEntryInstance.buttonEvents.spellKeyHoldTime > 1)
                {
                    if (e.IsMultipleOf(15))
                        spellHelper.CastSpell(farmer, modEntryInstance.buttonEvents.modKeyBinds);
                }

                // update active effects
                for (int i = modEntryInstance.ActiveEffects.Count - 1; i >= 0; i--)
                {
                    IActiveEffect effect = modEntryInstance.ActiveEffects[i];

                    //if (!effect.Update(e))
                    //    modEntryInstance.ActiveEffects.RemoveAt(i);

                    if (effect != null)
                        effect.Update(e);
                }

                if (modEntryInstance.isWalkOfLifeInstalled)
                {
                    var magicHelper = api.GetMagicHelper();

                    if (magicHelper.LearnedWizardy(farmer) && farmer.GetCustomSkillLevel(FarmerMagicHelper.Skill) == 0)
                    {
                        modEntryInstance.farmerMagicHelper.WalkOfLifeResetFix();
                    }
                    
                }
            }
        }

        public void OnOneSecondUpdateTicking(object sender, OneSecondUpdateTickingEventArgs e)
        {

            if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree)
                return;

            // update active effects
            for (int i = modEntryInstance.ActiveEffects.Count - 1; i >= 0; i--)
            {
                IActiveEffect effect = modEntryInstance.ActiveEffects[i];

                //if (!effect.OneSecondUpdate(e))
                //    modEntryInstance.ActiveEffects.RemoveAt(i);

                if (effect != null)
                    effect.OneSecondUpdate(e);
            }
        }
    }
}
