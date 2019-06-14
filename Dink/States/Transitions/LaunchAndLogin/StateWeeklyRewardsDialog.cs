using Dink.Actions;
using Dink.Services;
using Dink.States.BotState;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dink.States
{
    public class StateWeeklyRewardsDialog : State
    {
        public StateWeeklyRewardsDialog(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:weekly_rewards_menu:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:weekly_rewards_menu:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:weekly_rewards_menu:color"],
                X = x,
                Y = y
            };
            Next = new StateInGameMainScreen(data);
        }

        public override bool IsState(DeviceData device)
        {
            if (GStateInGameMagMainScreen.IsState(device, _data) || GStateInGameMainScreen.IsState(device, _data))
            {
                return true;
            }
            else if (ADBCommandService.CheckPixel(device, Sig.X, Sig.Y, Sig.Color))
            {
                return true;
            }
            return false;
        }

        public override bool Run(DeviceData device)
        {
            ushort maxWait = 60;

            while (maxWait > 0)
            {
                Console.WriteLine("State: " + GetType() + " " + maxWait);

                if (IsState(device))
                {
                    Thread.Sleep(3000);
                    ushort.TryParse(_data["macros:weekly_rewards_menu_exit:x"], out ushort x);
                    ushort.TryParse(_data["macros:weekly_rewards_menu_exit:y"], out ushort y);
                    ADBCommandService.SendTripleClick(device, x, y);
                    Thread.Sleep(3000);
                    return true;
                } else if (GStateInGameMagMainScreen.IsState(device, _data) || GStateInGameMainScreen.IsState(device, _data))
                {
                    Thread.Sleep(3000);
                    return true;
                }
                --maxWait;
            }

            return true;
        }
    }
}
