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
        // Ember
        Spade,
        // Spray
        Diamond,
        // Nuri
        Heart,
        // Fair_Wind
        Club
    }

    public enum Number
    {
        None,
        Ace,
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
        DiceHandler,
        MiniGameHandler
    }
    public enum HandRankings
    {
        None,
        Solo,           //노페어
        Dyad,           //원페어
        Dyad_Set,       //투페어
        Triad,          //트리플
        Hermes,         //스트레이트
        Ananke,         //백스트레이트
        Atlas,          //마운틴
        Soma,           //플러쉬
        Legion,         //풀하우스
        Tetrad,         //포카드
        Nemesis,        //스트레이트 플러쉬
        Atropos,        //백 스트레이트 플러쉬
        Aion            //로얄 스트레이트 플러쉬
    }

    public enum ElementType
    {
        None,
        Embers,
        Spray,
        Nuri,
        Fair_Wind
    }

    public enum EffectType
    {
        None,
        Attack,
        Heal,
        ShieldBaseCurrentHP,
        ShieldBaseLostHP,
        AdditionalCard,
        ThrowCount
    }
    public enum Operator
    {
        None,
        Plus,
        Minus,
        Percent,
        Divide,
        Multiply
    }
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public enum TZFZPuzzle
{
    None,
    SmallPuzzle,
    MediumPuzzle,
    LargePuzzle,
    SuperPuzzle,
    MegaPuzzle,
    HyperPuzzle
}