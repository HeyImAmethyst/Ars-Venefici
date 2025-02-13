using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Interfaces.Spells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.DragNDrop
{
    public class SpellGrammarArea : DragTargetArea<SpellPartDraggable>
    {
        private static int X_PADDING = 4;
        private Action<SpellPartDraggable, int> onDropAction;

        SpellBookMenu spellBookMenu;

        public SpellGrammarArea(Rectangle bounds, Action<SpellPartDraggable, int> onDrop, string name, SpellBookMenu spellBookMenu) : base(bounds, 8, name)
        {
            onDropAction = onDrop;

            this.spellBookMenu = spellBookMenu;
        }

        public SpellGrammarArea(Rectangle bounds, Action<SpellPartDraggable, int> onDrop, string name, string lable, SpellBookMenu spellBookMenu) : base(bounds, 8, name, lable)
        {
            onDropAction = onDrop;

            this.spellBookMenu = spellBookMenu;
        }

        public override SpellPartDraggable ElementAt(int mouseX, int mouseY)
        {
            if (mouseX < x + X_PADDING || mouseX >= x + maxElements * SpellPartDraggable.SIZE + X_PADDING || mouseY < y || mouseY >= y + SpellPartDraggable.SIZE)
                return null;

            int index = (mouseX - x - X_PADDING) / SpellPartDraggable.SIZE;

            return contents.Count() > index ? contents[index] : null;
        }

        public override bool CanPick(SpellPartDraggable draggable, int mouseX, int mouseY)
        {
            return contents.Count() < 2 || draggable.GetPart().GetType() == SpellPartType.MODIFIER || contents[0].GetPart() != draggable.GetPart() || contents[1].GetPart().GetType() != SpellPartType.MODIFIER;
        }

        public override bool CanDrop(SpellPartDraggable draggable, int mouseX, int mouseY)
        {
            return CanStore() && (draggable.GetPart().GetType() == SpellPartType.COMPONENT || draggable.GetPart().GetType() == SpellPartType.MODIFIER && contents.Any() && contents[0].GetPart().GetType() == SpellPartType.COMPONENT);
            //return canStore();
        }

        public override void Draw(SpriteBatch spriteBatch, int positionX, int positionY, float pPartialTick)
        {
            IClickableMenu.drawTextureBox(spriteBatch, x, y, width, height, Color.White);

            Rectangle labelRect = new Rectangle(bounds.X - 5, bounds.Y - 53, name.Length + 280, 45);
            Vector2 mousePos = new Vector2(Game1.getMouseX(), Game1.getMouseY());

            for (int i = 0; i < contents.Count(); i++)
            {
                contents[i].Draw(spriteBatch, x + i * SpellPartDraggable.SIZE + X_PADDING, y, pPartialTick);
            }

            if (labelRect.Contains(mousePos))
            {
                string spellGrammarAreaDescription = ModEntry.INSTANCE.Helper.Translation.Get("ui.spell_book.spell_grammar_area.description");


                int val1 = 272;
                if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
                    val1 = 384;
                if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
                    val1 = 336;

                int value = Math.Max(val1, (int)Game1.dialogueFont.MeasureString(spellGrammarAreaDescription == null ? "" : spellGrammarAreaDescription).X);

                string parsedText = Game1.parseText(spellGrammarAreaDescription, Game1.smallFont, value);

                //IClickableMenu.drawHoverText(spriteBatch, parsedText, Game1.smallFont);

                IClickableMenu.drawTextureBox(spriteBatch, spellBookMenu.xPositionOnScreen - 520, spellBookMenu.yPositionOnScreen + 200, 270, 255, Color.White);
                Utility.drawTextWithShadow(spriteBatch, parsedText, Game1.smallFont, new Vector2(spellBookMenu.xPositionOnScreen - 500, spellBookMenu.yPositionOnScreen + 230), Game1.textColor);
            }
        }

        public override void OnDrop(SpellPartDraggable draggable, int index)
        {
            onDropAction.Invoke(draggable, index);
        }
    }
}
