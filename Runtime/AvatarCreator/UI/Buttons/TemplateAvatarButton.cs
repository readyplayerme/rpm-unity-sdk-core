namespace ReadyPlayerMe.AvatarCreator
{
    public class TemplateAvatarButton : SimpleAssetButton
    {
        public TemplateData TemplateData { get; private set; }

        public void SetTemplateData(TemplateData templateData)
        {
            TemplateData = templateData;
            RawImage.texture = templateData.Texture;
            RawImage.SizeToParent();
        }
    }
}
