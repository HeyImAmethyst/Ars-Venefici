using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.affinity
{
    /// <summary>
    /// Interface for affinity related helper methods.
    /// </summary>
    public interface IAffinityHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affinity">affinity The id of the affinity to get the essence stack for.</param>
        /// <returns>An item stack containing the affinity essence.</returns>
        Item GetEssenceForAffinity(string affinity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affinity">The affinity to get the essence stack for.</param>
        /// <returns>An item containing the affinity essence.</returns>
        Item GetEssenceForAffinity(Affinity affinity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affinity">The id of the affinity to get the tome stack for.</param>
        /// <returns>An item stack containing the affinity tome.</returns>
        Item GetTomeForAffinity(string affinity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affinity">The id of the affinity to get the tome stack for.</param>
        /// <returns>An item stack containing the affinity tome.</returns>
        Item GetTomeForAffinity(Affinity affinity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stardewValleyObject">The stardew valley object to make the item stack from.</param>
        /// <param name="affinity">The id of the affinity to set on the item stack.</param>
        /// <returns>An item stack of the given item with the given affinity stored in it.</returns>
        Item GetStackForAffinity<T>(T stardewValleyObject, string affinity) where T : StardewValley.Object, IAffinityObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stardewValleyObject">The stardew valley object to make the item stack from.</param>
        /// <param name="affinity">The affinity to set on the item stack.</param>
        /// <returns>An item stack of the given item with the given affinity stored in it.</returns>
        Item GetStackForAffinity<T>(T stardewValleyObject, Affinity affinity) where T : StardewValley.Object, IAffinityObject;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">The stardew valley object to get the affinity from.</param>
        /// <returns>The affinity stored in the stack, or the NONE affinity if the stack does not contain one.</returns>
        Affinity GetAffinityForItem(StardewValley.Object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to get the affinity depth for.</param>
        /// <param name="affinity">The id of the affinity to get the depth for.</param>
        /// <returns>The depth of the given player in the given affinity.</returns>
        double GetAffinityDepth(Farmer farmer, string affinity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to get the affinity depth for.</param>
        /// <param name="affinity">The affinity to get the depth for.</param>
        /// <returns>The depth of the given player in the given affinity.</returns>
        double GetAffinityDepth(Farmer farmer, Affinity affinity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to get the affinity depth for.</param>
        /// <param name="affinity">The affinity to get the depth for.</param>
        /// <param name="defaultValue">The default value that will be returned if the affinity depth cannot be retrieved.</param>
        /// <returns>The depth of the given player in the given affinity, or a default value if the affinity depth cannot be determined.</returns>
        double GetAffinityDepthOrElse(Farmer farmer, Affinity affinity, double defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to get the affinity depth for.</param>
        /// <param name="affinity">The id of the affinity to get the depth for.</param>
        /// <param name="defaultValue">The default value that will be returned if the affinity depth cannot be retrieved.</param>
        /// <returns>The depth of the given player in the given affinity, or a default value if the affinity depth cannot be determined.</returns>
        double GetAffinityDepthOrElse(Farmer farmer, string affinity, double defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to set the affinity depth for.</param>
        /// <param name="affinity">The id of the affinity to set the depth for.</param>
        /// <param name="amount">The amount the affinity should have.</param>
        void SetAffinityDepth(Farmer farmer, string affinity, float amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to set the affinity depth for.</param>
        /// <param name="affinity">The affinity to set the depth for.</param>
        /// <param name="amount">The amount the affinity should have.</param>
        void SetAffinityDepth(Farmer farmer, Affinity affinity, float amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to add the affinity depth for.</param>
        /// <param name="affinity">The id of the affinity to add the depth for.</param>
        /// <param name="amount">The amount to add.</param>
        void IncreaseAffinityDepth(Farmer farmer, string affinity, float amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to add the affinity depth for.</param>
        /// <param name="affinity">The affinity to add the depth for.</param>
        /// <param name="amount">The amount to add.</param>
        void IncreaseAffinityDepth(Farmer farmer, Affinity affinity, float amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to subtract the affinity depth for.</param>
        /// <param name="affinity">The id of the affinity to subtract the depth for.</param>
        /// <param name="amount">The amount to subtract.</param>
        void DecreaseAffinityDepth(Farmer farmer, string affinity, float amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farmer">The farmer to subtract the affinity depth for.</param>
        /// <param name="affinity">The affinity to subtract the depth for.</param>
        /// <param name="amount">The amount to subtract.</param>
        void DecreaseAffinityDepth(Farmer farmer, Affinity affinity, float amount);

        /// <summary>
        /// Applies the affinity shift for the given farmer and affinity.
        /// </summary>
        /// <param name="farmer">The player to shift the affinity for.</param>
        /// <param name="affinity">The affinity to shift.</param>
        /// <param name="shift">The amount to shift.</param>
        void ApplyAffinityShift(Farmer farmer, string affinity, float shift);

        /// <summary>
        /// Applies the affinity shift for the given farmer and affinity.
        /// </summary>
        /// <param name="farmer">The player to shift the affinity for.</param>
        /// <param name="affinity">The affinity to shift.</param>
        /// <param name="shift">The amount to shift.</param>
        void ApplyAffinityShift(Farmer farmer, Affinity affinity, float shift);

        /// <summary>
        /// Locks the player's affinities.
        /// </summary>
        /// <param name="farmer"> The farmer to lock the affinities for</param>
        void Lock(Farmer farmer);

        /// <summary>
        /// Unlocks the player's affinities.
        /// </summary>
        /// <param name="farmer">The player to unlock the affinities for.</param>
        void Unlock(Farmer farmer);

        /// <summary>
        /// Locks or unlocks the player's affinities, depending on whether an affinity is at 1.0 or not.
        /// </summary>
        /// <param name="farmer">The player to lock or unlock the affinities for.</param>
        void UpdateLock(Farmer farmer);

        void UpdateIfNeeded(ModEntry modEntry, Farmer player);

        void SyncToPlayer(Farmer player);
    }
}
