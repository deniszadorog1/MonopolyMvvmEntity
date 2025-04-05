using System;

namespace MonopolyDLL.Services
{
    public static class RandomService
    {
        private static Random _rnd = new Random();
        public static int GetRandom()
        {
            int start = SystemParamsService.GetNumByName("MinCubeRib");
            int end = SystemParamsService.GetNumByName("MaxCubeRub");

            return _rnd.Next(start, end);
        }

        public static int GetRandom(int start, int end)
        {
            return _rnd.Next(start, end);
        }
    }
}
