﻿// Decompiled with JetBrains decompiler
// Type: SRPG.RoomList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 85BFDF7F-5712-4D45-9CD6-3465C703DFDF
// Assembly location: S:\Desktop\Assembly-CSharp.dll

using GR;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SRPG
{
  [FlowNode.Pin(2, "現状表示", FlowNode.PinTypes.Input, 2)]
  [FlowNode.Pin(3, "選択可能ものを表示", FlowNode.PinTypes.Input, 3)]
  [FlowNode.Pin(101, "選択された", FlowNode.PinTypes.Output, 101)]
  [FlowNode.Pin(201, "選択可能な部屋がある", FlowNode.PinTypes.Output, 201)]
  [FlowNode.Pin(200, "選択可能な部屋がない", FlowNode.PinTypes.Output, 200)]
  [FlowNode.Pin(0, "ノーマル表示", FlowNode.PinTypes.Input, 0)]
  [AddComponentMenu("Multi/参加募集中の部屋一覧")]
  [FlowNode.Pin(1, "イベント表示", FlowNode.PinTypes.Input, 1)]
  public class RoomList : MonoBehaviour, IFlowInterface
  {
    private const int PININ_DRAW_NORMAL = 0;
    private const int PININ_DRAW_EVENT = 1;
    private const int PININ_DRAW_CURRENT = 2;
    private const int PININ_DRAW_SELECTABLE = 3;
    private const int PINOUT_SELECTED = 101;
    private const int PINOUT_ROOM_EXISTS = 200;
    private const int PINOUT_ROOM_NOT_EXISTS = 201;
    private readonly Color EnableColor;
    private readonly Color DisableColor;
    [Description("リストアイテムとして使用するゲームオブジェクト")]
    public GameObject ItemTemplate;
    [Description("詳細画面として使用するゲームオブジェクト")]
    public GameObject DetailTemplate;
    private GameObject mDetailInfo;
    public ScrollRect ScrollRect;
    private List<GameObject> mRoomList;

    public RoomList()
    {
      base.\u002Ector();
    }

    public void Activated(int pinID)
    {
      switch (pinID)
      {
        case 0:
          this.Refresh(false, false);
          break;
        case 1:
          this.Refresh(true, false);
          break;
        case 2:
          this.Refresh(GlobalVars.SelectedMultiPlayQuestIsEvent, false);
          break;
        case 3:
          this.Refresh(false, true);
          break;
      }
    }

    private void Awake()
    {
      GlobalVars.SelectedMultiPlayQuestIsEvent = false;
      ((Behaviour) this).set_enabled(true);
      if (Object.op_Inequality((Object) this.ItemTemplate, (Object) null) && this.ItemTemplate.get_activeInHierarchy())
        this.ItemTemplate.SetActive(false);
      if (!Object.op_Inequality((Object) this.DetailTemplate, (Object) null) || !this.DetailTemplate.get_activeInHierarchy())
        return;
      this.DetailTemplate.SetActive(false);
    }

    private void Start()
    {
    }

    public void Refresh(bool isEvent, bool isSelect = false)
    {
      GlobalVars.SelectedMultiPlayQuestIsEvent = isEvent;
      this.RefreshItems(isSelect);
      if (!Object.op_Inequality((Object) this.ScrollRect, (Object) null))
        return;
      ListExtras component = (ListExtras) ((Component) this.ScrollRect).GetComponent<ListExtras>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.SetScrollPos(1f);
      else
        this.ScrollRect.set_normalizedPosition(Vector2.get_one());
    }

    private bool IsChooseRoom(MultiPlayAPIRoom room)
    {
      GameManager instance = MonoSingleton<GameManager>.Instance;
      PlayerData player = instance.Player;
      PartyData party = player.Partys[2];
      QuestParam quest = instance.FindQuest(room.quest.iname);
      bool flag = false;
      if (room.limit == 0)
        return true;
      if (party != null)
      {
        flag = true;
        for (int index = 0; index < (int) quest.unitNum; ++index)
        {
          long unitUniqueId = party.GetUnitUniqueID(index);
          if (unitUniqueId <= 0L)
          {
            flag = false;
            break;
          }
          UnitData unitDataByUniqueId = player.FindUnitDataByUniqueID(unitUniqueId);
          if (unitDataByUniqueId == null)
          {
            flag = false;
            break;
          }
          flag &= unitDataByUniqueId.CalcLevel() >= room.unitlv;
        }
      }
      return flag;
    }

    private void RefreshItems(bool isSelect)
    {
      Transform transform = ((Component) this).get_transform();
      int num = 0;
      GameManager instance = MonoSingleton<GameManager>.Instance;
      for (int index = 0; index < this.mRoomList.Count; ++index)
        Object.DestroyImmediate((Object) this.mRoomList[index]);
      this.mRoomList.Clear();
      if (Object.op_Equality((Object) this.ItemTemplate, (Object) null))
        return;
      MultiPlayAPIRoom[] multiPlayApiRoomArray = FlowNode_MultiPlayAPI.RoomList != null ? FlowNode_MultiPlayAPI.RoomList.rooms : (MultiPlayAPIRoom[]) null;
      if (multiPlayApiRoomArray == null)
        FlowNode_GameObject.ActivateOutputLinks((Component) this, 200);
      else if (multiPlayApiRoomArray.Length == 0)
      {
        FlowNode_GameObject.ActivateOutputLinks((Component) this, 200);
      }
      else
      {
        RoomList.MultiTowerFilterMode multiTowerFilterMode = RoomList.MultiTowerFilterMode.Default;
        string s = FlowNode_Variable.Get("MT_ROOM_FILTER_MODE");
        if (!string.IsNullOrEmpty(s))
          multiTowerFilterMode = (RoomList.MultiTowerFilterMode) int.Parse(s);
        for (int index = 0; index < multiPlayApiRoomArray.Length; ++index)
        {
          MultiPlayAPIRoom multiPlayApiRoom = multiPlayApiRoomArray[index];
          if (multiPlayApiRoom != null && multiPlayApiRoom.quest != null && !string.IsNullOrEmpty(multiPlayApiRoom.quest.iname))
          {
            QuestParam quest = MonoSingleton<GameManager>.Instance.FindQuest(multiPlayApiRoom.quest.iname);
            if (quest != null && quest.IsMultiEvent == GlobalVars.SelectedMultiPlayQuestIsEvent)
            {
              switch (multiTowerFilterMode)
              {
                case RoomList.MultiTowerFilterMode.Cleared:
                  if (multiPlayApiRoom.is_clear != 0)
                    break;
                  continue;
                case RoomList.MultiTowerFilterMode.NotClear:
                  if (multiPlayApiRoom.is_clear == 0)
                    break;
                  continue;
              }
              bool isChoose = this.IsChooseRoom(multiPlayApiRoom);
              if ((isChoose || !isSelect) && (multiPlayApiRoom.clear == 0 || instance.FindQuest(multiPlayApiRoom.quest.iname).state == QuestStates.Cleared))
              {
                GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.ItemTemplate);
                ((Object) gameObject).set_hideFlags((HideFlags) 52);
                Json_Unit[] jsonUnitArray = multiPlayApiRoom.owner != null ? multiPlayApiRoom.owner.units : (Json_Unit[]) null;
                if (jsonUnitArray != null && jsonUnitArray.Length > 0)
                {
                  UnitData data = new UnitData();
                  data.Deserialize(jsonUnitArray[0]);
                  DataSource.Bind<UnitData>(gameObject, data);
                }
                DataSource.Bind<MultiPlayAPIRoom>(gameObject, multiPlayApiRoom);
                DataSource.Bind<QuestParam>(gameObject, quest);
                DebugUtility.Log("found roomid:" + (object) multiPlayApiRoom.roomid + " room:" + multiPlayApiRoom.comment + " iname:" + quest.iname + " playerNum:" + (object) quest.playerNum + " unitNum:" + (object) quest.unitNum);
                ListItemEvents component = (ListItemEvents) gameObject.GetComponent<ListItemEvents>();
                if (Object.op_Inequality((Object) component, (Object) null))
                {
                  component.OnSelect = new ListItemEvents.ListItemEvent(this.OnSelectItem);
                  component.OnOpenDetail = new ListItemEvents.ListItemEvent(this.OnOpenItemDetail);
                  component.OnCloseDetail = new ListItemEvents.ListItemEvent(this.OnCloseItemDetail);
                }
                gameObject.get_transform().SetParent(transform, false);
                gameObject.get_gameObject().SetActive(true);
                this.mRoomList.Add(gameObject);
                this.SetSelectablParam(gameObject, multiPlayApiRoom, isChoose);
                ++num;
              }
            }
          }
        }
        GameParameter.UpdateAll(((Component) transform).get_gameObject());
        FlowNode_GameObject.ActivateOutputLinks((Component) this, num != 0 ? 201 : 200);
      }
    }

    private void SetSelectablParam(GameObject obj, MultiPlayAPIRoom room, bool isChoose)
    {
      SRPG_Button component1 = (SRPG_Button) obj.GetComponent<SRPG_Button>();
      Transform child1 = obj.get_transform().FindChild("fil");
      Transform child2 = obj.get_transform().FindChild("basewindow");
      if (isChoose)
      {
        if (Object.op_Inequality((Object) component1, (Object) null))
          ((Selectable) component1).set_interactable(true);
        if (Object.op_Inequality((Object) child1, (Object) null))
          ((Component) child1).get_gameObject().SetActive(false);
        if (!Object.op_Inequality((Object) child2, (Object) null))
          return;
        Image component2 = (Image) ((Component) child2).GetComponent<Image>();
        if (!Object.op_Inequality((Object) component2, (Object) null))
          return;
        ((Graphic) component2).set_color(this.EnableColor);
      }
      else
      {
        if (Object.op_Inequality((Object) component1, (Object) null))
          ((Selectable) component1).set_interactable(false);
        if (Object.op_Inequality((Object) child1, (Object) null))
          ((Component) child1).get_gameObject().SetActive(true);
        if (!Object.op_Inequality((Object) child2, (Object) null))
          return;
        Image component2 = (Image) ((Component) child2).GetComponent<Image>();
        if (!Object.op_Inequality((Object) component2, (Object) null))
          return;
        ((Graphic) component2).set_color(this.DisableColor);
      }
    }

    private void OnSelectItem(GameObject go)
    {
      MultiPlayAPIRoom dataOfClass = DataSource.FindDataOfClass<MultiPlayAPIRoom>(go, (MultiPlayAPIRoom) null);
      if (dataOfClass == null)
        return;
      GlobalVars.SelectedMultiPlayRoomID = dataOfClass.roomid;
      GlobalVars.SelectedMultiPlayRoomPassCodeHash = dataOfClass.pwd_hash;
      GlobalVars.SelectedMultiTowerFloor = dataOfClass.floor;
      DebugUtility.Log("Select RoomID:" + (object) GlobalVars.SelectedMultiPlayRoomID + " PassCodeHash:" + GlobalVars.SelectedMultiPlayRoomPassCodeHash);
      FlowNode_GameObject.ActivateOutputLinks((Component) this, 101);
      Transform transform1 = go.get_transform().Find("cursor");
      if (!Object.op_Inequality((Object) transform1, (Object) null))
        return;
      for (int index = 0; index < this.mRoomList.Count; ++index)
      {
        Transform transform2 = this.mRoomList[index].get_transform().Find("cursor");
        if (Object.op_Inequality((Object) transform2, (Object) null))
          ((Component) transform2).get_gameObject().SetActive(false);
      }
      ((Component) transform1).get_gameObject().SetActive(true);
    }

    private void OnCloseItemDetail(GameObject go)
    {
      if (!Object.op_Inequality((Object) this.mDetailInfo, (Object) null))
        return;
      Object.DestroyImmediate((Object) this.mDetailInfo.get_gameObject());
      this.mDetailInfo = (GameObject) null;
    }

    private void OnOpenItemDetail(GameObject go)
    {
      QuestParam dataOfClass = DataSource.FindDataOfClass<QuestParam>(go, (QuestParam) null);
      if (!Object.op_Equality((Object) this.mDetailInfo, (Object) null) || dataOfClass == null)
        return;
      this.mDetailInfo = (GameObject) Object.Instantiate<GameObject>((M0) this.DetailTemplate);
      DataSource.Bind<QuestParam>(this.mDetailInfo, dataOfClass);
      this.mDetailInfo.SetActive(true);
    }

    private void Update()
    {
    }

    private enum MultiTowerFilterMode
    {
      Default,
      Cleared,
      NotClear,
    }
  }
}
