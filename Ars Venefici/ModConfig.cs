using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici
{
    public class ModConfig
    {
        public class ModKeyBinds
        {
            /// <summary>A list of keybinds to open the spell book.</summary>
            public KeybindList OpenSpellBookButtons { get; set; }

            /// <summary>A list of keybinds to toggle spell casting on or off.</summary>
            public KeybindList SpellToggles { get; set; }

            /// <summary> A list of keybinds to move the spell lable.</summary>
            public KeybindList MoveSpellLabelButtons { get; set; }

            /// <summary>A list of keybinds to move to the next spell. Press Shift and this button to move to the next shape group on the selected spell.</summary>
            public KeybindList NextSpellButtons { get; set; }

            /// <summary>A list of keybinds to move to the next shape group on the selected spell.</summary>
            public KeybindList NextShapeGroupButtons { get; set; }

            /// <summary>A list of keybinds to move to the previous spell.</summary>
            public KeybindList PreviousSpellButtons { get; set; }

            /// <summary>A list of keybinds to move to the previous shape group on the selected spell.</summary>
            public KeybindList PreviousShapeGroupButtons { get; set; }

            /// <summary>A list of keybinds to cast the current spell.</summary>
            public KeybindList CastSpellButtons { get; set; }

            /// <summary>A list of keybinds to open a menu showing the text from the tutorial.</summary>
            public KeybindList OpenTutorialTextButtons { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 1st page of the spell book.</summary>
            public KeybindList CastSpellPage1 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 2nd page of the spell book.</summary>
            public KeybindList CastSpellPage2 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 3rd page of the spell book.</summary>
            public KeybindList CastSpellPage3 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 4th page of the spell book.</summary>
            public KeybindList CastSpellPage4 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 5th page of the spell book.</summary>
            public KeybindList CastSpellPage5 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 6th page of the spell book.</summary>
            public KeybindList CastSpellPage6 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 7th page of the spell book.</summary>
            public KeybindList CastSpellPage7 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 8th page of the spell book.</summary>
            public KeybindList CastSpellPage8 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 9th page of the spell book.</summary>
            public KeybindList CastSpellPage9 { get; set; }

            /// <summary>A list of keybinds to cast the spell in the 10th page of the spell book.</summary>
            public KeybindList CastSpellPage10 { get; set; }
        }

        public class KeyBoardKeyBinds
        {
            public ModKeyBinds modKeyBinds;

            public KeyBoardKeyBinds() 
            {
                modKeyBinds = new ModKeyBinds();

                modKeyBinds.OpenSpellBookButtons = new KeybindList(new Keybind(SButton.LeftShift, SButton.B));
                modKeyBinds.SpellToggles = new KeybindList(new Keybind(SButton.OemTilde));
                modKeyBinds.MoveSpellLabelButtons = new KeybindList(new Keybind(SButton.LeftShift, SButton.OemTilde));
                modKeyBinds.NextSpellButtons = new KeybindList(new Keybind(SButton.X));
                modKeyBinds.NextShapeGroupButtons = new KeybindList(new Keybind(SButton.LeftShift, SButton.X));
                modKeyBinds.PreviousSpellButtons = new KeybindList(new Keybind(SButton.Z));
                modKeyBinds.PreviousShapeGroupButtons = new KeybindList(new Keybind(SButton.LeftShift, SButton.Z));
                modKeyBinds.CastSpellButtons = new KeybindList(new Keybind(SButton.Q));
                modKeyBinds.OpenTutorialTextButtons = new KeybindList(new Keybind(SButton.G));

                modKeyBinds.CastSpellPage1 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage2 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage3 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage4 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage5 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage6 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage7 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage8 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage9 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage10 = new KeybindList(new Keybind());
            }
        }

        public class ControllerKeyBinds
        {
            public ModKeyBinds modKeyBinds;

            public ControllerKeyBinds()
            {
                modKeyBinds = new ModKeyBinds();

                modKeyBinds.OpenSpellBookButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.ControllerB));
                modKeyBinds.SpellToggles = new KeybindList(new Keybind(SButton.LeftStick));
                modKeyBinds.MoveSpellLabelButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.DPadUp));
                modKeyBinds.NextSpellButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.ControllerX));
                modKeyBinds.NextShapeGroupButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.RightShoulder));
                modKeyBinds.PreviousSpellButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.ControllerY));
                modKeyBinds.PreviousShapeGroupButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.LeftShoulder));
                modKeyBinds.CastSpellButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.RightTrigger));
                modKeyBinds.OpenTutorialTextButtons = new KeybindList(new Keybind(SButton.LeftStick, SButton.LeftTrigger));

                modKeyBinds.CastSpellPage1 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage2 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage3 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage4 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage5 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage6 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage7 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage8 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage9 = new KeybindList(new Keybind());
                modKeyBinds.CastSpellPage10 = new KeybindList(new Keybind());
            }
        }

        /// <summary>The pixel position at which to draw the spell lable, relative to the top-left corner of the screen.</summary>
        public Point Position { get; set; } = new(574, 896);

        public KeyBoardKeyBinds keyBoardKeyBinds;
        public ControllerKeyBinds controllerKeyBinds;

        public ModConfig()
        {
            keyBoardKeyBinds = new KeyBoardKeyBinds();
            controllerKeyBinds = new ControllerKeyBinds();
        }
    }
}
