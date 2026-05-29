using ArsVenefici.Framework.GUI;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewModdingAPI;
using StardewValley;
using SpaceCore;
using ArsVenefici.Framework.Skill;
using SpaceShared.APIs;
using SpaceCore.Events;
using ArsVenefici.Framework.Spells.Effects;
using ArsVenefici.Framework.Commands;
using ArsVenefici.Framework.GameSave;
using ArsVenefici.Framework.Events;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using Netcode;
using StardewValley.Network;
using System.Runtime.CompilerServices;
using System;
using ArsVenefici.Framework.Spells.Buffs;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework;
using ArsVenefici.Framework.ContentPacks;
using StardewModdingAPI.Events;

namespace ArsVenefici
{
    //DONE?: Fix same key has already been added error in SpellPartIconManager.PoplulateSprites when reloading a save in the same session

    //TODO???: Fix duration not working (Duration is working on my end. Idk whats going on.)

    //TODO???: Fix bug with AOE/wave + cone combination (Somehow, randomly, it seems to be working on my end)

    //TODO: BUG - It appears that when using the AOE modifier on any skill makes it free to cast in my current multiplayer game. The spell has an associated cost listed, but when casting it it does not actually drain any mana. These casts also do not increase experience at all.
    //TODO: Fix  wave shape devolves into only working on the 2nd, 3rd, 5th, and 7th tiles facing away from your character, in multiplayer
    //TODO: Fix  Wave + AOE on multiplayer: starts off as a "Wall" spell animation and only activates the spell if you walk in the opposite direction of how the spell is being fired. And when it does fire, it's super glitchy/laggy

    //TODO: Add affinity mechanic

    public class ModEntry : Mod
    {
        public static ModEntry INSTANCE;
        public ArsVeneficiAPILoader arsVeneficiAPILoader = new ArsVeneficiAPILoader();

        public static IModHelper helper;

        //-----------------Mod Related Strings-----------------

        public static string ArsVenificiContentPatcherId = "HeyImAmethyst.CP.ArsVenefici";
        public static string ArsVenificiModId = "HeyImAmethyst.ArsVenefici";
        public const string MsgCast = "HeyImAmethyst.ArsVenifici.Cast";

        //-----------------Config and Save Data-----------------

        public ModConfig Config;
        public ModSaveData ModSaveData;
        public const string SAVEDATA = "HeyImAmethyst-ArsVenifici-SaveData";

        //-----------------Content Packs-----------------

        public ContentPackHelper PackHelper;
        public string contentPackSpellIconsDirectory = "assets/icon/spellpart/";

        //-----------------Class Intances-----------------

        public FarmerMagicHelper farmerMagicHelper;
        public HarmonyHelper harmonyHelper;
        public DailyTracker dailyTracker;
        public Buffs buffs;
        public SpellPartManager spellPartManager;
        public SpellPartIconManager spellPartIconManager;
        public SpellPartSkillManager spellPartSkillManager;

        //-----------------Commands-----------------

        Commands commands;

        //-----------------APIs-----------------

        public static IManaBarApi ManaBarApi;
        public static ContentPatcher.IContentPatcherAPI ContentPatcherApi;
        public static Framework.ModAPIs.ItemExtensions.IApi ItemExtensionsApi;

        //-----------------Events-----------------

        public ButtonEvents buttonEvents;
        public CharacterEvents characterEvents;
        public DisplayEvents displayEvents;
        public GameloopEvents gameloopEvents;
        public WorldEvents worldEvents;
        public MultiplayerEvents multiplayerEvents;
        public PlayerEvents playerEvents;
        public SpellPartEvents spellPartEvents;

        //-----------------Textures-----------------

        //public ModTextures modTextures;

        //-----------------Active Effects-----------------

        /// <summary>The active effects, spells, or projectiles which should be updated or drawn.</summary>
        public readonly IList<IActiveEffect> ActiveEffects = new List<IActiveEffect>();

        //-----------------Booleans-----------------
        public bool isSVEInstalled;
        public bool isItemExtensionsInstalled;
        public bool isWalkOfLifeInstalled;

        //-----------------Random Generation-----------------

        //public static Random RandomGen = new Random();

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
            CheckIfWalkOfLifeIsInstalled();

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
            arsVeneficiAPILoader.SetAPI(new ArsVeneficiAPIImpl());
            PackHelper = new ContentPackHelper(this);
            farmerMagicHelper = new FarmerMagicHelper(this);

            SpaceCore.Skills.RegisterSkill(FarmerMagicHelper.Skill = new ArsVeneficiSkill(this));

            commands = new Commands(this);

            dailyTracker = new DailyTracker();
            buffs = new Buffs(this);
            spellPartManager = new SpellPartManager(this);
            spellPartIconManager = new SpellPartIconManager(this, PackHelper);
            spellPartSkillManager = new SpellPartSkillManager(this);

            buttonEvents = new ButtonEvents(this);
            characterEvents = new CharacterEvents();
            displayEvents = new DisplayEvents(this);
            gameloopEvents = new GameloopEvents(this);
            worldEvents = new WorldEvents(this);
            multiplayerEvents = new MultiplayerEvents(this);
            playerEvents = new PlayerEvents(this);
            spellPartEvents = new SpellPartEvents(this);

            harmonyHelper = new HarmonyHelper(this);
        }

        /// <summary>
        /// Loads any assests needed for the mod
        /// </summary>
        private void LoadAssets()
        {
            ModTextures.LoadAssets(helper);
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

            spellPartEvents.AddSpellParts += spellPartEvents.OnAddSpellParts;
            spellPartEvents.AddSpellPartSkills += spellPartEvents.OnAddSpellPartSkills;

            helper.Events.Player.Warped += playerEvents.OnWarped;
            helper.Events.Player.InventoryChanged += playerEvents.OnInventoryChanged;
            helper.Events.World.NpcListChanged += worldEvents.OnNpcListChanged;
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

        /// <summary>
        /// Checks if the mod Walk Of Life is currently installed.
        /// </summary>
        private void CheckIfWalkOfLifeIsInstalled()
        {
            isWalkOfLifeInstalled = Helper.ModRegistry.IsLoaded("DaLion.Professions");
            Monitor.Log($"Walk of Life Installed: {isItemExtensionsInstalled}", LogLevel.Trace);
        }
    }
}