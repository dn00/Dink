using Dink.Actions;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States
{
    class StateDeadL2R : State
    {
        public StateDeadL2R(IConfiguration data, Signifier s) : base(data, s)
        {
        }

        /// <summary>
        ///  L2R is dead and needs to be launched.
        /// </summary>
        /// <param name="device">ADB Device</param>
        /// <returns>The next state</returns>
        public override State Run(DeviceData device)
        {
            throw new NotImplementedException();
        }

    }
}
