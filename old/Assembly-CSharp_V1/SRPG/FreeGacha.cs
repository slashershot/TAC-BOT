﻿// Decompiled with JetBrains decompiler
// Type: SRPG.FreeGacha
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

namespace SRPG
{
  public class FreeGacha
  {
    public int num;
    public long at;

    public bool Deserialize(Json_FreeGacha json)
    {
      this.num = json.num;
      this.at = json.at;
      return true;
    }
  }
}
