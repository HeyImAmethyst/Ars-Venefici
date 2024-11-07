using StardewModdingAPI.Events;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Menus;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Emit;
using ArsVenefici.Framework.Interfaces.Spells;
using static HarmonyLib.Code;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Spells.Shape;
using StardewValley.Mods;
using StardewValley.Characters;
using SpaceCore;
using System.Reflection.PortableExecutable;
using ArsVenefici.Framework.Skill;
using SpaceCore.Content;
using System.Reflection;
using SpaceShared.APIs;
using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Spells.Effects;
using StardewValley.Network;
using static System.Net.Mime.MediaTypeNames;
using ArsVenefici.Framework.Spells.Components;
using StardewValley.Extensions;
using xTile.Tiles;
using ArsVenefici.Framework.Commands;
using StardewValley.GameData;
using ContentPatcher;
using StardewValley.ItemTypeDefinitions;
using StardewValley.GameData.Crops;
using StardewValley.GameData.Objects;
using SpaceCore.UI;
using static ArsVenefici.ModConfig;
using static StardewValley.Minigames.MineCart.Whale;
using System.Net;
using ArsVenefici.Framework.GameSave;
using static SpaceCore.Guidebooks.GuidebookData;

namespace ArsVenefici
{
    public class Events
    {

        DailyTracker dailyTracker;
        ModEntry modEntryInstance;

        StringBuilder tutorialTextString;

        public Events(ModEntry modEntry, DailyTracker dailyTracker)
        {
            modEntryInstance = modEntry;

            this.dailyTracker = dailyTracker;

            tutorialTextString = new StringBuilder();
            tutorialTextString.Clear();
        }

        /// <summary>
        /// Event that is called when the game launches.
        /// </summary>
        /// <param name="sender"> The object</param>
        /// <param name="e"> The Game Launched Event argument</param>
        /// <remarks> This is used to load the content packs</remarks>
        public void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {

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
        }

        public void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ReadModSaveData();

