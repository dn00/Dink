using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.Model
{
    /// <summary>
    /// Object that holds data for a Nox instance.
    /// </summary>
    public class NoxInstance
    {
        public String CharacterName { get; set; }
        public String NoxName { get; set; }
        public ushort CharacterSelectValue { get; set; }
        public String Serial { get; set; }
        public DeviceData ADB { get; set; }

    }
}
