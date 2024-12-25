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
using ArsVenefici.Framework.Events;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using Netcode;
using StardewValley.Network;
using System.Runtime.CompilerServices;
using System;
using ArsVenefici.Framework.Spell.Buffs;

namespace ArsVenefici
{
    public class ModEntry : Mod
    {
        public static ModEntry INSTANCE;

        public static IModHelper helper;
        public ModConfig Config;
        public ModSaveData ModSaveData;
        public const string SAVEDATA = "HeyImAmethyst-ArsVenifici-SaveData";

        public HarmonyHelper HarmonyHelper;
        public DailyTracker dailyTracker;
        public Buffs buffs;
        public SpellPartManager spellPartManager;
        public SpellPartIconManager spellPartIconManager;
        public SpellPartSkillManager spellPartSkillManager;

        //Commands

        Commands commands;

        //APIs

        public static IManaBarApi ManaBarApi;
        public static ContentPatcher.IContentPatcherAPI ContentPatcherApi;
        public static string ArsVenificiContentPatcherId = "HeyImAmethyst.CP.ArsVenefici";
        public static Framework.ModAPIs.ItemExtensions.IApi ItemExtensionsApi;

        public static Random RandomGen = new Random();

        //Events

        public ButtonEvents buttonEvents;
        public CharacterEvents characterEvents;
        public DisplayEvents displayEvents;
        public GameloopEvents gameloopEvents;
        public MultiplayerEvents multiplayerEvents;
        public PlayerEvents playerEvents;

        //Mana bar texture

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

        public bool isSVEInstalled;
        public bool isItemExtensionsInstalled;


        public static bool SpellCastingMode = true;

        public override void Entry(IModHelper helper)
        {
            INSTANCE = this;

            ModEntry.helper = helper;

            InitializeClasses();

            LoadAssets();

            this.Config = this.Helper.ReadConfig<ModConfig>();
            SetUpEvents();

            CheckIfSVEIsInstalled();
            CheckIfItemExtensionsIsInstalled();

            SpaceCore.Skills.RegisterSkill(ModEntry.Skill = new ArsVeneficiSkill(this));

            commands.AddCommands();

            try
            {
                HarmonyHelper.InitializeAndPatch();
            }
            catch (Exception e)
            {
                Monitor.Log($"Issue with Harmony patching: {e}", LogLevel.Info);
                return;
            }

        }

        /// <summary>
        /// Initializes the needed classes for the mod.
        /// </summary>
        private void InitializeClasses()
        {

            commands = new Commands(this);

            dailyTracker = new DailyTracker();
            buffs = new Buffs(this);
            spellPartManager = new SpellPartManager(this);
            spellPartIconManager = new SpellPartIconManager(this);

            buttonEvents = new ButtonEvents(this);
            characterEvents = new CharacterEvents();
            displayEvents = new DisplayEvents(this);
            gameloopEvents = new GameloopEvents(this);
            multiplayerEvents = new MultiplayerEvents(this);
            playerEvents = new PlayerEvents(this);

            HarmonyHelper = new HarmonyHelper(this);
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
            helper.Events.Input.ButtonsChanged += buttonEvents.OnButtonsChanged;

            helper.Events.Display.RenderingHud += displayEvents.OnRenderingHud;
            helper.Events.Display.RenderedHud += displayEvents.OnRenderedHud;
            helper.Events.Display.MenuChanged += displayEvents.OnMenuChanged;
            helper.Events.Display.RenderedWorld += displayEvents.OnRenderedWorld;

            helper.Events.GameLoop.GameLaunched += gameloopEvents.OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += gameloopEvents.OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += gameloopEvents.OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += gameloopEvents.OnUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicking += gameloopEvents.OnOneSecondUpdateTicking;

            helper.Events.Player.Warped += playerEvents.OnWarped;
            SpaceEvents.OnItemEaten += playerEvents.OnItemEaten;

            helper.Events.Multiplayer.PeerConnected += multiplayerEvents.OnPeerConnected;
            helper.Events.Multiplayer.ModMessageReceived += multiplayerEvents.OnModMessageReceived;
            Networking.RegisterMessageHandler(MsgCast, multiplayerEvents.OnNetworkCast);

            characterEvents.CharacterDamage += characterEvents.OnCharacterDamage;
            characterEvents.CharacterHeal += characterEvents.OnCharacterHeal;
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

        /// <summary>
        /// Checks if the mod Item Extensions is currently installed.
        /// </summary>
        private void CheckIfItemExtensionsIsInstalled()
        {
            isItemExtensionsInstalled = Helper.ModRegistry.IsLoaded("mistyspring.ItemExtensions");
            Monitor.Log($"Item Extensions Installed: {isItemExtensionsInstalled}", LogLevel.Trace);
        }
    }

    public class FarmerExtData
    {
        public static ConditionalWeakTable<Farmer, FarmerExtData> data = new();

        public float HealthRegen { get; set; } = 0;
        public float StaminaRegen { get; set; } = 0;

        public float healthBuffer { get; set; } = 0;
        public float staminaBuffer { get; set; } = 0;
    }
}