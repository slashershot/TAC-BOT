﻿// Decompiled with JetBrains decompiler
// Type: GachaDetailParam
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

public class GachaDetailParam
{
  private string localizedText;
  public int id;
  public string gname;
  public int type;
  public string text;
  public string image;
  public int width;
  public int height;

  protected void localizeFields(string language)
  {
    this.init();
    this.text = LocalizedText.SGGet(language, GameUtility.LocalizedGachaFileName, this.localizedText).Replace("\\n", "\n");
  }

  protected void init()
  {
    this.localizedText = this.GetType().GenerateLocalizedID(this.gname, "TEXT");
  }

  public bool Deserialize(string language, JSON_GachaDetailParam json)
  {
    if (!this.Deserialize(json))
      return false;
    this.localizeFields(language);
    return true;
  }

  public bool Deserialize(JSON_GachaDetailParam json)
  {
    if (json == null)
      return false;
    this.id = json.fields.id;
    this.gname = json.fields.gname;
    this.type = json.fields.type;
    this.text = json.fields.text;
    this.image = json.fields.image;
    this.width = json.fields.width;
    this.height = json.fields.height;
    return true;
  }

  public enum DetailType
  {
    none,
    text,
    image,
    all,
  }
}
