using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dink.Services;
using Dink.States.BotState;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;

namespace Dink.States.Transition.ToElite
{
    public class TransitionToMagThenOut : Transition
    {
        public TransitionToMagThenOut(IConfiguration data) : base(data)
        {
        }

        public override bool IsStartState(DeviceData device)
        {
            return GStateInGameMagMainScreen.IsState(device, _data) || GStateInGameMainScreen.IsState(device, _data);
        }
           

        public override bool Run(DeviceData device)
        {
            ushort maxTries = 6;

            while (maxTries > 0)
            {

                ushort.TryParse(_data["macros:map:x"], out ushort xMap);
                ushort.TryParse(_data["macros:map:y"], out ushort yMap);
                ADBCommandService.SendTripleClick(device, xMap, yMap);

                Thread.Sleep(5000);
                ushort.TryParse(_data["macros:map_portal_button:x"], out ushort xMapPortal);
                ushort.TryParse(_data["macros:map_portal_button:y"], out ushort yMapPortal);

                ADBCommandService.SendTripleClick(device, xMapPortal, yMapPortal);
                Thread.Sleep(2000);

                ushort.TryParse(_data["macros:goto_mag_button:x"], out ushort xGotoMag);
                ushort.TryParse(_data["macros:goto_mag_button:y"], out ushort yGotoMag);
                ADBCommandService.SendTripleClick(device, xGotoMag, yGotoMag);
                Thread.Sleep(2000);

                ushort.TryParse(_data["macros:enter_mag_button:x"], out ushort xEnterMag);
                ushort.TryParse(_data["macros:enter_mag_button:y"], out ushort yEnterMag);
                ADBCommandService.SendTripleClick(device, xEnterMag, yEnterMag);

                // Maybe we can wait for isstate here
                Thread.Sleep(10000);

                if (GStateInGameMagMainScreen.IsState(device, _data) || GStateInGameMainScreen.IsState(device, _data))
                {
                    ushort.TryParse(_data["macros:exit_instance_button:x"], out ushort xExitMag);
                    ushort.TryParse(_data["macros:exit_instance_button:y"], out ushort yExitMag);
                    ADBCommandService.SendTripleClick(device, xExitMag, yExitMag);
                    Thread.Sleep(2000);
                    return true;
                }
                else
                {
                    --maxTries;
                }
            }

            return false;

        }
    }
}
