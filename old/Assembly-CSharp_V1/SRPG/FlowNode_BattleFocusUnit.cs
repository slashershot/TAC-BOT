﻿// Decompiled with JetBrains decompiler
// Type: SRPG.FlowNode_BattleFocusUnit
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

namespace SRPG
{
  [FlowNode.NodeType("Battle/FocusUnit", 32741)]
  [FlowNode.Pin(0, "フォーカス", FlowNode.PinTypes.Input, 0)]
  public class FlowNode_BattleFocusUnit : FlowNode
  {
    public override void OnActivate(int pinID)
    {
      if (pinID != 0)
        return;
      SceneBattle.Instance.SetMoveCamera();
    }
  }
}
