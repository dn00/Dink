﻿using Dink.Actions;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States
{
    class StateInGameMainScreen : State
    {
        public StateInGameMainScreen(IConfiguration data, Signifier s) : base(data, s)
        {
        }

        public override State Run(DeviceData device)
        {
            throw new NotImplementedException();
        }
    }
}