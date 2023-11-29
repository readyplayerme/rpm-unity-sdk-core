namespace ReadyPlayerMe.AvatarCreator
{
    public interface IMouseInput
    {
        bool GetButtonDown(int index);
        bool GetButtonUp(int index);
        bool GetButtonPressed(int index);
    }
}
