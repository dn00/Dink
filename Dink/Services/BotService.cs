using Dink.Model;
using Dink.Services;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
            MainThreadRunning = true;
            MainThread = new System.Threading.Thread(MainThreadFunc);
            MainThread.Start();
        }

        private void MainThreadFunc()
        {
            try
            {
                while (MainThreadRunning)
                {
                    bool result = ADBCommandService.CheckPixel(NoxInstances["Denk"].ADBDevice, 841, 480, "8abd51");
                    Console.WriteLine(result);
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
                            ADBDevice = _adbService.getDevice(item["serial"])
                        });
                    }
                }
          
            }

            return ListOfInstances;
        }
    }


}
