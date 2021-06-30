using System;

namespace ET
{
    
    public class BaseBuffActionAttribute: BaseAttribute
    {
        public int Id;
        public BaseBuffActionAttribute(int BaseBuffActionId)
        {
            this.Id = BaseBuffActionId;
        }
    }
}