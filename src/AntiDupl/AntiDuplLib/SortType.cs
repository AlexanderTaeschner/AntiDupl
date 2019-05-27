/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar, 2013-2018 Borisov Dmitry.
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

namespace AntiDupl.NET
{
    public enum SortType : int
    {
        ByType = 0,
        BySortedPath = 1,
        BySortedName = 2,
        BySortedDirectory = 3,
        BySortedSize = 4,
        BySortedTime = 5,
        BySortedType = 6,
        BySortedWidth = 7,
        BySortedHeight = 8,
        BySortedArea = 9,
        BySortedRatio = 10,
        BySortedBlockiness = 11,
        BySortedBlurring = 12,
        ByFirstPath = 13,
        ByFirstName = 14,
        ByFirstDirectory = 15,
        ByFirstSize = 16,
        ByFirstTime = 17,
        ByFirstType = 18,
        ByFirstWidth = 19,
        ByFirstHeight = 20,
        ByFirstArea = 21,
        ByFirstRatio = 22,
        ByFirstBlockiness = 23,
        ByFirstBlurring = 24,
        BySecondPath = 25,
        BySecondName = 26,
        BySecondDirectory = 27,
        BySecondSize = 28,
        BySecondTime = 29,
        BySecondType = 30,
        BySecondWidth = 31,
        BySecondHeight = 32,
        BySecondArea = 33,
        BySecondRatio = 34,
        BySecondBlockiness = 35,
        BySecondBlurring = 36,
        ByDefect = 37,
        ByDifference = 38,
        ByTransform = 39,
        ByGroup = 40,
        ByGroupSize = 41,
        ByHint = 42,
    }
}
