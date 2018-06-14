﻿// Decompiled with JetBrains decompiler
// Type: ExitGames.UtilityScripts.ButtonInsideScrollList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
  public class ButtonInsideScrollList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private ScrollRect scrollRect;

    public ButtonInsideScrollList()
    {
      base.\u002Ector();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      if (!Object.op_Inequality((Object) this.scrollRect, (Object) null))
        return;
      this.scrollRect.StopMovement();
      ((Behaviour) this.scrollRect).set_enabled(false);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
      if (!Object.op_Inequality((Object) this.scrollRect, (Object) null) || ((Behaviour) this.scrollRect).get_enabled())
        return;
      ((Behaviour) this.scrollRect).set_enabled(true);
    }

    private void Start()
    {
      this.scrollRect = (ScrollRect) ((Component) this).GetComponentInParent<ScrollRect>();
    }
  }
}
