﻿// Decompiled with JetBrains decompiler
// Type: SRPG.MultiPlayAPIRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using System.Security.Cryptography;
using System.Text;

namespace SRPG
{
  public class MultiPlayAPIRoom
  {
    private static readonly string LOCKED = "1";
    public int roomid;
    public string comment;
    public MultiPlayAPIRoom.Quest quest;
    public string pwd_hash;
    public int limit;
    public int unitlv;
    public int clear;
    public int num;
    public MultiPlayAPIRoom.Owner owner;

    public static string CalcHash(string pwd)
    {
      if (string.IsNullOrEmpty(pwd))
        return string.Empty;
      byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(pwd));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.AppendFormat("{0:x2}", (object) hash[index]);
      return stringBuilder.ToString();
    }

    public static bool IsLocked(string pwd)
    {
      return pwd == MultiPlayAPIRoom.LOCKED;
    }

    public class Quest
    {
      public string iname;
    }

    public class Owner
    {
      public string name;
      public string level;
      public Json_Unit[] units;
    }
  }
}
