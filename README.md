# More languages
This mod makes it possible to add new custom translations with corresponding beaver names.

## Included language packs
- wanguage(UWU)

## Creating a custom language pack
Want to make a new language for yourself? No biggy it's very simple! Just copy the files (xxXX.txt, xxXX_names.txt) from an existing language pack and start translating.  
A row goes as following: ID,Text,Comment.
- **ID:** The key the game uses to pick a translation
- **Text:** The text that will show in the game
- **Comment:** Usefull information about it's usage

Do you need line breaks or did it stop working? try to add quotes around the text.  
**Example:** example.key,"Yes i'm a very special translation :D",This is a special one

## Submitting a custom language pack into the mod
Did you make a new language pack and want it to be added to the mod? Send a message in the **\#modding-and-whatnot**, or make a pullrequest in the github with the new pack added, optional add the versioning in minefest.json and MoreLanguagesPlugin.cs.  
**NOTE: No foul language packs will be added in the mod**

## Experimental
This mod is enabled while in experimental mode, but is not updated for the experimental mode. This means there will be missing text. You are able to add these yourself if you like. A community member needs to update the pack when Timberborn has updated the experimental to release.

## Developers
This mod removes the debugging message when a key is missing due experimental game keys. This can be disabled with a config file that will be generated with bepinex.

## Changelogs
### 1.0.0
- Creation MoreLanguage for Timberborn update 1
- Adding language wanguage(UWU)