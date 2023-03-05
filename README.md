# More languages
This mod makes it possible to add new custom translations with corresponding beaver names.

## Included language packs
- wanguage(UWU)
- Türkçe (Turkish)

## Creating a custom language pack
Want to make a new language for yourself? No biggy it's very simple! Just copy the files (xxXX.txt, xxXX_names.txt) from an existing language pack and start translating.  
A row goes as following: ID,Text,Comment.
- **ID:** The key the game uses to pick a translation
- **Text:** The text that will show in the game
- **Comment:** Usefull information about it's usage

Do you need line breaks or did it stop working? try to add quotes around the text.  
**Example:** example.key,"Yes i'm a very special translation :D",This is a special one

## Submitting a custom language pack into the mod
#### Option 1: Discord
Send a message in the **\#modding-and-whatnot** channel.
#### Option 2: Github issue
[Create a issue](https://github.com/Timberborn-Modding-Central/MoreLanguages/issues) include the 2 files for the language pack and name for credits.
#### Option 3: Github pull request
- Place the new language pack in Package > lang
- Start game and test the language pack out
- Create a PR

**NOTE: No foul language packs will be added in the mod**

## Experimental
This mod is enabled while in experimental mode, but is not updated for the experimental mode. This means there will be missing text. You are able to add these yourself if you like. A community member needs to update the pack when Timberborn has updated the experimental to release.

## Developers
This mod removes the debugging message when a key is missing due experimental game keys. This can be disabled with a config file that will be generated with bepinex.

## Credits
- BiGaripAdam (Creation Türkçe)
- TheBloodEyes (Creator, Added uwu)

## Changelogs
### 1.0.0
- Creation MoreLanguage for Timberborn update 1
- Added language wanguage(UWU)
### 1.1.0
- Added language Türkçe (Turkish)
- Changed README
- Build replaces ThunderStore DLL
### 1.2.0
- Moved to mod.io
- Automated uploading/versioning
- Removed complexity for adding new languages
- Updated uwu to update 3