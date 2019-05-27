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
    public enum LocalActionType : int //то же что и  enum adLocalActionType : adInt32, также еще в class HotKeyOptions enum Action
    {
        DeleteDefect = 0,
        DeleteFirst = 1,
        DeleteSecond = 2,
        DeleteBoth = 3,
        RenameFirstToSecond = 4,
        RenameSecondToFirst = 5,
        RenameFirstLikeSecond = 6,
        RenameSecondLikeFirst = 7,
        MoveFirstToSecond = 8,
        MoveSecondToFirst = 9,
        MoveAndRenameFirstToSecond = 10,
        MoveAndRenameSecondToFirst = 11,
        PerformHint = 12,
        Mistake = 13,
    }
}
