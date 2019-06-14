using Dink.Actions;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States
{
    class StateInGameTabbedMenu : State
    {
        public StateInGameTabbedMenu(IConfiguration data) : base(data)
        {
        }

        public override bool IsState(DeviceData device)
        {
            throw new NotImplementedException();
        }

        public override bool Run(DeviceData device)
        {
            throw new NotImplementedException();
        }
    }
}
