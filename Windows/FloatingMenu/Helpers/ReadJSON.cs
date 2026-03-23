using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.Json;

namespace FloatingMenu.Helpers
{
    internal static class ReadJSON
    {
        public static string GetPortFromExternalConfig()
        {
            string path = @"C:\Floating_Menu\config.json";

           
                if (!File.Exists(path))
                    throw new Exception("Config file not found");

                string json = File.ReadAllText(path);

                ConfigModel config;
                try
                {
                    config = JsonSerializer.Deserialize<ConfigModel>(json);
                }
                catch (JsonException ex)
                {
                    throw new Exception("Invalid JSON format: " + ex.Message);
                }

                if (config == null)
                    throw new Exception("Invalid config structure");

                if (string.IsNullOrWhiteSpace(config.Port))
                    throw new Exception("Port is missing in config");

                string port = config.Port.Trim().ToUpper();

                var availablePorts = SerialPort.GetPortNames();

                if (!availablePorts.Contains(port))
                    throw new Exception($"Port '{port}' not found on this machine");

                return port;
           
        }
     
    }
  
    class ConfigModel
    {
        public string Port { get; set; }
    }
}
