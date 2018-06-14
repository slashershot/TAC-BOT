﻿// Decompiled with JetBrains decompiler
// Type: SRPG.FlowNode_PlayEventScript
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace SRPG
{
  [FlowNode.Pin(10, "Finished", FlowNode.PinTypes.Output, 10)]
  [FlowNode.NodeType("SRPG/Play Event Script", 32741)]
  [FlowNode.Pin(1, "Start", FlowNode.PinTypes.Input, 1)]
  public class FlowNode_PlayEventScript : FlowNode
  {
    public string ScriptID;

    public override void OnActivate(int pinID)
    {
      if (pinID != 1 || ((Behaviour) this).get_enabled())
        return;
      ((Behaviour) this).set_enabled(true);
      this.StartCoroutine(this.LoadAndPlayAsync("Events/" + this.ScriptID));
    }

    [DebuggerHidden]
    private IEnumerator LoadAndPlayAsync(string path)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FlowNode_PlayEventScript.\u003CLoadAndPlayAsync\u003Ec__Iterator8B() { path = path, \u003C\u0024\u003Epath = path, \u003C\u003Ef__this = this };
    }
  }
}
