using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore.Events;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Events
{
    public class SpellPartEvents
    {
        public event EventHandler AddSpellParts;
        public event EventHandler AddSpellPartSkills;

        ModEntry modEntryInstance;

        public SpellPartEvents(ModEntry modEntry)
        {
            modEntryInstance = modEntry;
        }

        public virtual void InvokeOnAddSpellParts()
        {
            if (AddSpellParts == null)
                return;

            Utils.InvokeEvent("ArsVenefici.SpellPartEvents.OnAddSpellParts", AddSpellParts.GetInvocationList(), null);
        }

        public virtual void InvokeOnAddSpellPartSkills()
        {
            if (AddSpellPartSkills == null)
                return;

            Utils.InvokeEvent("ArsVenefici.SpellPartEvents.OnAddSpellPartSkills", AddSpellPartSkills.GetInvocationList(), null);
        }

        public void OnAddSpellParts(object sender, EventArgs args)
        {
            modEntryInstance.spellPartManager.dictionariesPoplulated = false;

            modEntryInstance.spellPartManager.AddShapes(ArsShapes.SHAPES.GetObjectList());
            modEntryInstance.spellPartManager.AddComponents(ArsComponents.COMPONENTS.GetObjectList());
            modEntryInstance.spellPartManager.AddModifiers(ArsModifiers.MODIFIERS.GetObjectList());

            //modEntryInstance.spellPartManager.ContentPackAddShapes(ArsShapes.CONTENTPACKSHAPES.GetObjectList());
            //modEntryInstance.spellPartManager.ContentPackAddComponents(ArsComponents.CONTENTPACKCOMPONENTS.GetObjectList());
            //modEntryInstance.spellPartManager.ContentPackAddModifiers(ArsModifiers.CONTENTPACKMODIFIERS.GetObjectList());

            modEntryInstance.spellPartManager.dictionariesPoplulated = true;
        }

        public void OnAddSpellPartSkills(object sender, EventArgs args)
        {
            modEntryInstance.spellPartSkillManager.dictionariesPoplulated = false;

            ArsSpellPartSkills.SetSkillParents();

            modEntryInstance.spellPartSkillManager.AddSpellPartSkills(ArsSpellPartSkills.SKILLS.GetObjectList());
            //modEntryInstance.spellPartSkillManager.ContentPackAddSpellPartSkills(ArsSpellPartSkills.CONTENTPACKSKILLS.GetObjectList());

            modEntryInstance.spellPartSkillManager.dictionariesPoplulated = true;
        }
    }
}
