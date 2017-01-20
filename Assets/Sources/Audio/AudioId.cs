namespace CreatingDust.GGJ2017.CrossContext.Services
{
    public enum AudioId
    {
        None = 0,

        UIContinueButtonPressed = 1,
        UIButtonPressed = 2,
        UICoinAdded = 3,

        GameZombieAttacks = 100,
        GameHeroHeartbeat = 102,
        GameZombieKilled = 103,
        GameDrawLine = 120,
        GameResolveItems = 125,
        GameNewWave = 130,
        GameSuperTriggeredDynamite = 140,
        GameSuperTriggeredDissolveColors = 141,
        GameSpecialTriggered = 150,
        GameMatch00 = 160,
        GameMatch01 = 161,
        GameMatch02 = 162,
        GameMatch03 = 163,
        GameMatch04 = 164,
        GameMatch05 = 165,
        GameMatch06 = 166,
        GameMatch07 = 167,
        GameMatch08 = 168,

        GameGameOver = 190,

        BackgroundMain = 200,
        BackgroundGame = 201,
    }
}