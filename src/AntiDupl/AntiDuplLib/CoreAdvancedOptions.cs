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
using System;
using System.Collections.Generic;
using System.Text;

namespace AntiDupl.NET
{
    public class CoreAdvancedOptions
    {
        public bool DeleteToRecycleBin { get; set; }
        public bool MistakeDataBase { get; set; }
        public int RatioResolution { get; set; }
        public int CompareThreadCount { get; set; }
        public int CollectThreadCount { get; set; }
        public int ReducedImageSize { get; set; }
        public int UndoQueueSize { get; set; }
        public int ResultCountMax { get; set; }
        public int IgnoreFrameWidth { get; set; }

        public CoreAdvancedOptions()
        {
        }

        public CoreAdvancedOptions(CoreAdvancedOptions advancedOptions)
        {
            DeleteToRecycleBin = advancedOptions.DeleteToRecycleBin;
            MistakeDataBase = advancedOptions.MistakeDataBase;
            RatioResolution = advancedOptions.RatioResolution;
            CompareThreadCount = advancedOptions.CompareThreadCount;
            CollectThreadCount = advancedOptions.CollectThreadCount;
            ReducedImageSize = advancedOptions.ReducedImageSize;
            UndoQueueSize = advancedOptions.UndoQueueSize;
            ResultCountMax = advancedOptions.ResultCountMax;
            IgnoreFrameWidth = advancedOptions.IgnoreFrameWidth;
        }

        internal CoreAdvancedOptions(ref CoreDll.adAdvancedOptions advancedOptions)
        {
            DeleteToRecycleBin = advancedOptions.deleteToRecycleBin != CoreDll.FALSE;
            MistakeDataBase = advancedOptions.mistakeDataBase != CoreDll.FALSE;
            RatioResolution = advancedOptions.ratioResolution;
            CompareThreadCount = advancedOptions.compareThreadCount;
            CollectThreadCount = advancedOptions.collectThreadCount;
            ReducedImageSize = advancedOptions.reducedImageSize;
            UndoQueueSize = advancedOptions.undoQueueSize;
            ResultCountMax = advancedOptions.resultCountMax;
            IgnoreFrameWidth = advancedOptions.ignoreFrameWidth;
        }

        internal void ConvertTo(ref CoreDll.adAdvancedOptions advancedOptions)
        {
            advancedOptions.deleteToRecycleBin = DeleteToRecycleBin ? CoreDll.TRUE : CoreDll.FALSE;
            advancedOptions.mistakeDataBase = MistakeDataBase ? CoreDll.TRUE : CoreDll.FALSE;
            advancedOptions.ratioResolution = RatioResolution;
            advancedOptions.compareThreadCount = CompareThreadCount;
            advancedOptions.collectThreadCount = CollectThreadCount;
            advancedOptions.reducedImageSize = ReducedImageSize;
            advancedOptions.undoQueueSize = UndoQueueSize;
            advancedOptions.resultCountMax = ResultCountMax;
            advancedOptions.ignoreFrameWidth = IgnoreFrameWidth;
        }

        public CoreAdvancedOptions Clone()
        {
            return new CoreAdvancedOptions(this);
        }

        public bool Equals(CoreAdvancedOptions advancedOptions)
        {
            return
                DeleteToRecycleBin == advancedOptions.DeleteToRecycleBin &&
                MistakeDataBase == advancedOptions.MistakeDataBase &&
                RatioResolution == advancedOptions.RatioResolution &&
                CompareThreadCount == advancedOptions.CompareThreadCount &&
                CollectThreadCount == advancedOptions.CollectThreadCount &&
                ReducedImageSize == advancedOptions.ReducedImageSize &&
                UndoQueueSize == advancedOptions.UndoQueueSize &&
                ResultCountMax == advancedOptions.ResultCountMax &&
                IgnoreFrameWidth == advancedOptions.IgnoreFrameWidth;
        }
    }
}
