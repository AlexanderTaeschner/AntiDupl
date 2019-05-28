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
using System.Runtime.InteropServices;
using System.Threading;
using AntiDuplLib;

namespace AntiDupl.NET
{
    public class CoreLib : IDisposable
    {
        private const uint PAGE_SIZE = 16;

        private readonly IntPtr _handle = IntPtr.Zero;
        private CoreDll _dll;

        //-----------Public functions----------------------------------------------

        public CoreLib(string userPath)
        {
            try
            {
                _dll = new CoreDll();
            }
            catch
            {
                throw new Exception("Can't load core library!");
            }
            if (Version.Compatible(GetVersion(VersionType.AntiDupl)))
            {
                _handle = _dll.adCreateW(userPath);
            }
            else
            {
                throw new Exception("Incompatible core library version!");
            }
        }

        ~CoreLib()
        {
            Dispose();
        }

        public void Release()
        {
            if (_dll != null && _handle != IntPtr.Zero)
            {
                if (_dll.adRelease(_handle) == Error.AccessDenied)
                {
                    Stop();
                    Thread.Sleep(10);
                    _dll.adRelease(_handle);
                }
            }
        }

        public void Dispose()
        {
            Release();
            if (_dll != null)
            {
                _dll.Dispose();
                _dll = null;
            }
            GC.SuppressFinalize(this);
        }

        public CoreVersion GetVersion(VersionType versionType)
        {
            string version;
            switch (versionType)
            {
                case VersionType.AntiDupl:
                    version = External.Version;
                    break;
                case VersionType.OpenJpeg:
                    version = OpenJpeg.Version();
                    break;
                case VersionType.Simd:
                    version = Simd.Version();
                    break;
                default:
                    throw new NotSupportedException();
            }

            return new CoreVersion(version);
        }

        public bool Stop()
        {
            return _dll.adStop(_handle) == Error.Ok;
        }

        public bool Search()
        {
            return _dll.adSearch(_handle) == Error.Ok;
        }

        public bool Load(FileType fileType, string fileName, bool check)
        {
            return _dll.adLoadW(_handle, fileType, fileName, check ? CoreDll.TRUE : CoreDll.FALSE) == Error.Ok;
        }

        public bool Save(FileType fileType, string fileName)
        {
            return _dll.adSaveW(_handle, fileType, fileName) == Error.Ok;
        }

        public bool Clear(FileType fileType)
        {
            return _dll.adClear(_handle, fileType) == Error.Ok;
        }

        public bool SetDefaultOptions()
        {
            return _dll.adOptionsSet(_handle, CoreDll.OptionsType.SetDefault, IntPtr.Zero) == Error.Ok;
        }

