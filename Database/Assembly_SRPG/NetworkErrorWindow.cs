﻿// Decompiled with JetBrains decompiler
// Type: SRPG.NetworkErrorWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using GR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRPG
{
  public class NetworkErrorWindow : MonoBehaviour
  {
    public Text Title;
    public Text StatusCode;
    public Text Message;

    public NetworkErrorWindow()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
    }

    private void Start()
    {
      if (Object.op_Inequality((Object) this.Title, (Object) null))
        this.Title.set_text(LocalizedText.Get("embed.CONN_ERR"));
      if (Object.op_Inequality((Object) this.StatusCode, (Object) null))
      {
        if (GameUtility.IsDebugBuild)
        {
          this.StatusCode.set_text(LocalizedText.Get("errorcode." + ((int) Network.ErrCode).ToString() + "_TITLE"));
          ((Component) this.StatusCode).get_gameObject().SetActive(true);
        }
        else
          ((Component) this.StatusCode).get_gameObject().SetActive(false);
      }
      if (Object.op_Inequality((Object) this.Message, (Object) null))
      {
        if (string.IsNullOrEmpty(Network.ErrMsg))
          this.Message.set_text(LocalizedText.Get("embed.APP_REBOOT", new object[1]
          {
            (object) Network.ErrCode.ToString()
          }));
        else
          this.Message.set_text(Network.ErrMsg);
      }
      Transform child = ((Component) this).get_transform().FindChild("window/Button");
      if (!Object.op_Inequality((Object) child, (Object) null))
        return;
      Button component = (Button) ((Component) child).GetComponent<Button>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      // ISSUE: method pointer
      ((UnityEvent) component.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClick)));
    }

    private void OnClick()
    {
      if (Network.ErrCode != Network.EErrCode.Authorize)
        return;
      MonoSingleton<GameManager>.Instance.ResetAuth();
    }

    public void OpenMaintenanceSite()
    {
      Application.OpenURL(Network.OfficialUrl);
    }

    public void OpenVersionUpSite()
    {
      Application.OpenURL(Network.OfficialUrl);
    }

    public void OpenStore()
    {
      Application.OpenURL("market://details?id=sg.gumi.alchemistww");
    }
  }
}
