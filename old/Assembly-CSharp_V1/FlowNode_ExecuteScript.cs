﻿// Decompiled with JetBrains decompiler
// Type: FlowNode_ExecuteScript
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

[FlowNode.Pin(1, "Output", FlowNode.PinTypes.Output, 1)]
[FlowNode.NodeType("Script", 32741)]
[FlowNode.Pin(10, "Input", FlowNode.PinTypes.Input, 0)]
public class FlowNode_ExecuteScript : FlowNode
{
  public string ScriptName = string.Empty;

  public override string GetCaption()
  {
    return base.GetCaption() + ":" + this.ScriptName;
  }

  public override void OnActivate(int pinID)
  {
    this.ActivateOutputLinks(1);
  }
}
