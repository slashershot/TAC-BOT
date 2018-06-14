﻿// Decompiled with JetBrains decompiler
// Type: FlowNode_GameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("")]
[FlowNode.NodeType("GameObject", 32741)]
public class FlowNode_GameObject : FlowNode
{
  [FlowNode.ShowInInfo(true)]
  public Component Target;

  public override FlowNode.Pin[] GetDynamicPins()
  {
    if (Object.op_Inequality((Object) this.Target, (Object) null))
      return (FlowNode.Pin[]) ((object) this.Target).GetType().GetCustomAttributes(typeof (FlowNode.Pin), true);
    return base.GetDynamicPins();
  }

  public override bool OnDragUpdate(object[] objectReferences)
  {
    return false;
  }

  public override bool OnDragPerform(object[] objectReferences)
  {
    return false;
  }

  public override void OnActivate(int pinID)
  {
    if (!Object.op_Inequality((Object) this.Target, (Object) null))
      return;
    IFlowInterface target = (IFlowInterface) this.Target;
    if (target == null)
      return;
    target.Activated(pinID);
  }

  public static void ActivateOutputLinks(Component caller, int pinID)
  {
    FlowNode_GameObject[] componentsInParent = (FlowNode_GameObject[]) caller.GetComponentsInParent<FlowNode_GameObject>();
    for (int index = 0; index < componentsInParent.Length; ++index)
    {
      if (Object.op_Equality((Object) componentsInParent[index].Target, (Object) caller))
        componentsInParent[index].ActivateOutputLinks(pinID);
    }
  }
}