        public CoreStatus? StatusGet(ThreadType threadType, int threadId)
        {
            object statusObject = new CoreDll.adStatusW();
            var status = new byte[Marshal.SizeOf(statusObject)];
            var pStatus = Marshal.UnsafeAddrOfPinnedArrayElement(status, 0);
            if (_dll.adStatusGetW(_handle, threadType, new IntPtr(threadId), pStatus) == Error.Ok)
            {
                var statusW = (CoreDll.adStatusW)Marshal.PtrToStructure(pStatus, statusObject.GetType());
                try
                {
                    return new CoreStatus(ref statusW);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public bool SortResult(SortType sortType, bool increasing)
        {
            return _dll.adResultSort(_handle, sortType, increasing ? CoreDll.TRUE : CoreDll.FALSE) == Error.Ok;
        }

        public bool ApplyToResult(GlobalActionType globalActionType)
        {
            return _dll.adResultApply(_handle, globalActionType) == Error.Ok;
        }

        public bool ApplyToResult(LocalActionType localActionType, TargetType targetType)
        {
            return _dll.adResultApplyTo(_handle, localActionType, targetType) == Error.Ok;
        }

        public bool CanApply(ActionEnableType actionEnableType)
        {
            var enable = new int[1];
            if (_dll.adCanApply(_handle, actionEnableType, Marshal.UnsafeAddrOfPinnedArrayElement(enable, 0)) != Error.Ok)
            {
                return false;
            }

            return enable[0] != CoreDll.FALSE;
        }

        public bool RenameCurrent(RenameCurrentType renameCurrentType, string newFileName)
        {
            return _dll.adRenameCurrentW(_handle, renameCurrentType, newFileName) == Error.Ok;
        }

        public bool MoveCurrentGroup(string directory)
        {
            return _dll.adMoveCurrentGroupW(_handle, directory) == Error.Ok;
        }

        public bool RenameCurrentGroupAs(string fileName)
        {
            return _dll.adRenameCurrentGroupAsW(_handle, fileName) == Error.Ok;
        }

        public CoreResult[]? GetResult(uint startFrom, uint size)
        {
            var resultSize = GetResultSize();
            if (resultSize > startFrom)
            {
                object resultObject = new CoreDll.adResultW();
                var sizeOfResult = Marshal.SizeOf(resultObject);
                var buffer = new byte[sizeOfResult * PAGE_SIZE];
                size = Math.Min(resultSize - startFrom, size);
                var results = new CoreResult[size];
                var pageCount = (uint)(size / PAGE_SIZE + (size % PAGE_SIZE > 0 ? 1 : 0));
                for (uint page = 0; page < pageCount; ++page)
                {
                    var pStartFrom = new UIntPtr[1];
                    pStartFrom[0] = new UIntPtr(startFrom + page * PAGE_SIZE);

                    var pSize = new UIntPtr[1];
                    pSize[0] = new UIntPtr(PAGE_SIZE);

                    if (_dll.adResultGetW(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(pSize, 0)) == Error.Ok)
                    {
                        for (uint i = 0; i < pSize[0].ToUInt32(); ++i)
                        {
                            var pResult = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, (int)(i * sizeOfResult));
                            var result = (CoreDll.adResultW)Marshal.PtrToStructure(pResult, resultObject.GetType());
                            results[page * PAGE_SIZE + i] = new CoreResult(ref result);
                        }

                    }
                }
                return results;
            }
            return null;
        }

        public uint GetResultSize()
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(uint.MaxValue);
            if (_dll.adResultGetW(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                new IntPtr(1), new IntPtr(1)) == Error.InvalidStartPosition)
            {
                return pStartFrom[0].ToUInt32();
            }
            return 0;
        }

        public bool SetSelection(uint startFrom, uint size, bool value)
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(startFrom);
            return _dll.adSelectionSet(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0), new UIntPtr(size),
                value ? CoreDll.TRUE : CoreDll.FALSE) == Error.Ok;
        }

