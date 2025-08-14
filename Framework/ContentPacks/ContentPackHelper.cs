using ArsVenefici.Framework.ContentPacks.DataModels;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.ContentPacks
{
    public class ContentPackHelper
    {
        // Instance of ModEntry
        private ModEntry modEntryInstance;

        ContentLoaders.SpellIconLoader spellIconLoader;

        // List of Skin Color Models
        public List<SpellIconModel> SpellIconsList = new List<SpellIconModel>();

        /// <summary>
        /// ContentPackHelpers Contructor.
        /// </summary>
        /// <param name="entry">An instance of <see cref="ModEntry"/></param>
        public ContentPackHelper(ModEntry entry)
        {
            // Set the var to the instance
            modEntryInstance = entry;
            spellIconLoader = new ContentLoaders.SpellIconLoader(entry, this);
        }

        /// <summary>
        /// Reads all the content packs for the mod.
        /// </summary>
        public void ReadContentPacks()
        {

            // Loop through each content pack
            foreach (IContentPack contentPack in modEntryInstance.Helper.ContentPacks.GetOwned())
            {
                modEntryInstance.Monitor.Log($"Reading content pack: {contentPack.Manifest.Name} {contentPack.Manifest.Version}", LogLevel.Info);
                LoadSpellIcons(contentPack);
            }

            // Add ImageInjector to the Asset Editor to start patching the images
            //Entry.Helper.Content.AssetEditors.Add(new ImageInjector(Entry, this));
        }



        /// <summary>
        /// Load Skin Color from a Content Pack.
        /// </summary>
        /// <param name="contentPack">The Current Content Pack.</param>
        private void LoadSpellIcons(IContentPack contentPack)
        {
            //SkinColorLoader skinColorLoader = new SkinColorLoader(Entry, contentPack, this);
            spellIconLoader.LoadSpellIcons(contentPack);
        }
    }
}
