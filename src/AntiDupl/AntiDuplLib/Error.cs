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
    public enum Error : int
    {
        Ok = 0,
        Unknown = 1,
        AccessDenied = 2,
        InvalidPointer = 3,
        InvalidFileFormat = 4,
        InvalidPathType = 5,
        InvalidOptionsType = 6,
        InvalidFileType = 7,
        InvalidSortType = 8,
        InvalidGlobalActionType = 9,
        InvalidThreadId = 10,
        InvalidStartPosition = 11,
        OutputBufferIsTooSmall = 12,
        FileIsNotExists = 13,
        CantOpenFile = 14,
        CantCreateFile = 15,
        CantReadFile = 16,
        CantWriteFile = 17,
        InvalidFileName = 18,
        InvalidLocalActionType = 19,
        InvalidTargetType = 20,
        InvalidIndex = 21,
        ZeroTarget = 22,
        PathTooLong = 23,
        CantLoadImage = 24,
        InvalidBitmap = 25,
        InvalidThreadType = 26,
        InvalidActionEnableType = 27,
        InvalidParameterCombination = 28,
        InvalidRenameCurrentType = 29,
        InvalidInfoType = 30,
        InvalidGroupId = 31,
        InvalidSelectionType = 32,
    }
}
