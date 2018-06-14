﻿// Decompiled with JetBrains decompiler
// Type: AndroidPermissionCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using System;
using UnityEngine;

public class AndroidPermissionCallback : AndroidJavaProxy
{
  public AndroidPermissionCallback(Action<string> onGrantedCallback, Action<string> onDeniedCallback)
  {
    this.\u002Ector("com.unity3d.plugin.UnityAndroidPermissions$IPermissionRequestResult");
    if (onGrantedCallback != null)
      this.OnPermissionGrantedAction += onGrantedCallback;
    if (onDeniedCallback == null)
      return;
    this.OnPermissionDeniedAction += onDeniedCallback;
  }

  private event Action<string> OnPermissionGrantedAction;

  private event Action<string> OnPermissionDeniedAction;

  public virtual void OnPermissionGranted(string permissionName)
  {
    if (this.OnPermissionGrantedAction == null)
      return;
    this.OnPermissionGrantedAction(permissionName);
  }

  public virtual void OnPermissionDenied(string permissionName)
  {
    if (this.OnPermissionDeniedAction == null)
      return;
    this.OnPermissionDeniedAction(permissionName);
  }
}
