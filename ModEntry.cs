using ArsVenefici.Framework.GUI;
using ArsVenefici.Framework.Spell;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewModdingAPI;
using StardewValley;
using SpaceCore;
using ArsVenefici.Framework.Skill;
using SpaceShared.APIs;
using SpaceCore.Events;
using ArsVenefici.Framework.Spell.Effects;
using ArsVenefici.Framework.Commands;
using ArsVenefici.Framework.GameSave;

namespace ArsVenefici
{
    public class ModEntry : Mod
    {
        public static IModHelper helper;
        public ModConfig Config;
        public ModSaveData ModSaveData;
        public const string SAVEDATA = "HeyImAmethyst-ArsVenifici-SaveData";

        ToggleWizardryCommand toggleWizardryCommand;
        SpellPartsCommand spellPartsCommand;

        public static IManaBarApi ManaBarApi;
        public static ContentPatcher.IContentPatcherAPI ContentPatcherApi;
        public static string ArsVenificiContentPatcherId = "HeyImAmethyst.CP.ArsVenefici";
        public static ItemExtensions.IApi ItemExtensionsApi;

        public DailyTracker dailyTracker;
        public SpellPartManager spellPartManager;
        public SpellPartIconManager spellPartIconManager;
        public SpellPartSkillManager spellPartSkillManager;

        public Events eventsHandler;

        private static Texture2D ManaBg;
        private static Texture2D ManaFg;

        /// <summary>The active effects, spells, or projectiles which should be updated or drawn.</summary>
        public readonly IList<IActiveEffect> ActiveEffects = new List<IActiveEffect>();

        /// <remarks>This should only be accessed through <see cref="GetSpellBook"/> or <see cref="Extensions.GetSpellBook"/> to make sure an updated instance is retrieved.</remarks>
        private static readonly IDictionary<long, SpellBook> SpellBookCache = new Dictionary<long, SpellBook>();

        /// <summary>The ID of the event in which the player learns magic from the Wizard.</summary>
        //public static int LearnedMagicEventId { get; } = 90002;
        public int LearnedWizardryEventId { get; } = 9918172;


        /// <summary>Whether the current player learned wizardry.</summary>
        public bool LearnedWizardy => Game1.player?.eventsSeen?.Contains(LearnedWizardryEventId.ToString()) == true ? true : false;

        public static ArsVeneficiSkill Skill;
        public const string MsgCast = "HeyImAmethyst.ArsVenifici.Cast";
        public static Random RandomGen = new Random();

        public bool isSVEInstalled;
        public bool isItemExtensionsInstalled;


        public static bool SpellCastingMode = true;

        public override void Entry(IModHelper helper)
        {
            ModEntry.helper = helper;

            InitializeClasses();

            LoadAssets();

            this.Config = this.Helper.ReadConfig<ModConfig>();
            SetUpEvents();

            CheckIfSVEIsInstalled();
            CheckIfItemExtensionsIsInstalled();

            SpaceCore.Skills.RegisterSkill(ModEntry.Skill = new ArsVeneficiSkill(this));

            AddCommands();
        }

        /// <summary>
        /// Initializes the needed classes for the mod.
        /// </summary>
        private void InitializeClasses()
        {
            toggleWizardryCommand = new ToggleWizardryCommand(this);
            spellPartsCommand = new SpellPartsCommand(this);

            dailyTracker = new DailyTracker();
            spellPartManager = new SpellPartManager(this);
            spellPartIconManager = new SpellPartIconManager(this);
            eventsHandler = new Events(this, dailyTracker);
        }

        private static void LoadAssets()
        {
            ModEntry.ManaBg = helper.ModContent.Load<Texture2D>("assets/farmer/manabg.png");

            Color manaCol = new Color(0, 48, 255);
            ModEntry.ManaFg = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            ModEntry.ManaFg.SetData(new[] { manaCol });
        }

