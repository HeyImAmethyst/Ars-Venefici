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
using ArsVenefici.Framework.FarmerPlayer;

namespace ArsVenefici
{
    public class ModEntry : Mod
    {
        //DONE: Create a way to grow the plants used in the mod
        //DONE: Add tabletop spell range mechanic, a line depending on where you are facing (N, S, E, W) where you are facing, a 15ft or 30ft cone etc... 
        //DONE: Change Effect Power modifier to be double the effect gained. That way smaller steps would be possible.
        //DONE: Change the plow-effect to not work on already hoed tiles

        public static ModEntry INSTANCE;

        public static IModHelper helper;

        public static string ArsVenificiContentPatcherId = "HeyImAmethyst.CP.ArsVenefici";
        public static string ArsVenificiModId = "HeyImAmethyst.ArsVenefici";
        public const string MsgCast = "HeyImAmethyst.ArsVenifici.Cast";

        public ModConfig Config;
        public ModSaveData ModSaveData;
        public const string SAVEDATA = "HeyImAmethyst-ArsVenifici-SaveData";

        public FarmerMagicHelper farmerMagicHelper;
        public HarmonyHelper harmonyHelper;
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
        public static Framework.ModAPIs.ItemExtensions.IApi ItemExtensionsApi;

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

        public bool isSVEInstalled;
        public bool isItemExtensionsInstalled;

        public static Random RandomGen = new Random();

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

            commands.AddCommands();

            try
            {
                harmonyHelper.InitializeAndPatch();
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
            farmerMagicHelper = new FarmerMagicHelper(this);

            SpaceCore.Skills.RegisterSkill(FarmerMagicHelper.Skill = new ArsVeneficiSkill(this));

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

            harmonyHelper = new HarmonyHelper(this);
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
            helper.Events.GameLoop.SaveCreating += gameloopEvents.OnSaveCreating;
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
}