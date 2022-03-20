namespace ET
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze = 1,
    }
    
    public class RoleInfo : Entity,IAwake
    {
        public string Name;

        public int ServerId;

        public int Status;

        public long AccountId;

        public long LastLoginTime;

        public long CreateTime;
    }
}