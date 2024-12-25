using StardewModdingAPI.Events;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Menus;
using ArsVenefici.Framework.Spell;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Emit;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Spell.Shape;
using SpaceCore;
using SpaceShared.APIs;
using ArsVenefici.Framework.GUI.Menus;
using ArsVenefici.Framework.Spell.Effects;
using StardewValley.Network;
using ArsVenefici.Framework.Spell.Components;
using static ArsVenefici.ModConfig;
using ArsVenefici.Framework.GameSave;
using Microsoft.Xna.Framework.Audio;
using ArsVenefici.Framework.Skill;
using StardewValley.ItemTypeDefinitions;
using ArsVenefici.Framework.Magic;
using static StardewValley.Minigames.TargetGame;
using ArsVenefici.Framework.Interfaces.Magic;

namespace ArsVenefici.Framework.Events
{
    public class Events
    {
        ModEntry modEntryInstance;
        public Events(ModEntry modEntry, DailyTracker dailyTracker)
        {
            modEntryInstance = modEntry;
        }

        
    }
}
