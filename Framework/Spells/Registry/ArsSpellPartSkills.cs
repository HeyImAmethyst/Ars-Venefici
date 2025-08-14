using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using ItemExtensions.Models.Enums;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Registry
{
    public class ArsSpellPartSkills
    {
        public static MagicAltarTab offenceTab = offenceTab = MagicAltarTab.create(ModEntry.INSTANCE.Helper.Translation.Get("ui.magic_altar.offense_tab.name"), ModEntry.INSTANCE.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/offense.png"), ModEntry.INSTANCE.Helper.ModContent.Load<Texture2D>("assets/icon/interface/offense.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 200, 50, 0); //226 //46
        public static MagicAltarTab defenseTab = defenseTab = MagicAltarTab.create(ModEntry.INSTANCE.Helper.Translation.Get("ui.magic_altar.defense_tab.name"), ModEntry.INSTANCE.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/defense.png"), ModEntry.INSTANCE.Helper.ModContent.Load<Texture2D>("assets/icon/interface/defense.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 400, 50, 1); //181
        public static MagicAltarTab utilityTab = utilityTab = MagicAltarTab.create(ModEntry.INSTANCE.Helper.Translation.Get("ui.magic_altar.utility_tab.name"), ModEntry.INSTANCE.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/utility.png"), ModEntry.INSTANCE.Helper.ModContent.Load<Texture2D>("assets/icon/interface/utility.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 400, 50, 2); //136

        public static ObjectRegister<SpellPartSkill> SKILLS = ObjectRegister<SpellPartSkill>.Create(ModEntry.ArsVenificiModId);
        
        //Offense

        public static ObjectHolder<SpellPartSkill> PROJECTILE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("projectile", new HashSet<SpellPartSkill>(), new Dictionary<Item, int> { { ItemRegistry.Create("(W)32"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Tarma_Root"), 1 } }, offenceTab, false)));
        
        public static ObjectHolder<SpellPartSkill> PHYSICAL_DAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("physical_damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 } }, offenceTab, false)));

        public static ObjectHolder<SpellPartSkill> BOUNCE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("bounce", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)766"), 20 } }, offenceTab, false)));

        public static ObjectHolder<SpellPartSkill> VELOCITY = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("velocity", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)395"), 1 } }, offenceTab, false)));

        public static ObjectHolder<SpellPartSkill> AOE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("aoe", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)621"), 1 } }, offenceTab, false)));
        public static ObjectHolder<SpellPartSkill> BEAM = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("beam", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)32"), 1 }, { ItemRegistry.Create("(O)74"), 5 } }, offenceTab, false)));
        public static ObjectHolder<SpellPartSkill> DAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)848"), 5 } }, offenceTab, false))); //210, 165

        public static ObjectHolder<SpellPartSkill> EXPLOSION = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("explosion", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)287"), 3 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Desert_Nova"), 2 } }, offenceTab, false))); //210, 300

        public static ObjectHolder<SpellPartSkill> WAVE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("wave", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Tarma_Root"), 1 } }, offenceTab, false))); //210, 210

        public static ObjectHolder<SpellPartSkill> MAGICDAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("magic_damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(O)769"), 1 }, { ItemRegistry.Create("(O)768"), 1 } }, offenceTab, false)));
        public static ObjectHolder<SpellPartSkill> FROSTDAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("frost_damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(O)84"), 1 } }, offenceTab, false)));
        public static ObjectHolder<SpellPartSkill> LIGHTNINGDAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("lightning_damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(BC)9"), 1 } }, offenceTab, false)));
        public static ObjectHolder<SpellPartSkill> FIREDAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("fire_damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(O)82"), 1 } }, offenceTab, false)));

        public static ObjectHolder<SpellPartSkill> PIERCING = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("piercing", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)691"), 5 } }, offenceTab, false)));
        public static ObjectHolder<SpellPartSkill> FORGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("forge", new Dictionary<Item, int>() { { ItemRegistry.Create("(BC)13"), 5 } }, offenceTab, false)));

        public static ObjectHolder<SpellPartSkill> RUNE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("rune", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)405"), 5 } }, offenceTab, false)));

        public static ObjectHolder<SpellPartSkill> CONE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("cone", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)405"), 1 }, { ItemRegistry.Create("(O)311"), 3 } }, offenceTab, false)));

        //Defense

        public static ObjectHolder<SpellPartSkill> SELF = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("self", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>() { { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Vinteum_Dust"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 } }, defenseTab, false)));

        public static ObjectHolder<SpellPartSkill> EFFECT_POWER = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("effect_power", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)787"), 1 }, { ItemRegistry.Create("(O)395"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Desert_Nova"), 1 } }, defenseTab, false)));

        public static ObjectHolder<SpellPartSkill> HASTE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("haste", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)395"), 1 } }, defenseTab, false)));
        public static ObjectHolder<SpellPartSkill> REGENERATION = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("regeneration", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false)));
        public static ObjectHolder<SpellPartSkill> MANA_REGENERATION = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("mana_regeneration", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)" + ModEntry.ArsVenificiContentPatcherId + "_Mana_Elixir"), 1 } }, defenseTab, false)));
        public static ObjectHolder<SpellPartSkill> HEAL = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("heal", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 } }, defenseTab, false))); //210, 120


        public static ObjectHolder<SpellPartSkill> LIFETAP = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("life_tap", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)768"), 5 } }, defenseTab, false))); //255, 120

        public static ObjectHolder<SpellPartSkill> LIFEDRAIN = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("life_drain", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false))); //255, 165

        public static ObjectHolder<SpellPartSkill> HEALING = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("healing", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 } }, defenseTab, false)));

        public static ObjectHolder<SpellPartSkill> DISPEL = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("dispel", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)184"), 1 } }, defenseTab, false)));
        public static ObjectHolder<SpellPartSkill> ZONE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("zone", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 } }, defenseTab, false))); //210, 210

        public static ObjectHolder<SpellPartSkill> WALL = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("wall", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)324"), 1 } }, defenseTab, false))); //210, 210

        public static ObjectHolder<SpellPartSkill> CONTINGENCY_DAMAGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("contingency_damage", new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 } }, defenseTab, false)));
        public static ObjectHolder<SpellPartSkill> CONTINGENCY_HEALTH = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("contingency_health", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false)));

        public static ObjectHolder<SpellPartSkill> SHIELD = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("shield", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)72"), 1 }, { ItemRegistry.Create("(O)74"), 1 } }, defenseTab, false)));

        public static ObjectHolder<SpellPartSkill> SUMMON = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("summon", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 }, { ItemRegistry.Create("(O)613"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Cerublossom"), 1 } }, defenseTab, false)));
        public static ObjectHolder<SpellPartSkill> DURATION = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("duration", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)787"), 3 } }, defenseTab, false))); //255, 345

        //Utility

        public static ObjectHolder<SpellPartSkill> TOUCH = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("touch", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 5 } }, utilityTab, false)));

        public static ObjectHolder<SpellPartSkill> DIG = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("dig", new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.stoneQID), 1 } }, utilityTab, false)));
        public static ObjectHolder<SpellPartSkill> BLINK = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("blink", new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.prismaticShardQID), 1 } }, utilityTab, false)));

        public static ObjectHolder<SpellPartSkill> LIGHT = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("light", new Dictionary<Item, int>() { { ItemRegistry.Create("((O)93"), 1 } }, utilityTab, false)));
        public static ObjectHolder<SpellPartSkill> MININGPOWER = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("mining_power", new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.diamondQID), 1 } }, utilityTab, false)));

        public static ObjectHolder<SpellPartSkill> PLOW = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("plow", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 15 } }, utilityTab, false)));
        public static ObjectHolder<SpellPartSkill> CREATEWATER = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("create_water", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)599"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Wakebloom"), 1 } }, utilityTab, false)));
        public static ObjectHolder<SpellPartSkill> GROW = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("grow", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)465"), 3 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Cerublossom"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Tarma_Root"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Desert_Nova"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Wakebloom"), 1 } }, utilityTab, false)));
        public static ObjectHolder<SpellPartSkill> RANGE = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("range", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)767"), 5 } }, utilityTab, false)));

        public static ObjectHolder<SpellPartSkill> ETHEREALTOUCH = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("ethereal_touch", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 5 }, { ItemRegistry.Create("(O)767"), 5 } }, utilityTab, false)));

        public static ObjectHolder<SpellPartSkill> HARVEST = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("harvest", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)24"), 1 } }, utilityTab, false)));
        public static ObjectHolder<SpellPartSkill> CHANNEL = RegisterSpellPartSkill(new ObjectHolder<SpellPartSkill>(new SpellPartSkill("channel", new Dictionary<Item, int>() { { ItemRegistry.Create("(O)24"), 10 } }, utilityTab, false)));

        public static void SetSkillParents()
        {
            //Offense

            PHYSICAL_DAMAGE.Get().SetParents(PROJECTILE.Get());
            BOUNCE.Get().SetParents(PROJECTILE.Get());
            VELOCITY.Get().SetParents(PHYSICAL_DAMAGE.Get());
            
            AOE.Get().SetParents(PHYSICAL_DAMAGE.Get());
            BEAM.Get().SetParents(AOE.Get());
            DAMAGE.Get().SetParents(AOE.Get());
            
            EXPLOSION.Get().SetParents(DAMAGE.Get());

            WAVE.Get().SetParents(VELOCITY.Get());

            MAGICDAMAGE.Get().SetParents(PHYSICAL_DAMAGE.Get());
            FROSTDAMAGE.Get().SetParents(PHYSICAL_DAMAGE.Get());
            LIGHTNINGDAMAGE.Get().SetParents(PHYSICAL_DAMAGE.Get());
            FIREDAMAGE.Get().SetParents(PHYSICAL_DAMAGE.Get());

            PIERCING.Get().SetParents(MAGICDAMAGE.Get());
            RUNE.Get().SetParents(MAGICDAMAGE.Get());

            CONE.Get().SetParents(AOE.Get());

            //Defense

            //SELF.Get().SetParents(PROJECTILE.Get());
            
            EFFECT_POWER.Get().SetParents(SELF.Get());
            
            HASTE.Get().SetParents(SELF.Get());
            REGENERATION.Get().SetParents(SELF.Get());
            MANA_REGENERATION.Get().SetParents(SELF.Get());
            HEAL.Get().SetParents(SELF.Get());

            LIFETAP.Get().SetParents(HEAL.Get());

            LIFEDRAIN.Get().SetParents(LIFETAP.Get());

            HEALING.Get().SetParents(HEAL.Get());

            DISPEL.Get().SetParents(HEAL.Get());
            ZONE.Get().SetParents(DISPEL.Get());

            WALL.Get().SetParents(SELF.Get());

            CONTINGENCY_DAMAGE.Get().SetParents(HEALING.Get());
            CONTINGENCY_HEALTH.Get().SetParents(ZONE.Get());

            SHIELD.Get().SetParents(ZONE.Get());

            SUMMON.Get().SetParents(LIFEDRAIN.Get());
            DURATION.Get().SetParents(SUMMON.Get());

            //Utility

            DIG.Get().SetParents(TOUCH.Get());
            BLINK.Get().SetParents(TOUCH.Get());


            FORGE.Get().SetParents(DIG.Get());
            LIGHT.Get().SetParents(DIG.Get());
            MININGPOWER.Get().SetParents(DIG.Get());
            
            PLOW.Get().SetParents(LIGHT.Get());
            CREATEWATER.Get().SetParents(LIGHT.Get());
            GROW.Get().SetParents(LIGHT.Get());
            RANGE.Get().SetParents(LIGHT.Get());

            ETHEREALTOUCH.Get().SetParents(RANGE.Get());

            HARVEST.Get().SetParents(PLOW.Get());
            CHANNEL.Get().SetParents(LIGHT.Get());
        }

        public static ObjectHolder<SpellPartSkill> RegisterSpellPartSkill(ObjectHolder<SpellPartSkill> obj)
        {
            ObjectHolder<SpellPartSkill> toReturn = SKILLS.Register(obj);
            return toReturn;
        }
    }
}
