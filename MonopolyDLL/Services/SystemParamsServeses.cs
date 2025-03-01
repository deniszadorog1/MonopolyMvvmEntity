using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace MonopolyDLL
{
    public static class SystemParamsServeses
    {
        private static Dictionary<string, string> _dict = null;
        private static void SetStringParams()
        {
            //B:\\GitHub\\MonopolyMvvmEntity\\MonopolyEntity"
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.Parent.FullName;
            string monopolyDllPath = Path.Combine(parentPath, "MonopolyDLL");
            string jsonFilePath = Path.Combine(monopolyDllPath, "params.json");

            string json = File.ReadAllText(jsonFilePath);
            _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static int GetNumByName(string name)
        {
            SetStringParams();
            int.TryParse(_dict[name], out int res);

            return res;
        }

        public static string GetStringByName(string name)
        {
            SetStringParams();
            return _dict[name];
        }
    }
}
