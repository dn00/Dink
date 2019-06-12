﻿using Dink.States;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.Actions
{
    public abstract class State
    {
        public IConfiguration _data { get; set; } 
        public Signifier Sig { get; set; }
        public State(IConfiguration data, Signifier s)
        {
            _data = data;
            Sig = s;
        }
        /// <summary>
        /// The next state
        /// </summary>
        //public abstract State Next { get; set; }

        /// <summary>
        /// The macro
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public abstract State Run(DeviceData device);

    }
}
