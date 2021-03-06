﻿// Decompiled with JetBrains decompiler
// Type: SRPG.EventAction_SpawnObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 85BFDF7F-5712-4D45-9CD6-3465C703DFDF
// Assembly location: S:\Desktop\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace SRPG
{
  [EventActionInfo("オブジェクト/配置", "シーンにオブジェクトを配置します。", 4478293, 4491400)]
  public class EventAction_SpawnObject : EventAction
  {
    public const string ResourceDir = "EventAssets/";
    [StringIsResourcePath(typeof (GameObject), "EventAssets/")]
    public string ResourceID;
    public string ObjectID;
    private LoadRequest mResourceLoadRequest;
    public bool Persistent;
    public Vector3 Position;
    private GameObject mGO;

    public override void OnActivate()
    {
      if (this.mResourceLoadRequest != null && Object.op_Inequality(this.mResourceLoadRequest.asset, (Object) null))
      {
        Transform transform = (this.mResourceLoadRequest.asset as GameObject).get_transform();
        this.mGO = Object.Instantiate(this.mResourceLoadRequest.asset, Vector3.op_Addition(this.Position, transform.get_position()), transform.get_rotation()) as GameObject;
        if (!string.IsNullOrEmpty(this.ObjectID))
          GameUtility.RequireComponent<GameObjectID>(this.mGO).ID = this.ObjectID;
        if (this.Persistent && Object.op_Inequality((Object) TacticsSceneSettings.Instance, (Object) null))
          this.mGO.get_transform().SetParent(((Component) TacticsSceneSettings.Instance).get_transform(), true);
        this.Sequence.SpawnedObjects.Add(this.mGO);
      }
      this.ActivateNext();
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      if (!Object.op_Inequality((Object) this.mGO, (Object) null) || this.Persistent && !Object.op_Equality((Object) this.mGO.get_transform().get_parent(), (Object) null))
        return;
      Object.Destroy((Object) this.mGO);
    }

    public override bool IsPreloadAssets
    {
      get
      {
        return true;
      }
    }

    [DebuggerHidden]
    public override IEnumerator PreloadAssets()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new EventAction_SpawnObject.\u003CPreloadAssets\u003Ec__Iterator98()
      {
        \u003C\u003Ef__this = this
      };
    }
  }
}
