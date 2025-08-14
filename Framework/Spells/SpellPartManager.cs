using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Util;

namespace ArsVenefici.Framework.Spells
{
    public class SpellPartManager
    {
        private Dictionary<string, ISpellPart> spellParts = new Dictionary<string, ISpellPart>();
        private Dictionary<string, ISpellPart> contentpackSpellParts = new Dictionary<string, ISpellPart>();
        
        private ModEntry modEntry;

        public bool dictionariesPoplulated = false;

        public SpellPartManager(ModEntry modEntry) 
        {
            this.modEntry = modEntry;
        }

        public void AddShapes(List<ObjectHolder<AbstractShape>> list)
        {
            foreach (ObjectHolder<AbstractShape> holder in list)
            {
                AddShape(holder.Get());
            }
        }

        public void AddComponents(List<ObjectHolder<AbstractComponent>> list)
        {
            foreach (ObjectHolder<AbstractComponent> holder in list)
            {
                AddComponent(holder.Get());
            }
        }

        public void AddModifiers(List<ObjectHolder<AbstractModifier>> list)
        {
            foreach (ObjectHolder<AbstractModifier> holder in list)
            {
                AddModifier(holder.Get());
            }
        }

        private void AddShape(AbstractShape shape)
        {
            if(!spellParts.ContainsKey(shape.GetId()))
                spellParts.Add(shape.GetId(), shape);
        }

        private void AddComponent(AbstractComponent component)
        {
            if (!spellParts.ContainsKey(component.GetId()))
                spellParts.Add(component.GetId(), component);
        }

        private void AddModifier(AbstractModifier modifier)
        {
            if (!spellParts.ContainsKey(modifier.GetId()))
                spellParts.Add(modifier.GetId(), modifier);
        }

        public Dictionary<string, ISpellPart> GetSpellParts()
        {
            return spellParts;
        }

        //For Content Packs

        public void ContentPackAddShapes(List<ObjectHolder<AbstractShape>> list)
        {
            foreach (ObjectHolder<AbstractShape> holder in list)
            {
                ContentPackAddShape(holder.Get());
            }
        }

        public void ContentPackAddComponents(List<ObjectHolder<AbstractComponent>> list)
        {
            foreach (ObjectHolder<AbstractComponent> holder in list)
            {
                ContentPackAddComponent(holder.Get());
            }
        }

        public void ContentPackAddModifiers(List<ObjectHolder<AbstractModifier>> list)
        {
            foreach (ObjectHolder<AbstractModifier> holder in list)
            {
                ContentPackAddModifier(holder.Get());
            }
        }

        public void ContentPackAddShape(AbstractShape shape)
        {
            if (!contentpackSpellParts.ContainsKey(shape.GetId()))
                contentpackSpellParts.Add(shape.GetId(), shape);
        }

        public void ContentPackAddComponent(AbstractComponent component)
        {
            if (!contentpackSpellParts.ContainsKey(component.GetId()))
                contentpackSpellParts.Add(component.GetId(), component);
        }

        public void ContentPackAddModifier(AbstractModifier modifier)
        {
            if (!contentpackSpellParts.ContainsKey(modifier.GetId()))
                contentpackSpellParts.Add(modifier.GetId(), modifier);
        }

        public Dictionary<string, ISpellPart> GetContentPackSpellParts()
        {
            return contentpackSpellParts;
        }

        public void SetDictionariesPopluated(bool popluated)
        {
            dictionariesPoplulated = popluated;
        }
    }
}
