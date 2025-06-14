﻿using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Spells;
using StardewValley.Buffs;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.GameData.Objects;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Internal;
using StardewValley.Projectiles;
using static StardewValley.Menus.CharacterCustomization;
using StardewValley.Objects;
using ArsVenefici.Framework.API.Skill;

namespace ArsVenefici.Framework.Skill
{
    public class SpellPartSkillManager
    {
        private ModEntry modEntry;

        public MagicAltarTab offenceTab;
        public MagicAltarTab defenseTab;
        public MagicAltarTab utilityTab;

        private Dictionary<string, SpellPartSkill> spellPartSkills = new Dictionary<string, SpellPartSkill>();

        public SpellPartSkillManager(ModEntry modEntry)
        {
            this.modEntry = modEntry;

            offenceTab = MagicAltarTab.create(modEntry.Helper.Translation.Get("ui.magic_altar.offense_tab.name"), modEntry.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/offense.png"), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/interface/offense.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 200, 50, 0); //226 //46
            defenseTab = MagicAltarTab.create(modEntry.Helper.Translation.Get("ui.magic_altar.defense_tab.name"), modEntry.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/defense.png"), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/interface/defense.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 400, 50, 1); //181
            utilityTab = MagicAltarTab.create(modEntry.Helper.Translation.Get("ui.magic_altar.utility_tab.name"), modEntry.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/utility.png"), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/interface/utility.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 400, 50, 2); //136
            
            PopluateDictionary();
        }

        public void PopluateDictionary()
        {
            AddOffense();
            AddDefense();
            AddUtility();
        }

        private void AddOffense()
        {

            SpellPartSkill projectile = new SpellPartSkill("projectile", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>{ { ItemRegistry.Create("(W)32"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Tarma_Root"), 1 } }, offenceTab, false);
            
            SpellPartSkill physicalDamage = new SpellPartSkill("physical_damage", new HashSet<SpellPartSkill> { projectile }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5") , 1} }, offenceTab, false);
            
            SpellPartSkill bounce = new SpellPartSkill("bounce", new HashSet<SpellPartSkill> { projectile }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)766"), 20 } }, offenceTab, false);
            
            SpellPartSkill velocity = new SpellPartSkill("velocity", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)395"), 1 } }, offenceTab, false);

            SpellPartSkill aoe = new SpellPartSkill("aoe", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)621"), 1 } }, offenceTab, false);
            SpellPartSkill beam = new SpellPartSkill("beam", new HashSet<SpellPartSkill> { aoe }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)32"), 1 }, { ItemRegistry.Create("(O)74"), 5 } }, offenceTab, false);
            SpellPartSkill damage = new SpellPartSkill("damage", new HashSet<SpellPartSkill> { aoe }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)848"), 5 } }, offenceTab, false); //210, 165

            SpellPartSkill explosion = new SpellPartSkill("explosion", new HashSet<SpellPartSkill> { damage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)287"), 3 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Desert_Nova"), 2 } }, offenceTab, false); //210, 300

            SpellPartSkill wave = new SpellPartSkill("wave", new HashSet<SpellPartSkill> { velocity }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Tarma_Root"), 1 } }, offenceTab, false); //210, 210

            SpellPartSkill magicDamage = new SpellPartSkill("magic_damage", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(O)769"), 1 }, { ItemRegistry.Create("(O)768"), 1 } }, offenceTab, false);
            SpellPartSkill frostDamage = new SpellPartSkill("frost_damage", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(O)84"), 1 } }, offenceTab, false);
            SpellPartSkill lightningDamage = new SpellPartSkill("lightning_damage", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(BC)9"), 1 } }, offenceTab, false);
            SpellPartSkill fireDamage = new SpellPartSkill("fire_damage", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 }, { ItemRegistry.Create("(O)82"), 1 } }, offenceTab, false);

            SpellPartSkill piercing = new SpellPartSkill("piercing", new HashSet<SpellPartSkill> { magicDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)691"), 5 } }, offenceTab, false);
            SpellPartSkill forge = new SpellPartSkill("forge", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(BC)13"), 5 } }, offenceTab, false);
            
            SpellPartSkill rune = new SpellPartSkill("rune", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)405"), 5 } }, offenceTab, false);
            
            SpellPartSkill cone = new SpellPartSkill("cone", new HashSet<SpellPartSkill> { aoe }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)405"), 1 }, { ItemRegistry.Create("(O)311"), 3 } }, offenceTab, false);

            AddSpellPartSkill(projectile);
            AddSpellPartSkill(physicalDamage);
            AddSpellPartSkill(magicDamage);
            AddSpellPartSkill(lightningDamage);
            AddSpellPartSkill(fireDamage);
            AddSpellPartSkill(bounce);
            AddSpellPartSkill(piercing);
            AddSpellPartSkill(velocity);
            AddSpellPartSkill(damage);
            AddSpellPartSkill(aoe);
            AddSpellPartSkill(explosion);
            AddSpellPartSkill(wave);
            AddSpellPartSkill(beam);
            AddSpellPartSkill(forge);
            AddSpellPartSkill(rune);
            AddSpellPartSkill(cone);
        }

        private void AddDefense()
        {
            SpellPartSkill self = new SpellPartSkill("self", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>() { { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Vinteum_Dust"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 } }, defenseTab, false);
            
            SpellPartSkill effect_power = new SpellPartSkill("effect_power", new HashSet<SpellPartSkill>() { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)787"), 1 }, { ItemRegistry.Create("(O)395"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Desert_Nova"), 1 } }, defenseTab, false);

            SpellPartSkill haste = new SpellPartSkill("haste", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)395"), 1 } }, defenseTab, false);
            SpellPartSkill regeneration = new SpellPartSkill("regeneration", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false);
            SpellPartSkill mana_regeneration = new SpellPartSkill("mana_regeneration", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)" + ModEntry.ArsVenificiContentPatcherId + "_Mana_Elixir"), 1 } }, defenseTab, false);
            SpellPartSkill heal = new SpellPartSkill("heal", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 } }, defenseTab, false); //210, 120

            
            SpellPartSkill lifeTap = new SpellPartSkill("life_tap", new HashSet<SpellPartSkill> { heal }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)768"), 5 } }, defenseTab, false); //255, 120

            SpellPartSkill lifeDrain = new SpellPartSkill("life_drain", new HashSet<SpellPartSkill> { lifeTap }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false); //255, 165

            SpellPartSkill healing = new SpellPartSkill("healing", new HashSet<SpellPartSkill> { heal }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 } }, defenseTab, false);
            
            SpellPartSkill dispel = new SpellPartSkill("dispel", new HashSet<SpellPartSkill> { heal }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)184"), 1 } }, defenseTab, false);
            SpellPartSkill zone = new SpellPartSkill("zone", new HashSet<SpellPartSkill> { dispel }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 } }, defenseTab, false); //210, 210
            
            SpellPartSkill wall = new SpellPartSkill("wall", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)324"), 1 } }, defenseTab, false); //210, 210

            SpellPartSkill contingency_damage = new SpellPartSkill("contingency_damage", new HashSet<SpellPartSkill> { healing }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5"), 1 } }, defenseTab, false);
            SpellPartSkill contingency_health = new SpellPartSkill("contingency_health", new HashSet<SpellPartSkill> { zone }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false);

            SpellPartSkill shield = new SpellPartSkill("shield", new HashSet<SpellPartSkill> { zone }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)72"), 1 }, { ItemRegistry.Create("(O)74"), 1 } }, defenseTab, false);
            
            SpellPartSkill summon = new SpellPartSkill("summon", new HashSet<SpellPartSkill> { lifeDrain }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 }, { ItemRegistry.Create("(O)613"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Cerublossom"), 1 } }, defenseTab, false);
            SpellPartSkill duration = new SpellPartSkill("duration", new HashSet<SpellPartSkill> { summon }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)787"), 3 } }, defenseTab, false); //255, 345

            AddSpellPartSkill(self);
            AddSpellPartSkill(haste);
            AddSpellPartSkill(regeneration);
            AddSpellPartSkill(mana_regeneration);
            AddSpellPartSkill(heal);
            AddSpellPartSkill(zone);
            AddSpellPartSkill(lifeTap);
            AddSpellPartSkill(lifeDrain);
            AddSpellPartSkill(duration);
            AddSpellPartSkill(healing);
            AddSpellPartSkill(dispel);
            AddSpellPartSkill(wall);
            AddSpellPartSkill(contingency_damage);
            AddSpellPartSkill(contingency_health);
            AddSpellPartSkill(shield);
            AddSpellPartSkill(summon);
            AddSpellPartSkill(effect_power);
        }

        private void AddUtility()
        {
            SpellPartSkill touch = new SpellPartSkill("touch", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 5 } }, utilityTab, false);


            SpellPartSkill dig = new SpellPartSkill("dig", new HashSet<SpellPartSkill> { touch }, new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.stoneQID), 1 } }, utilityTab, false);
            SpellPartSkill blink = new SpellPartSkill("blink", new HashSet<SpellPartSkill> { touch }, new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.prismaticShardQID), 1 } }, utilityTab, false);

