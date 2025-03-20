using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Services
{
    public static class GetRandomService
    {
        private static Random _rnd;
        public static int GetRandom()
        {
            int start = SystemParamsService.GetNumByName("MinCubeRib");
            int end = SystemParamsService.GetNumByName("MaxCubeRub");

            _rnd = new Random();
            return _rnd.Next(start, end);
        }
    }
}
