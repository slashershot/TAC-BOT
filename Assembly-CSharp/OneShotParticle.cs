﻿// Decompiled with JetBrains decompiler
// Type: OneShotParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

[DisallowMultipleComponent]
public class OneShotParticle : MonoBehaviour
{
  public OneShotParticle()
  {
    base.\u002Ector();
  }

  private void LateUpdate()
  {
    ParticleSystem[] componentsInChildren1 = (ParticleSystem[]) ((Component) this).get_gameObject().GetComponentsInChildren<ParticleSystem>();
    for (int index = componentsInChildren1.Length - 1; index >= 0; --index)
    {
      if (componentsInChildren1[index].IsAlive())
        return;
    }
    UIParticleSystem[] componentsInChildren2 = (UIParticleSystem[]) ((Component) this).get_gameObject().GetComponentsInChildren<UIParticleSystem>();
    for (int index = componentsInChildren2.Length - 1; index >= 0; --index)
    {
      if (componentsInChildren2[index].IsAlive())
        return;
    }
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }
}
