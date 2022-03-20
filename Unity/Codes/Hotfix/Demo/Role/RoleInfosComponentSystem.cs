namespace ET
{
    public class RoleInfosComponentDestroySystem: DestroySystem<RoleInfosComponent>
    {
        public override void Destroy(RoleInfosComponent self)
        {
            self.ClearRoleInfos();
            self.CurrentRoleId = 0;
        }
    }

    public static class RoleInfosComponentSystem
    {
        public static void ClearRoleInfos(this RoleInfosComponent self)
        {
            foreach (var roleInfo in self.RoleInfos)
            {
                roleInfo?.Dispose();
            }
            self.RoleInfos.Clear();
        }

        public static void Add(this RoleInfosComponent self, RoleInfoProto roleInfoProto)
        {
            RoleInfo roleInfo = self.AddChild<RoleInfo>();
            roleInfo.FromMessage(roleInfoProto);
            self.RoleInfos.Add(roleInfo);
        }

        public static void Remove(this RoleInfosComponent self, long roleId)
        {
            RoleInfo removeRoleInfo = null;
            foreach (var roleInfo in self.RoleInfos)
            {
                if (roleInfo.Id == roleId)
                {
                    removeRoleInfo = roleInfo;
                    break;
                }
            }

            if (removeRoleInfo==null)
            {
                Log.Error($"删除角色名不存在 ：{roleId}");
                return;
            }
            
            removeRoleInfo.Dispose();
            self.RoleInfos.Remove(removeRoleInfo);
        }
    }
    
}