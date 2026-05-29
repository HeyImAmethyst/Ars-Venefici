using ArsVenefici.Framework.API.affinity;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.Spell
{
    /// <summary>
    /// Base interface for a spell part. A spell part can be a component of type ISpellComponent, a modifier of type ISpellModifier or a shape of type ISpellShape.
    /// </summary>
    public interface ISpellPart
    {

        /// <returns>The type of this spell part.</returns>
        SpellPartType GetType();

        //MagicType GetMagicType();

        string GetId();

        float ManaCost();

        string DisplayName();

        string DisplayDiscription();

        //Affinity GetAffinity();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The affinities for this spell part.</returns>
        HashSet<Affinity> GetAffinities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The affinity shifts for this spell part.</returns>
        Dictionary<Affinity, float> GetAffinityShifts();
    }


    /// <summary>
    /// The types of the spell parts.
    /// </summary>
    public enum SpellPartType
    {
        SHAPE, COMPONENT, MODIFIER
    }
}
