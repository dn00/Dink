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
    public class TransitionMoveToElite : Transition
    {
        public TransitionMoveToElite(IConfiguration data) : base(data)
        {
        }

        public override bool IsStartState(DeviceData device)
        {
            return GStateInGameMagMainScreen.IsState(device, _data) || GStateInGameMainScreen.IsState(device, _data);
        }


        public override bool Run(DeviceData device)
        {
 
            ADBCommandService.SendTab(device);


            ushort.TryParse(_data["macros:tabbed_dungeon_button:x"], out ushort xDung);
            ushort.TryParse(_data["macros:tabbed_dungeon_button:y"], out ushort yDung);
            ADBCommandService.SendTripleClick(device, xDung, yDung);
            Thread.Sleep(5000);

            ushort.TryParse(_data["macros:tabbed_normal_dungeon_button:x"], out ushort xNormDung);
            ushort.TryParse(_data["macros:tabbed_normal_dungeon_button:y"], out ushort yNormDung);

            ADBCommandService.SendClick(device, xNormDung, yNormDung);
            Thread.Sleep(5000);

            ushort.TryParse(_data["macros:daily_dungeon_menu_elite_dungeon_button:x"], out ushort xElite);
            ushort.TryParse(_data["macros:daily_dungeon_menu_elite_dungeon_button:y"], out ushort yElite);
            ADBCommandService.SendTripleClick(device, xElite, yElite);
            Thread.Sleep(5000);


            ushort maxTries = 10;

            while (maxTries > 0)
            {
                if (GStateEliteDungeonEnterButton.IsState(device, _data))
                {
                    ushort.TryParse(_data["macros:enter_elite_dungeon_button:x"], out ushort xExitMag);
                    ushort.TryParse(_data["macros:enter_elite_dungeon_button:y"], out ushort yExitMag);
                    ADBCommandService.SendTripleClick(device, xExitMag, yExitMag);
                    Thread.Sleep(20000);
                    // CHECK IF IN ELITE HERE ELSE RETURN FALSE AND TRY AGAIN
                    return true;
                }
                Thread.Sleep(600);
                --maxTries;
            }
            return false;
        }
    }
}
