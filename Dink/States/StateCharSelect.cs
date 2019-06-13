using Dink.Actions;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States
{
    class StateCharSelect : State
    {
        public StateCharSelect(IConfiguration data) : base(data)
        {
         
            Sig = new Signifier
            {
                Color = _data["macro:"],
                X = 1,
                Y = 2
            };

            Next = new StateWeeklyRewardsDialog(data);
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
