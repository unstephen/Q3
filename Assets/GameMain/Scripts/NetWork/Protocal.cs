public class Protocal
{
    public const byte Login = 1;     //连接服务器
    public const byte RecvLogin = 35;     //登陸
    public const byte Error = 250; //錯誤

    public const byte NOTIFY_DRAW = 18;//私有牌发给player，player可以看到自己的点数、花色。收到的其他player的牌为NOTIFY_PRIVATE，无法看见其他player的手牌信息。
    public const byte NOTIFY_PRIVATE = 19;//将牌发给一个玩家，此时牌盖上，所有人不可见。
    public const byte NOTIFY_SHARED = 20;//公共牌共享给所有玩家看。参 NOTIFY_DRAW
    public const byte NOTIFY_JOIN = 21;//用户加入当局游戏通知。
    public const byte NOTIFY_LEAVE = 22;//用户推出当局游戏通知。
    public const byte NOTIFY_CHAT = 23;//来自PID的消息
    public const byte NOTIFY_START = 24;//游戏开始
    public const byte NOTIFY_END = 25;//游戏结束
    public const byte NOTIFY_WIN = 26;//PID wins Amount.
    public const byte NOTIFY_BET = 27;//PID posts a bet
    public const byte NOTIFY_RAISE = 29;//PID raises Amount.
    public const byte NOTIFY_CALL = 30;//PID calls Amount.
    public const byte NOTIFY_STATE = 31;//Player state change.1 - playing, 2 - folded, 3 - waiting for big blind, 4 - all in, 5 - sitting out.
    public const byte NOTIFY_STAGE = 32;//一局游戏的各个阶段 1 - turn       抢庄 2 - dealer    抢庄
    
    
    
    public const byte NOTIFY_BUTTON = 34;//Seat# is the button..
    public const byte BALANCE = 43;//玩家本场游戏账户积分情况
    
    
    public const byte NOTIFY_SB = 44;//Seat# is the small blind
    public const byte NOTIFY_BB = 45;//Seat# is the big blind
}
