using Dink.Actions.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.Actions
{
    public class ActionHandler
    {
        private static ushort resultCode;
        public static ushort DoAction(String noxName, ActionType actionType)
        {
            switch(actionType)
            {
                case ActionType.Respawn:
                    {
                        // call action respawn
                        resultCode = 0;
                        break;
                    }
            }

            return resultCode;
        }
    }
}
