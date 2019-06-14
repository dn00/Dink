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
    class StateCharSelect : State
    {
        public StateCharSelect(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:char_select_screen:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:char_select_screen:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:char_select_screen:color"],
                X = x,
                Y = y
            };

            Next = new StateWeeklyRewardsDialog(data);
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
            Thread.Sleep(2000);

            ushort maxWaitSeconds;

            ushort.TryParse(_data["macros:char_enter_game_button:x"], out ushort x);
            ushort.TryParse(_data["macros:char_enter_game_button:y"], out ushort y);
            maxWaitSeconds = 200;

        EnterGame:
            ADBCommandService.SendClick(device, x, y);

            while (maxWaitSeconds > 0)
            {
                Console.WriteLine("State: " + GetType());

                Thread.Sleep(1000);
                if (Next.IsState(device))
                {
                    return true;
                }
                else if (IsState(device))
                {
                    goto EnterGame;
                }
                --maxWaitSeconds;
            }

            return false;

        }
    }
}
