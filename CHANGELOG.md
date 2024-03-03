# Changelog

All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [6.1.0] - 2024.03.04

### Updated
- AvatarMeshHelper now supports multiple mesh and material transfer. [#241](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/241)

### Added
- GetHeadMeshes method to AvatarMeshHelper to get head related meshes from an avatar. [#242](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/242)
- Template avatar with all possible meshes is added to the Resources folder.

## [6.0.1] - 2024.02.26

### Updated

- updated default render settings to fix an issue causing incorrect halfbody avatar renders [#238](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/238)

## [6.0.0] - 2024.02.19

### Updated

- **BREAKING: Renamed Avatar Create Samples** [#210](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/210) _This change
may require updates to existing references in your projects._
- Small fix for button icon resizing [#215](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/215)
- LoginWithCode can now merge avatars into RPM account [#219](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/219)
- Recover hair when headwear is removed [#224](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/224)
- Added extra check to prevent settings override [#226](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/226)

### Added

- Logout Element for Avatar Creator [#216](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/216)
- New iFrame Events to WebFrameHandler [#212](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/212)
- Added support for XR Avatar skeleton [#217](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/217)
- Added Avatar List element [#218](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/218)
- Account creation and login elements in Avatar Creator sample [#230](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/230)

### Removed

- Quickstart Parameter from UrlConfig [#221](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/221)
- Selfie to Avatar Element* [#220](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/220)
- Removed WebView auto installation [#208](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/208)

## [5.0.0] - 2024.01.12

### Updated

- Refactor and extracted shared logic from network
  packages [#148](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/148)
- Replaced api urls in samples with models urls [#152](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/152)
- Added com.unity.cloud.gltfast as a dependency and removed auto install of gltfast from git
  url [#155](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/155)
- Updated to GLTFast 6.0.1 [#157](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/157)
- Replaced use of ienumerator coroutines with
  async/await [#172](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/172)
- Request class names updated to be more uniform  [#173](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/173)
- Endpoint classes removed and refactored  [#174](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/174)
- Added avatar creator POC sample using new
  elements  [#182](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/182)
- Removed "I don't have an account" checkbox from setup
  guide [#184](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/184)
- Restructure of avatar creator samples  [#185](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/185)
- Class and folder restructure to match Unity package
  standards [#190](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/190)
- Namespaces added to some scripts to meet asset store
  requirements [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)
- Ready Player Me top toolbar menu is under `Tools/Ready Player Me` to comply with Asset Store
  requirements [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)
- Samples renamed for Asset Store version of package and paths updated
  accordingly [#198](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/198)
- Quick start sample animations updated [#200](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/200)
- Draco compression package version updated [#202](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/202)

### Added

- Add gender select element for Avatar Creator [#159](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/159)
- Basic login element for Avatar Creator [#160](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/160)
- Photo capture element for Avatar Creator [#162](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/162)
- Avatar template element for Avatar Creator [#164](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/164)
- Selfie element for Avatar Creator [#166](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/166)
- Asset panel element for Avatar Creator [#175](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/175)
- Account creation element for Avatar Creator [#178](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/178)
- Avatar preview element for Avatar Creator [#181](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/181)

- Fixed some issues related to paths like in the Graphics Setting
  Utility [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)
- Shader override property added to avatar config [#199](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/199)

### Fixed

- Fix for handling pasted url text in subdomain
  field [#183](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/183)
- Added permission and orientation fix to photo capture
  element [#192](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/192)
- Added Panel Switcher clear functionality to fix issues related to relaunching the
  Creator [#194](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/194)
- Namespaces added to some scripts to meet asset store
  requirements [#195](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/195)

## [4.1.2] - 2023.12.20

### Fixed

- Add preserve attribute to CategoryConverter in
  AvatarCreator [#193](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/193)

## [4.1.1] - 2023.11.29

### Fixed

- Fixed json converters in AvatarCreator getting stripped on android or webgl
  builds [#188](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/188)

## [4.1.0] - 2023.11.29

### Updated

- Replaced API URLs with model URLs for shortcodes [#152](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/152)
- Updated render api and samples [#147](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/147)

### Added

- Added app id to setup guide [#145](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/145)

## [4.0.1] - 2023.11.14

### Fixed

- Fixed an issue causing avatars to be stored locally even if caching was
  disabled [#150](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/150)

## [4.0.0] - 2023.11.01

### Breaking Changes

- Merge avatar creator into core [#135](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/135).
- AvatarProcessor no longer searches and replaces existing
  avatar [#138](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/138)

### Added

- Show Avatar Creator sample button in guide [#141](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/141)

### Updated

- Merged related samples into single folders  [#139](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/139)

## [3.4.0] - 2023.10.24

### Added

- Breaking change popup  [#136](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/136)

### Updated

- Disable use demo toggle in setup guide [#131](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/131)
- Refactor define symbol add and remove logic [#133](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/133)

## [3.3.0] - 2023.10.05

### Added

- Moved core iframe and url logic from WebView
  package  [#125](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/125)

### Updated

- Refactored core settings handler  [#124](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/124)
- Centred all editor window content  [#122](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/122)

## [3.2.4] - 2023.09.28

### Fixed

- An issue causing WebView to be auto imported if
  removed  [#126](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/126)

## [3.2.3] - 2023.09.11

### Fixed

- An issue causing settings to be recreated when not
  needed  [#123](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/123)

## [3.2.2] - 2023.09.07

### Fixed

- An issue with module installer causing errors when importing on some Windows
  machines  [#117](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/117)

## [3.2.1] - 2023.08.28

### Fixed

-Issue of missing mesh references when prefabs were created by avatar loader
  window  [#109](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/109)

## [3.2.0] - 2023.08.24

### Added

- App Id is added to header of all web requests

### Fixed

- GLTF scripting define symbol not getting assigned

## [3.1.1] - 2023.08.11

### Fixed

- Fixed an issue causing analytics events being sent to development
  environment [#102](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/102)
- Re-added RPM define symbol required for supporting
  packages [#102](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/102)

## [3.1.0] - 2023.08.08

### Added

* Personal avatar loading in quick start  https://github.com/readyplayerme/rpm-unity-sdk-core/pull/97
* Runtime analytics to quick start  https://github.com/readyplayerme/rpm-unity-sdk-core/pull/98

## [3.0.0] - 2023.07.31

### Added

- **BREAKING: All scripts and assets from Avatar
  Loader** [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)
- Module installer now automatically removes Avatar Loader after
  update [#89](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/89)
- New Integration guide editor window [#91](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/91)

### Removed

- **BREAKING: all references to ReadyPlayerMe.AvatarLoader namespace** [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)

### Updated

- Moved GltFast dependent code behind scripting define
  symbol [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)
- Avatar config processor now uses new mesh LOD
  parameter [#90](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/90)
- Setup guide window improvements [#91](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/91)

## [1.3.0] - 2023.05.29

### Added

- Import timeout to module installer [#70](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/70)
- Add new setup guide window [#71](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/71)
- Added function for folder size in MB [#72](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/72)

### Fixed

- Various editor window layout fixes [#73](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/73)

## [1.2.0] - 2023.04.18

### Added

- Support for response codes [#62](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/62)

### Updated

- Refactor of WebRequestDispatcher [#59](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/59)

### Fixed

- Fixed an issue with the popup don't ask again pref was not updating
  correctly [#58](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/58)

## [1.1.0] - 2023.03.21

### Added

- Quick start sample popup
- Added operation completed event
- Discussion link to README.md

### Updated

- OpenUPM installation added to README.md

## [1.0.0] - 2023.02.20

### Added

- Optional sdk logging
- Don't ask again option for update check

### Updated

- PartnerSubdomainSettings refactored to a CoreSettings scriptable object

### Fixed

- Core settings asset now automatically created if it is missing
- Various bug fixes and improvements

## [0.2.0] - 2023.02.08

### Added

- Optional sdk logging

### Updated

- PartnerSubdomainSettings refactored to a CoreSettings scriptable object

### Fixed

- Various bug fixes and improvements

## [0.1.0] - 2023.01.22

### Updated

- Repository names in module list + version numbers

## [0.1.0] - 2023.01.12

### Added

- Inline code documentation
- Contribution guide and code of conduct
- Module installer and updater for handling package installation

### Updated

- A big refactor of code and classes

### Fixed

- Various bug fixes and improvements
