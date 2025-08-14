using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.ContentPacks.DataModels;
using ArsVenefici.Framework.GUI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.ContentPacks.ContentLoaders
{
    public class SpellIconLoader
    {
        // Instance of ModEntry
        private ModEntry modEntryInstance;

        // Directory where the spell icon files are stored
        private DirectoryInfo SpellIconsDirectory;

        // The model of the spell icon
        private SpellIconModel SpellIcons;

        // Current content pack being looked at
        private IContentPack CurrentContentPack;

        // Instance of ContentPackHelper
        private ContentPackHelper PackHelper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entry">Instance of ModEntry</param>
        /// <param name="contentPack">Current Content Pack</param>
        /// <param name="packHelper">Instance of ContentPackHelper</param>
        public SpellIconLoader(ModEntry entry, IContentPack contentPack, ContentPackHelper packHelper)
        {
            SpellIconsDirectory = new DirectoryInfo(Path.Combine(contentPack.DirectoryPath, entry.contentPackSpellIconsDirectory));
            modEntryInstance = entry;
            CurrentContentPack = contentPack;
            PackHelper = packHelper;
        }

        public SpellIconLoader(ModEntry entry, ContentPackHelper packHelper)
        {
            modEntryInstance = entry;
            PackHelper = packHelper;
        }

        /// <summary>
        /// Loads Skin Tone from a Content Pack.
        /// </summary>
        public void LoadSpellIcons(IContentPack contentPack)
        {
            CurrentContentPack = contentPack;
            SpellIconsDirectory = new DirectoryInfo(Path.Combine(contentPack.DirectoryPath, modEntryInstance.contentPackSpellIconsDirectory));
            modEntryInstance.Monitor.Log(SpellIconsDirectory.ToString(), LogLevel.Info);

            if (DoesSpellIconDirectoryExists())
            {
                try
                {
                    CreateNewSpellIconModel(contentPack);
                    SetSpellIconModelVariables();
                    AddSpellIconsToList();
                }
                catch
                {
                    modEntryInstance.Monitor.Log($"{CurrentContentPack.Manifest.Name} spell icons is empty. This pack was not added.", LogLevel.Warn);
                }
            }
        }

        /// <summary>
        /// Whether the Spell Icon Directory Exists.
        /// </summary>
        /// <returns></returns>
        private bool DoesSpellIconDirectoryExists()
        {
            return SpellIconsDirectory.Exists;
        }

        /// <summary>
        /// Creates a new SpellIconModel and sets the Textures.
        /// </summary>
        private void CreateNewSpellIconModel(IContentPack contentPack)
        {
            SpellIcons = new SpellIconModel();
            List<Texture2D> textures = new List<Texture2D>();

            FileInfo[] files = SpellIconsDirectory.GetFiles();

            foreach (FileInfo file in files)
            {
                //FileStream fileStream = new FileStream(Path.Combine(contentPack.DirectoryPath, modEntryInstance.contentPackSpellIconsDirectory) + "/" + key, FileMode.Open);
                //Texture2D spr = Texture2D.FromStream(graphics, fileStream);

                if (file.Extension == ".png")
                {
                    string key = Path.GetFileName(file.Name);
                    Texture2D spr = CurrentContentPack.ModContent.Load<Texture2D>(modEntryInstance.contentPackSpellIconsDirectory + key);
                    textures.Add(spr);
                }
            }

            //SpellIcon.Textures = CurrentContentPack.ModContent.Load<Texture2D>(Path.Combine(modEntryInstance.contentPackSpellIconDirectory, modEntryInstance.contentPackSkinTonePNGName));
            //SpellIcons.Textures = CurrentContentPack.ModContent.Load<Texture2D[]>(modEntryInstance.contentPackSpellIconsDirectory);
            SpellIcons.Textures = textures.ToArray();
        }

        /// <summary>
        /// Sets Texture Height and Mod Name for the Skin Tone Model.
        /// </summary>
        private void SetSpellIconModelVariables()
        {
            //SkinColor.TextureHeight = SkinColor.Texture.Height;
            SpellIcons.ModName = CurrentContentPack.Manifest.Name;
        }

        /// <summary>
        /// Adds Skin Tone to list of added Skin Tones.
        /// </summary>
        private void AddSpellIconsToList()
        {
            PackHelper.SpellIconsList.Add(SpellIcons);
        }
    }
}
