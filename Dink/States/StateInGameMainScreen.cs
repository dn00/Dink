using Dink.Actions;
using Dink.Services;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dink.States
{
    class StateInGameMainScreen : State
    {
        public StateInGameMainScreen(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:ingame_main_screen:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:ingame_main_screen:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:ingame_main_screen:color"],
                X = x,
                Y = y
            };

            Next = null;
        }

        public override bool IsState(DeviceData device)
        {

            if (ADBCommandService.CheckPixel(device, Sig.X, Sig.Y, Sig.Color))
            {
                return true;
            }
            return false;
        }

        public override bool Run(DeviceData device)
        {
            return true;
        }
    }
}
