/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy 
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/
using System.Collections.Generic;

namespace AntiDupl.NET
{
    public class CoreStatus
    {
        private readonly object _criticalSection = new object();
        private readonly Dictionary<int, CoreStatus> _collectThreadStatuses =
            new Dictionary<int, CoreStatus>();
        private readonly Dictionary<int, CoreStatus> _compareThreadStatuses =
            new Dictionary<int, CoreStatus>();
        
        public string Path { get; }
        public int Current { get; }
        public int Total { get; }

        internal StateType State { get; }

        internal CoreStatus(ref CoreDll.adStatusW status)
        {
            State = status.state;
            Path = status.path;
            Current = (int)status.current.ToUInt32();
            Total = (int)status.total.ToUInt32();
        }

        internal CoreStatus()
        {
        }

        internal CoreStatus(CoreStatus status)
        {
            State = status.State;
            Path = status.Path;
            Current = status.Current;
            Total = status.Total;
        }

        internal Error Export(ThreadType threadType, int threadId, out CoreStatus? status)
        {
            status = null;

            lock (_criticalSection)
            {
                switch (threadType)
                {
                    case ThreadType.Main:
                        status = new CoreStatus(this);
                        break;
                    case ThreadType.Collect:
                        if (!_collectThreadStatuses.TryGetValue(threadId, out var threadStatus))
                        {
                            return Error.InvalidThreadId;
                        }

                        status = new CoreStatus(threadStatus);
                        break;
                    case ThreadType.Compare:
                        if (!_compareThreadStatuses.TryGetValue(threadId, out threadStatus))
                        {
                            return Error.InvalidThreadId;
                        }

                        status = new CoreStatus(threadStatus);
                        break;
                }

                return Error.Ok;
            }
        }
    }
}