            SpellPartSkill light = new SpellPartSkill("light", new HashSet<SpellPartSkill> { dig }, new Dictionary<Item, int>() { { ItemRegistry.Create("((O)93"), 1 } }, utilityTab, false);
            SpellPartSkill miningPower = new SpellPartSkill("mining_power", new HashSet<SpellPartSkill> { dig }, new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.diamondQID), 1 } }, utilityTab, false);

            SpellPartSkill plow = new SpellPartSkill("plow", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 15 } }, utilityTab, false);
            SpellPartSkill createWater = new SpellPartSkill("create_water", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)599"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Wakebloom"), 1 } }, utilityTab, false);
            SpellPartSkill grow = new SpellPartSkill("grow", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)465"), 3 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Aum"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Cerublossom"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Tarma_Root"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Desert_Nova"), 1 }, { ItemRegistry.Create($"(O){ModEntry.ArsVenificiContentPatcherId}_Wakebloom"), 1 } }, utilityTab, false);
            SpellPartSkill range = new SpellPartSkill("range", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)767"), 5 } }, utilityTab, false);

            SpellPartSkill etherealTouch = new SpellPartSkill("ethereal_touch", new HashSet<SpellPartSkill> { range }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 5 }, { ItemRegistry.Create("(O)767"), 5 } }, utilityTab, false);

            SpellPartSkill harvest = new SpellPartSkill("harvest", new HashSet<SpellPartSkill> { plow }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)24"), 1 } }, utilityTab, false);
            SpellPartSkill channel = new SpellPartSkill("channel", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)24"), 10 } }, utilityTab, false);
            
            AddSpellPartSkill(touch);
            AddSpellPartSkill(dig);
            AddSpellPartSkill(blink);
            AddSpellPartSkill(light);
            AddSpellPartSkill(miningPower);
            AddSpellPartSkill(plow);
            AddSpellPartSkill(createWater);
            AddSpellPartSkill(grow);
            AddSpellPartSkill(range);
            AddSpellPartSkill(etherealTouch);
            AddSpellPartSkill(harvest);
            AddSpellPartSkill(channel);
        }

        public void AddSpellPartSkill(SpellPartSkill skill)
        {
            spellPartSkills.Add(skill.GetId(), skill);
        }

        public Dictionary<string, SpellPartSkill> GetSpellPartSkills()
        {
            return spellPartSkills;
        }
    }
}