        /// <summary>
        /// Sets up the events for the mod.
        /// </summary>
        private void SetUpEvents()
        {
            helper.Events.Input.ButtonsChanged += eventsHandler.OnButtonsChanged;

            helper.Events.Display.RenderingHud += eventsHandler.OnRenderingHud;
            helper.Events.Display.RenderedHud += eventsHandler.OnRenderedHud;
            helper.Events.Display.MenuChanged += eventsHandler.OnMenuChanged;
            helper.Events.Display.RenderedWorld += eventsHandler.OnRenderedWorld;

            helper.Events.GameLoop.GameLaunched += eventsHandler.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += eventsHandler.OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += eventsHandler.OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += eventsHandler.OnUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicking += eventsHandler.OnOneSecondUpdateTicking;

            helper.Events.Player.Warped += eventsHandler.OnWarped;

            SpaceEvents.OnItemEaten += eventsHandler.OnItemEaten;
            helper.Events.Multiplayer.PeerConnected += eventsHandler.OnPeerConnected;
            helper.Events.Multiplayer.ModMessageReceived += eventsHandler.OnModMessageReceived;
            Networking.RegisterMessageHandler(MsgCast, eventsHandler.OnNetworkCast);
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.ManaPointsPerLevel = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.ManaRegenRate = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.InfiniteMana = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.EnableGrowSickness = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.GrowSicknessDurationMillisecondsLessThanLevelSix = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelSix = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                        Monitor.Log("Player is not the host. Changes have not been made");
                        return;
                    }

                    ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelEight = value;

                    Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                    Helper.Multiplayer.SendMessage(
                        new ModSaveDataEntryMessage(ModSaveData),
                        ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
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
                            Monitor.Log("Player is not the host. Changes have not been made");
                            return;
                        }

                        ModSaveData.GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelTen = value;

                        Helper.Data.WriteSaveData(ModEntry.SAVEDATA, ModSaveData);

                        Helper.Multiplayer.SendMessage(
                            new ModSaveDataEntryMessage(ModSaveData),
                            ModEntry.SAVEDATA, modIDs: new[] { ModManifest.UniqueID });
                    }
                });
        }

        public void AddCommand(string commandName, string commandDescription, Action<string, string[]> callback)
        {
            Helper.ConsoleCommands.Add(commandName, commandDescription, callback);
        }

        /// <summary>Fix the player's mana pool to match their skill level if needed.</summary>
        /// <param name="player">The player to fix.</param>
        /// <param name="overrideWizardryLevel">The wizardry skill level, or <c>null</c> to get it from the player.</param>
        public void FixManaPoolIfNeeded(Farmer player, int? overrideWizardryLevel = null)
        {
            // skip if player hasn't learned wizardry
            if (!LearnedWizardy && overrideWizardryLevel is not > 0)
                return;

            // get wizardry info
            int wizardryLevel = overrideWizardryLevel ?? player.GetCustomSkillLevel(Skill);
            
            SpellBook spellBook = Game1.player.GetSpellBook();

            // fix mana pool

            //if(LearnedWizardy)
            //{
            //    int expectedPoints = wizardryLevel * ManaPointsPerLevel;

            //    if (player.GetMaxMana() < expectedPoints)
            //    {
            //        player.SetMaxMana(expectedPoints);
            //        player.AddMana(expectedPoints);
            //    }
            //}

            int expectedPoints = wizardryLevel * ModSaveData.ManaPointsPerLevel;

            if (player.GetMaxMana() < expectedPoints)
            {
                player.SetMaxMana(expectedPoints);
                player.AddMana(expectedPoints);
            }
        }

        /// <summary>Get a self-updating view of a player's magic metadata.</summary>
        /// <param name="player">The player whose spell book to get.</param>
        public static SpellBook GetSpellBook(Farmer player)
        {
            if (!ModEntry.SpellBookCache.TryGetValue(player.UniqueMultiplayerID, out SpellBook book) || !object.ReferenceEquals(player, book.Player))
                ModEntry.SpellBookCache[player.UniqueMultiplayerID] = book = new SpellBook(player);

            return book;
        }

        /// <summary>
        /// Checks if the mod Stardew Valley Expanded is currently installed.
        /// </summary>
        private void CheckIfSVEIsInstalled()
        {
            isSVEInstalled = Helper.ModRegistry.IsLoaded("FlashShifter.StardewValleyExpandedCP");
            Monitor.Log($"Stardew Valley Expanded Sense Installed: {isSVEInstalled}", LogLevel.Trace);
        }

        private void CheckIfItemExtensionsIsInstalled()
        {
            isItemExtensionsInstalled = Helper.ModRegistry.IsLoaded("mistyspring.ItemExtensions");
            Monitor.Log($"Item Extensions Installed: {isItemExtensionsInstalled}", LogLevel.Trace);
        }
    }
}