﻿// Decompiled with JetBrains decompiler
// Type: ScreenUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

public static class ScreenUtility
{
  private static int mDefaultScreenWidth = Screen.get_width();
  private static int mDefaultScreenHeight = Screen.get_height();

  public static void SetResolution(int w, int h)
  {
    Screen.SetResolution(w, h, true);
  }

  public static int DefaultScreenWidth
  {
    get
    {
      return ScreenUtility.mDefaultScreenWidth;
    }
  }

  public static int DefaultScreenHeight
  {
    get
    {
      return ScreenUtility.mDefaultScreenHeight;
    }
  }

  public static float ScreenWidthScale
  {
    get
    {
      return (float) ScreenUtility.mDefaultScreenWidth / (float) Screen.get_width();
    }
  }

  public static float ScreenHeightScale
  {
    get
    {
      return (float) ScreenUtility.mDefaultScreenHeight / (float) Screen.get_height();
    }
  }
}
