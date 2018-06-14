﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ChallengeMissionReward
// Assembly: Assembly-CSharp, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BA76916-D0BD-4DB6-A90B-FE0BCC53E511
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using GR;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SRPG
{
  [FlowNode.Pin(1, "完了", FlowNode.PinTypes.Output, 3)]
  [FlowNode.Pin(4, "継続", FlowNode.PinTypes.Input, 1)]
  [FlowNode.Pin(0, "更新", FlowNode.PinTypes.Input, 0)]
  [FlowNode.Pin(3, "コンプリート報酬", FlowNode.PinTypes.Output, 2)]
  [FlowNode.Pin(2, "ミッション報酬", FlowNode.PinTypes.Output, 1)]
  public class ChallengeMissionReward : MonoBehaviour, IFlowInterface
  {
    public GameObject PanelNormal;
    public GameObject PanelComplete;
    public RawImage ImageItem;
    public RawImage ImageExp;
    public RawImage ImageGold;
    public RawImage ImageStamina;
    public RawImage ImageReward;
    public UnityEngine.UI.Text TextMessage;
    private TrophyParam mTrophy;

    public ChallengeMissionReward()
    {
      base.\u002Ector();
    }

    public void Activated(int pinID)
    {
      if (pinID != 4)
        return;
      this.StartCoroutine(this.showRewardMessage());
    }

    private void Awake()
    {
      if (Object.op_Inequality((Object) this.PanelNormal, (Object) null))
        this.PanelNormal.SetActive(false);
      if (!Object.op_Inequality((Object) this.PanelComplete, (Object) null))
        return;
      this.PanelComplete.SetActive(false);
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this.TextMessage, (Object) null))
        ((Behaviour) this).set_enabled(false);
      else if (string.IsNullOrEmpty(GlobalVars.SelectedChallengeMissionTrophy))
      {
        ((Behaviour) this).set_enabled(false);
      }
      else
      {
        this.mTrophy = ChallengeMission.GetTrophy(GlobalVars.SelectedChallengeMissionTrophy);
        if (this.mTrophy == null)
        {
          ((Behaviour) this).set_enabled(false);
        }
        else
        {
          if (this.mTrophy.IsChallengeMissionRoot)
          {
            this.PanelNormal.SetActive(false);
            this.PanelComplete.SetActive(true);
            FlowNode_GameObject.ActivateOutputLinks((Component) this, 3);
          }
          else
          {
            this.PanelNormal.SetActive(true);
            this.PanelComplete.SetActive(false);
            FlowNode_GameObject.ActivateOutputLinks((Component) this, 2);
          }
          this.UpdateReward(this.mTrophy);
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator showRewardMessage()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChallengeMissionReward.\u003CshowRewardMessage\u003Ec__IteratorA3() { \u003C\u003Ef__this = this };
    }

    private void UpdateReward(TrophyParam trophy)
    {
      if (trophy == null)
        return;
      string format1 = LocalizedText.Get("sys.CHALLENGE_MSG_REWARD_ITEM");
      string format2 = LocalizedText.Get("sys.CHALLENGE_MSG_REWARD_OTHER");
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      string str1 = string.Empty;
      ItemParam data = (ItemParam) null;
      if (trophy.Gold != 0)
      {
        flag3 = true;
        string str2 = string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_GOLD"), (object) trophy.Gold);
        str1 = string.Format(format2, (object) str2);
      }
      else if (trophy.Exp != 0)
      {
        flag2 = true;
        string str2 = string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_EXP"), (object) trophy.Exp);
        str1 = string.Format(format2, (object) str2);
      }
      else if (trophy.Coin != 0)
      {
        flag1 = true;
        string str2 = string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_COIN"), (object) trophy.Coin);
        str1 = string.Format(format2, (object) str2);
        data = MonoSingleton<GameManager>.Instance.GetItemParam("$COIN");
      }
      else if (trophy.Stamina != 0)
      {
        flag4 = true;
        string str2 = string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_STAMINA"), (object) trophy.Stamina);
        str1 = string.Format(format2, (object) str2);
      }
      else if (trophy.Items != null && trophy.Items.Length > 0)
      {
        flag1 = true;
        data = MonoSingleton<GameManager>.Instance.GetItemParam(trophy.Items[0].iname);
        if (data != null)
        {
          if (data.type == EItemType.Unit)
          {
            UnitParam unitParam = MonoSingleton<GameManager>.Instance.GetUnitParam(data.iname);
            if (unitParam != null)
            {
              string str2 = LocalizedText.Get("sys.CHALLENGE_DETAIL_REWARD_UNIT_BR", (object) ((int) unitParam.rare + 1), (object) unitParam.name);
              str1 = string.Format(format2, (object) str2);
            }
          }
          else
            str1 = string.Format(format1, (object) data.name, (object) trophy.Items[0].Num);
        }
      }
      if (Object.op_Inequality((Object) this.ImageItem, (Object) null))
        ((Component) this.ImageItem).get_gameObject().SetActive(flag1);
      if (Object.op_Inequality((Object) this.ImageExp, (Object) null))
        ((Component) this.ImageExp).get_gameObject().SetActive(flag2);
      if (Object.op_Inequality((Object) this.ImageGold, (Object) null))
        ((Component) this.ImageGold).get_gameObject().SetActive(flag3);
      if (Object.op_Inequality((Object) this.ImageStamina, (Object) null))
        ((Component) this.ImageStamina).get_gameObject().SetActive(flag4);
      if (data != null)
        DataSource.Bind<ItemParam>(((Component) this).get_gameObject(), data);
      if (Object.op_Inequality((Object) this.TextMessage, (Object) null))
        this.TextMessage.set_text(str1);
      if (this.IsInvoking("WaitLoadTexture"))
        return;
      this.StartCoroutine(this.WaitLoadTexture(trophy));
    }

    [DebuggerHidden]
    private IEnumerator WaitLoadTexture(TrophyParam trophy)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChallengeMissionReward.\u003CWaitLoadTexture\u003Ec__IteratorA4() { trophy = trophy, \u003C\u0024\u003Etrophy = trophy, \u003C\u003Ef__this = this };
    }

    private string GetAllRewardText(TrophyParam trophy)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      if (trophy.Items != null && trophy.Items.Length > 0)
      {
        foreach (TrophyParam.RewardItem rewardItem in this.mTrophy.Items)
        {
          ItemParam itemParam = MonoSingleton<GameManager>.GetInstanceDirect().GetItemParam(rewardItem.iname);
          if (itemParam != null)
          {
            if (itemParam.type == EItemType.UnitPiece)
            {
              stringBuilder.AppendLine(string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_PIECE"), (object) itemParam.name, (object) rewardItem.Num));
              flag = true;
            }
            else if (itemParam.type == EItemType.Unit)
            {
              UnitParam unitParam = MonoSingleton<GameManager>.Instance.GetUnitParam(itemParam.iname);
              if (unitParam != null)
              {
                string str = LocalizedText.Get("sys.CHALLENGE_DETAIL_REWARD_UNIT", (object) ((int) unitParam.rare + 1), (object) unitParam.name);
                stringBuilder.AppendLine(LocalizedText.Get("sys.CHALLENGE_DETAIL_REWARD_GET", new object[1]
                {
                  (object) str
                }));
              }
            }
            else
              stringBuilder.AppendLine(string.Format(LocalizedText.Get("sys.CHALLENGE_DETAIL_REWARD"), (object) itemParam.name, (object) rewardItem.Num));
            if (itemParam.type == EItemType.Ticket)
              AnalyticsManager.TrackCurrencyObtain(AnalyticsManager.CurrencyType.SummonTicket, AnalyticsManager.CurrencySubType.FREE, (long) rewardItem.Num, "Challenges", new Dictionary<string, object>()
              {
                {
                  "item_id",
                  (object) rewardItem.iname
                }
              });
          }
        }
      }
      if (trophy.Gold != 0)
      {
        stringBuilder.AppendLine(string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_GOLD"), (object) trophy.Gold));
        AnalyticsManager.TrackCurrencyObtain(AnalyticsManager.CurrencyType.Zeni, AnalyticsManager.CurrencySubType.FREE, (long) trophy.Gold, "Challenges", (Dictionary<string, object>) null);
      }
      else if (trophy.Exp != 0)
        stringBuilder.AppendLine(string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_EXP"), (object) trophy.Exp));
      else if (trophy.Coin != 0)
      {
        stringBuilder.AppendLine(string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_COIN"), (object) trophy.Coin));
        AnalyticsManager.TrackCurrencyObtain(AnalyticsManager.CurrencyType.Gem, AnalyticsManager.CurrencySubType.FREE, (long) trophy.Coin, "Challenges", (Dictionary<string, object>) null);
      }
      else if (trophy.Stamina != 0)
      {
        stringBuilder.AppendLine(string.Format(LocalizedText.Get("sys.CHALLENGE_REWARD_STAMINA"), (object) trophy.Stamina));
        AnalyticsManager.TrackCurrencyObtain(AnalyticsManager.CurrencyType.AP, AnalyticsManager.CurrencySubType.FREE, (long) trophy.Stamina, "Challenges", (Dictionary<string, object>) null);
      }
      if (flag)
      {
        stringBuilder.AppendLine(string.Empty);
        stringBuilder.AppendLine(LocalizedText.Get("sys.CHALLENGE_REWARD_NOTE"));
      }
      return stringBuilder.ToString();
    }
  }
}
