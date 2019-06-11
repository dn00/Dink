using Dink.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink
{
    public class BotService
    {
        private System.Threading.Thread MainThread { get; set; }
        private bool MainThreadRunning;
        private List<NoxInstance> NoxInstances;
        private IConfiguration _config { get; set; }
        public BotService(IConfiguration conf)
        {
            _config = conf;
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
                    Console.WriteLine(NoxInstances.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private List<NoxInstance> BuildNoxInstancesFromConfig()
        {
            var instanceNames = _config.GetSection("instances").GetChildren();
            List<NoxInstance> ListOfInstances = new List<NoxInstance>();
            foreach (var item in instanceNames)
            {
                Console.WriteLine(item["name"]);
                ushort.TryParse(item["char_value"], out ushort charValue);
                ListOfInstances.Add(new NoxInstance
                {
                    CharacterName = item["name"],
                    NoxName = item["nox_name"],
                    CharacterSelectValue = charValue
                });

            }

            return ListOfInstances;
        }
    }


}
