using AntiDupl.NET;
using System;

namespace AntiDuplLib
{
    internal class Engine
    {
        private string _userPath;

        public Engine(string userPath) => _userPath = userPath;

        public CoreStatus Status { get; } = new CoreStatus();

        public ResultStorage Result { get; } = new ResultStorage();

        internal void Search() => throw new NotImplementedException();
    }
}
