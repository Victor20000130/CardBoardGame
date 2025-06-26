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
        Buff = 6,
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
    public enum HandlerType
    {
        None,
        BattleHandler,
        CardHandler,
        GameUIHandler,
        GridHandler,
        PieceHandler,
        StageHandler,
        DiceHandler
    }
    public enum PokerHands
    {
        Solo,           //숫자가 가장 높은 카드 1장
        Dyad,           //숫자가 같은 카드 2장
        Dyad_Set,       //다이어드 2개
        Triad,          //숫자가 같은 카드 3장
        Tetrad,         //숫자가 같은 카드 4장
        Soma,           //같은 무늬 카드 5장
        Legion,         //숫자가 같은 카드 3장과, 2장
        Nemesis,        //무늬가 같고 숫자가 연속되는 카드 5장
        Atropos,        //무늬가 다른, 최고 또는 최소 숫자카드 4장
        Aion            //무늬가 같은 최고 숫자카드 5장
    }
}