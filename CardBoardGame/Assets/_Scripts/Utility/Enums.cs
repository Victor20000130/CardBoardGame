namespace CardBoardGame.Assets._Scripts.Utility
{
    public enum Stage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5
    }
    public enum Difficulty
    {
        None,
        Easy,
        Normal,
        Hard
    }
    public enum BattleState
    {
        PlayerTurn,
        MonsterTurn,
        Win,
        Lose
    }
    public enum CardType
    {
        Attack,
        Defense,
        Skill
    }
    public enum GridType
    {
        Start = 0,
        Day = 1,
        Night = 2,
        PlayerHeal = 3,
        MonsterHeal = 4,
        Draw = 5,
        MiniGame = 6
    }
}