﻿# Ars Venefici

Ars Venefici is a Stardew Valley mod that adds the ability to craft and cast spells with a spell book!

The functionality for the spell making is a port of the spell making functionality from the [Ars Magica Legacy Mod for Minecraft](https://www.curseforge.com/minecraft/mc-mods/ars-magica-legacy) to C# (I asked for permision to turn this into a Stardew Mod and got the okay to do so!).
Took inspiration from the [Ars Nouveau mod for Minecraft](https://www.curseforge.com/minecraft/mc-mods/ars-nouveau) to make spells using a spell book format.

Some functionality for some spell parts is based on some spells from the [Magic mod by spacechase0](https://www.nexusmods.com/stardewvalley/mods/2007). 
The cutscene of the wizard giving you a spell book is also based on the learning magic cutscene from the magic mod.
The professions in this mode are also based on the professions from the magic mod.

# Install

- Make sure the game is updated to the latest version.
- Install the latest version of [SMAPI](https://smapi.io/).
- Install [SpaceCore](https://www.nexusmods.com/stardewvalley/mods/1348).
- Install [Mana Bar](https://www.nexusmods.com/stardewvalley/mods/7831).
- Install [Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915).
- Install [this mod]() from Nexus.
- Run the game using SMAPI.

# Uninstall
- Simply delete the mod from your mods folder.


# Learning Wizardry

To gain access to crafting spells and the wizardry skill, you need to be at 4 hearts with the wizard. Then you need to simply enter his tower. If [Stardew Valley Expanded](https://www.nexusmods.com/stardewvalley/mods/3753) is installed, you will need to enter the new custom basement the mod adds to his tower.

# Using magic

- Press shift and 'B' to open the spell book menu.The spell book menu will show you all the different spell parts you can use.To create a spells, you will need to place one or multiple shapes, which are squares, into each shape group on the spell page you are on. 
    - Each shape group can hold up to 4 shapes/modifiers and if you want a spell to have more than one shape group, they have to be filled in order (Left to right) with no empty ones in between (the spell will not be valid and will not cast if that happens).
- Then you will need to place one or multiple components, which are octagons, into the spell grammar section of the page.
- You can change the way shapes, and components behave with modifiers, which are diamonds.
- Be sure to give your spell a name. Once you have created your spell, you can close your spell book and the spell will be saved.
- A spell label that show the current selected spell will appear on the screen once the spell book menu is closed. It can be moved with shift + the OemTilde button (~)
- Use 'Z' and 'X' to move through your list of spells. You can also use shift and 'Z' and shift and 'X' to cycle through a spell's shape groups
- Once you have the spell you want, you will be able to cast it. To cast a spell, press Q. 
- These controls can be adjusted in Ars Venefici's config file or if you have [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) installed, a section for the Ars Venefici config should appear in its menu, allowing you to edit the config values in game.

# Professions

You gain magic XP by casting spells. Leveling up increases your mana regen amount, and unlocks some professions:

- Level 5: Mana Efficiency (Total mana cost of spells is reduced by about a third).
    - Level 10: Mana Efficiency (Total mana cost of spells is reduced by about half).
    - Level 10: Mana Conservation (25% chance of spells not costing any mana).
- Level 5: Mana Regen I (doubles base mana regen amount).
    - Level 10: Mana Regen II (triples base mana regen amount).
    - Level 10: Mana Reserve (+500 max mana).


# Replenishing Mana

Your mana will regen over time. Each level you gain for wizardry as well as the Mana Efficiency professions will inrease the mana regen amount. You can also replenish some mana by eating a food item that has a custom field for regenerating mana.

# How Mana is Calculated

Every shape and modifier is multiplied together to create a multiplier value. Every compenent is added together to create a cost value. Then those two values are multiplied together to get the final mana cost.

# Adding New Items That Replenish Mana

Version 1.2.0 of this mod adds the ability for you to add food items that replenish mana using Content Patcher! [Take a look at this wiki page for a guide to add in custom items via Content Patcher﻿](https://stardewvalleywiki.com/Modding:Items#Define_a_custom_item). For your item to replenish mana, add this field to your item:

```
"CustomFields":
{
    ﻿"HeyImAmethyst.CP.ArsVenefici/Mana": "NUMBERVALUEHERE"
}
```

# Rune Table

Version 1.10 adds a new machine called a rune table. It will allow you to make different colored runes! You would need to craft a blank rune (which is a crafting recipe unlocked if you have learned wizardry) and an additional item (similar to how the furnace needs coal and the ore you need to smelt). To make any of the colored runes, an item with the color you are looking for needs to be placed into the table. So if you want a red rune, you need to have a red item (like a strawberry for example) and right click the table with it, while having a blank rune in your inventory. Some of the new items added with this mod can be used to make runes as well!

Any item that has a color context tag that the rune table is able to make a rune for can be used. If you have [LookupAnything](https://www.nexusmods.com/stardewvalley/mods/541?tab=description), the item information menu should tell you if an item can be used to make a colored rune.

The value in "NUMBERVALUEHERE" needs to be a number (ex: "20", "150", "200"). If the value isn't a number or has any letters or other characters in it, the item will not replenish any mana.

# Console Commands

- player_togglewizardry  <true | false> : Toggles the player's the ability to cast spells
- player_learnspellpart < spellpartId > :  Allows the player to learn a spell part
- player_forgetspellpart < spellpartId > : Allows the player to forget a spell part
- player_learnallspellparts: Allows the player to learn all spell parts
- player_forgetallspellparts: Allows the player to forget all spell parts
- player_knowsspellpart < spellpartId > : Checks if a player knows a spell part

- save_manapointsperlevel < numberValue > : Sets the amount of mana players have per Wizardry level
- save_manaregenrate < numberValue > : Sets the mana regen rate of players
- save_enableinfinitemana <true | false> : Toggles infinate mana
- save_enablegrowsickness <true | false> : Toggles the grow sickness debuff

- save_growsicknessdurationmillisecondslessthanlevelsix < numberValue > :  Sets the grow sickness debuff duration for Wizardry levels less than level 6

- save_growsicknessdurationmillisecondsgreaterthanorequaltolevelsix < numberValue > : Sets the grow sickness debuff duration for Wizardry levels greater than or equal to level 6

- save_growsicknessdurationmillisecondsgreaterthanorequaltoleveleight < numberValue > : Sets the grow sickness debuff duration for Wizardry levels greater than or equal to level 8

- save_growsicknessdurationmillisecondsgreaterthanorequaltolevelten < numberValue > : Sets the grow sickness debuff duration for Wizardry levels greater than or equal to level 10

- save_savedata: Returns the save data values for this mod

# List of Spell Part Ids

> # Shapes
> - self
> - projectile
> - touch
> - ethereal_touch
> - aoe
> - zone
> - wave
> - wall
> - rune
> - channel
> - contingency_damage
> - contingency_health
> - cone
> - beam
> # Components
> - dig
> - plow
> - grow
> - harvest
> - create_water
> - explosion
> - blink
> - light
> - heal
> - life_drain
> - life_tap
> - physical_damage
> - magic_damage
> - frost_damage
> - lightning_damage
> - fire_damage
> - haste
> - regeneration
> - mana_regeneration 
> - dispel
> - forge
> - shield 
> - summon 
> # Modifiers
> - damage
> - range
> - bounce
> - piercing
> - velocity
> - healing
> - duration
> - mining_power
> - effect_power

# Additional Notes

Ars Venefici seems to roughly translates to 'The Art of Wizardry' or 'The Art of Sorcery' based on translating using google and using some latin translation sites to see if the words are acurate. I wanted to use 'Ars' in the name as a refrence to Ars Magica Legacy, the original Ars Magica Mod by AWildCanadianEh, and Ars Nouveau. If there is a more acurate translation of 'The Art of Wizardry' or 'The Art of Sorcery' that includes the word 'Ars' please let me know ^^'
