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
        public bool deleteToRecycleBin { get; set; }
        public bool mistakeDataBase { get; set; }
        public int ratioResolution { get; set; }
        public int compareThreadCount { get; set; }
        public int collectThreadCount { get; set; }
        public int reducedImageSize { get; set; }
        public int undoQueueSize { get; set; }
        public int resultCountMax { get; set; }
        public int ignoreFrameWidth { get; set; }

        public CoreAdvancedOptions()
        {
        }

        public CoreAdvancedOptions(CoreAdvancedOptions advancedOptions)
        {
            deleteToRecycleBin = advancedOptions.deleteToRecycleBin;
            mistakeDataBase = advancedOptions.mistakeDataBase;
            ratioResolution = advancedOptions.ratioResolution;
            compareThreadCount = advancedOptions.compareThreadCount;
            collectThreadCount = advancedOptions.collectThreadCount;
            reducedImageSize = advancedOptions.reducedImageSize;
            undoQueueSize = advancedOptions.undoQueueSize;
            resultCountMax = advancedOptions.resultCountMax;
            ignoreFrameWidth = advancedOptions.ignoreFrameWidth;
        }

        internal CoreAdvancedOptions(ref CoreDll.adAdvancedOptions advancedOptions)
        {
            deleteToRecycleBin = advancedOptions.deleteToRecycleBin != CoreDll.FALSE;
            mistakeDataBase = advancedOptions.mistakeDataBase != CoreDll.FALSE;
            ratioResolution = advancedOptions.ratioResolution;
            compareThreadCount = advancedOptions.compareThreadCount;
            collectThreadCount = advancedOptions.collectThreadCount;
            reducedImageSize = advancedOptions.reducedImageSize;
            undoQueueSize = advancedOptions.undoQueueSize;
            resultCountMax = advancedOptions.resultCountMax;
            ignoreFrameWidth = advancedOptions.ignoreFrameWidth;
        }

        internal void ConvertTo(ref CoreDll.adAdvancedOptions advancedOptions)
        {
            advancedOptions.deleteToRecycleBin = deleteToRecycleBin ? CoreDll.TRUE : CoreDll.FALSE;
            advancedOptions.mistakeDataBase = mistakeDataBase ? CoreDll.TRUE : CoreDll.FALSE;
            advancedOptions.ratioResolution = ratioResolution;
            advancedOptions.compareThreadCount = compareThreadCount;
            advancedOptions.collectThreadCount = collectThreadCount;
            advancedOptions.reducedImageSize = reducedImageSize;
            advancedOptions.undoQueueSize = undoQueueSize;
            advancedOptions.resultCountMax = resultCountMax;
            advancedOptions.ignoreFrameWidth = ignoreFrameWidth;
        }

        public CoreAdvancedOptions Clone()
        {
            return new CoreAdvancedOptions(this);
        }

        public bool Equals(CoreAdvancedOptions advancedOptions)
        {
            return
                deleteToRecycleBin == advancedOptions.deleteToRecycleBin &&
                mistakeDataBase == advancedOptions.mistakeDataBase &&
                ratioResolution == advancedOptions.ratioResolution &&
                compareThreadCount == advancedOptions.compareThreadCount &&
                collectThreadCount == advancedOptions.collectThreadCount &&
                reducedImageSize == advancedOptions.reducedImageSize &&
                undoQueueSize == advancedOptions.undoQueueSize &&
                resultCountMax == advancedOptions.resultCountMax &&
                ignoreFrameWidth == advancedOptions.ignoreFrameWidth;
        }
    }
}
