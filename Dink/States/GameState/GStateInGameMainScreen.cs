using System;
using System.Collections.Generic;
using System.Text;
using Dink.Services;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;

namespace Dink.States.BotState
{
    public static class GStateInGameMainScreen
    {

        public static bool IsState(DeviceData device, IConfiguration _data)
        {
            ushort.TryParse(_data["pixels:ingame_main_screen:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:ingame_main_screen:coords:y"], out ushort y);
            String color = _data["pixels:ingame_main_screen:color"];

            return ADBCommandService.CheckPixel(device, x, y, color);




        }
    }
}
