using System;

namespace ReadyPlayerMe.Core
{
    [Serializable]
    public class ShaderProperty
    {
        public TextureChannel TextureChannel;
        public string PropertyName;

        public ShaderProperty(TextureChannel textureChannel, string propertyName)
        {
            TextureChannel = textureChannel;
            PropertyName = propertyName;
        }
    }
}
