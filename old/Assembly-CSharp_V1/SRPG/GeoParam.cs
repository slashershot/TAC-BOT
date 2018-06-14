﻿// Decompiled with JetBrains decompiler
// Type: SRPG.GeoParam
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using System;

namespace SRPG
{
  [Serializable]
  public class GeoParam
  {
    private string localizedNameID;
    public string iname;
    public string name;
    public OInt cost;
    public OBool DisableStopped;

    protected void localizeFields(string language)
    {
      this.init();
      this.name = LocalizedText.SGGet(language, GameUtility.LocalizedMasterParamFileName, this.localizedNameID);
    }

    protected void init()
    {
      this.localizedNameID = this.GetType().GenerateLocalizedID(this.iname, "NAME");
    }

    public void Deserialize(string language, JSON_GeoParam json)
    {
      this.Deserialize(json);
      this.localizeFields(language);
    }

    public bool Deserialize(JSON_GeoParam json)
    {
      if (json == null)
        return false;
      this.iname = json.iname;
      this.name = json.name;
      this.cost = (OInt) Math.Max(json.cost, 1);
      this.DisableStopped = (OBool) (json.stop != 0);
      return true;
    }
  }
}
