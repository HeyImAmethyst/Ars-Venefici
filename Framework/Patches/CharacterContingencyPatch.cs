using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ArsVenefici.Framework.Magic;
using static StardewValley.Minigames.TargetGame;
using ArsVenefici.Framework.Interfaces.Magic;
using StardewValley.Monsters;
using ArsVenefici.Framework.Events;

namespace ArsVenefici.Framework.Patches
{
    public class CharacterContingencyPatch
    {

        // Instance of ModEntry
        private static ModEntry modEntryInstance;



        /// <summary>
        /// FarmerContingencyPatch Constructor
        /// </summary>
        /// <param name="entry">The instance of ModEntry</param>
        public CharacterContingencyPatch(ModEntry entry)
        {
            // Set the field
            modEntryInstance = entry;
        }

        internal void Apply(Harmony harmony)
        {
            modEntryInstance.Monitor.Log("Patching character", LogLevel.Info);

            harmony.Patch(
                 original: AccessTools.Method(typeof(Farmer), nameof(Farmer.takeDamage)),
                 prefix: new HarmonyMethod(GetType(), nameof(FarmerTakeDamagePrefix))
             );

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.takeDamage)),
                postfix: new HarmonyMethod(GetType(), nameof(FarmerTakeDamagePostfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.doneEating)),
                prefix: new HarmonyMethod(GetType(), nameof(FarmerDoneEatingPrefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.doneEating)),
                postfix: new HarmonyMethod(GetType(), nameof(FarmerDoneEatingPostfix))
            );

            harmony.Patch(AccessTools.Method(typeof(Monster), nameof(Monster.takeDamage), new[] { typeof(int), typeof(int), typeof(int), typeof(bool), typeof(double), typeof(Farmer) }),
                prefix: new HarmonyMethod(GetType(), nameof(MonsterTakeDamagePrefix)));

            harmony.Patch(AccessTools.Method(typeof(Monster), nameof(Monster.takeDamage), new[] { typeof(int), typeof(int), typeof(int), typeof(bool), typeof(double), typeof(Farmer) }),
                postfix: new HarmonyMethod(GetType(), nameof(MonsterTakeDamagePostfix)));
        }

        private static void MonsterTakeDamagePrefix(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, Farmer who, Monster __instance, out int __state)
        {
            __state = __instance.Health;
        }

        private static void MonsterTakeDamagePostfix(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, Farmer who, Monster __instance, int __state)
        {
            if (__instance.Health < __state)
                modEntryInstance.characterEvents.InvokeOnCharacterDamage(__instance);
        }

        private static void FarmerTakeDamagePrefix(int damage, bool overrideParry, Monster damager, Farmer __instance, out int __state)
        {
            __state = __instance.health;
        }

        private static void FarmerTakeDamagePostfix(int damage, bool overrideParry, Monster damager, Farmer __instance, int __state)
        {
            if (__instance.health < __state)
                modEntryInstance.characterEvents.InvokeOnCharacterDamage(__instance);
        }

        private static void FarmerDoneEatingPrefix(Farmer __instance, out int __state)
        {
            __state = __instance.health;
        }

        private static void FarmerDoneEatingPostfix(Farmer __instance, int __state)
        {
            StardewValley.Object @object = __instance.itemToEat as StardewValley.Object;

            if(@object.healthRecoveredOnConsumption() != 0 && __state < __instance.health)
                modEntryInstance.characterEvents.InvokeOnCharacterHeal(__instance);
        }
    }
}
