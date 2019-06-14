using System;
using System.Collections.Generic;
using System.Text;
using Dink.Services;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;

namespace Dink.States.BotState
{
    public static class GStateEliteDungeonEnterButton
    {

        public static bool IsState(DeviceData device, IConfiguration _data)
        {
            ushort.TryParse(_data["pixels:enter_dungeon_button:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:enter_dungeon_button:coords:y"], out ushort y);
            String color = _data["pixels:enter_dungeon_button:color"];

            return ADBCommandService.CheckPixel(device, x, y, color);

        }
    }
}
