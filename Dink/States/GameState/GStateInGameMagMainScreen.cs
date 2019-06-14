using System;
using System.Collections.Generic;
using System.Text;
using Dink.Services;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;

namespace Dink.States.BotState
{
    public static class GStateInGameMagMainScreen
    {
        public static bool IsState(DeviceData device, IConfiguration _data)
        {
            ushort.TryParse(_data["pixels:ingame_mag_main_screen:coords:x"], out ushort xMag);
            ushort.TryParse(_data["pixels:ingame_mag_main_screen:coords:y"], out ushort yMag);
            String colorMag = _data["pixels:ingame_mag_main_screen:color"];

            return ADBCommandService.CheckPixel(device, xMag, yMag, colorMag);
        }
    }
}
