using ArsVenefici.Framework.API.Client;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.GUI.DragNDrop;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Spells.Registry;
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
        SpellPartSkillManager spellPartSkillManager;

        public static int GUI_WIDTH = 210 + borderWidth * 2;
        public static int GUI_HEIGHT = 210 + borderWidth * 2 + Game1.tileSize;

        public MagicAltarTabRenderer activeTab;
        public int activeTabIndex = 0;

        private Dictionary<MagicAltarTab, MagicAltarTabRenderer> occulusTabs = new Dictionary<MagicAltarTab, MagicAltarTabRenderer>();
        
        private readonly List<MagicAltarTabButton> tabButtons = new List<MagicAltarTabButton>();
        private readonly List<MagicAltarTabButton> tabButtonsFullScreen = new List<MagicAltarTabButton>();

        private ClickableTextureComponent fullScreenToggleButton;

        public bool isFullScreen = false;

        public MagicAltarMenu(ModEntry modEntry)
           : base((int)GetAppropriateMenuPosition().X, (int)GetAppropriateMenuPosition().Y, GUI_WIDTH, GUI_HEIGHT, true)
        {
            this.modEntry = modEntry;
            spellBook = Game1.player.GetSpellBook();
            this.spellPartSkillManager = modEntry.spellPartSkillManager;

            UpdateMenu();
        }

        public static Vector2 GetAppropriateMenuPosition()
        {

            int x = Game1.uiViewport.Width / 2 - GUI_WIDTH / 2;
            int y = Game1.uiViewport.Height / 2 - GUI_HEIGHT / 2;

            Vector2 defaultPosition = new Vector2(x, y);

            if (defaultPosition.X + GUI_WIDTH > Game1.viewport.Width)
            {
                defaultPosition.X = 0;
            }

            if (defaultPosition.Y + GUI_HEIGHT > Game1.viewport.Height)
            {
                defaultPosition.Y = 0;
            }

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

            //SetUpPositions();
            UpdateMenu();
        }

        protected override void Init()
        {
            if (modEntry != null && spellBook != null)
            {

                //modEntry.Monitor.Log(occulusTabs.Count.ToString(), LogLevel.Info);

                //occulusTabs.Clear();

                //MagicAltarSkillTreeTabRenderer offenceTabRenderer = new MagicAltarSkillTreeTabRenderer(spellPartSkillManager.offenceTab, this);
                //MagicAltarSkillTreeTabRenderer defenceTabRenderer = new MagicAltarSkillTreeTabRenderer(spellPartSkillManager.defenseTab, this);
                //MagicAltarSkillTreeTabRenderer utilityTabRenderer = new MagicAltarSkillTreeTabRenderer(spellPartSkillManager.utilityTab, this);

                //occulusTabs.Add(spellPartSkillManager.offenceTab, offenceTabRenderer);
                //occulusTabs.Add(spellPartSkillManager.defenseTab, defenceTabRenderer);
                //occulusTabs.Add(spellPartSkillManager.utilityTab, utilityTabRenderer);

                //upperRightCloseButton = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 700, yPositionOnScreen - 100, 48, 48), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), 4f)
                //{
                //    myID = 9175502
                //};

                //setActiveTab(activeTabIndex);
            }
        }

        protected override void SetUpPositions()
        {
            if (modEntry != null)
            {
                if (spellBook == null)
                    return;

                occulusTabs.Clear();

                MagicAltarSkillTreeTabRenderer offenceTabRenderer = new MagicAltarSkillTreeTabRenderer(ArsSpellPartSkills.offenceTab, this);
                MagicAltarSkillTreeTabRenderer defenceTabRenderer = new MagicAltarSkillTreeTabRenderer(ArsSpellPartSkills.defenseTab, this);
                MagicAltarSkillTreeTabRenderer utilityTabRenderer = new MagicAltarSkillTreeTabRenderer(ArsSpellPartSkills.utilityTab, this);

                occulusTabs.Add(ArsSpellPartSkills.offenceTab, offenceTabRenderer);
                occulusTabs.Add(ArsSpellPartSkills.defenseTab, defenceTabRenderer);
                occulusTabs.Add(ArsSpellPartSkills.utilityTab, utilityTabRenderer);

                upperRightCloseButton = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 600, yPositionOnScreen - 150, 48, 48), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), 4f)
                {
                    myID = 9175502
                };

                fullScreenToggleButton = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 605, yPositionOnScreen - 100, 48, 48), Game1.mouseCursors, new Rectangle(256, 256, 10, 10), 4f)
                {
                    myID = 91755081
                };

                tabButtons.Clear();
                tabButtonsFullScreen.Clear();

                //activeTabIndex = 0;
                setActiveTab(activeTabIndex);

                int tabSize = 22;

                foreach (var kvp in occulusTabs.ToList())
                {
                    MagicAltarTab tab = kvp.Key;
                    int tabIndex = tab.GetIndex();

                    //OcculusTabButton occulusTabButton = new OcculusTabButton(tabIndex, 7 + tabIndex % 8 * (tabSize + 2), -tabSize, xPositionOnScreen - 150, yPositionOnScreen - 142, tab, tab.GetName());
                    //MagicAltarTabButton occulusTabButton = null;

                    //MagicAltarTabButton occulusTabButton = new MagicAltarTabButton(tabIndex, xPositionOnScreen - (7 + tabIndex % 8 * (tabSize + 35)), yPositionOnScreen - 135, tab, tab.GetName(), this);
                    //tabButtons.Add(occulusTabButton);

                    if (isFullScreen)
                    {
                        MagicAltarTabButton occulusTabButton = new MagicAltarTabButton(tabIndex, xPositionOnScreen - (7 + tabIndex % 8 * (tabSize + 35)), yPositionOnScreen - 305, tab, tab.GetName(), this);
                        tabButtonsFullScreen.Add(occulusTabButton);
                    }
                    else
                    {
                        //MagicAltarTabButton occulusTabButton = new MagicAltarTabButton(tabIndex, 0, 0, tab, tab.GetName(), this);
                        MagicAltarTabButton occulusTabButton = new MagicAltarTabButton(tabIndex, xPositionOnScreen - (7 + tabIndex % 8 * (tabSize + 35)), yPositionOnScreen - 135, tab, tab.GetName(), this);
                        tabButtons.Add(occulusTabButton);
                    }

                    //MagicAltarTabButton occulusTabButton = new MagicAltarTabButton(tabIndex, xPositionOnScreen - (7 + tabIndex % 8 * (tabSize + 35)), yPositionOnScreen - 135, tab, tab.GetName(), this);
                    //tabButtons.Add(occulusTabButton);
                }

            }
        }

        public override void populateClickableComponentList()
        {
            if (tabButtons != null)
            {
                allClickableComponents = new List<ClickableComponent>(this.tabButtons.Count + 1);
                this.allClickableComponents.AddRange(this.tabButtons);
                this.allClickableComponents.AddRange(this.tabButtonsFullScreen);
                this.allClickableComponents.Add(this.upperRightCloseButton);
                this.allClickableComponents.Add(this.fullScreenToggleButton);
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

            if (activeTab != null)
                activeTab.Draw(spriteBatch, pMouseX, pMouseY, 0);

            //activeTab.Draw(spriteBatch, pMouseX, pMouseY, 0);

            foreach (MagicAltarTabRenderer renderer in occulusTabs.Values)
            {
                if (isFullScreen)
                {
                    foreach (MagicAltarTabButton tabButton in tabButtonsFullScreen)
                    {
                        if (tabButton != null)
                        {
                            tabButton.visible = true;
                            tabButton.Draw(spriteBatch, x, y);
                        }
                    }

                    foreach (MagicAltarTabButton tabButton in tabButtons)
                    {
                        if (tabButton != null)
                        {
                            tabButton.visible = false;
                        }
                    }

                    if (fullScreenToggleButton.containsPoint(pMouseX, pMouseY))
                    {
                        drawToolTip(spriteBatch, modEntry.Helper.Translation.Get("ui.magic_altar.shrink.name"), null, null);
                    }
                }
                else
                {
                    foreach (MagicAltarTabButton tabButton in tabButtons)
                    {
                        if (tabButton != null)
                        {
                            tabButton.visible = true;
                            tabButton.Draw(spriteBatch, x, y);
                        }
                    }

                    foreach (MagicAltarTabButton tabButton in tabButtonsFullScreen)
                    {
                        if (tabButton != null)
                        {
                            tabButton.visible = false;
                        }
                    }


                    if (fullScreenToggleButton.containsPoint(pMouseX, pMouseY))
                    {
                        drawToolTip(spriteBatch, modEntry.Helper.Translation.Get("ui.magic_altar.enlarge.name"), null, null);
                    }
                }
            }

            if (fullScreenToggleButton != null)
                fullScreenToggleButton.draw(spriteBatch);

            //fullScreenToggleButton.draw(spriteBatch);

            string dragLabel = modEntry.Helper.Translation.Get("ui.magic_altar.drag_label.description");

            int val1 = 272;
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
                val1 = 384;
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
                val1 = 336;

            //int value = Math.Max(val1, (int)Game1.dialogueFont.MeasureString(dragLabel == null ? "" : dragLabel).X);

            string parsedText = Game1.parseText(dragLabel.ToString(), Game1.smallFont, val1);

            if (isFullScreen)
            {
                IClickableMenu.drawTextureBox(spriteBatch, xPositionOnScreen - 720, yPositionOnScreen + 200, 320, 250, Color.White);
                Utility.drawTextWithShadow(spriteBatch, parsedText, Game1.smallFont, new Vector2(xPositionOnScreen - 700, yPositionOnScreen + 230), Game1.textColor);
            }
            else
            {
                IClickableMenu.drawTextureBox(spriteBatch, xPositionOnScreen - 520, yPositionOnScreen + 200, 320, 250, Color.White);
                Utility.drawTextWithShadow(spriteBatch, parsedText, Game1.smallFont, new Vector2(xPositionOnScreen - 500, yPositionOnScreen + 230), Game1.textColor);
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
            base.receiveLeftClick(x, y, playSound);

            if (activeTab != null)
                activeTab.MouseClicked(x, y);

            foreach (MagicAltarTabButton button in tabButtonsFullScreen)
            {
                if (button.visible)
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
            }


            foreach (MagicAltarTabButton button in tabButtons)
            {
                if (button.visible)
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
            }

            if (fullScreenToggleButton != null && fullScreenToggleButton.containsPoint(x, y))
            {
                isFullScreen = !isFullScreen;
                UpdateMenu();

                Game1.playSound("grassyStep");
            }
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

            foreach (MagicAltarTabButton tabButton in tabButtonsFullScreen)
            {
                if(tabButton.visible)
                {
                    if (tabButton != null)
                    {
                        tabButton.IsHovered(x, y);

                        tabButton.scale -= 0.5f;
                        tabButton.scale = Math.Max(3.5f, tabButton.scale);
                    }
                }
            }

            foreach (MagicAltarTabButton tabButton in tabButtons)
            {
                if (tabButton.visible)
                {
                    if (tabButton != null)
                    {
                        tabButton.IsHovered(x, y);

                        tabButton.scale -= 0.5f;
                        tabButton.scale = Math.Max(3.5f, tabButton.scale);
                    }
                }
            }

            if (fullScreenToggleButton != null)
                fullScreenToggleButton.tryHover(x, y, 0.5f);

            base.performHoverAction(x, y);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (activeTab != null)
                activeTab.MouseScroll(direction);

            base.receiveScrollWheelAction(direction);
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

            if (activeTab != null)
                ((MagicAltarSkillTreeTabRenderer)activeTab).ResetOffset();
        }
    }
}
