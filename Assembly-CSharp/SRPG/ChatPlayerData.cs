﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ChatPlayerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

namespace SRPG
{
  public class ChatPlayerData
  {
    public string name;
    public int exp;
    public UnitData unit;
    public long lastlogin;
    public byte is_friend;
    public byte is_favorite;
    public string fuid;
    public int lv;
    public string award;

    public bool IsFriend
    {
      get
      {
        return (int) this.is_friend != 0;
      }
    }

    public bool IsFavorite
    {
      get
      {
        return (int) this.is_favorite != 0;
      }
    }

    public void Deserialize(JSON_ChatPlayerData json)
    {
      if (json == null)
        return;
      this.name = json.name;
      this.exp = json.exp;
      this.lastlogin = json.lastlogin;
      this.fuid = json.fuid;
      this.is_friend = json.is_friend;
      this.is_favorite = json.is_favorite;
      this.lv = PlayerData.CalcLevelFromExp(this.exp);
      this.award = json.award;
      if (json.unit == null)
        return;
      UnitData unitData = new UnitData();
      unitData.Deserialize(json.unit);
      this.unit = unitData;
    }

    public FriendData ToFriendData()
    {
      FriendData friendData = new FriendData();
      friendData.FUID = this.fuid;
      friendData.PlayerName = this.name;
      friendData.PlayerLevel = this.lv;
      friendData.LastLogin = this.lastlogin;
      friendData.Unit = this.unit;
      friendData.SelectAward = this.award;
      friendData.IsFavorite = this.IsFavorite;
      if (this.unit != null)
      {
        friendData.UnitID = this.unit.UnitID;
        friendData.UnitLevel = this.unit.Lv;
        friendData.UnitRarity = this.unit.Rarity;
      }
      return friendData;
    }
  }
}
