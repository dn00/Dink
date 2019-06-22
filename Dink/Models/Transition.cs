using Dink.States;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States.Transition
{
    /// <summary>
    /// The transition from one state to another
    /// </summary>
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
        /// Method to run the macro.
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <returns></returns>
        public abstract bool Run(DeviceData device);
        /// <summary>
        /// Return true if we are at the state where this macro begins.
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <returns></returns>
        public abstract bool IsStartState(DeviceData device);

    }
}
