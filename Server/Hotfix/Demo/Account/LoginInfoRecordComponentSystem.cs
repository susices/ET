namespace ET
{
    
    public class LoginInfoRecordComponentDestroySystem : DestroySystem<LoginInfoRecordComponent>
    {
        public override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDic.Clear();
        }
    }

    public static class LoginInfoRecordComponentSystem 
    {
        public static void Add(this LoginInfoRecordComponent self, long key, int value)
        {
            if (self.AccountLoginInfoDic.ContainsKey(key))
            {
                self.AccountLoginInfoDic[key] = value;
            }
            self.AccountLoginInfoDic.Add(key,value);
        }

        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDic.ContainsKey(key))
            {
                self.AccountLoginInfoDic.Remove(key);
            }
        }

        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDic.TryGetValue(key, out int value))
            {
                return value;
            }
            return -1;
        }

        public static bool IsExist(this LoginInfoRecordComponent self, long key)
        {
            return self.AccountLoginInfoDic.ContainsKey(key);
        }
    }
}