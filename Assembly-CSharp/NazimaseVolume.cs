﻿// Decompiled with JetBrains decompiler
// Type: NazimaseVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("SRPG/背景ツール/なじませボリューム")]
public class NazimaseVolume : MonoBehaviour
{
  public NazimaseVolume()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    ((Component) this).set_tag("EditorOnly");
  }

  public Bounds Bounds
  {
    get
    {
      return new Bounds(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_localScale());
    }
  }
}
