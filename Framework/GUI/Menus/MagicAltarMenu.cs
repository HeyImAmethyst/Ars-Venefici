using ArsVenefici.Framework.GUI.DragNDrop;
using ArsVenefici.Framework.Interfaces.GUI;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Force.DeepCloner;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceCore;
using SpaceCore.UI;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Buffs;
using StardewValley.Enchantments;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static StardewValley.Menus.CoopMenu;
using Object = StardewValley.Object;

namespace ArsVenefici.Framework.GUI.Menus
{
    public class MagicAltarMenu : ModdedClickableMenu
    {
        public ModEntry modEntry;
        SpellBook spellBook;
        SpellPartSkillManager knowlegeManager;

        public static int GUI_WIDTH = 210 + borderWidth * 2;
        public static int GUI_HEIGHT = 210 + borderWidth * 2 + Game1.tileSize;

        private MagicAltarTabRenderer activeTab;
        private int activeTabIndex = 0;

        private Dictionary<MagicAltarTab, MagicAltarTabRenderer> occulusTabs = new Dictionary<MagicAltarTab, MagicAltarTabRenderer>();
        private readonly List<MagicAltarTabButton> tabButtons = new List<MagicAltarTabButton>();

        public MagicAltarMenu(ModEntry modEntry)
           : base((int)GetAppropriateMenuPosition().X, (int)GetAppropriateMenuPosition().Y, GUI_WIDTH, GUI_HEIGHT, true)
        {
            this.modEntry = modEntry;
            spellBook = Game1.player.GetSpellBook();
            this.knowlegeManager = modEntry.spellPartSkillManager;

            UpdateMenu();
        }

        public static Vector2 GetAppropriateMenuPosition()
        {
            //Game1.uiViewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2

            //int x = Game1.viewport.Size.Width / 2 - GUI_WIDTH / 2;
            //int y = Game1.viewport.Size.Height / 2 - GUI_HEIGHT / 2;

            int x = Game1.uiViewport.Width / 2 - GUI_WIDTH / 2;
            int y = Game1.uiViewport.Height / 2 - GUI_HEIGHT / 2;

            //int x = Game1.uiViewport.Width / 2 - (GUI_WIDTH + IClickableMenu.borderWidth * 2) / 2;
            //int y = Game1.uiViewport.Height / 2 - (GUI_HEIGHT + IClickableMenu.borderWidth * 2) / 2;

            Vector2 defaultPosition = new Vector2(x, y);

            //Using Both doesnt seem to fix my issue

            //defaultPosition = defaultPosition * Game1.options.zoomLevel;
            //defaultPosition = defaultPosition * (1f / Game1.options.zoomLevel);

            return defaultPosition;

        }

        /// <summary>The method called when the game window changes size.</summary>
        /// <param name="oldBounds">The former viewport.</param>
        /// <param name="newBounds">The new viewport.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            this.xPositionOnScreen = (int)GetAppropriateMenuPosition().X;
            this.yPositionOnScreen = (int)GetAppropriateMenuPosition().Y;

            SetUpPositions();
        }

