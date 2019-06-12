using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dink.Services
{
    public static class BotCommandService
    {
        private static IConfiguration _data { get; set; }

        public static void init()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();                   // Create a new instance of the config builder
            builder.SetBasePath(AppContext.BaseDirectory);                                // Specify the default location for the config file
            builder = SelectConfigureFilesAsync(builder);                                 // Select custom config.json files for Bekim or Tiff
            _data = builder.Build();
        }

        private static IConfigurationBuilder SelectConfigureFilesAsync(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("_data.json", optional: false, reloadOnChange: true);     // Add this (json encoded) file to the configuration
            return builder;
        }

        public static bool IsLoginScreen(DeviceData device)
        {
            String color = _data["pixels:login_screen:color"];
            ushort.TryParse(_data["pixels:login_screen:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:login_screen:coords:y"], out ushort y);

            if (ADBCommandService.CheckPixel(device, x, y, color))
            {
                return true;
            }
            return false;
        }

        public static void EnterFarm(DeviceData device, bool isElite)
        {
            if (!isElite) return;

            //TODO SEND TAB

            ushort.TryParse(_data["macros:normal_menu_button:x"], out ushort xMenu);
            ushort.TryParse(_data["macros:normal_menu_button:y"], out ushort yMenu);
            ADBCommandService.SendClick(device, xMenu, yMenu);
            Thread.Sleep(5000);

            ushort.TryParse(_data["macros:elite_dungeons_button:x"], out ushort xElite);
            ushort.TryParse(_data["macros:elite_dungeons_button:y"], out ushort yElite);
            ADBCommandService.SendClick(device, xElite, yElite);
            Thread.Sleep(5000);

            ushort.TryParse(_data["respawn:elite_select:x"], out ushort xSelect);
            ushort.TryParse(_data["respawn:elite_select:y"], out ushort ySelect);
            ADBCommandService.SendClick(device, xSelect, ySelect);
            Thread.Sleep(5000);

            ushort.TryParse(_data["macros:elite_enter_button:x"], out ushort xEnter);
            ushort.TryParse(_data["macros:elite_enter_button:y"], out ushort yEnter);
            ADBCommandService.SendClick(device, xEnter, yEnter);
            Thread.Sleep(30000);

        }

        public static bool NeedRespawn(DeviceData device)
        {
            String color = _data["pixels:respawn_button:color"];
            ushort.TryParse(_data["pixels:respawn_button:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:respawn_button:coords:y"], out ushort y);

            if (ADBCommandService.CheckPixel(device, x, y, color))
            {
                return true;
            }
            return false;
        }

        public static bool Respawn(DeviceData device, bool isElite = true)
        {
            try
            {
                Thread.Sleep(5000);

                ushort.TryParse(_data["macros:respawn_button:x"], out ushort xRespawnButton);
                ushort.TryParse(_data["macros:respawn_button:y"], out ushort yRespawnButton);
                TripleClick(device, xRespawnButton, yRespawnButton);
                Thread.Sleep(5000);

                ushort.TryParse(_data["macros:hottime_x:x"], out ushort xHotTime);
                ushort.TryParse(_data["macros:hottime_x:y"], out ushort yHotTime);
                TripleClick(device, xHotTime, yHotTime);
                Thread.Sleep(3000);

                ushort.TryParse(_data["macros:map:x"], out ushort xMap);
                ushort.TryParse(_data["macros:map:y"], out ushort yMap);
                TripleClick(device, xMap, yMap);
                Thread.Sleep(6000);

                if (isElite)
                {
                    ushort.TryParse(_data["respawn:elite:x"], out ushort xElite);
                    ushort.TryParse(_data["respawn:elite:y"], out ushort yElite);
                    TripleClick(device, xElite, yElite);
                }
                else
                {
                    ushort.TryParse(_data["respawn:field:x"], out ushort xField);
                    ushort.TryParse(_data["respawn:field:y"], out ushort yField);
                    TripleClick(device, xField, yField);
                }

                int.TryParse(_data["respawn:check_sleep_time"], out int sleeptime);
                Thread.Sleep(sleeptime);

                ushort.TryParse(_data["macros:auto_button:x"], out ushort xAuto);
                ushort.TryParse(_data["macros:auto_button:y"], out ushort yAuto);
                ADBCommandService.SendClick(device, xAuto, yAuto);
                Thread.Sleep(2000);

                ushort.TryParse(_data["macros:mount_bug:x"], out ushort xMount);
                ushort.TryParse(_data["macros:mount_bug:y"], out ushort yMount);
                ADBCommandService.SendClick(device, xMount, yMount);
                return true;
            } 
            catch (Exception ex)
            {
                Console.WriteLine("WARNING --- Something went wrong in respawn() " + ex.ToString());
                return false;
            }
        }

        public static void TripleClick(DeviceData device, ushort x, ushort y)
        {
            ADBCommandService.SendClick(device, x, y);
            Thread.Sleep(500);
            ADBCommandService.SendClick(device, x, y);
            Thread.Sleep(500);
            ADBCommandService.SendClick(device, x, y);
            Thread.Sleep(500);
        }

        public static bool StartL2R(DeviceData device)
        {
        Restart:
            ADBCommandService.LaunchL2R(device);
            Thread.Sleep(40000);

            if (!IsLoginScreen(device))
            {
                ADBCommandService.KillL2R(device);
                goto Restart;
            }

            ushort.TryParse(_data["macros:login:x"], out ushort xLog);
            ushort.TryParse(_data["macros:login:y"], out ushort yLog);
            ADBCommandService.SendClick(device, xLog, yLog);
            Thread.Sleep(100000);

            // Maybe add another check here

            ushort.TryParse(_data["macros:exit_ad:x"], out ushort xExit);
            ushort.TryParse(_data["macros:exit_ad:y"], out ushort yExit);
            ADBCommandService.SendClick(device, xExit, yExit);
            Thread.Sleep(3000);

            ushort.TryParse(_data["macros:char_enter:x"], out ushort xChar);
            ushort.TryParse(_data["macros:char_enter:y"], out ushort yChar);
            ADBCommandService.SendClick(device, xChar, yChar);
            Thread.Sleep(60000);

            // Maybe add another check here

            ushort.TryParse(_data["macros:exit_reward:x"], out ushort xRew);
            ushort.TryParse(_data["macros:exit_reward:y"], out ushort yRew);
            ADBCommandService.SendClick(device, xRew, yRew);
            Thread.Sleep(2000);

            return true;
        }
    }
}
