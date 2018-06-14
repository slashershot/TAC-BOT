﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ReqGachaExec
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

namespace SRPG
{
  public class ReqGachaExec : WebAPI
  {
    public ReqGachaExec(string gachaid, Network.ResponseCallback response)
    {
      this.name = "gacha/exec";
      this.body = WebAPI.GetRequestString("\"gachaid\":\"" + gachaid + "\"");
      this.callback = response;
    }

    public ReqGachaExec(string iname, Network.ResponseCallback response, int free = 0, int num = 0)
    {
      this.name = "gacha/exec";
      this.body = "\"gachaid\":\"" + iname + "\",";
      ReqGachaExec reqGachaExec1 = this;
      reqGachaExec1.body = reqGachaExec1.body + "\"free\":" + free.ToString();
      if (num > 0)
      {
        ReqGachaExec reqGachaExec2 = this;
        reqGachaExec2.body = reqGachaExec2.body + ",\"ticketnum\":" + num.ToString();
      }
      this.body = WebAPI.GetRequestString(this.body);
      this.callback = response;
    }
  }
}
