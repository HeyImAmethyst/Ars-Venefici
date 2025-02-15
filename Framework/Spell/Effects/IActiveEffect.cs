﻿using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell.Effects
{
    /// <summary>An active spell, projectile, or effect which should be updated or drawn.</summary>
    public interface IActiveEffect
    {
        /*********
        ** Methods
        *********/
        /// <summary>Update the effect state if needed.</summary>
        /// <param name="e">The update tick event args.</param>
        /// <returns>Returns true if the effect is still active, or false if it can be discarded.</returns>
        void Update(UpdateTickedEventArgs e);

        void OneSecondUpdate(OneSecondUpdateTickingEventArgs e);

        /// <summary>Draw the effect to the screen if needed.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
