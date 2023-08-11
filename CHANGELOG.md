# Changelog

All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [3.1.1] - 2023.08.11

### Fixed
- fixed an issue causing analytics events being sent to development environmenmt [#102](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/102)
- readded RPM define symbol required for supporting packages [#102](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/102)

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
- OpenUPM installation added to README.md

## [1.0.0] - 2023.02.20

### Added
- optional sdk logging
- don't ask again option for update check

### Updated
- PartnerSubdomainSettings refactored to a CoreSettings scriptable object

### Fixed
- core settings asset now automatically created if it is missing
- Various bug fixes and improvements

## [0.2.0] - 2023.02.08

### Added
- optional sdk logging

### Updated
- PartnerSubdomainSettings refactored to a CoreSettings scriptable object

### Fixed
- Various bug fixes and improvements

## [0.1.0] - 2023.01.22

### Updated
- repository names in module list + version numbers

## [0.1.0] - 2023.01.12

### Added
- inline code documentation
- Contribution guide and code of conduct
- module installer and updater for handling package installation

### Updated
- A big refactor of code and classes

### Fixed
- Various bug fixes and improvements
