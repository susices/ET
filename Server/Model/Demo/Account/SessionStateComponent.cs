namespace ET
{

    public enum SessionState
    {
        Normal,
        Game,
    }
    
    public class SessionStateComponent : Entity,IAwake
    {
        public SessionState State;
    }
}