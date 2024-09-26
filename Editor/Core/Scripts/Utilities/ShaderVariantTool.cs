using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ReadyPlayerMe.Core.Editor
{
    [System.Serializable]
    public struct ShaderVariantData
    {
        public string ShaderName;
        public PassType PassType;
        public string Keywords;

        public ShaderVariantData(string shaderName, PassType passType, string keywords)
        {
            ShaderName = shaderName;
            PassType = passType;
            Keywords = keywords;
        }
    }

    public class ShaderVariantTool
    {
         public List<ShaderVariantData> ShaderVariants { get; private set; } = new List<ShaderVariantData>();

        /// <summary>
        /// Reads the shader variants from a ShaderVariantCollection asset.
        /// </summary>
        /// <param name="variantCollection">The ShaderVariantCollection asset to load from.</param>
        public void ReadShaderVariantsFromCollection(ShaderVariantCollection variantCollection)
        {
            ShaderVariants.Clear(); // Clear any existing data
            try
            {
                var serializedObject = new SerializedObject(variantCollection);
                var shaderArrayProperty = serializedObject.FindProperty("m_Shaders");

                if (shaderArrayProperty == null)
                {
                    Debug.Log("Error: Unable to find Shaders in the ShaderVariantCollection.");
                    return;
                }

                for (int i = 0; i < shaderArrayProperty.arraySize; i++)
                {
                    var shaderProperty = shaderArrayProperty.GetArrayElementAtIndex(i);
                    var shaderReferenceProp = shaderProperty.FindPropertyRelative("first");
                    var shader = shaderReferenceProp.objectReferenceValue as Shader;

                    if (shader == null)
                    {
                        Debug.LogError("Shader not found.");
                        continue;
                    }

                    var variantsProperty = shaderProperty.FindPropertyRelative("second.variants");
                    for (int j = 0; j < variantsProperty.arraySize; j++)
                    {
                        var variantProperty = variantsProperty.GetArrayElementAtIndex(j);
                        var passTypeProperty = variantProperty.FindPropertyRelative("passType");
                        var keywordsProperty = variantProperty.FindPropertyRelative("keywords");

                        var passType = (PassType)passTypeProperty.intValue;

                        ShaderVariants.Add(new ShaderVariantData
                        {
                            ShaderName = shader.name,
                            PassType = passType,
                            Keywords = keywordsProperty.stringValue
                        });
                    }
                }

                Debug.Log($"Successfully loaded {ShaderVariants.Count} shader variants.");
            }
            catch
            {
                Debug.LogError("An error occurred while reading the ShaderVariantCollection.");
            }
        }

        /// <summary>
        /// Exports the shader variants to a script for hard-coding.
        /// </summary>
        public void ExportShaderVariantsToFile(string outputPath)
        {
            List<string> lines = new List<string>();
            lines.Add("using UnityEngine.Rendering;\n");
            lines.Add("namespace ReadyPlayerMe.Core.Editor\n{");
            lines.Add(" public static class ShaderVariantConstants\n    {");
            lines.Add("     public static ShaderVariantData[] Variants = new [] \n {");

            foreach (ShaderVariantData variant in ShaderVariants)
            {
                var keywordsString = variant.Keywords.Length > 0 ? $"\"{string.Join("\", \"", variant.Keywords)}\"" : "\"\"";
                lines.Add($"            new ShaderVariantData(\"{variant.ShaderName}\", PassType.{variant.PassType}, {keywordsString} ),");
            }

            lines.Add("     };");
            lines.Add("   }");
            lines.Add("}");

            File.WriteAllLines(outputPath, lines.ToArray());
            AssetDatabase.Refresh();
            Debug.Log($"Shader variants exported to {outputPath}");
        }

        /// <summary>
        /// Creates a new .shadervariants file based on the shader variants list.
        /// </summary>
        public void CreateNewShaderVariantsFile(string outputPath)
        {
            ShaderVariantCollection newCollection = new ShaderVariantCollection();

            foreach (ShaderVariantData variantData in ShaderVariantConstants.Variants)
            {
                Shader shader = Shader.Find(variantData.ShaderName);
                if (shader == null)
                {
                    Debug.LogError($"Shader not found: {variantData.ShaderName}");
                    continue;
                }

                var keywordsArray = variantData.Keywords.Split(' ');
                ShaderVariantCollection.ShaderVariant newVariant = new ShaderVariantCollection.ShaderVariant(
                    shader,
                    variantData.PassType,
                    keywordsArray);
                newCollection.Add(newVariant);
            }

            AssetDatabase.CreateAsset(newCollection, outputPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"New .shadervariants file created at {outputPath}");
        }
    }
}