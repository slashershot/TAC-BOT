﻿// Decompiled with JetBrains decompiler
// Type: GooglePlayGames.Native.Cwrapper.SnapshotMetadataChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
  internal static class SnapshotMetadataChange
  {
    [DllImport("gpg")]
    internal static extern UIntPtr SnapshotMetadataChange_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

    [DllImport("gpg")]
    internal static extern IntPtr SnapshotMetadataChange_Image(HandleRef self);

    [DllImport("gpg")]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool SnapshotMetadataChange_PlayedTimeIsChanged(HandleRef self);

    [DllImport("gpg")]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool SnapshotMetadataChange_Valid(HandleRef self);

    [DllImport("gpg")]
    internal static extern ulong SnapshotMetadataChange_PlayedTime(HandleRef self);

    [DllImport("gpg")]
    internal static extern void SnapshotMetadataChange_Dispose(HandleRef self);

    [DllImport("gpg")]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool SnapshotMetadataChange_ImageIsChanged(HandleRef self);

    [DllImport("gpg")]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool SnapshotMetadataChange_DescriptionIsChanged(HandleRef self);
  }
}
