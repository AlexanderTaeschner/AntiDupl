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
    public class CoreCompareOptions
    {
        public bool checkOnEquality { get; set; }
        public bool transformedImage { get; set; }
        public bool sizeControl { get; set; }
        public bool typeControl { get; set; }
        public bool ratioControl { get; set; }
        public int thresholdDifference { get; set; }
        public int minimalImageSize { get; set; }
        public int maximalImageSize { get; set; }
        public bool compareInsideOneFolder { get; set; }
        public bool compareInsideOneSearchPath { get; set; }
        public AlgorithmComparing algorithmComparing { get; set; }

        public CoreCompareOptions()
        {
        }

        public CoreCompareOptions(CoreCompareOptions compareOptions)
        {
            checkOnEquality = compareOptions.checkOnEquality;
            transformedImage = compareOptions.transformedImage;
            sizeControl = compareOptions.sizeControl;
            typeControl = compareOptions.typeControl;
            ratioControl = compareOptions.ratioControl;
			algorithmComparing = compareOptions.algorithmComparing;
            thresholdDifference = compareOptions.thresholdDifference;
            minimalImageSize = compareOptions.minimalImageSize;
            maximalImageSize = compareOptions.maximalImageSize;
            compareInsideOneFolder = compareOptions.compareInsideOneFolder;
            compareInsideOneSearchPath = compareOptions.compareInsideOneSearchPath;
        }

        internal CoreCompareOptions(ref CoreDll.adCompareOptions compareOptions)
        {
            checkOnEquality = compareOptions.checkOnEquality != CoreDll.FALSE;
            transformedImage = compareOptions.transformedImage != CoreDll.FALSE;
            sizeControl = compareOptions.sizeControl != CoreDll.FALSE;
            typeControl = compareOptions.typeControl != CoreDll.FALSE;
            ratioControl = compareOptions.ratioControl != CoreDll.FALSE;
			algorithmComparing = compareOptions.algorithmComparing;
            thresholdDifference = compareOptions.thresholdDifference;
            minimalImageSize = compareOptions.minimalImageSize;
            maximalImageSize = compareOptions.maximalImageSize;
            compareInsideOneFolder = compareOptions.compareInsideOneFolder != CoreDll.FALSE;
            compareInsideOneSearchPath = compareOptions.compareInsideOneSearchPath != CoreDll.FALSE;
        }

        internal void ConvertTo(ref CoreDll.adCompareOptions compareOptions)
        {
            compareOptions.checkOnEquality = checkOnEquality ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.transformedImage = transformedImage ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.sizeControl = sizeControl ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.typeControl = typeControl ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.ratioControl = ratioControl ? CoreDll.TRUE : CoreDll.FALSE;
			compareOptions.algorithmComparing = algorithmComparing;
            compareOptions.thresholdDifference = thresholdDifference;
            compareOptions.minimalImageSize = minimalImageSize;
            compareOptions.maximalImageSize = maximalImageSize;
            compareOptions.compareInsideOneFolder = compareInsideOneFolder ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.compareInsideOneSearchPath = compareInsideOneSearchPath ? CoreDll.TRUE : CoreDll.FALSE;
        }

        public CoreCompareOptions Clone()
        {
            return new CoreCompareOptions(this);
        }

        public bool Equals(CoreCompareOptions compareOptions)
        {
            return
                checkOnEquality == compareOptions.checkOnEquality &&
                transformedImage == compareOptions.transformedImage &&
                sizeControl == compareOptions.sizeControl &&
                typeControl == compareOptions.typeControl &&
                ratioControl == compareOptions.ratioControl &&
				algorithmComparing == compareOptions.algorithmComparing &&
                thresholdDifference == compareOptions.thresholdDifference &&
                minimalImageSize == compareOptions.minimalImageSize &&
                maximalImageSize == compareOptions.maximalImageSize &&
                compareInsideOneFolder == compareOptions.compareInsideOneFolder &&
                compareInsideOneSearchPath == compareOptions.compareInsideOneSearchPath;
        }
    }
}
