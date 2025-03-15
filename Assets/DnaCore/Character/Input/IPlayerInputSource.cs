namespace DnaCore.Character.Input
{
    public interface IPlayerInputSource
    {
        CharacterInput FixedInput { get; }
        CharacterInput Input { get; }
        void Enable();
        void Disable();
    }
}
