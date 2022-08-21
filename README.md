# Casino Mortale Ventilation Ducts
A minigame in which players control a robot that crawls through ventilation ducts, listening in on the various rooms it passes. This game was developed for Casino Mortale, a LARP event by [RSV Arcana](https://www.arcana.nl).

![main](https://user-images.githubusercontent.com/18243979/139534386-71396837-a6b1-4b70-8e2f-34eae307c7e8.png)

## How to use
Wait until the boot screen asks you to press the CONNECT button (The C key or the right shoulder key on an Xbox controller). Control the robot with the arrow keys or a controller. 
If you set the IsPowerButtonRequired button setting to True, the game will stay black on startup untill you press and hold the power button (The V key or the left shoulder button on an Xbox 360 controller.) To make this less awkward, it is recommended to solder a toggle switch to the controller button when using this mode.

### Editing the map
Press E to open the map editor. Types of tiles can be selected on the toolbar at the top. Click on any cell in the grid to place the currently selected tile or to clear the cell if the clear tool is selected. Use the mouse wheel to rotate the tile in 90 degree increments before placing it.

Rows and columns of cells can be added to or removed from the map with the buttons at the top. Hold the middle mouse button to drag the map.

When selecting a sound tile, select the audio files that should be played from the dialog. These will be played sequentially: if you return to the audio tile after having heard the first sound, the second sound will play, etc. New sounds can be added by copying MP3's to the StreamingAssets folder. 
The order of the audio clips cannot be changed in the editor, but they can be reordered by editing the map file, which is generated on first startup in %localappdata%\\..\LocalLow\Casino Mortale\Ventilation Ducts\map.xml.

Press E again to close the editor.

### Resetting the map
Press X to reset the map to the default version.

## Attribution
- "Do you expect me to talk?" audio from _Goldfinger_ (1964), United Artists.
- "Trololololo" audio from _I Am Very Glad, as I'm Finally Returning Back Home_, Eduard Khil
- "[Every Single Scandinavian Crime Drama](https://www.youtube.com/watch?v=I-OOpZitfd0)" audio by the interdimensional [Alasdair Beckett-King](https://www.youtube.com/c/ABeckettKing). Subscribe to his YouTube channel, he's awesome.
- [Casino audio](https://sound-effects.bbcrewind.co.uk/search?q=07021005) from [BBC Sound Effects](https://sound-effects.bbcrewind.co.uk/search?q=&source=bbc_archive) bbc.co.uk – © copyright [2021] BBC
- [Domestic Sewing Machines audio](https://sound-effects.bbcrewind.co.uk/search?q=07039136) from [BBC Sound Effects](https://sound-effects.bbcrewind.co.uk/search?q=&source=bbc_archive) bbc.co.uk – © copyright [2021] BBC
- [National Park Typeface](https://nationalparktypeface.com/License) Copyright (c) 2018, Jeremy Shellhorn | Design Outside Studio | with Reserved Font Name "National Park Typeface". Licensed under the [SIL Open Font License, version 1.1.](https://scripts.sil.org/OFL)
- Retro screen effects based on Cyanlux's [Retro CRT Shader](https://cyangamedev.wordpress.com/2020/09/10/retro-crt-shader-breakdown/).
## License
Licensed under the MIT license
