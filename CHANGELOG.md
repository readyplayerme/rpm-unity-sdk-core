# Changelog

All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [5.0.0] - 2024.01.12

### Updated
- refactor and extracted shared logic from network packages @harrisonhough in [#148](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/148)
- replaced api urls in samples with models urls by @rk132 in [#152](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/152)
- added com.unity.cloud.gltfast as a dependency and removed auto install of gltfast from git url by @harrisonhough in [#155](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/155)
- updated to GLTFast 6.0.1 by @harrisonhough in by @harrisonhough in [#157](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/157)
- replaced use of ienumerator coroutines with async/await by @rYuuk in [#172](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/172)
- request class names updated to be more uniform by @MaxAndreassenRPM in [#173](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/173)
- endpoint classes removed and refactored by @MaxAndreassenRPM in [#174](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/174)
- added avatar creator POC sample using new elements by @ryuuk in [#182](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/182)
- removed "I don't have an account" checkbox from setup guide by @rk132 in [#184](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/184)
- restructure of avatar creator samples by @rYuuk in [#185](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/185)
- class and folder restructure to match Unity package standards by @harrisonhough in [#190](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/190)
- namespaces added to some scripts to meet asset store requirements by @harrisonhough in [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)
- Ready Player Me top toolbar menu is under `Tools/Ready Player Me` to comply with Asset Store requirements by @harrisonhough in [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)
- samples renamed for Asset Store version of package and paths updated accordingly by @harrisonhough in [#198](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/198)
- quick start sample animations updated by @rYuuk in [#200](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/200)
- draco compression package version updated by @harrisonhough in [#202](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/202)

### Added
- add gender select element for Avatar Creator by @harrisonhough in [#159](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/159)
- basic login element for Avatar Creator by @harrisonhough in [#160](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/160)
- photo capture element for Avatar Creator by @ryuuk in [#162](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/162)
- avatar template element for Avatar Creator by @harrisonhough in [#164](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/164)
- selfie element for Avatar Creator by @ryuuk in [#166](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/166)
- asset panel element for Avatar Creator by @harrisonhough in [#175](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/175)
- account creation element for Avatar Creator by @ryuuk in [#178](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/178)
- avatar preview element for Avatar Creator by @ryuuk in [#181](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/181)

- fixed some issues related to paths like in the Graphics Setting Utility by @harrisonhough in [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)
- shader override property added to avatar config by @ryuuk in [#199](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/199)

### Fixed
- fix for handling pasted url text in subdomain field by @harrisonhough in [#183](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/183)
- added permission and orientation fix to photo capture element by @ryuuk in [#192](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/192)
- added Panel Switcher clear functionality to fix issues related to relaunching the creator by @ryuuk in [#194](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/194)
- namespaces added to some scripts to meet asset store requirements by @harrisonhough in [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)

## [4.1.2] - 2023.12.20

### Fixed
- add preserve attribute to CategoryConverter in AvatarCreator by @harrisonHough in [#193](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/193)

## [4.1.1] - 2023.11.29

### Fixed
- fixed json converters in AvatarCreator getting stripped on android or webgl builds by @rYuuk in [#188](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/188)

## [4.1.0] - 2023.11.29

### Updated
- replaced API URLs with model URLs for shortcodes by @rk132 in [#152](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/152)
- updated render api and samples @rYuuk in [#147](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/147)

### Added
- Added app id to setup guide by @ryuuk in [#145](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/145)

## [4.0.1] - 2023.11.14

### Fixed
- fixed an issue causing avatars to be stored locally even if caching was disabled by @harrisonhough in [#150](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/150)

## [4.0.0] - 2023.11.01

### Breaking Changes
- Merge avatar creator into core by @ryuuk in [#135](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/135). 
- AvatarProcessor no longer searches and replaces existing avatar by @rk132 in [#138](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/138)

### Added
- show Avatar Creator sample button in guide by @ryuuk in [#141](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/141)

### Updated
- merged related samples into single folders by @harrisonhough in [#139](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/139)

## [3.4.0] - 2023.10.24

### Added
- breaking change popup @ryuuk in [#136](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/136)

### Updated
- disable use demo toggle in setup guide [#131](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/131)
- refactor define symbol add and remove logic [#133](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/133)

## [3.3.0] - 2023.10.05

### Added
- moved core iframe and url logic from WebView package @harrisonhough in [#125](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/125)

### Updated
- refactored core settings handler @harrisonhough in [#124](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/124)
- centred all editor window content @harrisonhough in [#122](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/122)

## [3.2.4] - 2023.09.28
### Fixed
- an issue causing WebView to be auto imported if removed @harrisonhough in [#126](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/126)

## [3.2.3] - 2023.09.11
### Fixed
- an issue causing settings to be recreated when not needed @harrisonhough in [#123](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/123)

## [3.2.2] - 2023.09.07

### Fixed
- an issue with module installer causing errors when importing on some Windows machines by @rYuuk in [#117](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/117)

## [3.2.1] - 2023.08.28

### Fixed
- issue of missing mesh references when prefabs were created by avatar loader window by @harrisonhough in [#109](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/109)

## [3.2.0] - 2023.08.24

### Added
- app Id is added to header of all web requests

### Fixed
- GLTF scripting define symbol not getting assigned

## [3.1.1] - 2023.08.11

### Fixed
- fixed an issue causing analytics events being sent to development environment [#102](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/102)
- re-added RPM define symbol required for supporting packages [#102](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/102)

## [3.1.0] - 2023.08.08

### Added
* personal avatar loading in quick start by @rYuuk in https://github.com/readyplayerme/rpm-unity-sdk-core/pull/97
* runtime analytics to quick start by @rYuuk in https://github.com/readyplayerme/rpm-unity-sdk-core/pull/98

## [3.0.0] - 2023.07.31

### Added
- **Breaking:** all scripts and assets from Avatar Loader [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)
- module installer now automatically removes Avatar Loader after update [#89](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/89)
- new Integration guide editor window [#91](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/91)

### Removed
- **Breaking:** all references to ReadyPlayerMe.AvatarLoader namespace [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)

### Updated
- moved GltFast dependent code behind scripting define symbol [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87) 
- avatar config processor now uses new mesh LOD parameter [#90](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/90)
- setup guide window improvements [#91](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/91)


## [1.3.0] - 2023.05.29

### Added
- import timeout to module installer [#70](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/70)
- Add new setup guide window [#71](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/71)
- added function for folder size in MB [#72](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/72)

### Fixed
- various editor window layout fixes [#73](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/73)

## [1.2.0] - 2023.04.18

### Added
- support for response codes [#62](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/62)

### Updated
- refactor of WebRequestDispatcher [#59](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/59)

### Fixed
- fixed an issue with the popup don't ask again pref was not updating correctly [#58](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/58)

## [1.1.0] - 2023.03.21

### Added
- quick start sample popup
- added operation completed event
- discussion link to README.md

### Updated
- openUPM installation added to README.md

## [1.0.0] - 2023.02.20

### Added
- optional sdk logging
- don't ask again option for update check

### Updated
- partnerSubdomainSettings refactored to a CoreSettings scriptable object

### Fixed
- core settings asset now automatically created if it is missing
- Various bug fixes and improvements

## [0.2.0] - 2023.02.08

### Added
- optional sdk logging

### Updated
- partnerSubdomainSettings refactored to a CoreSettings scriptable object

### Fixed
- various bug fixes and improvements

## [0.1.0] - 2023.01.22

### Updated
- repository names in module list + version numbers

## [0.1.0] - 2023.01.12

### Added
- inline code documentation
- Contribution guide and code of conduct
- module installer and updater for handling package installation

### Updated
- a big refactor of code and classes

### Fixed
- various bug fixes and improvements
