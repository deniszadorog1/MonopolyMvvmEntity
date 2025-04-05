using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace MonopolyDLL
{
    public static class SystemParamsService
    {
        private static Dictionary<string, string> _dict = null;
        private static void SetStringParams(string fileName)
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.Parent.FullName;
            string monopolyDllPath = Path.Combine(parentPath, "MonopolyDLL");
            string jsonFilePath = Path.Combine(monopolyDllPath, fileName);

            string json = File.ReadAllText(jsonFilePath);
            _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static int GetNumByName(string name)
        {
            SetStringParams("params.json");
            int.TryParse(_dict[name], out int res);

            return res;
        }

        public static string GetStringByName(string name)
        {
            SetStringParams("params.json");
            return _dict[name];
        }

        public static List<string> GetImageExpansions()
        {
            List<string> res = new List<string>();
            SetStringParams("ImageExpansions.json");

            return _dict.Values.ToList();
        }
    }
}
