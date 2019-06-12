using Dink.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dink
{
    public class Bot
    {
        public IConfiguration _config { get; set; }
        public Bot(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();                     // Create a new instance of the config builder
            System.Collections.IDictionary env = Environment.GetEnvironmentVariables();     // Get the environment variables
            builder.SetBasePath(AppContext.BaseDirectory);                                  // Specify the default location for the config file
            builder = SelectConfigureFilesAsync(builder, env);                              // Select custom config.json files for Bekim or Tiff
            _config = builder.Build();
        }

        private IConfigurationBuilder SelectConfigureFilesAsync(IConfigurationBuilder builder, System.Collections.IDictionary env)
        {
            //string hostingEnv = (string)env["Hosting:Environment"];

            builder.AddJsonFile("_config.json", optional: false, reloadOnChange: true);     // Add this (json encoded) file to the configuration
            return builder;
        }

        public static async Task Start(string[] args)
        {
            Bot bot = new Bot(args);
            await bot.Run();
        }

        public async Task Run()
        {
            ServiceCollection services = new ServiceCollection();
            await ConfigureServicesAsync(services);
            ServiceProvider provider = services.BuildServiceProvider();

            provider.GetRequiredService<BotService>();
            provider.GetRequiredService<ADBService>();

            await Task.Delay(-1);
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task ConfigureServicesAsync(IServiceCollection services)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            services.AddSingleton<IConfiguration>(_config);
            services.AddSingleton<BotService>();
            services.AddSingleton<ADBService>();
        }
    }
}