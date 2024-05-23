using ArsVenefici.Framework.GUI.DragNDrop;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using SpaceCore.Content;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using xTile;
using static System.Net.Mime.MediaTypeNames;

namespace ArsVenefici.Framework.GUI.Menus
{
    public class SpellBookMenu : ModdedClickableMenu
    {

        public ModEntry modEntry;

        /// <summary>The areas to draw.</summary>
        private List<DragArea<SpellPartDraggable>> dragAreas = new List<DragArea<SpellPartDraggable>>();

        SpellBook spellBook;

        private SpellPartSourceArea sourceArea;
        private SpellGrammarArea spellGrammarArea;
        private ShapeGroupListArea shapeGroupArea;

        private SpellPartDraggable dragged;
        DragArea<SpellPartDraggable> hoveredArea;
        SpellPartDraggable hoveredPart;

        public List<ClickableTextureComponent> buttons = new List<ClickableTextureComponent>();

        private TextBox nameBox;
        public ClickableComponent nameBoxCC;

        /// <summary>The labels to draw.</summary>
        private readonly List<ClickableComponent> labels = new List<ClickableComponent>();

        private readonly List<ClickableComponent> clickables = new List<ClickableComponent>();

        public static int windowWidth = 220 + borderWidth * 2;
        public static int windowHeight = 252 + borderWidth * 2 + Game1.tileSize;

        int currentPageIndex = 0;

        public SpellBookMenu(ModEntry modEntry)
            : base((int)GetAppropriateMenuPosition().X, (int)GetAppropriateMenuPosition().Y, windowWidth, windowHeight, true)
        {
            this.modEntry = modEntry;
            spellBook = Game1.player.GetSpellBook();

            exitFunction = () =>
            {
                SaveSpellBook(modEntry);

                string filePath = Path.Combine(modEntry.Helper.DirectoryPath + "/Saves", $"{Constants.SaveFolderName}_spellbook_data.json");

                if (File.Exists(filePath))
                {
                    Game1.player.GetSpellBook().SyncSpellBook(modEntry);
                }

                spellBook.CreateSpells(modEntry);
            };

            UpdateMenu();
        }

        public static Vector2 GetAppropriateMenuPosition()
        {

            int x = Game1.viewport.Size.Width / 2 - windowWidth / 2;
            int y = Game1.viewport.Size.Height / 2 - windowHeight / 2;

            Vector2 defaultPosition = new Vector2(x, y);

            defaultPosition = defaultPosition * Game1.options.zoomLevel;

            return defaultPosition;

        }

        /// <summary>The method called when the game window changes size.</summary>
        /// <param name="oldBounds">The former viewport.</param>
        /// <param name="newBounds">The new viewport.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            SetUpPositions();
        }

