namespace MA.Systems.Fading
{
    public interface IFadeListener
    {
        public EChannelMask ChannelMask { get; }

        public void ApplyAlpha(float alpha);
    }
}
