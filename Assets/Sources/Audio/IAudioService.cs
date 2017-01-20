namespace CreatingDust.GGJ2017.CrossContext.Services
{
    public interface IAudioService
    {
        bool fxEnabled { get; set; }
        bool musicEnabled { get; set; }

        void Setup();
        void Play(AudioId id);
        void SetBackground(AudioId id);
    }
}