            if (Context.IsWorldReady)
            {
                modEntryInstance.spellPartSkillManager = new SpellPartSkillManager(modEntryInstance);
                SpellPartSkillHelper.Instance().UpdateIfNeeded(modEntryInstance, Game1.player);
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

        public void OnPeerConnected(object sender, PeerConnectedEventArgs e)
        {
            if (!Game1.IsMasterGame)
                return;

            modEntryInstance.Helper.Multiplayer.SendMessage(
                new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
        }

        /// <summary>
        /// Receive saved data sent from host
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == modEntryInstance.ModManifest.UniqueID)
            {
                switch (e.Type)
                {
                    // entire saveData
                    case ModEntry.SAVEDATA:
                        try
                        {
                            modEntryInstance.ModSaveData = e.ReadAs<ModSaveData>();
                            modEntryInstance.FixManaPoolIfNeeded(Game1.player);

                        }
                        catch (InvalidOperationException)
                        {
                            modEntryInstance.Monitor.Log($"Failed to read save data sent by host.", LogLevel.Warn);
                            modEntryInstance.ModSaveData = null;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Event that is called when a save is loaded.
        /// </summary> 
        /// <param name="sender"> The object</param>
        /// <param name="e"> The Save Loaded Event arguement</param>
        public void OnDayStarted(object sender, DayStartedEventArgs e)
        {

            modEntryInstance.FixManaPoolIfNeeded(Game1.player);

            if (Context.IsWorldReady)
            {
                if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree)
                    return;

                if (modEntryInstance.LearnedWizardy)
                {
                    string s = $"{ModEntry.ArsVenificiContentPatcherId}_MagicAltar";

                    CraftingRecipe craftingRecipe = new CraftingRecipe(s);

                    if (!Game1.player.craftingRecipes.Keys.Contains(s))
                        Game1.player.craftingRecipes.Add(s, 0);
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

                    if (Game1.player.HasCustomProfession(Skill.ManaEfficiencyProfession))
                    {
                        spellBook.SetManaCostReductionAmount(Skill.ManaEfficiencyProfession.GetValue<int>());
                    }
                    else if (Game1.player.HasCustomProfession(Skill.ManaEfficiency2Profession))
                    {
                        spellBook.SetManaCostReductionAmount(Skill.ManaEfficiency2Profession.GetValue<int>());
                    }

                    spellBook.SaveSpellBook(modEntryInstance);
                }

                if(Game1.player.GetCustomSkillLevel(ModEntry.Skill) >= 6)
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
            if (Game1.activeClickableMenu == null || !Game1.eventUp || Context.IsPlayerFree)
                modEntryInstance.dailyTracker.Update(modEntryInstance, e, Game1.currentLocation);

            // update active effects
            for (int i = modEntryInstance.ActiveEffects.Count - 1; i >= 0; i--)
            {
                IActiveEffect effect = modEntryInstance.ActiveEffects[i];

                //if (!effect.Update(e))
                //    modEntryInstance.ActiveEffects.RemoveAt(i);

                if (effect != null)
                    effect.Update(e);
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

        public void OnItemEaten(object sender, EventArgs args)
        {
            if (Game1.player != null)
            {
                if (Game1.player.itemToEat == null)
                {
                    modEntryInstance.Monitor.Log("No item eaten for the item eat event?!?", LogLevel.Error);
                    return;
                }

                if (Game1.player.itemToEat != null && Game1.objectData != null)
                {
                    if (Game1.objectData.TryGetValue(Game1.player.itemToEat.ItemId, out var data))
                    {
                        if (data != null)
                        {
                            if(data.CustomFields != null)
                            {
                                if(data.CustomFields.TryGetValue($"{ModEntry.ArsVenificiContentPatcherId}/Mana", out string manaValue))
                                {
                                    if (int.TryParse(manaValue, out int value))
                                        Game1.player.AddMana(value);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.OldMenu is SpellBookMenu)
            {
                string filePath = Path.Combine(modEntryInstance.Helper.DirectoryPath + "/Saves", $"{Constants.SaveFolderName}_spellbook_data.json");

                if (File.Exists(filePath))
                {
                    Game1.player.GetSpellBook().SyncSpellBook(modEntryInstance);
                }

                Game1.player.GetSpellBook().CreateSpells(modEntryInstance);
            }
        }

        /// <inheritdoc cref="IContentEvents.AssetRequested" />
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {

        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (Game1.activeClickableMenu != null)
                return;

            if (!modEntryInstance.LearnedWizardy)
                return;

            Farmer farmer = Game1.player;
            SpellBook spellBook = farmer.GetSpellBook();

            var spellHelper = SpellHelper.Instance();
            SpellPartSkillHelper knowlegeHelper = SpellPartSkillHelper.Instance();

            if (Context.IsPlayerFree)
            {
                var input = modEntryInstance.Helper.Input;

                //if(Game1.isAnyGamePadButtonBeingPressed == true)
                //if(Game1.options.gamepadMode == Options.GamepadModes.Auto)

                ModKeyBinds modKeyBinds;

                //if (Game1.input.GetGamePadState().)
                //{
                //    modKeyBinds = modEntryInstance.Config.controllerKeyBinds.modKeyBinds;
                //}
                //else 
                //{
                //    modKeyBinds = modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds;
                //}

                if (Game1.input.GetKeyboardState().GetPressedKeys().Length != 0)
                {
                    modKeyBinds = modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds;
                }
                else
                {
                    modKeyBinds = modEntryInstance.Config.controllerKeyBinds.modKeyBinds;
                }

                //modKeyBinds = modEntryInstance.Config.keyBoardKeyBinds.modKeyBinds;
                //modKeyBinds = modEntryInstance.Config.controllerKeyBinds.modKeyBinds;

                if (modKeyBinds.OpenSpellBookButtons.JustPressed())//if (input.IsDown(modEntryInstance.Config.OpenSpellBookButton))
                {
                    Game1.activeClickableMenu = new SpellBookMenu(modEntryInstance);
                }

                if (modKeyBinds.SpellToggles.JustPressed())
                {
                    ModEntry.SpellCastingMode = !ModEntry.SpellCastingMode;
                }

                if (modKeyBinds.MoveSpellLabelButtons.JustPressed())
                {
                    modEntryInstance.Config.Position = new Point((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y);
                    modEntryInstance.Helper.WriteConfig(modEntryInstance.Config);
                }

                if (modKeyBinds.NextSpellButtons.JustPressed())
                {
                    spellBook.SetCurrentSpellIndex(ValidateSpellIndex(spellBook.GetCurrentSpellIndex() + 1));

                    spellBook.TurnToSpell();
                    spellBook.SaveSpellBook(modEntryInstance);
                }

                if (modKeyBinds.NextShapeGroupButtons.JustPressed())
                {
                    spellHelper.NextShapeGroup(spellBook.GetCurrentSpell());
                }

                if (modKeyBinds.PreviousSpellButtons.JustPressed())
                {
                    spellBook.SetCurrentSpellIndex(ValidateSpellIndex(spellBook.GetCurrentSpellIndex() - 1));

                    spellBook.TurnToSpell();
                    spellBook.SaveSpellBook(modEntryInstance);
                }

                if (modKeyBinds.PreviousShapeGroupButtons.JustPressed())
                {
                    spellHelper.PrevShapeGroup(spellBook.GetCurrentSpell());
                }

                if (ModEntry.SpellCastingMode)
                {
                    if(modKeyBinds.CastSpellButtons.JustPressed())
                        CastSpell(farmer);

                    if (modKeyBinds.CastSpellPage1.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(0);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage2.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(1);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage3.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(2);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage4.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(3);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage5.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(4);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage6.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(5);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage7.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(6);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage8.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(7);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage9.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(8);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }

                    if (modKeyBinds.CastSpellPage10.JustPressed())
                    {
                        spellBook.SetCurrentSpellIndex(9);
                        spellBook.TurnToSpell();
                        spellBook.SaveSpellBook(modEntryInstance);
                        CastSpell(farmer);
                    }
                }

                if (modEntryInstance.LearnedWizardy && modKeyBinds.OpenTutorialTextButtons.JustPressed())
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

                if (modEntryInstance.LearnedWizardy && (input.GetState(SButton.MouseRight) == SButtonState.Pressed || input.GetState(SButton.ControllerA) == SButtonState.Pressed))
                {
                    Vector2 toolLocationTile = Utils.AbsolutePosToTilePos(Utility.clampToTile(Game1.player.GetToolLocation(true)));
                    Vector2 toolPixel = (toolLocationTile * Game1.tileSize) + new Vector2(Game1.tileSize / 2f); // center of tile

                    if (Game1.player.currentLocation.objects.TryGetValue(toolLocationTile, out StardewValley.Object obj))
                    {
                        Game1.player.lastClick = toolPixel;

                        //string s = ModEntry.JsonAssetsApi.GetBigCraftableId("Magic Altar");
                        string s = $"(BC){ModEntry.ArsVenificiContentPatcherId}_MagicAltar";

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
            }
        }

        public void OnNetworkCast(IncomingMessage msg)
        {
            Farmer player = Game1.getFarmer(msg.FarmerID);

            if (player == null)
                return;

            CastSpell(player);
        }

        private void CastSpell(Farmer farmer)
        {
            SpellHelper spellHelper = SpellHelper.Instance();
            ISpell spell = farmer.GetSpellBook().GetCurrentSpell();

            if (spell == null)
                modEntryInstance.Monitor.Log("Spell is null!", LogLevel.Trace);

            if (spell != null && spell.IsValid())
            {
                //if (!spell.IsContinuous()) return;

                foreach (ISpellPart spellPart in spell.SpellStack().Parts)
                {
                    if (spellPart is Grow && Game1.player.hasBuff("HeyImAmethyst.ArsVenifici_GrowSickness") == false)
                    {
                        dailyTracker.SetDailyGrowCastCount(dailyTracker.GetDailyGrowCastCount() + 1);
                    }
                    else
                        continue;
                }

                SpellCastResult result = spell.Cast(new CharacterEntityWrapper(farmer), farmer.currentLocation, 0, true, true);

                if (result.GetSpellCastResultType() == SpellCastResultType.EFFECT_FAILED)
                {
                    //Game1.addHUDMessage(new HUDMessage("Error Casting Spell", 3));
                    Game1.showRedMessage("Failed Casting Spell");
                }
                else if (result.GetSpellCastResultType() == SpellCastResultType.NOT_ENOUGH_MANA)
                {
                    Game1.showRedMessage("Failed Casting Spell: Not Enough Mana!");
                }
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


        public void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree)
                return;

            //GameLocation location = Game1.currentLocation;

            //foreach (Character character in location.characters)
            //{
            //    DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, character.GetBoundingBox()), Color.Red, 1);
            //}

            //foreach (Farmer farmer in location.farmers)
            //{
            //    DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, farmer.GetBoundingBox()), Color.Red, 1);
            //}

            //foreach (IActiveEffect effect in modEntryInstance.ActiveEffects)
            //{
            //    if (effect != null && effect is AbstractSpellEffect abstractSpellEffect)
            //        DrawSprite.DrawRectangle(e.SpriteBatch, Game1.GlobalToLocal(Game1.viewport, abstractSpellEffect.GetBoundingBox()), Color.Red, 1);
            //}

            RenderTouchIndicator(e.SpriteBatch);

            //draw active effects
            foreach (IActiveEffect effect in modEntryInstance.ActiveEffects)
                effect.Draw(e.SpriteBatch);
        }

        public void RenderTouchIndicator(SpriteBatch spriteBatch)
        {

            if (!modEntryInstance.LearnedWizardy || !ModEntry.SpellCastingMode)
                return;

            SpellBook spellBook = Game1.player.GetSpellBook();
            //ISpell spell = spellBook.GetCurrentSpell();
            ISpell spell = spellBook.GetSpells()[spellBook.GetCurrentSpellIndex()];

            if (spell != null)
            {
                if (Game1.activeClickableMenu == null && !Game1.eventUp && Game1.player.IsLocalPlayer && /*Game1.player.CurrentTool != null &&*/ (Game1.oldKBState.IsKeyDown(Keys.LeftShift) || Game1.options.alwaysShowToolHitLocation) && /*this.CurrentTool.doesShowTileLocationMarker() &&*/ (!Game1.options.hideToolHitLocationWhenInMotion || !Game1.player.isMoving()))
                {
                    Vector2 local = Vector2.One;
                    Texture2D texture = modEntryInstance.Helper.ModContent.Load<Texture2D>("assets/farmer/touch_indicator.png");

                    if (spell.FirstShape(spell.CurrentShapeGroupIndex()) != null && spell.FirstShape(spell.CurrentShapeGroupIndex()) is Touch)
                    {
                        local = Utils.AbsolutePosToScreenPos(Utility.clampToTile(Game1.player.GetToolLocation(true)));

                        spriteBatch.Draw(texture, local, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, local.Y / 10000f);
                    }
                    else if(spell.FirstShape(spell.CurrentShapeGroupIndex()) != null &&  spell.FirstShape(spell.CurrentShapeGroupIndex()) is EtherialTouch)
                    {
                        //Vector2 mousePos = Utility.PointToVector2(Game1.getMousePosition()) + new Vector2(Game1.viewport.X, Game1.viewport.Y);
                        //Vector2 absoluteClampedMousePos = Utility.clampToTile(mousePos);

                        //local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        ICursorPosition cursorPosition = modEntryInstance.Helper.Input.GetCursorPosition();
                        
                        Vector2 absoluteClampedMousePos = Utility.clampToTile(cursorPosition.AbsolutePixels);
                        local = Utils.AbsolutePosToScreenPos(absoluteClampedMousePos);

                        spriteBatch.Draw(texture, local, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, local.Y / 10000f);
                    }
                }
            }
        }

        public void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {
            //// draw active effects
            //foreach (IActiveEffect effect in modEntryInstance.ActiveEffects)
            //    effect.Draw(e.SpriteBatch);
        }


        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (Game1.activeClickableMenu != null || Game1.eventUp || !Context.IsPlayerFree || !ModEntry.SpellCastingMode)
                return;

            if (!modEntryInstance.LearnedWizardy)
                return;

            int x = modEntryInstance.Config.Position.X;
            int y = modEntryInstance.Config.Position.Y;

            RenderSpellLable(x, y, Game1.player.GetSpellBook());
        }

        public void RenderSpellLable(int x, int y, SpellBook spellBook)
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;

            ClickableComponent spellNameLable = new ClickableComponent(new Rectangle(x, y, 64, 64), "Spell Name");
            //spellNameLable.draw(spriteBatch);

            int spellPageIndex = spellBook.GetCurrentSpellIndex() + 1;
            string spellName = "";
            int spellShapeGroupIndex = 0;

            int manaCost = 0;

            string spellText;

            if (spellBook.GetCurrentSpell().GetName() != null)
                spellName = spellBook.GetCurrentSpell().GetName();

            if (spellBook.GetCurrentSpell().IsValid())
            {
                spellShapeGroupIndex = spellBook.GetCurrentSpell().CurrentShapeGroupIndex() + 1;
                manaCost = spellBook.GetCurrentSpell().Mana();
            }

            if (spellName == "")
            {
                spellText = spellName;
            }
            else
            {
                spellText = spellName + " | " + modEntryInstance.Helper.Translation.Get("ui.spell_label.shape_group.name") + ": " + spellShapeGroupIndex + " | " + modEntryInstance.Helper.Translation.Get("ui.mana_cost.name") + ": " + manaCost;
            }

            string text = spellPageIndex + " : " + spellText;

            //if (text != null)
            //{
            //    //IClickableMenu.drawTextureBox(spriteBatch, spellNameLable.bounds.X - 15, spellNameLable.bounds.Y - 50, spellNameLable.bounds.Width + 100 + text.Length, spellNameLable.bounds.Height, Color.White);
            //    //IClickableMenu.drawTextureBox(spriteBatch, spellNameLable.bounds.X - 15, spellNameLable.bounds.Y - 50, spellNameLable.bounds.Width + (int)Game1.smallFont.MeasureString(text).Y + 32 , spellNameLable.bounds.Height, Color.White);

            //    //IClickableMenu.drawTextureBox(spriteBatch, spellNameLable.bounds.X - 15, spellNameLable.bounds.Y - 50, text.Length + 400, spellNameLable.bounds.Height, Color.White);
            //}

            //Color color = Game1.textColor;
            Color color = Color.White;

            //Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, new Vector2(spellNameLable.bounds.X, spellNameLable.bounds.Y - 30), color);
            Utility.drawTextWithColoredShadow(spriteBatch, text, Game1.smallFont, new Vector2(spellNameLable.bounds.X, spellNameLable.bounds.Y - 30), color, Color.Gray);

            //if (text.Length > 0)
            //    Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, new Vector2(spellNameLable.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, spellNameLable.bounds.Y + Game1.tileSize / 2), color);
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
            if (!modEntryInstance.LearnedWizardy && Game1.player.friendshipData.TryGetValue("Wizard", out Friendship wizardFriendship) && wizardFriendship.Points >= 1000)
            {

                string s = $"{ModEntry.ArsVenificiContentPatcherId}_MagicAltar";
                CraftingRecipe craftingRecipe = new CraftingRecipe(s);

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
                gameLocation.currentEvent = new Event(eventStr, null, modEntryInstance.LearnedWizardryEventId.ToString());

                Game1.eventUp = true;
                Game1.displayHUD = false;
                Game1.player.CanMove = false;
                Game1.player.showNotCarrying();

                Game1.player.AddCustomSkillExperience(ModEntry.Skill, ModEntry.Skill.ExperienceCurve[0]);
                modEntryInstance.FixManaPoolIfNeeded(Game1.player, overrideWizardryLevel: 1); // let player start using magic immediately
                Game1.player.eventsSeen.Add(modEntryInstance.LearnedWizardryEventId.ToString());

                if (!Game1.player.craftingRecipes.Keys.Contains(s))
                    Game1.player.craftingRecipes.Add(s, 0);
            }
        }
    }
}
