using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI.Menus
{

    public abstract class ModdedClickableMenu : IClickableMenu
    {

        public bool gamePadMoveWithRightStickEnabled = false;

        private bool Dragging;

        /// <summary>The callback to invoke when the birthday value changes.</summary>
        private Action<string, int> OnChanged;

        public ModdedClickableMenu(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
            : base(x, y, width, height, showUpperRightCloseButton)
        {
            UpdateMenu();
        }

        public ModdedClickableMenu(int x, int y, int width, int height, Action<string, int> onChanged, bool showUpperRightCloseButton = false)
            : base(x, y, width, height, showUpperRightCloseButton)
        {
            UpdateMenu(onChanged);
        }

        public virtual void UpdateMenu(Action<string, int> OnChanged)
        {
            this.OnChanged = OnChanged;
            SetUpPositions();
        }


        public virtual void UpdateMenu()
        {
            SetUpPositions();
        }

        /// <summary>Regenerate the UI.</summary>
        protected abstract void SetUpPositions();

        public new abstract void populateClickableComponentList();

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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //              Gamepad controlls here           //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        public override void gamePadButtonHeld(Buttons b)
        {
            //if (b.Equals(Buttons.A))
            //{
            //    leftClickHeld(Game1.getMouseX(), Game1.getMouseY());
            //}
        }

        public override void receiveGamePadButton(Buttons b)
        {
            if (b.Equals(Buttons.A))
            {
                performHoverAction(Game1.getMouseX(), Game1.getMouseY());
                receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
                releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
            }
            else if (b.Equals(Buttons.DPadUp))
            {
                setDragging(!Dragging);
            }
        }

        public override void update(GameTime time)
        {
            performHoverAction(Game1.getMouseX(), Game1.getMouseY());

            base.update(time);
        }

        public override bool areGamePadControlsImplemented()
        {
            return true;
        }

        /// <summary>
        /// Make this true if free cursor movement is desired.
        /// </summary>
        /// <returns></returns>
        public override bool overrideSnappyMenuCursorMovementBan()
        {
            return true;
        }

        public override void applyMovementKey(int direction)
        {
            if (allClickableComponents == null || allClickableComponents.Count == 0)
                populateClickableComponentList();

            ClickableComponent old = currentlySnappedComponent;

            base.applyMovementKey(direction);

            if (currentlySnappedComponent != null && currentlySnappedComponent != old)
            {
                currentlySnappedComponent.snapMouseCursorToCenter();
            }
        }

        public override void setCurrentlySnappedComponentTo(int id)
        {
            base.setCurrentlySnappedComponentTo(id);
            if (currentlySnappedComponent != null)
            {
                this.currentlySnappedComponent.snapMouseCursorToCenter();
            }
        }

        public virtual void setDragging(bool dragging)
        {
            this.Dragging = dragging;
        }

        public virtual bool getDragging()
        {
            return this.Dragging;
        }
    }
}
