namespace GamePlay
{
    /// <summary>
    /// 阵营类型。
    /// </summary>
    public enum CampType
    {
        Unknown = 0,

        /// <summary>
        /// 第一玩家阵营。
        /// </summary>
        Player,

        /// <summary>
        /// 第一敌人阵营。
        /// </summary>
        Enemy,

        /// <summary>
        /// 第一中立阵营。
        /// </summary>
        Neutral,

        /// <summary>
        /// 第二玩家阵营。
        /// </summary>
        Player2,

        /// <summary>
        /// 第二敌人阵营。
        /// </summary>
        Enemy2,

        /// <summary>
        /// 第二中立阵营
        /// </summary>
        Neutral2,
    }
    public class CardTypes
    {
        //方片
        public const int Diamonds = 2;
        //梅花
        public const int Clubs = 1;
       
        //红桃
        public const int Hearts=3;
        //黑桃
        public const int Spades=4;
    }
    public enum CardManagerStates
    {
        //准备
        Ready,
        //洗牌
        ShuffleCards,
        //发牌
        DealCards,
        //出牌
        Playing
    }
}
