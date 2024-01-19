# Avatar Creator Elements

As part of the Unity SDK 5.0.0 update we added a new set of classes we call Avatar Creator elements. 
These elements are designed to be simple and modular and can be used as simplified building blocks when it comes to
creating your own Avatar Creator UI. We tried to keep them as simple as possible so that the code in each script would
be relatively simple to understand, which will hopefully make it easier for you to create your own using the same Avatar
Creator API's we provide.

## Account Creation Element

This element is useful for creating an Account Creation UI. 

**Prefab Location:**
`Runtime/AvatarCreator/Prefabs/AccountCreationElement.prefab`.

**Key features include:**
- an input field for the user to enter their email address
- a button to send an account creation sign up email to the user 
- a button to continue without signup

If you view this AccountCreationElement in the unity inspector you will find a number of assignable fields and 2 events
that can be easily subscribed to. 
The first event OnSendEmail, is fired when the send button is clicked.
It also has a string parameter that can be used to retrieve the email address entered by the user if needed.

The second event OnContinue, is fired when the skip button is clicked.

Both of these are UnityEvents so they can be subscribed to in the inspector using the `+` symbol or in code.

## Asset Selection Element

This element, useful for creating asset selection UI panels or windows.

**Prefab Location:**
`Runtime/AvatarCreator/Prefabs/AssetSelectionElement.prefab`.

This is best used for assets of the following asset types:
- BeardStyle
- Shapes (EyeShape, LipShape, etc)
- Facewear
- Headwear
- Clothing (Outfit, Shirt, Bottom, Top, etc)
- Facemask
- Glasses

It is not recommended to use this for Color selection or Template avatar selection, for these we recommend the ColorSelectionElement & TemplateSelectionElement.

### How it works 

If you find and open the AssetSelectionElement prefab we can go through the properties you see in the inspector. 

**ButtonElementPrefab:** This is the prefab that will be used to display each asset.

**ButtonContainer:** This is the transform that the ButtonElementPrefabs will be instantiated under.

**SelectedIcon:** This is the icon that will be displayed on the selected asset to indicate to the user which one is currently selected. NOTE: This should exist already as a child of the AssetSelectionPrefab.

**OnAssetSelected:** This is a UnityEvent that will be fired when an asset is selected. It has a single parameter of type AvatarCreatorAsset. This can be used to retrieve the asset that was selected.

**BodyType:** This is the body type that the assets should be filtered by. This is used to ensure that only assets that are compatible with the selected body type are displayed. 

**AssetType:** This is the asset type that the assets should be filtered by. This is used to determine which assets should be displayed and equipped.

**IconSize:** This is the square dimension size of the icon that will be requested. NOTE: bigger sizes will result in larger file sizes.

You can add the AssetSelectionElement to your scene by following these steps.
1. Find the AssetSelectionElement prefab in the Unity Project window.
2. Drag the AssetSelectionElement prefab into your scene. NOTE: It should be placed as a child of a Canvas object as it is a UI element
3. Find the AssetSelectionElement in the hierarchy and select it to open the inspector.
4. From here you will need to set your Asset Type 
5. You can also adjust any other settings as needed. 
6. Lastly you will see an OnAssetSelected event. You can subscribe to this event by clicking the `+`.

#### OnAssetSelected event

This event is called whenever an asset button is clicked (selected) and it has a single parameter of IAssetData.
The purpose of this event is to allow you to retrieve the asset ID and AssetType from other scripts. With this data you 
can make requests to our API's to update the avatar customizations. For example changing hair styles.

## Gender Selection Element

This element is useful for creating a gender selection UI. 

**Prefab Location:**
`Runtime/AvatarCreator//Prefabs/GenderSelectionElement.prefab`.

**Key features include:**
- 2 buttons (male, female)
- A OnGenderSelected UnityEvent that can be subscribed to in the inspector or in code to retrieve the selected gender.

## Login Element

This element is useful for creating Ready Player Me login UI.

**Prefab Location:**
`Runtime/AvatarCreator//Prefabs/LoginElement.prefab`.

Key features include:
- an input field for the user to enter their email address
- an input field for the user to enter in their 1 time login code
- functionality to automatically send a login code to the user after they enter their email address
- OnLoginSuccess event that can be subscribed to in the inspector
- OnLoginFailed event that can be subscribed to in the inspector and passes a string containing the error message

## Photo Capture Element

This element is useful for creating a photo capture UI.

**Prefab Location:**
`Runtime/AvatarCreator/Prefabs/PhotoCaptureElement.prefab`.

## Selfie To Avatar Element

This a higher level element is useful for creating a UI that not only enables photo capture, it also uses the selected
photo to request an avatar before finally invoking the OnAvatarCreated event passing the GameObject and AvatarProperties.

**Prefab Location:**
`Runtime/AvatarCreator/Prefabs/SelfieToAvatarElement.prefab`.

## ButtonElement

## ColorButton