        public bool[]? GetSelection(uint startFrom, uint size)
        {
            var pSelection = new int[size];
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(startFrom);
            var pSelectionSize = new UIntPtr[1];
            pSelectionSize[0] = new UIntPtr(size);
            if (_dll.adSelectionGet(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelection, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelectionSize, 0)) == Error.Ok)
            {
                var selection = new bool[pSelectionSize[0].ToUInt32()];
                for (var i = 0; i < selection.Length; ++i)
                {
                    selection[i] = pSelection[i] != CoreDll.FALSE;
                }

                return selection;
            }
            return null;
        }

        public bool SetCurrent(int index)
        {
            return _dll.adCurrentSet(_handle, new IntPtr(index)) == Error.Ok;
        }

        public int GetCurrent()
        {
            var index = new IntPtr[1];
            index[0] = new IntPtr();
            if (_dll.adCurrentGet(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(index, 0)) == Error.Ok)
            {
                return index[0].ToInt32();
            }
            return -1;
        }

        public CoreGroup[]? GetGroup(uint startFrom, uint size)
        {
            var groupSize = GetGroupSize();
            if (groupSize > startFrom)
            {
                object groupObject = new CoreDll.adGroup();
                var sizeOfGroup = Marshal.SizeOf(groupObject);
                size = Math.Min(groupSize - startFrom, size);
                var buffer = new byte[sizeOfGroup * size];
                var groups = new CoreGroup[size];
                var pStartFrom = new UIntPtr[1];
                pStartFrom[0] = new UIntPtr(startFrom);
                var pSize = new UIntPtr[1];
                pSize[0] = new UIntPtr(size);
                if (_dll.adGroupGet(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                    Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                    Marshal.UnsafeAddrOfPinnedArrayElement(pSize, 0)) == Error.Ok)
                {
                    for (uint i = 0; i < pSize[0].ToUInt32(); ++i)
                    {
                        var pGroup = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, (int)(i * sizeOfGroup));
                        var group = (CoreDll.adGroup)Marshal.PtrToStructure(pGroup, groupObject.GetType());
                        groups[i] = new CoreGroup(ref group, this);
                    }
                }
                return groups;
            }
            return null;
        }

        /// <summary>
        /// Âîçâðàøàåò îáùåå êîëè÷åñòâî ãðóïï.
        /// </summary>
        /// <returns></returns>
        public uint GetGroupSize()
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(uint.MaxValue);
            if (_dll.adGroupGet(_handle, Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                new IntPtr(1), new IntPtr(1)) == Error.InvalidStartPosition)
            {
                return pStartFrom[0].ToUInt32();
            }
            return 0;
        }

        /// <summary>
        /// Âîçâðàøàåò ìàññèâ CoreImageInfo ñîäåðæàùèõñÿ â ïåðåäàííîé ãðóïïå.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="startFrom"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public CoreImageInfo[]? GetImageInfo(int groupId, uint startFrom, uint size)
        {
            var imageInfoSize = GetImageInfoSize(groupId);
            if (imageInfoSize > startFrom)
            {
                object imageInfoObject = new CoreDll.adImageInfoW();
                var sizeOfImageInfo = Marshal.SizeOf(imageInfoObject);
                var buffer = new byte[sizeOfImageInfo * PAGE_SIZE];
                size = Math.Min(imageInfoSize - startFrom, size);
                var imageInfos = new CoreImageInfo[size];
                var pageCount = (uint)(size / PAGE_SIZE + (size % PAGE_SIZE > 0 ? 1 : 0));
                for (uint page = 0; page < pageCount; ++page)
                {
                    var pStartFrom = new UIntPtr[1];
                    pStartFrom[0] = new UIntPtr(startFrom + page * PAGE_SIZE);

                    var pSize = new UIntPtr[1];
                    pSize[0] = new UIntPtr(PAGE_SIZE);

                    if (_dll.adImageInfoGetW(_handle, new IntPtr(groupId), Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                        Marshal.UnsafeAddrOfPinnedArrayElement(pSize, 0)) == Error.Ok)
                    {
                        for (uint i = 0; i < pSize[0].ToUInt32(); ++i)
                        {
                            var pImageInfo = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, (int)(i * sizeOfImageInfo));
                            var imageInfo = (CoreDll.adImageInfoW)Marshal.PtrToStructure(pImageInfo, imageInfoObject.GetType());
                            imageInfos[page * PAGE_SIZE + i] = new CoreImageInfo(ref imageInfo);
                        }

                    }
                }
                return imageInfos;
            }
            return null;
        }

        /// <summary>
        /// Âîçâðàùàåò êîëè÷åñòâî èçîáðàæåíèé â ïåðåäàííîé ãðóïïå.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public uint GetImageInfoSize(int groupId)
        {
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(uint.MaxValue);
            if (_dll.adImageInfoGetW(_handle, new IntPtr(groupId), Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                new IntPtr(1), new IntPtr(1)) == Error.InvalidStartPosition)
            {
                return pStartFrom[0].ToUInt32();
            }
            return 0;
        }

        public bool SetSelection(int groupId, int index, SelectionType selectionType)
        {
            return _dll.adImageInfoSelectionSet(_handle, new IntPtr(groupId), new IntPtr(index), selectionType) == Error.Ok;
        }

        public bool[]? GetSelection(int groupId, uint startFrom, uint size)
        {
            var pSelection = new int[size];
            var pStartFrom = new UIntPtr[1];
            pStartFrom[0] = new UIntPtr(startFrom);
            var pSelectionSize = new UIntPtr[1];
            pSelectionSize[0] = new UIntPtr(size);
            if (_dll.adImageInfoSelectionGet(_handle, new IntPtr(groupId), Marshal.UnsafeAddrOfPinnedArrayElement(pStartFrom, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelection, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(pSelectionSize, 0)) == Error.Ok)
            {
                var selection = new bool[pSelectionSize[0].ToUInt32()];
                for (var i = 0; i < selection.Length; ++i)
                {
                    selection[i] = pSelection[i] != CoreDll.FALSE;
                }

                return selection;
            }
            return null;
        }

        public bool Rename(int groupId, int index, string newFileName)
        {
            return _dll.adImageInfoRenameW(_handle, new IntPtr(groupId), new IntPtr(index), newFileName) == Error.Ok;
        }

        public System.Drawing.Bitmap? LoadBitmap(int width, int height, string path)
        {
            if (height * width == 0)
            {
                return null;
            }

            System.Drawing.Bitmap? bitmap = null;
            try
            {
                bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            catch (System.Exception)
            {
                GC.Collect();
                try
                {
                    bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
            var bitmapData = new System.Drawing.Imaging.BitmapData();
            bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                bitmapData);
            var pBitmap = new CoreDll.adBitmap[1];
            pBitmap[0].width = (uint)bitmapData.Width;
            pBitmap[0].height = (uint)bitmapData.Height;
            pBitmap[0].stride = bitmapData.Stride;
            pBitmap[0].format = CoreDll.PixelFormatType.Argb32;
            pBitmap[0].data = bitmapData.Scan0;
            var error = _dll.adLoadBitmapW(_handle, path, Marshal.UnsafeAddrOfPinnedArrayElement(pBitmap, 0));
            bitmap.UnlockBits(bitmapData);
            return error == Error.Ok ? bitmap : null;
        }

        /// <summary>
        /// Âîçâðàøàåò çàãðóæåííîå èçîáðàæåíèå ïî çàëàííîìó ïóòè è çàäàííîãî ðàçìåðà.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap? LoadBitmap(System.Drawing.Size size, string path)
        {
            return LoadBitmap(size.Width, size.Height, path);
        }

        public System.Drawing.Bitmap? LoadBitmap(CoreImageInfo imageInfo)
        {
            return LoadBitmap((int)imageInfo.Width, (int)imageInfo.Height, imageInfo.Path);
        }

        public CoreSearchOptions searchOptions
        {
            get
            {
                var options = new CoreDll.adSearchOptions[1];
                _dll.adOptionsGet(_handle, CoreDll.OptionsType.Search, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreSearchOptions(options[0]);
            }
            set
            {
                var options = new CoreDll.adSearchOptions[1];
                value.ConvertTo(ref options[0]);
                _dll.adOptionsSet(_handle, CoreDll.OptionsType.Search, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CoreCompareOptions compareOptions
        {
            get
            {
                var options = new CoreDll.adCompareOptions[1];
                _dll.adOptionsGet(_handle, CoreDll.OptionsType.Compare, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreCompareOptions(ref options[0]);
            }
            set
            {
                var options = new CoreDll.adCompareOptions[1];
                value.ConvertTo(ref options[0]);
                _dll.adOptionsSet(_handle, CoreDll.OptionsType.Compare, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CoreDefectOptions defectOptions
        {
            get
            {
                var options = new CoreDll.adDefectOptions[1];
                _dll.adOptionsGet(_handle, CoreDll.OptionsType.Defect, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreDefectOptions(ref options[0]);
            }
            set
            {
                var options = new CoreDll.adDefectOptions[1];
                value.ConvertTo(ref options[0]);
                _dll.adOptionsSet(_handle, CoreDll.OptionsType.Defect, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CoreAdvancedOptions advancedOptions
        {
            get
            {
                var options = new CoreDll.adAdvancedOptions[1];
                _dll.adOptionsGet(_handle, CoreDll.OptionsType.Advanced, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
                return new CoreAdvancedOptions(ref options[0]);
            }
            set
            {
                var options = new CoreDll.adAdvancedOptions[1]; //ñîçäàåì ìàññèâ èç îäíîãî çíà÷åíèÿ
                value.ConvertTo(ref options[0]); //êîíâåðòèðóåì ïåðåäàííûé êëàññ
                _dll.adOptionsSet(_handle, CoreDll.OptionsType.Advanced, Marshal.UnsafeAddrOfPinnedArrayElement(options, 0));
            }
        }

        public CorePathWithSubFolder[] searchPath
        {
            get
            {
                return GetPath(PathType.Search);
            }
            set
            {
                SetPath(PathType.Search, value);
            }
        }

        public CorePathWithSubFolder[] ignorePath
        {
            get
            {
                return GetPath(PathType.Ignore);
            }
            set
            {
                SetPath(PathType.Ignore, value);
            }
        }

        public CorePathWithSubFolder[] validPath
        {
            get
            {
                return GetPath(PathType.Valid);
            }
            set
            {
                SetPath(PathType.Valid, value);
            }
        }

        public CorePathWithSubFolder[] deletePath
        {
            get
            {
                return GetPath(PathType.Delete);
            }
            set
            {
                SetPath(PathType.Delete, value);
            }
        }

        static private string? BufferToString(char[] buffer, int startIndex, int maxSize)
        {
            if (startIndex >= buffer.Length)
            {
                return null;
            }

            int i = 0, n = Math.Min(maxSize, buffer.Length - startIndex);
            for (; i < n; ++i)
            {
                if (buffer[startIndex + i] == (char)0)
                {
                    break;
                }
            }
            return new string(buffer, startIndex, i);
        }

        private CorePathWithSubFolder[] GetPath(PathType pathType)
        {
            var pathWSF = new CorePathWithSubFolder[0];
            var size = new IntPtr[1];
            var path = new string[0];
            if (_dll.adPathGetW(_handle, pathType, new IntPtr(1), Marshal.UnsafeAddrOfPinnedArrayElement(size, 0)) ==
                            Error.OutputBufferIsTooSmall)
            {
                var buffer = new char[(CoreDll.MAX_PATH_EX + 1) * size[0].ToInt32()];
                if (_dll.adPathGetW(_handle, pathType, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                    Marshal.UnsafeAddrOfPinnedArrayElement(size, 0)) == Error.Ok)
                {
                    pathWSF = new CorePathWithSubFolder[size[0].ToInt32()];
                    for (var i = 0; i < size[0].ToInt32(); ++i)
                    {
                        pathWSF[i] = new CorePathWithSubFolder();
                        pathWSF[i].path = BufferToString(buffer, i * (CoreDll.MAX_PATH_EX + 1), CoreDll.MAX_PATH_EX);
                        if (buffer[(CoreDll.MAX_PATH_EX + 1) * i + CoreDll.MAX_PATH_EX] == (char)1)
                        {
                            pathWSF[i].enableSubFolder = true;
                        }
                        else
                        {
                            pathWSF[i].enableSubFolder = false;
                        }
                    }
                }
            }
            return pathWSF;
        }

        private bool SetPath(PathType pathType, CorePathWithSubFolder[] path)
        {
            var buffer = new char[path.Length * (CoreDll.MAX_PATH_EX + 1)];
            for (var i = 0; i < path.Length; i++)
            {
                path[i].path.CopyTo(0, buffer, i * (CoreDll.MAX_PATH_EX + 1), path[i].path.Length);
                buffer[(CoreDll.MAX_PATH_EX + 1) * i + CoreDll.MAX_PATH_EX] = path[i].enableSubFolder ? (char)1 : (char)0;
            }

            return _dll.adPathWithSubFolderSetW(_handle,
                pathType,
                Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0),
                new IntPtr(path.Length)) == Error.Ok;
        }
    };
}
