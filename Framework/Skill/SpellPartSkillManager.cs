using ArsVenefici.Framework.Interfaces.Spells;
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
using ArsVenefici.Framework.GUI.Menus;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.GameData.Objects;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Internal;
using StardewValley.Projectiles;
using static StardewValley.Menus.CharacterCustomization;
using StardewValley.Objects;

namespace ArsVenefici.Framework.Skill
{
    public class SpellPartSkillManager
    {
        public Dictionary<string, SpellPartSkill> spellPartSkills = new Dictionary<string, SpellPartSkill>();
        private ModEntry modEntry;

        public MagicAltarTab offenceTab;
        public MagicAltarTab defenseTab;
        public MagicAltarTab utilityTab;

        public SpellPartSkillManager(ModEntry modEntry)
        {
            this.modEntry = modEntry;

            offenceTab = MagicAltarTab.create(modEntry.Helper.Translation.Get("ui.magic_altar.offense_tab.name"), modEntry.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/offense.png"), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/interface/offense.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 226, 46, 0);
            defenseTab = MagicAltarTab.create(modEntry.Helper.Translation.Get("ui.magic_altar.defense_tab.name"), modEntry.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/defense.png"), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/interface/defense.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 181, 46, 1);
            utilityTab = MagicAltarTab.create(modEntry.Helper.Translation.Get("ui.magic_altar.utility_tab.name"), modEntry.Helper.ModContent.Load<Texture2D>("assets/gui/occulus/utility.png"), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/interface/utility.png"), MagicAltarTab.TEXTURE_WIDTH, MagicAltarTab.TEXTURE_HEIGHT, 136, 46, 2);
            
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

            SpellPartSkill projectile = new SpellPartSkill("projectile", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>{ { ItemRegistry.Create("(W)32"), 1 } }, offenceTab, false);
            
            SpellPartSkill physicalDamage = new SpellPartSkill("physical_damage", new HashSet<SpellPartSkill> { projectile }, new Dictionary<Item, int>() { { ItemRegistry.Create("(W)5") , 1} }, offenceTab, false);
            SpellPartSkill bounce = new SpellPartSkill("bounce", new HashSet<SpellPartSkill> { projectile }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)766"), 20 } }, offenceTab, false);
            SpellPartSkill piercing = new SpellPartSkill("piercing", new HashSet<SpellPartSkill> { projectile }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)691"), 5 } }, offenceTab, false);
            SpellPartSkill velocity = new SpellPartSkill("velocity", new HashSet<SpellPartSkill> { projectile }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)395"), 1 } }, offenceTab, false);

            SpellPartSkill damage = new SpellPartSkill("damage", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)848"), 5 } }, offenceTab, false); //210, 165
            SpellPartSkill aoe = new SpellPartSkill("aoe", new HashSet<SpellPartSkill> { physicalDamage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)621"), 1 } }, offenceTab, false);


            SpellPartSkill explosion = new SpellPartSkill("explosion", new HashSet<SpellPartSkill> { damage }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)287"), 3 } }, offenceTab, false); //210, 300

            spellPartSkills.Add(projectile.GetId(), projectile);
            spellPartSkills.Add(physicalDamage.GetId(), physicalDamage);
            spellPartSkills.Add(bounce.GetId(), bounce);
            spellPartSkills.Add(piercing.GetId(), piercing);
            spellPartSkills.Add(velocity.GetId(), velocity);
            spellPartSkills.Add(damage.GetId(), damage);
            spellPartSkills.Add(aoe.GetId(), aoe);
            spellPartSkills.Add(explosion.GetId(), explosion);
        }

        private void AddDefense()
        {
            SpellPartSkill self = new SpellPartSkill("self", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>(), defenseTab, false);

            SpellPartSkill haste = new SpellPartSkill("haste", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)395"), 1 } }, defenseTab, false);
            SpellPartSkill heal = new SpellPartSkill("heal", new HashSet<SpellPartSkill> { self }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false); //210, 120

            SpellPartSkill zone = new SpellPartSkill("zone", new HashSet<SpellPartSkill> { heal }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)74"), 1 } }, defenseTab, false); //210, 210
            SpellPartSkill lifeTap = new SpellPartSkill("life_tap", new HashSet<SpellPartSkill> { heal }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)768"), 5 } }, defenseTab, false); //255, 120

            SpellPartSkill lifeDrain = new SpellPartSkill("life_drain", new HashSet<SpellPartSkill> { lifeTap }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false); //255, 165

            SpellPartSkill duration = new SpellPartSkill("duration", new HashSet<SpellPartSkill> { lifeTap }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)787"), 3 } }, defenseTab, false); //255, 345
            SpellPartSkill healing = new SpellPartSkill("healing", new HashSet<SpellPartSkill> { lifeTap }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)773"), 1 } }, defenseTab, false);

            spellPartSkills.Add(self.GetId(), self);
            spellPartSkills.Add(haste.GetId(), haste);
            spellPartSkills.Add(heal.GetId(), heal);
            spellPartSkills.Add(zone.GetId(), zone);
            spellPartSkills.Add(lifeTap.GetId(), lifeTap);
            spellPartSkills.Add(lifeDrain.GetId(), lifeDrain);
            spellPartSkills.Add(duration.GetId(), duration);
            spellPartSkills.Add(healing.GetId(), healing);
        }

        private void AddUtility()
        {
            SpellPartSkill touch = new SpellPartSkill("touch", new HashSet<SpellPartSkill>(), new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 5 } }, utilityTab, false);


            SpellPartSkill dig = new SpellPartSkill("dig", new HashSet<SpellPartSkill> { touch }, new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.stoneQID), 1 } }, utilityTab, false);
            SpellPartSkill blink = new SpellPartSkill("blink", new HashSet<SpellPartSkill> { touch }, new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.prismaticShardQID), 1 } }, utilityTab, false);

            SpellPartSkill light = new SpellPartSkill("light", new HashSet<SpellPartSkill> { dig }, new Dictionary<Item, int>() { { ItemRegistry.Create("((O)93"), 1 } }, utilityTab, false);
            SpellPartSkill miningPower = new SpellPartSkill("mining_power", new HashSet<SpellPartSkill> { dig }, new Dictionary<Item, int>() { { ItemRegistry.Create(StardewValley.Object.diamondQID), 1 } }, utilityTab, false);

            SpellPartSkill plow = new SpellPartSkill("plow", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 15 } }, utilityTab, false);
            SpellPartSkill createWater = new SpellPartSkill("create_water", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)599"), 1 } }, utilityTab, false);
            SpellPartSkill grow = new SpellPartSkill("grow", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)465"), 3 } }, utilityTab, false);
            SpellPartSkill range = new SpellPartSkill("range", new HashSet<SpellPartSkill> { light }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)767"), 5 } }, utilityTab, false);

            SpellPartSkill etherialTouch = new SpellPartSkill("etherial_touch", new HashSet<SpellPartSkill> { range }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)330"), 5 }, { ItemRegistry.Create("(O)767"), 5 } }, utilityTab, false);

            SpellPartSkill harvest = new SpellPartSkill("harvest", new HashSet<SpellPartSkill> { plow }, new Dictionary<Item, int>() { { ItemRegistry.Create("(O)24"), 1 } }, utilityTab, false);

            spellPartSkills.Add(touch.GetId(), touch);
            spellPartSkills.Add(dig.GetId(), dig);
            spellPartSkills.Add(blink.GetId(), blink);
            spellPartSkills.Add(light.GetId(), light);
            spellPartSkills.Add(miningPower.GetId(), miningPower);
            spellPartSkills.Add(plow.GetId(), plow);
            spellPartSkills.Add(createWater.GetId(), createWater);
            spellPartSkills.Add(grow.GetId(), grow);
            spellPartSkills.Add(range.GetId(), range);
            spellPartSkills.Add(etherialTouch.GetId(), etherialTouch);
            spellPartSkills.Add(harvest.GetId(), harvest);
        }
    }
}
