﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ItemListDetailWindow
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using GR;
using UnityEngine;

namespace SRPG
{
  [FlowNode.Pin(1, "Reload", FlowNode.PinTypes.Input, 1)]
  public class ItemListDetailWindow : MonoBehaviour, IFlowInterface
  {
    public ItemListDetailWindow()
    {
      base.\u002Ector();
    }

    public void Activated(int pinID)
    {
      this.Refresh();
    }

    private void Start()
    {
      this.Refresh();
    }

    private void Refresh()
    {
      ItemData itemDataByItemId = MonoSingleton<GameManager>.Instance.Player.FindItemDataByItemID(GlobalVars.SelectedItemID);
      if (itemDataByItemId != null)
      {
        DataSource.Bind<ItemData>(((Component) this).get_gameObject(), itemDataByItemId);
      }
      else
      {
        ItemParam itemParam = MonoSingleton<GameManager>.Instance.MasterParam.GetItemParam(GlobalVars.SelectedItemID);
        if (itemParam == null)
          return;
        DataSource.Bind<ItemParam>(((Component) this).get_gameObject(), itemParam);
      }
      GameParameter.UpdateAll(((Component) this).get_gameObject());
      ((Behaviour) this).set_enabled(true);
    }
  }
}
