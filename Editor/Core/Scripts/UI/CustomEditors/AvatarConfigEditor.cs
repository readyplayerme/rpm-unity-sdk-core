using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.Core.Editor
{
    [CustomEditor(typeof(AvatarConfig))]
    public class AvatarConfigEditor : UnityEditor.Editor
    {
        private const string DIALOG_TITLE = "Read Player Me";
        private const string DIALOG_MESSAGE = "Do you want to install {0} Unity Package: {1} ?";
        private const string DIALOG_OK = "Ok";
        private const string DIALOG_CANCEL = "Cancel";
        private const string ADD_MORPH_TARGET = "Add Morph Target";
        private const string DELETE_MORPH_TARGET = "Delete Morph Target";
        private const string REMOVE_BUTTON_TEXT = "Remove";
        private const string MESH_OPT_PACKAGE_NAME = "com.unity.meshopt.decompress";

        [SerializeField] private VisualTreeAsset visualTreeAsset;

        private AvatarConfig avatarConfigTarget;
        private List<Label> morphTargetLabels;

        private Dictionary<VisualElement, string> morphTargetsParentVisualElement;

        private VisualElement selectedMorphTargets;

        private SerializedProperty useDracoCompressionField;
        private bool previousDracoCompressionValue;

        private SerializedProperty useMeshOptCompressionField;
        private bool previousMeshOptCompressionValue;

        private VisualElement root;
        private Action textureChannelChanged;

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement();
            visualTreeAsset.CloneTree(root);

            avatarConfigTarget = (AvatarConfig) target;

            SetupLod();
            SetupPose();
            SetupTextureAtlas();
            SetupTextureSizeLimit();
            SetupUseHands();
            SetupCompressionPackages();
            SetupTextureChannel();
            SetupMorphTargets();
            SetupShader();
            return root;
        }

        private void SetupLod()
        {
            var lod = root.Q<EnumField>("Lod");
            lod.SetValueWithoutNotify(avatarConfigTarget.Lod);
            lod.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.Lod = (Lod) x.newValue;
                    Save();
                }
            );
        }

        private void SetupPose()
        {
            var pose = root.Q<EnumField>("Pose");
            pose.SetValueWithoutNotify(avatarConfigTarget.Pose);
            pose.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.Pose = (Pose) x.newValue;
                    Save();
                }
            );
        }

        private void Save()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(serializedObject.targetObject);
        }

        private void SetupTextureAtlas()
        {
            var textureAtlas = root.Q<EnumField>("TextureAtlas");
            textureAtlas.SetValueWithoutNotify(avatarConfigTarget.TextureAtlas);
            textureAtlas.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.TextureAtlas = (TextureAtlas) x.newValue;
                    Save();
                }
            );
        }

        private void SetupTextureSizeLimit()
        {
            var textureSizeLimit = root.Q<SliderInt>("TextureSizeLimit");
            textureSizeLimit.SetValueWithoutNotify(avatarConfigTarget.TextureSizeLimit);
            textureSizeLimit.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.TextureSizeLimit = x.newValue;
                    Save();
                }
            );
        }

        private void SetupUseHands()
        {
            var useHands = root.Q<Toggle>("UseHands");
            useHands.SetValueWithoutNotify(avatarConfigTarget.UseHands);
            useHands.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.UseHands = x.newValue;
                    Save();
                }
            );
        }

        private void SetupCompressionPackages()
        {
            var useDracoCompression = root.Q<Toggle>("UseDracoCompression");
            var useMeshOptCompression = root.Q<Toggle>("UseMeshOptCompression");

            var optimizationPackages = root.Q<Foldout>("OptimizationPackages");
            optimizationPackages.RegisterValueChangedCallback(x =>
            {
                useDracoCompression.SetValueWithoutNotify(ModuleInstaller.IsModuleInstalled(ModuleList.DracoCompression.name));
                useMeshOptCompression.SetValueWithoutNotify(PackageManagerHelper.IsPackageInstalled(MESH_OPT_PACKAGE_NAME));
            });

            useDracoCompression.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.UseDracoCompression = x.newValue;
                    if (ModuleInstaller.IsModuleInstalled(ModuleList.DracoCompression.name))
                    {
                        return;
                    }

                    if (EditorUtility.DisplayDialog(
                            DIALOG_TITLE,
                            string.Format(DIALOG_MESSAGE, "Draco compression", ModuleList.DracoCompression.name),
                            DIALOG_OK,
                            DIALOG_CANCEL))
                    {
                        ModuleInstaller.AddModuleRequest(ModuleList.DracoCompression.Identifier);
                    }
                    else
                    {
                        avatarConfigTarget.UseDracoCompression = false;
                        useDracoCompression.SetValueWithoutNotify(false);
                    }

                    Save();
                }
            );

            useMeshOptCompression.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.UseMeshOptCompression = x.newValue;
                    if (PackageManagerHelper.IsPackageInstalled(MESH_OPT_PACKAGE_NAME))
                    {
                        return;
                    }

                    if (EditorUtility.DisplayDialog(
                            DIALOG_TITLE,
                            string.Format(DIALOG_MESSAGE, "Mesh opt compression", MESH_OPT_PACKAGE_NAME),
                            DIALOG_OK,
                            DIALOG_CANCEL))
                    {
                        PackageManagerHelper.AddPackage(MESH_OPT_PACKAGE_NAME);
                    }
                    else
                    {
                        avatarConfigTarget.UseMeshOptCompression = false;
                        useMeshOptCompression.SetValueWithoutNotify(false);
                    }

                    Save();
                }
            );
        }

        private void SetupTextureChannel()
        {
            var items = new List<string>();
            foreach (TextureChannel textureChannel in Enum.GetValues(typeof(TextureChannel)))
            {
                items.Add(textureChannel.ToString());
            }

            VisualElement MakeItem()
            {
                var toggle = new Toggle();
                toggle.style.alignItems = Align.Center;
                return toggle;
            }

            void BindItem(VisualElement e, int i)
            {
                var toggle = (Toggle) e;
                toggle.label = items[i];
                if (avatarConfigTarget.TextureChannel.Contains((TextureChannel) i))
                {
                    toggle.SetValueWithoutNotify(true);
                }

                toggle.RegisterValueChangedCallback(x =>
                {
                    if (x.newValue)
                    {
                        var textureChannels = avatarConfigTarget.TextureChannel.ToList();
                        textureChannels.Add((TextureChannel) i);
                        avatarConfigTarget.TextureChannel = textureChannels.ToArray();
                        textureChannelChanged?.Invoke();
                    }
                    else
                    {
                        var textureChannels = avatarConfigTarget.TextureChannel.ToList();
                        textureChannels.Remove((TextureChannel) i);
                        avatarConfigTarget.TextureChannel = textureChannels.ToArray();
                        textureChannelChanged?.Invoke();
                    }
                    Save();
                });
            }

            var listView = root.Q<ListView>();
            listView.style.height = 30 * Enum.GetValues(typeof(TextureChannel)).Length + 2;
            listView.makeItem = MakeItem;
            listView.bindItem = BindItem;
            listView.itemsSource = items;
            listView.selectionType = SelectionType.Multiple;

            listView.onItemsChosen += Debug.Log;
            listView.onSelectionChange += Debug.Log;
        }

        private void SetupShader()
        {
            var shader = root.Q<ObjectField>("ShaderOverride");
            shader.SetValueWithoutNotify(avatarConfigTarget.Shader);

            var shaderPropertiesContainer = root.Q<VisualElement>("ShaderProperties");
            CreateShaderProperties(shaderPropertiesContainer);

            textureChannelChanged += () => ShowShaderProperties(shaderPropertiesContainer);
            if (shader.value == null)
            {
                shaderPropertiesContainer.style.display = DisplayStyle.None;
            }
            else
            {
                ShowShaderProperties(shaderPropertiesContainer);
            }

            shader.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.Shader = (Shader) x.newValue;
                    Save();
                    if (x.newValue == null)
                    {
                        shaderPropertiesContainer.style.display = DisplayStyle.None;
                    }
                    else
                    {
                        ShowShaderProperties(shaderPropertiesContainer);
                    }
                }
            );
        }

        private void ShowShaderProperties(VisualElement shaderPropertiesContainer)
        {
            shaderPropertiesContainer.style.display = DisplayStyle.Flex;
            foreach (var child in shaderPropertiesContainer.Children())
            {
                if (avatarConfigTarget.TextureChannel.Contains((TextureChannel) Enum.Parse(typeof(TextureChannel), child.name)))
                {
                    child.style.display = DisplayStyle.Flex;
                }
                else
                {
                    child.style.display = DisplayStyle.None;
                }
            }
        }

        private void CreateShaderProperties(VisualElement shaderPropertiesContainer)
        {
            foreach (TextureChannel textureChannel in Enum.GetValues(typeof(TextureChannel)))
            {
                var field = new TextField(textureChannel.ToString());
                field.name = textureChannel.ToString();
                var property = avatarConfigTarget.ShaderProperties.FindIndex(x => x.TextureChannel == textureChannel);
                field.SetValueWithoutNotify(avatarConfigTarget.ShaderProperties[property].PropertyName);
                field.RegisterValueChangedCallback(x =>
                {
                    avatarConfigTarget.ShaderProperties[property].PropertyName = x.newValue;
                    Save();
                });
                shaderPropertiesContainer.Add(field);
            }
        }

        private void SetupMorphTargets()
        {
            morphTargetLabels = AvatarMorphTarget.MorphTargetAvatarAPI.Select(x => new Label(x)).ToList();
            morphTargetsParentVisualElement = new Dictionary<VisualElement, string>();
            selectedMorphTargets = root.Q<VisualElement>("SelectedMorphTargets");

            for (var i = 0; i < avatarConfigTarget.MorphTargets.Count; i++)
            {
                var defaultIndex = AvatarMorphTarget.MorphTargetAvatarAPI.IndexOf(avatarConfigTarget.MorphTargets[i]);
                CreateNewElement(defaultIndex);
            }

            var addButton = root.Q<Button>("AddButton");
            addButton.clicked += OnAddButtonClicked;
        }

        private void OnAddButtonClicked()
        {
            Undo.RecordObject(avatarConfigTarget, ADD_MORPH_TARGET);
            avatarConfigTarget.MorphTargets.Add(AvatarMorphTarget.MorphTargetAvatarAPI[0]);
            EditorUtility.SetDirty(avatarConfigTarget);
            CreateNewElement(0);
        }

        private void CreateNewElement(int popFieldDefaultIndex)
        {
            var parent = new VisualElement();
            parent.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            parent.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

            morphTargetsParentVisualElement.Add(parent, AvatarMorphTarget.MorphTargetAvatarAPI[popFieldDefaultIndex]);
            parent.Add(CreatePopupField(popFieldDefaultIndex, parent));
            parent.Add(CreateRemoveButton(parent));
            selectedMorphTargets.Add(parent);
        }

        private PopupField<Label> CreatePopupField(int defaultIndex, VisualElement parent)
        {
            return new PopupField<Label>(string.Empty,
                morphTargetLabels,
                defaultIndex,
                x =>
                {
                    avatarConfigTarget.MorphTargets[GetIndex(morphTargetsParentVisualElement[parent])] = x.text;
                    morphTargetsParentVisualElement[parent] = x.text;
                    return x.text;
                },
                x => x.text);
        }

        private VisualElement CreateRemoveButton(VisualElement parent)
        {
            var removeButton = new Button(() =>
            {
                Undo.RecordObject(avatarConfigTarget, DELETE_MORPH_TARGET);
                avatarConfigTarget.MorphTargets.RemoveAt(GetIndex(morphTargetsParentVisualElement[parent]));
                selectedMorphTargets.Remove(parent);
                EditorUtility.SetDirty(avatarConfigTarget);
            });
            removeButton.text = REMOVE_BUTTON_TEXT;
            return removeButton;
        }

        private int GetIndex(string morphTarget)
        {
            return avatarConfigTarget.MorphTargets.FindIndex(x => x == morphTarget);
        }
    }
}
