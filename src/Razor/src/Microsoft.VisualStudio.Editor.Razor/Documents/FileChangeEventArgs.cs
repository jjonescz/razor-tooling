﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;

namespace Microsoft.VisualStudio.Editor.Razor.Documents
{
    internal sealed class FileChangeEventArgs : EventArgs
    {
        public FileChangeEventArgs(string filePath, FileChangeKind kind)
        {
            FilePath = filePath;
            Kind = kind;
        }

        public string FilePath { get; }

        public FileChangeKind Kind { get; }
    }
}
