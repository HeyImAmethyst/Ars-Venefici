using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.ability;
using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Monsters;
using System.Threading;
using static ArsVenefici.Framework.Spells.affinity.AffinityHelper;

namespace ArsVenefici.Framework.Patches
{
    public class CharacterPatch
    {

        // Instance of ModEntry
        private static ModEntry modEntryInstance;

        /// <summary>
        /// FarmerContingencyPatch Constructor
        /// </summary>
        /// <param name="entry">The instance of ModEntry</param>
        public CharacterPatch(ModEntry entry)
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

        //private static void MonsterConstuctorPrefix()
        //{

        //}

        private static void MonsterTakeDamagePrefix(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, Farmer who, Monster __instance, out int __state)
        {
            __state = __instance.Health;

            var api = modEntryInstance.arsVeneficiAPILoader.GetAPI();

            if (api.GetMagicHelper().LearnedWizardy(Game1.player))
            {

                Ability ability = Abilities.SMITE.Get();

                if (ability != null && ability.Test(who))
                {
                    if (__instance.Name.Equals("Skeleton") ||
                        __instance.Name.Equals("Mummy") ||
                        __instance.Name.Equals("Skeleton Mage"))
                    {
                        var affinityHelper = api.GetAffinityHelper();
                        damage = (int)(damage + affinityHelper.GetAffinityDepthOrElse(who, ability.affinity, 0) * 4);
                    }
                }
            }
        }

        private static void MonsterTakeDamagePostfix(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, Farmer who, Monster __instance, int __state)
        {
            if (__instance.Health < __state)
                modEntryInstance.characterEvents.InvokeOnCharacterDamage(__instance);
        }

        private static void FarmerTakeDamagePrefix(int damage, bool overrideParry, Monster damager, Farmer __instance, out int __state)
        {
            __state = __instance.health;

            if (__instance.hasBuff("HeyImAmethyst.ArsVenifici_Shield") == true)
                __instance.temporarilyInvincible = true;

            var api = modEntryInstance.arsVeneficiAPILoader.GetAPI();
            var affinityHelper = api.GetAffinityHelper();

            if (api.GetMagicHelper().LearnedWizardy(Game1.player))
            {

                Ability ability = Abilities.RESISTANCE.Get();

                if (ability != null && ability.Test(__instance))
                {
                    damage = (int)(damage * (1 - affinityHelper.GetAffinityDepthOrElse(__instance, ability.affinity, 0) / 2));
                }

                ability = Abilities.FIRE_RESISTANCE.Get();

                if (ability != null && ability.Test(__instance) && damager.GetAffinity().id == Affinity.FIRE)
                {
                    damage = (int)(damage * (1 - affinityHelper.GetAffinityDepthOrElse(__instance, ability.affinity, 0) / 2));
                }

                ability = Abilities.MAGIC_DAMAGE.Get();

                if (ability != null && ability.Test(__instance) && damager.GetAffinity().id == Affinity.ARCANE)
                {
                    damage = (int)(damage * (1 + affinityHelper.GetAffinityDepthOrElse(__instance, ability.affinity, 0) / 2));
                }
            }
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
