namespace CardBoardGame.Assets._Scripts.Utility
{
    public enum Stage
    {
        None = 0,
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
        None = 0,
        Start = 1,
        Day = 2,
        Night = 3,
        PlayerHeal = 4,
        MonsterHeal = 5,
        Draw = 6,
        MiniGame = 7
    }

    public enum Shape
    {
        None,
        Spade,
        Club,
        Diamond,
        Heart
    }

    public enum Number
    {
        None,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

}