We're excited to introduce the 3.0 release of our Unity package, which merges the Avatar Loader plugin into Core for improved maintenance and package installation. Please note that this update includes breaking changes. We recommend backing up your project and consulting our documentation if you encounter any installation issues. Thank you for your continued support, and we're here to assist throughout the update process.

## Changelog

### Added
- **Breaking:** all scripts and assets from Avatar Loader [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)
- module installer now automatically removes Avatar Loader after update [#89](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/89)
- new Integration guide editor window [91](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/91)

### Removed
- **Breaking:** all references to ReadyPlayerMe.AvatarLoader namespace [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87)

### Updated
- moved GltFast dependent code behind scripting define symbol [#87](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/87) 
- avatar config processor now uses new mesh LOD parameter [#90](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/90)
- setup guide window improvements [#91](https://github.com/readyplayerme/rpm-unity-sdk-core/pull/91)
