﻿// Decompiled with JetBrains decompiler
// Type: Win_Btn_YN_Title_Flx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("UI/Drafts/Win_Btn_YN_Title_Flx")]
public class Win_Btn_YN_Title_Flx : UIDraft
{
  [UIDraft.AutoGenerated]
  public Text Text_Title;
  [UIDraft.AutoGenerated]
  public Text Text_Message;
  [UIDraft.AutoGenerated]
  public Button Btn_No;
  [UIDraft.AutoGenerated]
  public Button Btn_Yes;
  public Text Txt_No;
  public Text Txt_Yes;
  public UIUtility.DialogResultEvent OnClickYes;
  public UIUtility.DialogResultEvent OnClickNo;

  private void OnWindowClose(UIWindow window)
  {
    Object.DestroyImmediate((Object) ((Component) ((Component) this).GetComponentInParent<Canvas>()).get_gameObject());
  }

  public void BeginClose()
  {
    UIUtility.PopCanvas(true);
    UIWindow component = (UIWindow) ((Component) this).GetComponent<UIWindow>();
    component.OnWindowClose = new UIWindow.WindowEvent(this.OnWindowClose);
    component.Close();
  }

  private void OnClickButton(GameObject obj)
  {
    this.BeginClose();
    if (Object.op_Equality((Object) obj, (Object) ((Component) this.Btn_Yes).get_gameObject()))
    {
      if (this.OnClickYes == null)
        return;
      this.OnClickYes(((Component) this).get_gameObject());
    }
    else
    {
      if (this.OnClickNo == null)
        return;
      this.OnClickNo(((Component) this).get_gameObject());
    }
  }

  private void Awake()
  {
    UIUtility.AddEventListener(((Component) this.Btn_Yes).get_gameObject(), (UnityEvent) this.Btn_Yes.get_onClick(), new UIUtility.EventListener(this.OnClickButton));
    UIUtility.AddEventListener(((Component) this.Btn_No).get_gameObject(), (UnityEvent) this.Btn_No.get_onClick(), new UIUtility.EventListener(this.OnClickButton));
  }
}
