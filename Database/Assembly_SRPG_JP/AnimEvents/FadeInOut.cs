﻿// Decompiled with JetBrains decompiler
// Type: SRPG.AnimEvents.FadeInOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 85BFDF7F-5712-4D45-9CD6-3465C703DFDF
// Assembly location: S:\Desktop\Assembly-CSharp.dll

using UnityEngine;

namespace SRPG.AnimEvents
{
  public class FadeInOut : AnimEvent
  {
    public Color FadeColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    public bool IsFadeOut;
    public bool IsAdditive;

    public override void OnStart(GameObject go)
    {
      if (this.IsFadeOut)
        this.FadeColor = Color.get_clear();
      FadeController.Instance.FadeTo(this.FadeColor, this.End - this.Start, !this.IsAdditive ? 2 : 1);
    }
  }
}
