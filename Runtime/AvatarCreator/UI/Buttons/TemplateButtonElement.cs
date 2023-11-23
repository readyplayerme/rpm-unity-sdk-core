namespace ReadyPlayerMe.AvatarCreator
{
    public class TemplateButtonElement : ButtonElement
    {
        public TemplateData TemplateData { get; private set; }

        public void SetTemplateData(TemplateData templateData)
        {
            TemplateData = templateData;
            SetIcon(templateData.Texture);
        }
    }
}
