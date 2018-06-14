﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ReqVersusRoomJoin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

namespace SRPG
{
  public class ReqVersusRoomJoin : WebAPI
  {
    public ReqVersusRoomJoin(int roomID, Network.ResponseCallback response)
    {
      this.name = "vs/friendmatch/join";
      this.body = string.Empty;
      ReqVersusRoomJoin reqVersusRoomJoin = this;
      reqVersusRoomJoin.body = reqVersusRoomJoin.body + "\"roomid\":" + (object) roomID;
      this.body = WebAPI.GetRequestString(this.body);
      this.callback = response;
    }

    public class Quest
    {
      public string iname;
    }

    public class Response
    {
      public string app_id;
      public string token;
      public ReqVersusRoomJoin.Quest quest;
    }
  }
}
