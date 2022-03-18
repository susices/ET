namespace ET
{
    public enum AccountType
    {
        General = 0,
        
        Blacklist =1,
    }
    
    public class Account : Entity, IAwake
    {
        public string AccountName;

        public string Password;

        public long CreateTime;

        public int AccountType;
    }
}