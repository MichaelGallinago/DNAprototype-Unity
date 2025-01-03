namespace Utilities
{
    //TODO: init storage
    public static class Storage
    {
        //TODO: use serialized file and recreate from menu 
        public static readonly GameOptions GameOptions = new(GameOptions.BaseFrameRate, GameOptions.BaseFrameRate);
    }
}
