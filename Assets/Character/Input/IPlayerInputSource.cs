namespace Character.Input
{
    public interface IPlayerInputSource
    {
        PlayerInput PlayerInput { get; }
        void Enable();
        void Disable();
    }
}
