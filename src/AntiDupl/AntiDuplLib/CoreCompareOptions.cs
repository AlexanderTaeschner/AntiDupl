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
        public bool CheckOnEquality { get; set; }
        public bool TransformedImage { get; set; }
        public bool SizeControl { get; set; }
        public bool TypeControl { get; set; }
        public bool RatioControl { get; set; }
        public int ThresholdDifference { get; set; }
        public int MinimalImageSize { get; set; }
        public int MaximalImageSize { get; set; }
        public bool CompareInsideOneFolder { get; set; }
        public bool CompareInsideOneSearchPath { get; set; }
        public AlgorithmComparing AlgorithmComparing { get; set; }

        public CoreCompareOptions()
        {
        }

        public CoreCompareOptions(CoreCompareOptions compareOptions)
        {
            CheckOnEquality = compareOptions.CheckOnEquality;
            TransformedImage = compareOptions.TransformedImage;
            SizeControl = compareOptions.SizeControl;
            TypeControl = compareOptions.TypeControl;
            RatioControl = compareOptions.RatioControl;
			AlgorithmComparing = compareOptions.AlgorithmComparing;
            ThresholdDifference = compareOptions.ThresholdDifference;
            MinimalImageSize = compareOptions.MinimalImageSize;
            MaximalImageSize = compareOptions.MaximalImageSize;
            CompareInsideOneFolder = compareOptions.CompareInsideOneFolder;
            CompareInsideOneSearchPath = compareOptions.CompareInsideOneSearchPath;
        }

        internal CoreCompareOptions(ref CoreDll.adCompareOptions compareOptions)
        {
            CheckOnEquality = compareOptions.checkOnEquality != CoreDll.FALSE;
            TransformedImage = compareOptions.transformedImage != CoreDll.FALSE;
            SizeControl = compareOptions.sizeControl != CoreDll.FALSE;
            TypeControl = compareOptions.typeControl != CoreDll.FALSE;
            RatioControl = compareOptions.ratioControl != CoreDll.FALSE;
			AlgorithmComparing = compareOptions.algorithmComparing;
            ThresholdDifference = compareOptions.thresholdDifference;
            MinimalImageSize = compareOptions.minimalImageSize;
            MaximalImageSize = compareOptions.maximalImageSize;
            CompareInsideOneFolder = compareOptions.compareInsideOneFolder != CoreDll.FALSE;
            CompareInsideOneSearchPath = compareOptions.compareInsideOneSearchPath != CoreDll.FALSE;
        }

        internal void ConvertTo(ref CoreDll.adCompareOptions compareOptions)
        {
            compareOptions.checkOnEquality = CheckOnEquality ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.transformedImage = TransformedImage ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.sizeControl = SizeControl ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.typeControl = TypeControl ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.ratioControl = RatioControl ? CoreDll.TRUE : CoreDll.FALSE;
			compareOptions.algorithmComparing = AlgorithmComparing;
            compareOptions.thresholdDifference = ThresholdDifference;
            compareOptions.minimalImageSize = MinimalImageSize;
            compareOptions.maximalImageSize = MaximalImageSize;
            compareOptions.compareInsideOneFolder = CompareInsideOneFolder ? CoreDll.TRUE : CoreDll.FALSE;
            compareOptions.compareInsideOneSearchPath = CompareInsideOneSearchPath ? CoreDll.TRUE : CoreDll.FALSE;
        }

        public CoreCompareOptions Clone()
        {
            return new CoreCompareOptions(this);
        }

        public bool Equals(CoreCompareOptions compareOptions)
        {
            return
                CheckOnEquality == compareOptions.CheckOnEquality &&
                TransformedImage == compareOptions.TransformedImage &&
                SizeControl == compareOptions.SizeControl &&
                TypeControl == compareOptions.TypeControl &&
                RatioControl == compareOptions.RatioControl &&
				AlgorithmComparing == compareOptions.AlgorithmComparing &&
                ThresholdDifference == compareOptions.ThresholdDifference &&
                MinimalImageSize == compareOptions.MinimalImageSize &&
                MaximalImageSize == compareOptions.MaximalImageSize &&
                CompareInsideOneFolder == compareOptions.CompareInsideOneFolder &&
                CompareInsideOneSearchPath == compareOptions.CompareInsideOneSearchPath;
        }
    }
}
