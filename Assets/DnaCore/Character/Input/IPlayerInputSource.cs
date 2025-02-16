namespace DnaCore.Character.Input
{
    public interface IPlayerInputSource
    {
        PlayerInput FixedInput { get; }
        PlayerInput Input { get; }
        void Enable();
        void Disable();
    }
}
