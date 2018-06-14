﻿// Decompiled with JetBrains decompiler
// Type: SyncStateTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

public class SyncStateTime : StateMachineBehaviour
{
  public SyncStateTime()
  {
    base.\u002Ector();
  }

  public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    // ISSUE: explicit reference operation
    // ISSUE: explicit reference operation
    // ISSUE: explicit reference operation
    if ((double) ((AnimatorStateInfo) @stateInfo).get_length() <= 0.0 || (double) (((AnimatorStateInfo) @stateInfo).get_length() * (((AnimatorStateInfo) @stateInfo).get_normalizedTime() % 1f)) > (animator.get_updateMode() != 2 ? (double) Time.get_deltaTime() : (double) Time.get_unscaledDeltaTime()))
      return;
    // ISSUE: explicit reference operation
    float length = ((AnimatorStateInfo) @stateInfo).get_length();
    float num = (animator.get_updateMode() != 2 ? Time.get_time() : Time.get_unscaledTime()) % length / length;
    if ((double) num <= 0.0)
      return;
    // ISSUE: explicit reference operation
    animator.Play(((AnimatorStateInfo) @stateInfo).get_fullPathHash(), layerIndex, num);
  }
}
