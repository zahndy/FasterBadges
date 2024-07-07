# FasterBadges

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod for [Resonite](https://resonite.com/) that Injects configurable Badges Into the local user<br>
Requires ResoniteModSettings in order to set the badges string<br>
badges string is formatted as a comma separated list of resdb: strings.<br>
For example:
resdb:///515d6210069e5e0a498894d93735ab14fbd68a183a47c1d71a96caa3aba1a786.webp,resdb:///0e33a4809f27cfd1004e9a622efa615c0adec1ba2efb381c0d14db866c3042a5.webp<br>
<br>
this mod exists because AxisAligner is currently Inefficient/Heavy.<br>
When people want to add custom badges they get directed to or are handed nametags or badge-injectors that contain multiple AxisAligners.<br>
My alternative is creating some protoflux to deal with the positioning of the badges but it feels like a bad workaround for a issue that shouldnt exist in the first place.<br>
To solve this we can just inject our badges into the badge templates and let the AvatarBadgeManager deal with aligning them. But this has to be done before the component on the avatar initializes, Thus the creation this mod.<br>

## Installation
1. Install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
1. Place [FasterBadges.dll](https://github.com/zahndy/FasterBadges/releases/latest/download/FasterBadges.dll) into your `rml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` for a default install. You can create it if it's missing, or if you launch the game once with ResoniteModLoader installed it will create the folder for you.
1. Start the game. If you want to verify that the mod is working you can check your Resonite logs.
