using Dink.Actions;
using Dink.Model;
using Dink.Services;
using Dink.States;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Dink
{
    public class BotService
    {
        private System.Threading.Thread MainThread { get; set; }
        private bool MainThreadRunning;
        private Dictionary<String, NoxInstance> NoxInstances { get; set; }
        private IConfiguration _config { get; set; }
        private ADBService _adbService { get; set; }

        public BotService(IConfiguration conf, ADBService adbService)
        {
            _config = conf;
            _adbService = adbService;
            NoxInstances = BuildNoxInstancesFromConfig();
            BotCommandService.init();
            MainThreadRunning = true;
            MainThread = new System.Threading.Thread(MainThreadFunc);
            MainThread.Start();

 
        }

        private void MainThreadFunc()
        {
            try
            {
                // Main Bot logic. Starting out, we only deal with one instance of Nox.
                while (MainThreadRunning)
                {
                    foreach (KeyValuePair<String,NoxInstance> item in NoxInstances)
                    {
                        DeviceData device = item.Value.ADB;
                        if (!ADBCommandService.IsL2RRunning(device))
                        {
                            BotCommandService.StartL2R(device);
                            //BotCommandService.EnterFarm(device, true);
                            //BotCommandService.Respawn(device, true);
                        }

                    }

                    //DeviceData device = NoxInstances["Denk"].ADB;
                    //if (!ADBCommandService.IsL2RRunning(device))
                    //{
                    //    BotCommandService.StartL2R(device);
                        //BotCommandService.EnterFarm(device, true);
                        //BotCommandService.Respawn(device, true);
                    //}
                    
                    //if (BotCommandService.NeedRespawn(device))
                    //{
                    //    BotCommandService.Respawn(device);
                    //} else
                    //{
                        // We failed so we should restart Nox. Or close L2R and wait
                    //}

                    Thread.Sleep(20000); // Sleep 20 seconds
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private Dictionary<String, NoxInstance> BuildNoxInstancesFromConfig()
        {
            var instanceNames = _config.GetSection("instances").GetChildren();
            Dictionary<String, NoxInstance> ListOfInstances = new Dictionary<String, NoxInstance>();

            Process[] processlist = Process.GetProcesses();

            foreach (var item in instanceNames)
            {
                bool isFound = false;
                // For now we only run bot if Nox is already opened
                foreach (Process theprocess in processlist)
                {
                    //Console.WriteLine("Process: {0} ID: {1}", theprocess.MainWindowTitle, theprocess.MainWindowHandle);
                    if (theprocess.MainWindowTitle == item["name"])
                    {
                        Console.WriteLine("Creating: " + item["name"] + " Serial: " + item["serial"]);
                        ushort.TryParse(item["char_value"], out ushort charValue);
                        ListOfInstances.Add(item["name"], new NoxInstance
                        {
                            CharacterName = item["name"],
                            NoxName = item["nox_name"],
                            CharacterSelectValue = charValue,
                            Serial = item["serial"],
                            ADB = _adbService.getDevice(item["serial"])
                        });

                        isFound = true;
                    }
                }

                if (!isFound)
                {
                    Console.WriteLine(String.Format("Warning --- {0} Nox is not opened", item["name"]));
                }
          
            }

            return ListOfInstances;
        }
    }


}
