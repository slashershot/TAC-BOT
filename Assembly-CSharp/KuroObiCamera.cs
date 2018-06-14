﻿// Decompiled with JetBrains decompiler
// Type: KuroObiCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
[DisallowMultipleComponent]
public class KuroObiCamera : MonoBehaviour
{
  private Camera mCamera;

  public KuroObiCamera()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.mCamera = (Camera) ((Component) this).GetComponent<Camera>();
    this.Update();
  }

  private void Update()
  {
    if (!SRPG_CanvasScaler.UseKuroObi)
    {
      this.mCamera.set_rect(new Rect(0.0f, 0.0f, 1f, 1f));
    }
    else
    {
      float num1 = (float) Screen.get_width() / (float) Screen.get_height();
      float num2 = 1.6f;
      if ((double) num1 >= (double) num2)
      {
        this.mCamera.set_rect(new Rect(0.0f, 0.0f, 1f, 1f));
      }
      else
      {
        float num3 = (float) (1.0 - (double) num1 / (double) num2);
        this.mCamera.set_rect(new Rect(0.0f, num3 * 0.5f, 1f, 1f - num3));
      }
    }
  }
}
