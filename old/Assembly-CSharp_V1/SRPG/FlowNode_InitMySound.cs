﻿// Decompiled with JetBrains decompiler
// Type: SRPG.FlowNode_InitMySound
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

namespace SRPG
{
  [FlowNode.Pin(0, "In", FlowNode.PinTypes.Input, 0)]
  [FlowNode.NodeType("System/InitMySound", 65535)]
  [FlowNode.Pin(1, "Out", FlowNode.PinTypes.Output, 1)]
  public class FlowNode_InitMySound : FlowNode
  {
    private void Init()
    {
      MySound.RestoreFromClearCache();
    }

    public override void OnActivate(int pinID)
    {
      if (pinID != 0)
        return;
      this.Init();
      this.ActivateOutputLinks(1);
    }
  }
}
