using Dink.Actions;
using Dink.Model;
using Dink.Services;
using Dink.States;
using Dink.States.BotState;
using Dink.States.Enum;
using Dink.States.Transition;
using Dink.States.Transition.ToElite;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections;
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
        private IConfiguration _data { get; set; }
        private ADBService _adbService { get; set; }

        private List<State> LoginStates { get; set; }
        private List<Transition> EliteTransition { get; set; }

        // These fields needs to be in NoxInstances
        private bool IsMainGameScreen { get; set; }
        private bool RunElite { get; set; }

        private BotState _State;

        public BotService(IConfiguration conf, ADBService adbService)
        {
            _config = conf;

            IConfigurationBuilder builder = new ConfigurationBuilder();                     // Create a new instance of the config builder
            builder.SetBasePath(AppContext.BaseDirectory);                                  // Specify the default location for the config file
            builder = SelectConfigureFilesAsync(builder);                              // Select custom config.json files for Bekim or Tiff
            _data = builder.Build();


            BuildLoginStates();
            BuildEliteDungeonTransition();

            _adbService = adbService;
            NoxInstances = BuildNoxInstancesFromConfig();
            BotCommandService.init();
            MainThreadRunning = true;
            MainThread = new System.Threading.Thread(MainThreadFunc);
            MainThread.Start();

            // For now we only run elite dungeons
            RunElite = true;

 
        }

        private IConfigurationBuilder SelectConfigureFilesAsync(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("_data.json", optional: false, reloadOnChange: true);     // Add this (json encoded) file to the configuration
            return builder;
        }

        // Maybe use factory pattern for this
        private void BuildLoginStates()
        {
            //IsMainGameScreen = false;
            LoginStates = new List<State>();
            LoginStates.Add(new StateDeadL2R(_data));
            LoginStates.Add(new StateLoginScreen(_data));
            LoginStates.Add(new StateCharSelectAD(_data));
            LoginStates.Add(new StateCharSelect(_data));
            LoginStates.Add(new StateWeeklyRewardsDialog(_data));
            LoginStates.Add(new StateInGameMainScreen(_data));
        }

        private void BuildEliteDungeonTransition()
        {
            EliteTransition = new List<Transition>();
            EliteTransition.Add(new TransitionMoveToElite(_data));

        }

        private BotState GetCurrentBotState(DeviceData device)
        {
            // Check if in Main screen
            if (!ADBCommandService.IsL2RRunning(device))
            {
                return BotState.L2R_DEAD;
            }
            else if (GStateInGameMagMainScreen.IsState(device, _data) || GStateInGameMainScreen.IsState(device, _data))
            {
                if (RunElite)
                {
                    return BotState.GOTO_ELITE;
                }
                return BotState.MAIN_INGAME_SCREEN;
            }
            //if (EliteTransition[EliteTransition.Count - 1].IsState(device))
            //{
             //   if (RunElite)
             //   {
            //        return BotState.RUN_ELITE;
            //    }
            //}
     
            return BotState.L2R_DEAD;

        }

        private void MainThreadFunc()
        {
            try
            {
                // Main Bot logic. Starting out, we only deal with one instance of Nox.
                while (MainThreadRunning)
                {
          

                        DeviceData device = NoxInstances["Kazu"].ADB;

                        // Check if in Main screen
                        //if (LoginStates[LoginStates.Count - 1].IsState(device))
                        //{
                        //    IsMainGameScreen = true;
                        //} 
                        //else
                        //{
                        //    IsMainGameScreen = false;
                        //    ADBCommandService.KillL2R(device);
                        //}
                        _State = GetCurrentBotState(device);
                        Console.WriteLine("----BotState: " + _State);

                        if (_State == BotState.L2R_DEAD)
                        {
                            foreach (State item in LoginStates)
                            {
                                Console.WriteLine("State: " + item.GetType());

                                if (item.IsState(device))
                                {
                                    if (item.Run(device))
                                    {

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                        else if (_State == BotState.GOTO_ELITE)
                        {
                            foreach (Transition item in EliteTransition)
                            {
                                Console.WriteLine("State: " + item.GetType());

                                if (item.IsStartState(device))
                                {
                                    if (item.Run(device))
                                    {

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else if (_State == BotState.RUN_ELITE)
                        {

                        }
                        else if (_State == BotState.RUN_RESPAWN_ELITE)
                        {

                        }

                   
                    //foreach (KeyValuePair<String,NoxInstance> item in NoxInstances)
                    //{
                    //    DeviceData device = item.Value.ADB;
                    //    if (!ADBCommandService.IsL2RRunning(device))
                    //    {
                    //        BotCommandService.StartL2R(device);
                    //        //BotCommandService.EnterFarm(device, true);
                    //       //BotCommandService.Respawn(device, true);
                    //    }
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
