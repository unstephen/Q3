
public class Packet
{
    public int code;
    public int state;
    public WS_MsgBase args;
    public Packet()
    {

    }

    public Packet(int code, WS_MsgBase args)
    {
        this.code = code;
        this.args = args;
    }

    public override string ToString()
    {
        return string.Format("[Packet: code={0}, state={1}, args={2}]", code, state, args);
    }
}

