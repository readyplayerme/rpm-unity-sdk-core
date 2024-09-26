using UnityEngine.Rendering;

namespace ReadyPlayerMe.Core.Editor
{
 public static class ShaderVariantConstants
    {
     public static ShaderVariantData[] Variants = new [] 
 {
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN _ALPHATEST_ON" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN _EMISSION _METALLICGLOSSMAP" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN _EMISSION _METALLICGLOSSMAP _NORMALMAP" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN _METALLICGLOSSMAP _NORMALMAP" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN _METALLICGLOSSMAP _NORMALMAP _OCCLUSION" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH SHADOWS_SCREEN _NORMALMAP" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ForwardBase, "DIRECTIONAL LIGHTPROBE_SH _ALPHABLEND_ON" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ShadowCaster, "SHADOWS_DEPTH" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ShadowCaster, "SHADOWS_DEPTH _ALPHABLEND_ON" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ShadowCaster, "SHADOWS_DEPTH _ALPHATEST_ON" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ShadowCaster, "SHADOWS_DEPTH _METALLICGLOSSMAP" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ShadowCaster, "SHADOWS_DEPTH _METALLICGLOSSMAP _OCCLUSION" ),
            new ShaderVariantData("glTF/PbrMetallicRoughness", PassType.ShadowCaster, "_METALLICGLOSSMAP _OCCLUSION" ),
     };
   }
}
