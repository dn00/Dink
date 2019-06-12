using Dink.Actions;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States
{
    class StateLoginScreen : State
    {
        public StateLoginScreen(IConfiguration data, Signifier s) : base(data, s)
        {
        }


        /// <summary>
        /// Macro through the login screen
        /// </summary>
        /// <param name="device">ADB Device</param>
        /// <returns>The next State. Returns null if there's an error</returns>
        public override State Run(DeviceData device)
        {
            throw new NotImplementedException();
        }
    }
}
