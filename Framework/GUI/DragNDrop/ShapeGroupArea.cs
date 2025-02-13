using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.DragNDrop
{
    public class ShapeGroupArea : DragTargetArea<SpellPartDraggable>
    {
        [JsonIgnore]
        private static int ROWS = 2;

        [JsonIgnore]
        private static int COLUMNS = 2;

        [JsonIgnore]
        private static int X_PADDING = 2;

        [JsonIgnore]
        private static int Y_PADDING = 1;

        [JsonIgnore]
        public static int WIDTH = 128;

        [JsonIgnore]
        public static int HEIGHT = 128;

        [JsonIgnore]
        private Action<SpellPartDraggable, int> onDropAction;

        private LockState lockState = LockState.NONE;

        SpellBookMenu spellBookMenu;

        public ShapeGroupArea(int x, int y, Action<SpellPartDraggable, int> onDrop, string name, SpellBookMenu spellBookMenu) : base(new Rectangle(x, y, WIDTH, HEIGHT), ROWS * COLUMNS, name)
        {
            onDropAction = onDrop;
            this.spellBookMenu = spellBookMenu;
        }

        public ShapeGroupArea(int x, int y, Action<SpellPartDraggable, int> onDrop, string name, string lable, SpellBookMenu spellBookMenu) : base(new Rectangle(x, y, WIDTH, HEIGHT), ROWS * COLUMNS, name, lable)
        {
            onDropAction = onDrop;
            this.spellBookMenu = spellBookMenu;
        }

        public void SetLockState(LockState lockState)
        {
            this.lockState = lockState;
        }

        public override SpellPartDraggable ElementAt(int mouseX, int mouseY)
        {
            mouseX -= x;
            mouseY -= y;
            mouseX -= X_PADDING;
            mouseY -= Y_PADDING;
            if (mouseX < 0 || mouseX >= ROWS * SpellPartDraggable.SIZE || mouseY < 0 || mouseY >= COLUMNS * SpellPartDraggable.SIZE) return null;
            int index = 0;
            index += mouseX / SpellPartDraggable.SIZE;
            index += mouseY / SpellPartDraggable.SIZE * COLUMNS;

            return contents.Count() > index ? contents[index] : null;
        }

        public override bool CanPick(SpellPartDraggable draggable, int mouseX, int mouseY)
        {
            if (lockState == LockState.ALL)
                return false;

            //if (lockState == LockState.FIRST && contents.Any() &&  contents[0].getPart() == draggable.getPart()) 
            //    return false;

            if (lockState == LockState.FIRST && !(contents.Count() == 0) && contents[0].GetPart() == draggable.GetPart())
                return false;

            List<SpellPartDraggable> list = new List<SpellPartDraggable>(contents);
            list.Remove(draggable);

            return IsValid(list);
        }

        public override void Draw(SpriteBatch spriteBatch, int positionX, int positionY, float pPartialTick)
        {
            Rectangle labelRect = new Rectangle(bounds.X - 5, bounds.Y - 53, name.Length + 280, 45);
            Vector2 mousePos = new Vector2(Game1.getMouseX(), Game1.getMouseY());
            if (lockState == LockState.ALL)
            {
                IClickableMenu.drawTextureBox(spriteBatch, x, y, WIDTH, HEIGHT, Color.Gray);
            }
            else
            {
                IClickableMenu.drawTextureBox(spriteBatch, x, y, WIDTH, HEIGHT, Color.White);
            }

            // drawTextureBox(spriteBatch, area.bounds.X - 5, area.bounds.Y - 53, area.name.Length + 280, 45, Color.White);

            if (labelRect.Contains(mousePos))
            {
                string shapeGroupAreaDescription = ModEntry.INSTANCE.Helper.Translation.Get("ui.spell_book.shape_group_area.description");

                int val1 = 272;
                if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
                    val1 = 384;
                if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
                    val1 = 336;

                int value = Math.Max(val1, (int)Game1.dialogueFont.MeasureString(shapeGroupAreaDescription == null ? "" : shapeGroupAreaDescription).X);

                string parsedText = Game1.parseText(shapeGroupAreaDescription, Game1.smallFont, value);

                //IClickableMenu.drawHoverText(spriteBatch, parsedText, Game1.smallFont);

                IClickableMenu.drawTextureBox(spriteBatch, spellBookMenu.xPositionOnScreen - 520, spellBookMenu.yPositionOnScreen + 200, 270, 375, Color.White);
                Utility.drawTextWithShadow(spriteBatch, parsedText, Game1.smallFont, new Vector2(spellBookMenu.xPositionOnScreen - 500, spellBookMenu.yPositionOnScreen + 230), Game1.textColor);
            }

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    int index = i * COLUMNS + j;

                    if (index >= contents.Count())
                        return;

                    contents[index].Draw(spriteBatch, x + j * SpellPartDraggable.SIZE + X_PADDING, y + i * SpellPartDraggable.SIZE + Y_PADDING, pPartialTick);
                }

            }
        }

        public override bool CanDrop(SpellPartDraggable draggable, int mouseX, int mouseY)
        {
            if (lockState == LockState.ALL)
                return false;

            if (!CanStore() || draggable.GetPart().GetType() == SpellPartType.COMPONENT)
                return false;

            List<SpellPartDraggable> list = new List<SpellPartDraggable>(contents);
            list.Add(draggable);

            return IsValid(list);
        }

        public static bool IsValid(List<SpellPartDraggable> list)
        {
            if (!list.Any())
                return true;

            SpellPartDraggable first = list[0];

            if (first.GetPart().GetType() != SpellPartType.SHAPE)
                return false;

            if (((ISpellShape)first.GetPart()).NeedsPrecedingShape())
                return false;

            Predicate<SpellPartDraggable> p = IsShape;

            SpellPartDraggable last = Utils.GetLastMatching(list, p);

            if (last != null)
            {
                for (int i = 1; i < list.Count(); i++)
                {
                    SpellPartDraggable part = list[i];
                    if (part.GetPart().GetType() == SpellPartType.MODIFIER)
                        continue;

                    if (((ISpellShape)part.GetPart()).NeedsToComeFirst())
                        return false;

                    if (part != last && ((ISpellShape)part.GetPart()).IsEndShape())
                        return false;
                }
            }

            return true;
        }

        private static bool IsShape(SpellPartDraggable e)
        {
            return e.GetPart().GetType() == SpellPartType.SHAPE;
        }

        public enum LockState
        {
            NONE, FIRST, ALL
        }
    }
}