        protected override void SetUpPositions()
        {
            if (modEntry != null)
            {
                if (spellBook == null)
                    return;

                occulusTabs.Clear();
                tabButtons.Clear();

                MagicAltarSkillTreeTabRenderer offenceTabRenderer = new MagicAltarSkillTreeTabRenderer(knowlegeManager.offenceTab, this);
                MagicAltarSkillTreeTabRenderer defenceTabRenderer = new MagicAltarSkillTreeTabRenderer(knowlegeManager.defenseTab, this);
                MagicAltarSkillTreeTabRenderer utilityTabRenderer = new MagicAltarSkillTreeTabRenderer(knowlegeManager.utilityTab, this);

                occulusTabs.Add(knowlegeManager.offenceTab, offenceTabRenderer);
                occulusTabs.Add(knowlegeManager.defenseTab, defenceTabRenderer);
                occulusTabs.Add(knowlegeManager.utilityTab, utilityTabRenderer);

                activeTabIndex = 0;
                setActiveTab(activeTabIndex);

                int tabSize = 22;

                foreach (var kvp in occulusTabs.ToList())
                {
                    MagicAltarTab tab = kvp.Key;
                    int tabIndex = tab.GetIndex();

                    //OcculusTabButton occulusTabButton = new OcculusTabButton(tabIndex, 7 + tabIndex % 8 * (tabSize + 2), -tabSize, xPositionOnScreen - 150, yPositionOnScreen - 142, tab, tab.GetName());
                    MagicAltarTabButton occulusTabButton = new MagicAltarTabButton(tabIndex, xPositionOnScreen - (7 + tabIndex % 8 * (tabSize + 35)), yPositionOnScreen - 135, tab, tab.GetName());
                    tabButtons.Add(occulusTabButton);
                }

                upperRightCloseButton = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 500, yPositionOnScreen - 150, 48, 48), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), 4f)
                {
                    myID = 9175502
                };
            }
        }

        public override void populateClickableComponentList()
        {
            if (tabButtons != null)
            {
                allClickableComponents = new List<ClickableComponent>(this.tabButtons.Count + 1);
                this.allClickableComponents.AddRange(this.tabButtons);
                this.allClickableComponents.Add(this.upperRightCloseButton);
            }
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void draw(SpriteBatch spriteBatch)
        {

            if (spellBook == null)
                return;

            base.draw(spriteBatch);

            int pMouseX;
            int pMouseY;

            //GamePadState gamePadState = Game1.input.GetGamePadState();

            //if (gamePadMoveWithRightStickEnabled)
            //{
            //    pMouseX = (int)gamePadState.ThumbSticks.Right.X;
            //    pMouseY = (int)gamePadState.ThumbSticks.Right.Y;

            //    Game1.setMousePosition(pMouseX, pMouseY);
            //    leftClickHeld(pMouseX, pMouseY);
            //}
            //else
            //{
            //    pMouseX = Game1.getOldMouseX();
            //    pMouseY = Game1.getOldMouseY();
            //}

            pMouseX = Game1.getOldMouseX();
            pMouseY = Game1.getOldMouseY();

            int x = xPositionOnScreen;
            int y = yPositionOnScreen;

            if(activeTab != null)
                activeTab.Draw(spriteBatch, pMouseX, pMouseY, 0);

            foreach (MagicAltarTabButton tabButton in tabButtons)
            {
                if (tabButton != null)
                {
                    tabButton.Draw(spriteBatch, x, y);
                }
            }

            // draw cursor
            drawMouse(spriteBatch);
        }

        /// <summary>The method invoked when the player left-clicks on the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {

            if (activeTab != null)
                activeTab.MouseClicked(x, y);

            foreach (MagicAltarTabButton button in tabButtons)
            {
                Rectangle rect = new Rectangle(button.bounds.X, button.bounds.Y, button.GetTab().GetIcon().Width + 20, button.GetTab().GetIcon().Width + 20);

                if (rect.Contains(x, y))
                {
                    int tabIndex = button.GetIndex();
                    setActiveTab(tabIndex);

                    Game1.playSound("grassyStep");

                    button.scale -= 0.5f;
                    button.scale = Math.Max(3.5f, button.scale);
                }
            }

            base.receiveLeftClick(x, y, playSound);
        }

        public override void leftClickHeld(int x, int y)
        {
            foreach (MagicAltarTabRenderer renderer in occulusTabs.Values)
            {
                if (renderer.bounds.Contains(x, y))
                    setDragging(true);
            }

            base.leftClickHeld(x, y);
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
            setDragging(false);
        }

        /// <summary>The method invoked when the player hovers the cursor over the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        public override void performHoverAction(int x, int y)
        {
            if (activeTab != null)
                activeTab.MouseHover(x, y);

            foreach (MagicAltarTabButton tabButton in tabButtons)
            {
                if (tabButton != null)
                {
                    tabButton.IsHovered(x, y);
                }
            }

            base.performHoverAction(x, y);
        }

        private void setActiveTab(int tabIndex)
        {
            activeTabIndex = tabIndex;

            foreach (var kvp in occulusTabs.ToList())
            {
                MagicAltarTab tab = kvp.Key;

                if (tab.GetIndex() == activeTabIndex)
                    activeTab = kvp.Value;
            }
        }
    }
}
