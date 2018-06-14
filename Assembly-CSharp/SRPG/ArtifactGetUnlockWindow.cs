﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ArtifactGetUnlockWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using GR;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SRPG
{
  [FlowNode.Pin(1, "Refresh", FlowNode.PinTypes.Input, 1)]
  [FlowNode.Pin(100, "Unlock", FlowNode.PinTypes.Output, 100)]
  [FlowNode.Pin(101, "Selected Quest", FlowNode.PinTypes.Output, 101)]
  public class ArtifactGetUnlockWindow : MonoBehaviour, IFlowInterface
  {
    private ArtifactData UnlockArtifact;
    public StatusList ArtifactStatus;
    public GameObject AbilityListItem;
    public float ability_unlock_alpha;
    public float ability_hidden_alpha;
    public GameObject lock_object;

    public ArtifactGetUnlockWindow()
    {
      base.\u002Ector();
    }

    private void Start()
    {
    }

    public void Activated(int pinID)
    {
      if (pinID != 1)
        return;
      this.Refresh();
    }

    private void Refresh()
    {
      ArtifactData artifactData = new ArtifactData();
      artifactData.Deserialize(new Json_Artifact()
      {
        iname = GlobalVars.ArtifactListItem.param.iname,
        rare = GlobalVars.ArtifactListItem.param.rareini
      });
      this.UnlockArtifact = artifactData;
      DataSource.Bind<ArtifactData>(((Component) this).get_gameObject(), this.UnlockArtifact);
      DataSource.Bind<ArtifactParam>(((Component) this).get_gameObject(), this.UnlockArtifact.ArtifactParam);
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.AbilityListItem, (UnityEngine.Object) null))
      {
        MasterParam masterParam = MonoSingleton<GameManager>.Instance.MasterParam;
        GameObject abilityListItem = this.AbilityListItem;
        CanvasGroup component = (CanvasGroup) abilityListItem.GetComponent<CanvasGroup>();
        bool flag = false;
        ArtifactParam artifactParam = artifactData.ArtifactParam;
        List<AbilityData> learningAbilities = artifactData.LearningAbilities;
        if (artifactParam.abil_inames != null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ArtifactGetUnlockWindow.\u003CRefresh\u003Ec__AnonStorey2F6 refreshCAnonStorey2F6 = new ArtifactGetUnlockWindow.\u003CRefresh\u003Ec__AnonStorey2F6();
          AbilityParam data1 = (AbilityParam) null;
          // ISSUE: reference to a compiler-generated field
          refreshCAnonStorey2F6.abil_iname = (string) null;
          for (int index = 0; index < artifactParam.abil_inames.Length; ++index)
          {
            if (!string.IsNullOrEmpty(artifactParam.abil_inames[index]) && artifactParam.abil_shows[index] != 0)
            {
              // ISSUE: reference to a compiler-generated field
              refreshCAnonStorey2F6.abil_iname = artifactParam.abil_inames[index];
              data1 = masterParam.GetAbilityParam(artifactParam.abil_inames[index]);
              if (data1 != null)
                break;
            }
          }
          if (data1 == null)
          {
            component.set_alpha(this.ability_hidden_alpha);
            DataSource.Bind<AbilityParam>(this.AbilityListItem, (AbilityParam) null);
            DataSource.Bind<AbilityData>(this.AbilityListItem, (AbilityData) null);
            return;
          }
          DataSource.Bind<AbilityParam>(this.AbilityListItem, data1);
          DataSource.Bind<AbilityData>(abilityListItem, (AbilityData) null);
          if (UnityEngine.Object.op_Inequality((UnityEngine.Object) component, (UnityEngine.Object) null) && learningAbilities != null && learningAbilities != null)
          {
            // ISSUE: reference to a compiler-generated method
            AbilityData data2 = learningAbilities.Find(new Predicate<AbilityData>(refreshCAnonStorey2F6.\u003C\u003Em__2F7));
            if (data2 != null)
            {
              DataSource.Bind<AbilityData>(abilityListItem, data2);
              flag = true;
            }
          }
        }
        if (flag)
          component.set_alpha(this.ability_unlock_alpha);
        else
          component.set_alpha(this.ability_hidden_alpha);
      }
      BaseStatus fixed_status = new BaseStatus();
      BaseStatus scale_status = new BaseStatus();
      this.UnlockArtifact.GetHomePassiveBuffStatus(ref fixed_status, ref scale_status, (UnitData) null, 0, true);
      this.ArtifactStatus.SetValues(fixed_status, scale_status);
      GameParameter.UpdateAll(((Component) this).get_gameObject());
    }

    public void SetArtifactData()
    {
      GlobalVars.ConditionJobs = GlobalVars.ArtifactListItem.param.condition_jobs;
    }
  }
}
