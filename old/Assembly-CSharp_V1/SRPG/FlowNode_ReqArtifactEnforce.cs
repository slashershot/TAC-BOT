﻿// Decompiled with JetBrains decompiler
// Type: SRPG.FlowNode_ReqArtifactEnforce
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using GR;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SRPG
{
  [FlowNode.NodeType("System/ReqArtifactEnforce", 32741)]
  [FlowNode.Pin(1, "Success", FlowNode.PinTypes.Output, 1)]
  [FlowNode.Pin(0, "Request", FlowNode.PinTypes.Input, 0)]
  public class FlowNode_ReqArtifactEnforce : FlowNode_Network
  {
    public override void OnActivate(int pinID)
    {
      if (pinID != 0)
        return;
      long artifactUniqueId = (long) GlobalVars.SelectedArtifactUniqueID;
      Dictionary<string, int> artifactExpItems = GlobalVars.UsedArtifactExpItems;
      if (Network.Mode == Network.EConnectMode.Online)
      {
        if (artifactUniqueId < 1L || artifactExpItems.Count < 1)
        {
          ((Behaviour) this).set_enabled(false);
          this.Success();
        }
        else
        {
          ((Behaviour) this).set_enabled(true);
          this.ExecRequest((WebAPI) new ReqArtifactEnforce(artifactUniqueId, artifactExpItems, new Network.ResponseCallback(((FlowNode_Network) this).ResponseCallback)));
        }
      }
      else
        this.Success();
    }

    private void Success()
    {
      ((Behaviour) this).set_enabled(false);
      this.ActivateOutputLinks(1);
    }

    public override void OnSuccess(WWWResult www)
    {
      if (Network.IsError)
      {
        if (Network.ErrCode == Network.EErrCode.ArtifactMatShort)
          this.OnBack();
        else
          this.OnRetry();
      }
      else
      {
        WebAPI.JSON_BodyResponse<Json_PlayerDataAll> jsonObject = JSONParser.parseJSONObject<WebAPI.JSON_BodyResponse<Json_PlayerDataAll>>(www.text);
        DebugUtility.Assert(jsonObject != null, "res == null");
        if (jsonObject.body == null)
        {
          this.OnRetry();
        }
        else
        {
          try
          {
            MonoSingleton<GameManager>.Instance.Deserialize(jsonObject.body.player);
            MonoSingleton<GameManager>.Instance.Deserialize(jsonObject.body.items);
            MonoSingleton<GameManager>.Instance.Deserialize(jsonObject.body.artifacts, false);
          }
          catch (Exception ex)
          {
            DebugUtility.LogException(ex);
            this.OnRetry();
            return;
          }
          Network.RemoveAPI();
          this.Success();
        }
      }
    }
  }
}
