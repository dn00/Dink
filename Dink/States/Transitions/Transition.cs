using Dink.States;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States.Transition
{
    public abstract class Transition
    {
        public IConfiguration _data { get; set; } 
        public Signifier Sig { get; set; }
        public Transition Next { get; set; }
        public Transition(IConfiguration data)
        {
            _data = data;
            Next = null;
        }

        /// <summary>
        /// The macro
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public abstract bool Run(DeviceData device);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public abstract bool IsStartState(DeviceData device);

    }
}