        protected override void SetUpPositions()
        {
            if (modEntry != null)
            {
                if (spellBook == null)
                    return;

                dragAreas.Clear();

                buttons.Clear();
                labels.Clear();

                sourceArea = new SpellPartSourceArea(new Rectangle(xPositionOnScreen - 142, yPositionOnScreen - 100, 530, 190), modEntry, "Source Area");
                shapeGroupArea = new ShapeGroupListArea(xPositionOnScreen - 200, yPositionOnScreen + 150, this, (part, i, j) => OnPartDropped(part), "ShapeGroup Area");
                spellGrammarArea = new SpellGrammarArea(new Rectangle(xPositionOnScreen - 142, yPositionOnScreen + 344, 436, 70), (part, i) => OnPartDropped(part), "Spell Grammar Area");

                dragAreas.Add(sourceArea);
                dragAreas.Add(spellGrammarArea);
                dragAreas.Add(shapeGroupArea);

                ClickableTextureComponent leftPageButton =
                    new ClickableTextureComponent("LeftPage", new Rectangle(xPositionOnScreen - 250, yPositionOnScreen + 450, 64, 64), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);

                leftPageButton.myID = 629;
                leftPageButton.upNeighborID = -99998;
                leftPageButton.leftNeighborID = -99998;
                leftPageButton.rightNeighborID = -99998;
                leftPageButton.downNeighborID = -99998;

                ClickableTextureComponent rightPageButton =
                    new ClickableTextureComponent("RightPage", new Rectangle(xPositionOnScreen + 350, yPositionOnScreen + 450, 64, 64), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false);

                rightPageButton.myID = 630;
                rightPageButton.upNeighborID = -99998;
                rightPageButton.leftNeighborID = -99998;
                rightPageButton.rightNeighborID = -99998;
                rightPageButton.downNeighborID = -99998;

                nameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
                {
                    X = xPositionOnScreen + 300,
                    Y = yPositionOnScreen + 360,
                    textLimit = 20
                };

                nameBoxCC = new ClickableComponent(new Rectangle(xPositionOnScreen + 64 + spaceToClearSideBorder + borderWidth + 256, yPositionOnScreen + borderWidth + spaceToClearTopBorder - 16, 192, 48), "")
                {
                    myID = 536,
                    upNeighborID = -99998,
                    leftNeighborID = -99998,
                    rightNeighborID = -99998,
                    downNeighborID = -99998
                };

                //this.nameLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + num1 + 16 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + 192 + 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - 8, 1, 1), "Spell Name");

                upperRightCloseButton = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 380, yPositionOnScreen - 150, 48, 48), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), 4f)
                {
                    myID = 9175502
                };

                buttons.Add(leftPageButton);
                buttons.Add(rightPageButton);

                clickables.Add(nameBoxCC);

                //this.labels.Add(this.nameLabel);
                labels.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 50, yPositionOnScreen + 480, 64, 64), "Page Number"));

                currentPageIndex = spellBook.GetCurrentSpellPageIndex();

                //string filePath = Path.Combine(modEntry.helper.DirectoryPath + "/Saves", $"{new DirectoryInfo(Game1.player.slotName)}_spellbook_data.json");

                string filePath = Path.Combine(modEntry.Helper.DirectoryPath + "/Saves", $"{Constants.SaveFolderName}_spellbook_data.json");

                if (File.Exists(filePath))
                {
                    Game1.player.GetSpellBook().SyncSpellBook(modEntry);
                }

                if (spellBook.GetCurrentSpellPage() != null)
                {
                    if (spellBook.GetCurrentSpellPage().GetSpellShapeAreas() != null && spellBook.GetCurrentSpellPage().GetSpellGrammerList() != null)
                    {
                        foreach (DragArea<SpellPartDraggable> area in dragAreas)
                        {
                            GetSpellPage(area, currentPageIndex);
                        }
                    }
                }

                SetDragged(null);
            }
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void draw(SpriteBatch spriteBatch)
        {
            if (spellBook == null)
                return;

            base.draw(spriteBatch);

            int pMouseX = Game1.getOldMouseX();
            int pMouseY = Game1.getOldMouseY();

            //int pMouseX = Game1.getMouseX();
            //int pMouseY = Game1.getMouseY();

            //pMouseX = Game1.getMouseX();
            //pMouseY = Game1.getMouseY();

            int x = xPositionOnScreen;
            int y = yPositionOnScreen;

            foreach (DragArea<SpellPartDraggable> area in dragAreas)
            {
                if (area != null)
                {
                    area.Draw(spriteBatch, x, y, 0);
                }
            }

            // draw labels
            foreach (DragArea<SpellPartDraggable> area in dragAreas)
            {
                Color color = Color.Violet;

                drawTextureBox(spriteBatch, area.bounds.X, area.bounds.Y - 50, area.name.Length + 280, 45, Color.White);
                Utility.drawTextWithShadow(spriteBatch, area.name, Game1.smallFont, new Vector2(area.bounds.X + 10, area.bounds.Y - 45), color);
            }

            foreach (DragArea<SpellPartDraggable> area in dragAreas)
            {
                string text = "";
                Color color = Game1.textColor;
                //Color color = Color.Purple;

                Utility.drawTextWithShadow(spriteBatch, area.name, Game1.smallFont, new Vector2(area.bounds.X + 10, area.bounds.Y - 45), color);
                //Utility.drawBoldText(spriteBatch, area.name, Game1.smallFont, new Vector2(area.bounds.X + 10, area.bounds.Y - 45), color);

                if (text.Length > 0)
                    Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, new Vector2(area.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, area.bounds.Y + Game1.tileSize / 2), color);
            }

            foreach (ClickableComponent label in labels)
            {
                string text = "";
                Color color = Game1.textColor;

                int pageNumber = spellBook.GetPages().IndexOf(spellBook.GetCurrentSpellPage()) + 1;

                drawTextureBox(spriteBatch, label.bounds.X, label.bounds.Y - 30, label.bounds.Width + 15, label.bounds.Height, Color.White);

                Utility.drawTextWithShadow(spriteBatch, pageNumber.ToString(), Game1.smallFont, new Vector2(label.bounds.X + 23, label.bounds.Y - 10), color);

                if (text.Length > 0)
                    Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, new Vector2(label.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, label.bounds.Y + Game1.tileSize / 2), color);
            }

            // draw buttons
            foreach (ClickableTextureComponent button in buttons)
                button.draw(spriteBatch);

            //draw textbox
            nameBox.Draw(spriteBatch);

            spellBook.GetCurrentSpellPage().SetName(nameBox.Text);

            string spellPartNameText = null;
            string spellPartDescriptionText = null;

            if (dragged != null)
            {
                dragged.Draw(spriteBatch, pMouseX - SpellPartDraggable.SIZE / 2, pMouseY - SpellPartDraggable.SIZE / 2, 0);
            }

            else
            {
                if (hoveredPart != null)
                {

                    spellPartNameText = hoveredPart.GetNameTranslationKey();
                    spellPartDescriptionText = hoveredPart.GetDescriptionTranslationKey();

                    int val1 = 272;
                    if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
                        val1 = 384;
                    if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
                        val1 = 336;

                    int value = Math.Max(val1, (int)Game1.dialogueFont.MeasureString(spellPartNameText == null ? "" : spellPartNameText).X);

                    if (spellPartNameText != null && spellPartDescriptionText != null)
                        drawToolTip(spriteBatch, Game1.parseText(spellPartDescriptionText, Game1.smallFont, value), spellPartNameText, null);
                }
            }

            // draw cursor
            drawMouse(spriteBatch);
        }

        public void SaveSpellBook(ModEntry modEntry)
        {
            if (spellBook == null)
                return;

            spellBook.Mutate(_ => spellBook.SaveSpellBook(modEntry));
        }

        public void GetSpellPage(DragArea<SpellPartDraggable> area, int spellPageIndex)
        {
            if (spellBook == null)
                return;

            if (area is ShapeGroupListArea)
            {
                int index = 0;

                foreach (ShapeGroupArea shapeGroupArea in ((ShapeGroupListArea)area).shapeGroups)
                {
                    //area.setAll(index, spellBook.pages[spellPageIndex].GetSpellShapes()[index].getAll());

                    if (spellBook.GetPages()[spellPageIndex].GetSpellShapeAreas() != null && spellBook.GetPages()[spellPageIndex].GetSpellShapeAreas().Length > 0)
                        shapeGroupArea.SetAll(spellBook.GetPages()[spellPageIndex].GetSpellShapeAreas()[index].GetAll());

                    index++;
                }
            }
            else if (area is SpellGrammarArea)
            {
                area.SetAll(spellBook.GetPages()[spellPageIndex].GetSpellGrammerList());
            }

            nameBox.Text = spellBook.GetPages()[spellPageIndex].GetName();
        }

        private void LogSavedSpellPages(SavedSpellPage[] spellPages)
        {
            modEntry.Monitor.Log("Logging saved spell pages for list " + spellPages, LogLevel.Info);

            for (int i = 0; i < spellPages.Length; i++)
            {
                SavedSpellPage page = spellPages[i];

                //modEntry.Monitor.Log("Listing saved shape group areas in spell page " + i, LogLevel.Info);

                for (int j = 0; j < page.GetSpellShapes().Length; j++)
                {
                    SavedShapeGroupArea<SpellPartDraggable> area = page.GetSpellShapes()[j];

                    modEntry.Monitor.Log("Listing spell parts in shape group area " + j, LogLevel.Info);

                    List<SpellPartDraggable> draggedShapes = page.GetSpellShapes()[j].GetAll();

                    foreach (SpellPartDraggable item in draggedShapes)
                    {
                        modEntry.Monitor.Log(item.GetPart().GetId(), LogLevel.Info);
                    }
                }
            }
        }

        private void LogSpellPages(List<SpellPage> spellPages)
        {
            modEntry.Monitor.Log("Logging spell pages for list " + spellPages, LogLevel.Info);

            for (int i = 0; i < spellPages.Count; i++)
            {
                SpellPage page = spellPages[i];

                //modEntry.Monitor.Log("Listing saved shape group areas in spell page " + i, LogLevel.Info);

                for (int j = 0; j < page.GetSpellShapeAreas().Length; j++)
                {
                    ShapeGroupArea area = page.GetSpellShapeAreas()[j];

                    modEntry.Monitor.Log("Listing spell parts in shape group area " + j, LogLevel.Info);

                    List<SpellPartDraggable> draggedShapes = page.GetSpellShapeAreas()[j].GetAll();

                    foreach (SpellPartDraggable item in draggedShapes)
                    {
                        modEntry.Monitor.Log(item.GetPart().GetId(), LogLevel.Info);
                    }
                }
            }
        }

        /// <summary>Handle a button click.</summary>
        /// <param name="name">The button name that was clicked.</param>
        private void HandleButtonClick(string name)
        {
            if (name == null || spellBook == null)
                return;

            switch (name)
            {
                case "LeftPage":
                    spellBook.SetCurrentSpellPageIndex(spellBook.GetCurrentSpellPageIndex() - 1);
                    spellBook.TurnToSpellPage();
                    SaveSpellBook(modEntry);
                    UpdateMenu();
                    break;
                case "RightPage":
                    spellBook.SetCurrentSpellPageIndex(spellBook.GetCurrentSpellPageIndex() + 1);
                    spellBook.TurnToSpellPage();
                    SaveSpellBook(modEntry);
                    UpdateMenu();
                    break;

                case "OK":

                    break;
            }

            Game1.playSound("grassyStep");
        }

        /// <summary>The method invoked when the player left-clicks on the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (spellBook == null)
                return;

            if (hoveredPart == null)
            {
                base.receiveLeftClick(x, y, playSound);
            }

            int mouseX = x;
            int mouseY = y;

            if (dragged == null)
            {
                if (hoveredArea != null && hoveredPart != null && hoveredArea.CanPick(hoveredPart, mouseX, mouseY))
                {
                    hoveredArea.Pick(hoveredPart, mouseX, mouseY);
                    SetDragged(hoveredPart);
                }

                foreach (ClickableTextureComponent button in buttons.ToList())
                {
                    if (button.containsPoint(mouseX, mouseY))
                    {
                        HandleButtonClick(button.name);
                        button.scale -= 0.5f;
                        button.scale = Math.Max(3.5f, button.scale);
                    }
                }
            }
            else if (dragged != null)
            {
                if (hoveredArea != null && hoveredArea.CanDrop(dragged, mouseX, mouseY))
                {
                    hoveredArea.Drop(dragged, mouseX, mouseY);
                    SetDragged(null);
                }
                else
                {
                    SetDragged(null);
                }
            }

            nameBox.Update();

            //modEntry.Monitor.Log("Mouse clicked at " + x + " , " + y, LogLevel.Info);
            SaveSpellBook(modEntry);
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
        }

        /// <summary>The method invoked when the player right-clicks on the lookup UI.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

        }

        /// <summary>The method invoked when the player hovers the cursor over the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        public override void performHoverAction(int x, int y)
        {
            if (spellBook == null)
                return;

            hoveredArea = GetHoveredArea(x, y);
            hoveredPart = GetHoveredElement(x, y);

            foreach (ClickableTextureComponent button in buttons)
                ChangeHoverActionScale(button, x, y, 0.01f, 0.1f);

            ChangeHoverActionScale(upperRightCloseButton, x, y, 0.01f, 0.1f);

            nameBox.Hover(x, y);

            //if (hoveredArea != null)
            //    modEntry.Monitor.Log("Mouse hovering over " + hoveredArea.name, LogLevel.Info);

            //if (hoveredPart != null)
            //    modEntry.Monitor.Log("Mouse hovering over spell part " + hoveredPart.getPart().GetId(), LogLevel.Info);
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key != 0)
            {
                if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && readyToClose())
                {
                    if (key != Keys.E)
                        exitThisMenu();
                }
                else if (Game1.options.snappyMenus && Game1.options.gamepadControls && !overrideSnappyMenuCursorMovementBan())
                {
                    applyMovementKey(key);
                }
            }
        }

        /// <summary>
        /// Changes Scale When A Component Is Hovered.
        /// </summary>
        /// <param name="component">Current Component</param>
        /// <param name="x">X Position Of The Mouse</param>
        /// <param name="y">Y Position Of The Mouse</param>
        /// <param name="min">The Minimum Scale</param>
        /// <param name="max">The Maximum Scale</param>
        public void ChangeHoverActionScale(ClickableTextureComponent component, int x, int y, float min, float max)
        {
            if (component.containsPoint(x, y))
                component.scale = Math.Min(component.scale + min, component.baseScale + max);
            else
                component.scale = Math.Max(component.scale - min, component.baseScale);
        }

        private void SetDragged(SpellPartDraggable dragged)
        {
            this.dragged = dragged;
            sourceArea?.SetTypeFilter(shapeGroupArea.CanStore(), spellGrammarArea.CanStore(), spellGrammarArea.GetAll().Any() && spellGrammarArea.GetAll()[0].GetPart().GetType() == SpellPartType.COMPONENT || shapeGroupArea.GetAll().Any() && shapeGroupArea.GetAll()[0].GetPart().GetType() == SpellPartType.SHAPE);
        }

        private DragArea<SpellPartDraggable> GetHoveredArea(int mouseX, int mouseY)
        {
            foreach (DragArea<SpellPartDraggable> area in dragAreas)
            {
                if (area.bounds.Contains(mouseX, mouseY))
                    return area;
            }

            return null;
        }

        private SpellPartDraggable GetHoveredElement(int mouseX, int mouseY)
        {
            DragArea<SpellPartDraggable> area = GetHoveredArea(mouseX, mouseY);
            return area == null ? null : area.ElementAt(mouseX, mouseY);
        }

        public int AllowedShapeGroups()
        {
            return 5;
        }

        private void OnPartDropped(SpellPartDraggable part)
        {
            ISpellPart spellPart = part.GetPart();

            //if (spellPart == AMSpellParts.COLOR.get())
            //{
            //    openColorPicker(part);
            //}
        }
    }
}
