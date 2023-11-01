# Ready Player Me Unity SDK Core

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/readyplayerme/rpm-unity-sdk-core)](https://github.com/readyplayerme/rpm-unity-sdk-core/releases/latest) [![GitHub Discussions](https://img.shields.io/github/discussions/readyplayerme/rpm-unity-sdk-core)](https://github.com/readyplayerme/rpm-unity-sdk-core/discussions) [![Run Integration Tests](https://github.com/readyplayerme/rpm-unity-sdk-core/actions/workflows/integration-test.yml/badge.svg)](https://github.com/readyplayerme/rpm-unity-sdk-core/actions/workflows/integration-test.yml)

This is an open source Unity plugin that contains all the core functionality required for a Ready Player Me avatar integration, and manages all the plugin updates and dependencies including Ready Player Me Avatar Loader and glTFast. 

Please visit the online documentation and join our public `discord` community.

![](https://i.imgur.com/zGamwPM.png) **[Online Documentation]( https://readyplayer.me/docs )**

![](https://i.imgur.com/FgbNsPN.png) **[Discord Channel]( https://discord.gg/9veRUu2 )**

:octocat: **[GitHub Discussions]( https://github.com/readyplayerme/rpm-unity-sdk-core/discussions )**

## Quick Start
The installation steps can be found [here.](Documentation~/QuickStart.md)

## Avatar Creator

### Supported Platforms
- Windows/Mac/Linux Standalone
- Android*
- iOS*

### Sample
Steps for trying out avatar creator sample can be found [here.](Documentation~/AvatarCreatorSample.md).

### Structure
- It provides APIs for creating, customizing and loading the avatar.
- It also contains a sample which demonstrates the usage of the APIs and replicates RPM web avatar creator.
- The documentation of provided sample can be found [here.](Documentation~/SampleStructure.md)

### Customization Guide
A guide for customizing avatar creator can be found [here.](Documentation~/CustomizationGuide.md)

### Note
- [*]Camera support is only provided for Windows and WebGL, using Unity’s webcam native API.
- Unity does not have a native file picker, so we have discontinued support for this feature.
- To add support for file picker (for selfies) you have to implement it yourself

**4.** Paste in this url 

`https://github.com/readyplayerme/rpm-unity-sdk-core.git`

![paste-git-url](https://user-images.githubusercontent.com/7085672/206432731-f9e0d161-7843-4d6e-8851-47b1f3bfb3bc.png)

**5.** Click add and wait for the import process to finish.

After the process is complete you project will have imported these packages:

- **Ready Player Me Core**
- **Ready Player Me WebView**
- **glTFast**

![image](https://github.com/readyplayerme/rpm-unity-sdk-core/assets/1121080/234ff559-cb19-4b39-bb14-a5621db1811b)

## Alternate Installation

### Using Git URL

1. Navigate to your project's Packages folder and open the manifest.json file.
2. Add this line below the `"dependencies": {` line
    - ```json title="Packages/manifest.json"
      "com.readyplayerme.core": "https://github.com/readyplayerme/rpm-unity-sdk-core.git",
      ```
3. UPM should now install the package.

### OpenUPM (using command line)

1. The package is available on the [openupm registry](https://openupm.com).
2. Execute the openum command.
    - ```
      openupm add com.readyplayerme.core
      ```
### OpenUPM (using editor)

1. Open `Edit | Project Settings | Package Manager`
2. Add a new Scoped Registry (or edit the existing OpenUPM entry)
   - ```
     Name     package.openupm.com
     URL      https://package.openupm.com
     Scope(s) com.readyplayerme.core
     ```
3. Click Save (or Apply)
4. Open Window/Package Manager
5. Click +
6. Select Add package by name... or Add package from git URL...
7. Paste `com.readyplayerme.core` into name
8. Click Add

### OpenUPM (using manifest)

1. Add this to manifest 
   - ```{
     "scopedRegistries": [
        {
            "name": "package.openupm.com",
            "url": "https://package.openupm.com",
            "scopes": [
                "com.readyplayerme.core"
            ]
        }
     ],
     "dependencies": {
        "com.readyplayerme.core": "4.0.0"
       }
     }
     ```
   
