﻿// Decompiled with JetBrains decompiler
// Type: SRPG.ChatWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using GR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRPG
{
  [FlowNode.Pin(13, "Multi Screen Enter", FlowNode.PinTypes.Input, 13)]
  [FlowNode.Pin(101, "スタンプ送信", FlowNode.PinTypes.Output, 101)]
  [FlowNode.Pin(100, "スタンプ送信セット", FlowNode.PinTypes.Input, 100)]
  [FlowNode.Pin(70, "チャットログ更新失敗", FlowNode.PinTypes.Input, 70)]
  [FlowNode.Pin(50, "チャットログリスト更新", FlowNode.PinTypes.Input, 50)]
  [FlowNode.Pin(46, "ルームチャットログ取得Request", FlowNode.PinTypes.Output, 46)]
  [FlowNode.Pin(45, "前回の発言から10秒以内にメッセージ送信時", FlowNode.PinTypes.Input, 45)]
  [FlowNode.Pin(43, "ワールドチャットログ取得", FlowNode.PinTypes.Input, 43)]
  [FlowNode.Pin(42, "ワールドチャットログリセット", FlowNode.PinTypes.Input, 42)]
  [FlowNode.Pin(40, "ワールドチャットログ取得Request", FlowNode.PinTypes.Output, 40)]
  [FlowNode.Pin(30, "ユーザーのアイコンをタップ", FlowNode.PinTypes.Output, 30)]
  [FlowNode.Pin(22, "ウィンドウクローズ終了", FlowNode.PinTypes.Output, 22)]
  [FlowNode.Pin(21, "ウィンドウオープン終了", FlowNode.PinTypes.Output, 21)]
  [FlowNode.Pin(23, "チャットウィンドウを閉じる", FlowNode.PinTypes.Input, 23)]
  [FlowNode.Pin(20, "チャットウィンドウを開く", FlowNode.PinTypes.Input, 20)]
  [FlowNode.Pin(12, "Home Screen Enter", FlowNode.PinTypes.Input, 12)]
  [FlowNode.Pin(11, "Chat Leave", FlowNode.PinTypes.Input, 11)]
  [FlowNode.Pin(10, "Chat Enter", FlowNode.PinTypes.Input, 10)]
  public class ChatWindow : MonoSingleton<ChatWindow>, IFlowInterface
  {
    private static readonly float SPAN_UPDATE_WORLD_MESSAGE_UIOPEN = 10f;
    private static readonly float SPAN_UPDATE_WORLD_MESSAGE_UICLOSE = 20f;
    private static readonly float SPAN_UPDATE_ROOM_MESSAGE_UIOPEN = 7f;
    private static readonly float SPAN_UPDATE_ROOM_MESSAGE_UICLOSE = 20f;
    private static readonly float SPAN_UPDATE_ROOM_MEMBER = 1f;
    private static readonly float LOG_PUSH_X_HOME = 138f;
    private static readonly float LOG_PUSH_X_MULTI = 265f;
    private static readonly float CLOSE_BUTTON_X_DIST = 300f;
    public static readonly byte MAX_CHAT_LOG_ITEM = 30;
    private string MaintenanceMsg = string.Empty;
    public int CharacterLimit = 140;
    private int mLastChannelID = -1;
    private List<ChatLogItem> mItems = new List<ChatLogItem>();
    private List<ChatUtility.ChatInspectionMaster> mChatInspectionMaster = new List<ChatUtility.ChatInspectionMaster>();
    private const int PINID_IN_CHAT_ENTER = 10;
    private const int PINID_IN_CHAT_LEAVE = 11;
    private const int PINID_IN_HOME_ENTER = 12;
    private const int PINID_IN_MULTI_ENTER = 13;
    private const int PINID_IN_CAHTWINDOW_OPEN = 20;
    private const int PINID_OU_OPEN_OUTPUT = 21;
    private const int PINID_OU_CLOSE_OUTPUT = 22;
    private const int PINID_IN_CAHTWINDOW_CLOSE = 23;
    private const int PINID_OU_UNITICON_TAP = 30;
    private const int PINID_OU_REQUEST_CHATLOG_WORLD = 40;
    private const int PINID_IN_UPDATE_CHATLOG_RESET = 42;
    private const int PINID_IN_UPDATE_CHATLOG = 43;
    private const int PINID_IN_SEND_CHAT_INTERVAL = 45;
    private const int PINID_OU_REQUEST_CHATLOG_ROOM = 46;
    private const int PINID_IN_REFRESH_CHATLOGLIST = 50;
    private const int PINID_IN_UPDATE_CHATLOG_FAILURE = 70;
    private const int PINID_IN_SEND_STAMP = 100;
    private const int PINID_OU_REQUEST_SEND_STAMP = 101;
    private float elapsed_time_for_photon_room_member;
    private StateMachine<ChatWindow> mState;
    private bool mOpened;
    private bool Maintenance;
    [SerializeField]
    private GameObject MessageRoot;
    [SerializeField]
    private GameObject MessageTemplate;
    [SerializeField]
    private Scrollbar ChatScrollBar;
    [SerializeField]
    private InputFieldCensorship InputFieldMessage;
    [SerializeField]
    private SRPG_Button SendMessageButton;
    [SerializeField]
    private GameObject ChannelPanel;
    private Dictionary<ChatWindow.eChatType, SRPG_ToggleButton> mTabButtons;
    [SerializeField]
    private SRPG_ToggleButton Tab_World;
    [SerializeField]
    private SRPG_ToggleButton Tab_Room;
    [SerializeField]
    private GameObject MaintenancePanel;
    [SerializeField]
    private GameObject MaintenanceText;
    [SerializeField]
    private GameObject NoUsedChatText;
    [SerializeField]
    private GameObject ClosedShowMessage;
    [SerializeField]
    private GameObject UpdateMessageBadge;
    [SerializeField]
    private GameObject WordlChatBadge;
    [SerializeField]
    private GameObject RoomChatBadge;
    [SerializeField]
    private ScrollRect ScrollView;
    [SerializeField]
    private UnityEngine.UI.Text Caution;
    [SerializeField]
    private Animator CautionAnimator;
    [SerializeField]
    private UnityEngine.UI.Text MaintenanceMsgText;
    [SerializeField]
    private Button UsefulButton;
    [SerializeField]
    private GameObject UsefulRootObject;
    [SerializeField]
    private UnityEngine.UI.Text InputPlaceholderText;
    [SerializeField]
    private GameObject CloseButton;
    private GameManager gm;
    private float mRestTime_Opend_UpdateWorldChat;
    private float mRestTime_Opend_UpdateRoomChat;
    private float mRestTime_Closed_UpdateWorldChat;
    private float mRestTime_Closed_UpdateRoomChat;
    private static long system_message_local_id;
    private UnityEngine.UI.Text mMaintenance;
    private UnityEngine.UI.Text mNoUsedChat;
    private bool is_need_reset_world;
    private bool is_need_reset_room;
    private ChatLog mWorldChatLog;
    private ChatLog mRoomChatLog;
    private ChatLog mOfficalChatLog;
    private ChatWindow.eChatType mCurrentChatType;
    private string mInputPlaceholderDefaultText_World;
    private string mInputPlaceholderDefaultText_Room;
    private static ChatLog CacheRoomChatLog;
    private static ChatUtility.RoomInfo room_info;
    public static ChatUtility.RoomMemberManager room_member_manager;
    private bool mRequesting;
    private bool mInitialized;
    [SerializeField]
    private GameObject RootWindow;
    private bool mChatPermit;
    private FlowNode_SendChatMessage mFNode_Sendmessage;

    public bool IsOpened
    {
      get
      {
        return this.mOpened;
      }
    }

    public bool IsRequesting
    {
      get
      {
        return this.mRequesting;
      }
    }

    public bool IsInitialized
    {
      get
      {
        return this.mInitialized;
      }
      set
      {
        this.mInitialized = value;
      }
    }

    public bool IsPermit
    {
      get
      {
        return this.mChatPermit;
      }
    }

    private FlowNode_SendChatMessage FlowNodeSendChatMessage
    {
      get
      {
        if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.mFNode_Sendmessage, (UnityEngine.Object) null))
          this.mFNode_Sendmessage = (FlowNode_SendChatMessage) ((Component) this).get_gameObject().GetComponent<FlowNode_SendChatMessage>();
        return this.mFNode_Sendmessage;
      }
    }

    private ChatLog GetChatLogInstance(ChatWindow.eChatType _chat_type)
    {
      switch (_chat_type)
      {
        case ChatWindow.eChatType.World:
          if (this.mWorldChatLog == null)
            this.mWorldChatLog = new ChatLog();
          return this.mWorldChatLog;
        case ChatWindow.eChatType.Room:
          if (this.mRoomChatLog == null)
            this.mRoomChatLog = new ChatLog();
          return this.mRoomChatLog;
        default:
          return (ChatLog) null;
      }
    }

    public void SetChatLog(ChatLog _chat_log, ChatWindow.eChatType _chat_type)
    {
      this.GetChatLogInstance(_chat_type).AddMessage(_chat_log.messages);
      this.SaveRoomChatLogCache();
    }

    public void SetChatLogAndSystemMessageMerge(ChatLog _server_chat_log, long _exclude_id)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ChatWindow.\u003CSetChatLogAndSystemMessageMerge\u003Ec__AnonStorey31A mergeCAnonStorey31A = new ChatWindow.\u003CSetChatLogAndSystemMessageMerge\u003Ec__AnonStorey31A();
      // ISSUE: reference to a compiler-generated field
      mergeCAnonStorey31A._exclude_id = _exclude_id;
      // ISSUE: reference to a compiler-generated field
      mergeCAnonStorey31A._server_chat_log = _server_chat_log;
      ChatLog chatLog = new ChatLog();
      List<ChatLogParam> chatLogParamList = new List<ChatLogParam>();
      // ISSUE: reference to a compiler-generated method
      ChatLogParam chatLogParam = ChatWindow.CacheRoomChatLog.messages.Find(new Predicate<ChatLogParam>(mergeCAnonStorey31A.\u003C\u003Em__334));
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ChatWindow.\u003CSetChatLogAndSystemMessageMerge\u003Ec__AnonStorey31B mergeCAnonStorey31B = new ChatWindow.\u003CSetChatLogAndSystemMessageMerge\u003Ec__AnonStorey31B();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (mergeCAnonStorey31B.i = 0; mergeCAnonStorey31B.i < ChatWindow.CacheRoomChatLog.messages.Count; ++mergeCAnonStorey31B.i)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((int) ChatWindow.CacheRoomChatLog.messages[mergeCAnonStorey31B.i].message_type == 3 || ChatWindow.CacheRoomChatLog.messages[mergeCAnonStorey31B.i].id == chatLogParam.id || mergeCAnonStorey31A._server_chat_log.messages.Find(new Predicate<ChatLogParam>(mergeCAnonStorey31B.\u003C\u003Em__335)) != null)
        {
          // ISSUE: reference to a compiler-generated field
          chatLog.AddMessage(ChatWindow.CacheRoomChatLog.messages[mergeCAnonStorey31B.i]);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          chatLogParamList.Add(ChatWindow.CacheRoomChatLog.messages[mergeCAnonStorey31B.i]);
        }
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ChatWindow.\u003CSetChatLogAndSystemMessageMerge\u003Ec__AnonStorey31C mergeCAnonStorey31C = new ChatWindow.\u003CSetChatLogAndSystemMessageMerge\u003Ec__AnonStorey31C();
      // ISSUE: reference to a compiler-generated field
      mergeCAnonStorey31C.\u003C\u003Ef__ref\u0024794 = mergeCAnonStorey31A;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (mergeCAnonStorey31C.i = 0; mergeCAnonStorey31C.i < mergeCAnonStorey31A._server_chat_log.messages.Count; ++mergeCAnonStorey31C.i)
      {
        // ISSUE: reference to a compiler-generated method
        if (chatLog.messages.Find(new Predicate<ChatLogParam>(mergeCAnonStorey31C.\u003C\u003Em__336)) == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          chatLog.AddMessage(mergeCAnonStorey31A._server_chat_log.messages[mergeCAnonStorey31C.i]);
        }
      }
      int _index = chatLog.messages.IndexOf(chatLogParam);
      if (_index >= 0)
      {
        for (int index = 0; index < chatLogParamList.Count; ++index)
        {
          if (!(chatLogParamList[index].fuid != chatLogParam.fuid))
          {
            chatLog.RemoveByIndex(_index);
            break;
          }
        }
      }
      this.mRoomChatLog = chatLog;
      this.SaveRoomChatLogCache();
    }

    private void UpdateRootWindowState(bool state)
    {
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.RootWindow, (UnityEngine.Object) null))
        this.RootWindow.SetActive(state);
      if (!UnityEngine.Object.op_Inequality((UnityEngine.Object) this.MessageRoot, (UnityEngine.Object) null))
        return;
      this.MessageRoot.SetActive(state);
    }

    public void Activated(int pinID)
    {
      int num = pinID;
      switch (num)
      {
        case 10:
          this.UpdateRootWindowState(true);
          this.mChatPermit = true;
          FlowNode_Variable.Set("IS_EXTERNAL_API_PERMIT", string.Empty);
          break;
        case 11:
          if (string.IsNullOrEmpty(FlowNode_Variable.Get("CHAT_SCENE_STATE")))
          {
            this.ExitRoomSelf();
            this.UpdateRootWindowState(false);
            this.mChatPermit = false;
          }
          FlowNode_Variable.Set("CHAT_SCENE_STATE", string.Empty);
          break;
        case 12:
          this.ClosedShowMessage.get_transform().set_localPosition(new Vector3(ChatWindow.LOG_PUSH_X_HOME, (float) this.ClosedShowMessage.get_transform().get_localPosition().y, 0.0f));
          this.CloseButton.get_transform().set_localPosition(new Vector3(ChatWindow.LOG_PUSH_X_HOME + ChatWindow.CLOSE_BUTTON_X_DIST, (float) this.CloseButton.get_transform().get_localPosition().y, 0.0f));
          break;
        case 13:
          this.ClosedShowMessage.get_transform().set_localPosition(new Vector3(ChatWindow.LOG_PUSH_X_MULTI, (float) this.ClosedShowMessage.get_transform().get_localPosition().y, 0.0f));
          this.CloseButton.get_transform().set_localPosition(new Vector3(ChatWindow.LOG_PUSH_X_MULTI + ChatWindow.CLOSE_BUTTON_X_DIST, (float) this.CloseButton.get_transform().get_localPosition().y, 0.0f));
          break;
        case 20:
          this.ResetCloseShowMessage();
          this.UpdateWindowState(pinID);
          this.UpdateMessageBadgeState();
          break;
        case 23:
          this.UpdateWindowState(pinID);
          this.UpdateMessageBadgeState();
          this.RefreshChatLogView(false);
          break;
        default:
          switch (num - 42)
          {
            case 0:
              this.ResetChatLog(this.mCurrentChatType);
              return;
            case 1:
              this.InputFieldMessage.set_text(string.Empty);
              this.RequestChatLog(this.mCurrentChatType, false);
              return;
            case 3:
              this.RefreshCaution();
              return;
            default:
              if (num != 50)
              {
                if (num != 70)
                {
                  if (num != 100)
                    return;
                  this.SetSendStamp();
                  this.SetActiveUsefulWindowObject(false);
                  return;
                }
                this.RefreshChatLogView(false);
                this.Maintenance = true;
                this.MaintenanceMsg = Network.ErrMsg;
                Network.ResetError();
                return;
              }
              this.UpdateMessageBadgeState();
              this.RefreshChatLogView(false);
              this.Maintenance = false;
              return;
          }
      }
    }

    private void ResetChatLog(ChatWindow.eChatType _chat_type)
    {
      if (_chat_type == ChatWindow.eChatType.Room)
      {
        this.ClearAllItems();
        this.CheckChannelUpdate();
        this.UpdateChannelPanel();
        this.SaveRoomChatLogCache();
        ChatLog chatLogInstance = this.GetChatLogInstance(ChatWindow.eChatType.Room);
        long topMessageIdServer = chatLogInstance.TopMessageIdServer;
        chatLogInstance.Reset();
        this.RequestRoomChatLog(topMessageIdServer, true);
        this.is_need_reset_world = true;
      }
      else
      {
        this.ClearAllItems();
        this.CheckChannelUpdate();
        this.UpdateChannelPanel();
        this.GetChatLogInstance(ChatWindow.eChatType.World).Reset();
        this.RequestChatLog(ChatWindow.eChatType.World, false);
        this.is_need_reset_room = true;
      }
    }

    private void RefreshCaution()
    {
      string str = FlowNode_Variable.Get("MESSAGE_CAUTION_SEND_MESSAGE");
      if (string.IsNullOrEmpty(str))
      {
        if (!UnityEngine.Object.op_Inequality((UnityEngine.Object) this.CautionAnimator, (UnityEngine.Object) null))
          return;
        this.CautionAnimator.ResetTrigger("onShowCaution");
      }
      else
      {
        this.Caution.set_text(str);
        if (!UnityEngine.Object.op_Inequality((UnityEngine.Object) this.CautionAnimator, (UnityEngine.Object) null))
          return;
        this.CautionAnimator.SetTrigger("onShowCaution");
      }
    }

    private void UpdateWindowState(int inputPinID)
    {
      int pinID = 21;
      if (inputPinID == 23)
      {
        this.UsefulRootObject.SetActive(false);
        pinID = 22;
        this.mOpened = false;
      }
      else
        this.mOpened = true;
      FlowNode_GameObject.ActivateOutputLinks((Component) this, pinID);
    }

    private void Awake()
    {
      ChatWindow.system_message_local_id = 0L;
      if (ChatWindow.room_info == null)
        ChatWindow.room_info = new ChatUtility.RoomInfo();
      if (ChatWindow.room_member_manager == null)
        ChatWindow.room_member_manager = new ChatUtility.RoomMemberManager();
      if (ChatWindow.CacheRoomChatLog != null)
        this.CopyChatLog(ChatWindow.CacheRoomChatLog, ref this.mRoomChatLog);
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.MessageTemplate, (UnityEngine.Object) null))
        this.MessageTemplate.SetActive(false);
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.InputFieldMessage, (UnityEngine.Object) null))
      {
        // ISSUE: method pointer
        ((UnityEvent<string>) this.InputFieldMessage.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__337)));
      }
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.MaintenancePanel, (UnityEngine.Object) null))
        this.MaintenancePanel.SetActive(false);
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.UpdateMessageBadge, (UnityEngine.Object) null))
        this.UpdateMessageBadge.SetActive(false);
      this.mItems = new List<ChatLogItem>((int) ChatWindow.MAX_CHAT_LOG_ITEM);
      for (int index = 0; index < (int) ChatWindow.MAX_CHAT_LOG_ITEM; ++index)
      {
        GameObject gameObject = (GameObject) UnityEngine.Object.Instantiate<GameObject>((M0) this.MessageTemplate);
        gameObject.get_transform().SetParent(this.MessageRoot.get_transform(), false);
        this.mItems.Add((ChatLogItem) gameObject.GetComponent<ChatLogItem>());
      }
      this.mTabButtons = new Dictionary<ChatWindow.eChatType, SRPG_ToggleButton>()
      {
        {
          ChatWindow.eChatType.World,
          this.Tab_World
        },
        {
          ChatWindow.eChatType.Room,
          this.Tab_Room
        }
      };
      using (Dictionary<ChatWindow.eChatType, SRPG_ToggleButton>.KeyCollection.Enumerator enumerator = this.mTabButtons.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ChatWindow.eChatType current = enumerator.Current;
          if (!UnityEngine.Object.op_Equality((UnityEngine.Object) this.mTabButtons[current], (UnityEngine.Object) null))
            this.mTabButtons[current].AddListener(new SRPG_Button.ButtonClickEvent(this.OnTabChange));
        }
      }
      this.ChangeChatTypeTab(!ChatWindow.room_info.IsActive ? ChatWindow.eChatType.World : ChatWindow.eChatType.Room);
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.UsefulButton, (UnityEngine.Object) null))
      {
        // ISSUE: method pointer
        ((UnityEvent) this.UsefulButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnUsefulButton)));
      }
      ChatWindow.room_info.Init(this);
    }

    private void OnTabChange(SRPG_Button button)
    {
      if (!this.TabChange(button))
        ;
    }

    private bool TabChange(SRPG_Button button)
    {
      if (!((Selectable) button).IsInteractable())
        return false;
      using (Dictionary<ChatWindow.eChatType, SRPG_ToggleButton>.KeyCollection.Enumerator enumerator = this.mTabButtons.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ChatWindow.eChatType current = enumerator.Current;
          if (((UnityEngine.Object) this.mTabButtons[current]).get_name() == ((UnityEngine.Object) button).get_name())
          {
            this.ChangeChatTypeTab(current);
            break;
          }
        }
      }
      return true;
    }

    public void ChangeChatTypeTab(ChatWindow.eChatType _chat_type)
    {
      if (this.mCurrentChatType == _chat_type)
        return;
      using (Dictionary<ChatWindow.eChatType, SRPG_ToggleButton>.KeyCollection.Enumerator enumerator = this.mTabButtons.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.mTabButtons[enumerator.Current].IsOn = false;
      }
      this.mTabButtons[_chat_type].IsOn = true;
      this.mCurrentChatType = _chat_type;
      this.SetMessageDataToFlowNode(this.InputFieldMessage.get_text(), false);
      this.SetActiveUsefulWindowObject(false);
      if (this.is_need_reset_world)
      {
        this.is_need_reset_world = false;
        this.ClearAllItems();
        this.GetChatLogInstance(ChatWindow.eChatType.World).Reset();
        this.RequestChatLog(ChatWindow.eChatType.World, false);
      }
      else if (this.is_need_reset_room)
      {
        this.is_need_reset_room = false;
        this.ClearAllItems();
        ChatLog chatLogInstance = this.GetChatLogInstance(ChatWindow.eChatType.Room);
        long topMessageIdServer = chatLogInstance.TopMessageIdServer;
        chatLogInstance.Reset();
        this.RequestRoomChatLog(topMessageIdServer, true);
      }
      else
      {
        this.UpdateMessageBadgeState();
        this.RefreshChatLogView(true);
      }
    }

    private void OnUsefulButton()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.UsefulRootObject, (UnityEngine.Object) null))
        return;
      this.SetActiveUsefulWindowObject(!this.UsefulRootObject.get_activeSelf());
    }

    public void SetActiveUsefulWindowObject(bool _active)
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.UsefulRootObject, (UnityEngine.Object) null))
        return;
      this.UsefulRootObject.SetActive(_active);
    }

    public void LoadTemplateMessage()
    {
      ChatTemplateMessage componentInChildren = (ChatTemplateMessage) ((Component) this).GetComponentInChildren<ChatTemplateMessage>(true);
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) componentInChildren, (UnityEngine.Object) null))
        return;
      componentInChildren.LoadTemplateMessage();
    }

    private void Start()
    {
      this.gm = MonoSingleton<GameManager>.Instance;
      if (this.mChatInspectionMaster == null || this.mChatInspectionMaster.Count <= 0)
      {
        bool is_success = false;
        this.mChatInspectionMaster = ChatUtility.LoadInspectionMaster(ref is_success);
        if (!is_success)
        {
          DebugUtility.LogError("ChatWindow Error:Failed Load InspectionMaster!");
          return;
        }
      }
      if (!ChatUtility.SetupChatChannelMaster())
      {
        DebugUtility.LogError("ChatWindow Error:Failed Load ChatChannelMaster!");
      }
      else
      {
        if (UnityEngine.Object.op_Inequality((UnityEngine.Object) this.InputPlaceholderText, (UnityEngine.Object) null))
        {
          this.mInputPlaceholderDefaultText_World = this.InputPlaceholderText.get_text();
          this.mInputPlaceholderDefaultText_Room = LocalizedText.Get("sys.CHAT_DISABLE_INPUT_FIELD_ROOM");
        }
        if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.InputPlaceholderText, (UnityEngine.Object) null))
          DebugUtility.LogError("InputPlaceholderText is NULL");
        this.mState = new StateMachine<ChatWindow>(this);
        this.mState.GotoState<ChatWindow.State_Init>();
      }
    }

    public void SetMessageDataToFlowNode(string input_text, bool is_force_send = false)
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.FlowNodeSendChatMessage, (UnityEngine.Object) null))
        DebugUtility.LogError("CHatWindow.cs -> OnSetSendMessage():FlowNode_SendChatMessage is Null References!");
      else if (string.IsNullOrEmpty(input_text))
      {
        this.FlowNodeSendChatMessage.ResetParam();
      }
      else
      {
        string message = input_text;
        if (message.Length > this.CharacterLimit)
          message = message.Substring(0, this.CharacterLimit);
        switch (this.mCurrentChatType)
        {
          case ChatWindow.eChatType.World:
            this.FlowNodeSendChatMessage.SetMessageData((int) GlobalVars.CurrentChatChannel, message);
            break;
          case ChatWindow.eChatType.Room:
            this.FlowNodeSendChatMessage.SetMessageData(GlobalVars.SelectedMultiPlayRoomName, message);
            break;
        }
        if (!is_force_send)
          return;
        this.FlowNodeSendChatMessage.ReqestSendMessage();
      }
    }

    private void OnSetSendMessage()
    {
      this.SetMessageDataToFlowNode(this.InputFieldMessage.get_text(), false);
    }

    private void SetSendStamp()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.FlowNodeSendChatMessage, (UnityEngine.Object) null))
        return;
      int stamp_id = int.Parse(FlowNode_Variable.Get("SELECT_STAMP_ID"));
      switch (this.mCurrentChatType)
      {
        case ChatWindow.eChatType.World:
          this.FlowNodeSendChatMessage.SetStampData((int) GlobalVars.CurrentChatChannel, stamp_id);
          break;
        case ChatWindow.eChatType.Room:
          this.FlowNodeSendChatMessage.SetStampData(GlobalVars.SelectedMultiPlayRoomName, stamp_id);
          break;
      }
      FlowNode_GameObject.ActivateOutputLinks((Component) this, 101);
    }

    private void UpdateSendMessageButtonInteractable()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.InputFieldMessage, (UnityEngine.Object) null) || UnityEngine.Object.op_Equality((UnityEngine.Object) this.SendMessageButton, (UnityEngine.Object) null) || UnityEngine.Object.op_Equality((UnityEngine.Object) this.UsefulButton, (UnityEngine.Object) null))
        return;
      if (this.mCurrentChatType == ChatWindow.eChatType.Room && !ChatWindow.room_info.IsActive)
      {
        SRPG_Button sendMessageButton = this.SendMessageButton;
        bool flag1 = false;
        ((Selectable) this.InputFieldMessage).set_interactable(flag1);
        bool flag2 = flag1;
        ((Selectable) this.UsefulButton).set_interactable(flag2);
        int num = flag2 ? 1 : 0;
        ((Selectable) sendMessageButton).set_interactable(num != 0);
      }
      else
      {
        SRPG_Button sendMessageButton = this.SendMessageButton;
        bool flag1 = true;
        ((Selectable) this.InputFieldMessage).set_interactable(flag1);
        bool flag2 = flag1;
        ((Selectable) this.UsefulButton).set_interactable(flag2);
        int num = flag2 ? 1 : 0;
        ((Selectable) sendMessageButton).set_interactable(num != 0);
      }
    }

    private void UpdateInputPlaceholderText()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.InputFieldMessage, (UnityEngine.Object) null) || UnityEngine.Object.op_Equality((UnityEngine.Object) this.InputPlaceholderText, (UnityEngine.Object) null))
        return;
      if (this.mCurrentChatType == ChatWindow.eChatType.Room && !ChatWindow.room_info.IsActive)
        this.InputPlaceholderText.set_text(this.mInputPlaceholderDefaultText_Room);
      else
        this.InputPlaceholderText.set_text(this.mInputPlaceholderDefaultText_World);
    }

    private void Update()
    {
      if (this.UpdateMaintenancePanel() || !ChatUtility.IsMultiQuestNow() && !this.IsPermit || (!this.gm.IsExternalPermit() || this.mState == null))
        return;
      this.UpdateSendMessageButtonInteractable();
      this.UpdateInputPlaceholderText();
      ChatWindow.room_info.Run();
      this.CheckRoomMember();
      this.mState.Update();
    }

    private void ClearAllItems()
    {
      for (int index = 0; index < this.mItems.Count; ++index)
        this.mItems[index].Clear();
    }

    private void UpdateChannelPanel()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.ChannelPanel, (UnityEngine.Object) null))
        return;
      int num1 = this.ChannelPanel.get_transform().get_childCount() - 1;
      int length = GlobalVars.CurrentChatChannel.ToString().Length;
      int currentChatChannel = (int) GlobalVars.CurrentChatChannel;
      for (int index = num1; index > 0; --index)
      {
        Transform child = this.ChannelPanel.get_transform().FindChild("value_" + Mathf.Pow(10f, (float) (index - 1)).ToString());
        if (!UnityEngine.Object.op_Equality((UnityEngine.Object) child, (UnityEngine.Object) null))
        {
          if (length < index)
          {
            ((Component) child).get_gameObject().SetActive(false);
          }
          else
          {
            int num2 = (int) Mathf.Pow(10f, (float) (index - 1));
            int num3 = currentChatChannel / num2;
            ((Component) child).get_gameObject().SetActive(true);
            ((ImageArray) ((Component) child).GetComponent<ImageArray>()).ImageIndex = num3;
            currentChatChannel %= num2;
          }
        }
      }
    }

    private void RequestChatLog(ChatWindow.eChatType select, bool force_request = false)
    {
      switch (select)
      {
        case ChatWindow.eChatType.World:
          this.RequestWorldChatLog();
          break;
        case ChatWindow.eChatType.Room:
          this.RequestRoomChatLog(this.GetChatLogInstance(ChatWindow.eChatType.Room).LastMessageIdServer, force_request);
          break;
      }
    }

    private void RequestWorldChatLog()
    {
      ChatLog chatLogInstance = this.GetChatLogInstance(ChatWindow.eChatType.World);
      FlowNode_ReqChatMessageWorld component = (FlowNode_ReqChatMessageWorld) ((Component) this).get_gameObject().GetComponent<FlowNode_ReqChatMessageWorld>();
      if (UnityEngine.Object.op_Inequality((UnityEngine.Object) component, (UnityEngine.Object) null))
      {
        component.SetChatMessageInfo((int) GlobalVars.CurrentChatChannel, 0L, (int) ChatWindow.MAX_CHAT_LOG_ITEM, chatLogInstance.LastMessageIdServer);
        this.mRequesting = true;
      }
      if (!this.mRequesting)
        return;
      FlowNode_GameObject.ActivateOutputLinks((Component) this, 40);
    }

    private void RequestRoomChatLog(long exclude_id, bool force_request = false)
    {
      if (!ChatWindow.room_info.IsActive && exclude_id <= 0L)
        return;
      if (force_request || ChatWindow.room_info.IsActive)
      {
        FlowNode_ReqChatMessageRoom component = (FlowNode_ReqChatMessageRoom) ((Component) this).get_gameObject().GetComponent<FlowNode_ReqChatMessageRoom>();
        if (UnityEngine.Object.op_Inequality((UnityEngine.Object) component, (UnityEngine.Object) null))
        {
          string room_token = !ChatWindow.room_info.IsActive ? string.Empty : GlobalVars.SelectedMultiPlayRoomName;
          bool is_sys_msg_merge = force_request;
          component.SetChatMessageInfo(room_token, 0L, (int) ChatWindow.MAX_CHAT_LOG_ITEM, exclude_id, is_sys_msg_merge);
          this.mRequesting = true;
        }
      }
      if (!this.mRequesting)
        return;
      FlowNode_GameObject.ActivateOutputLinks((Component) this, 46);
    }

    private void RefreshChatLogView(bool _force_refresh = false)
    {
      ChatLog chatLogInstance = this.GetChatLogInstance(this.mCurrentChatType);
      if (chatLogInstance.is_dirty || _force_refresh)
      {
        chatLogInstance.is_dirty = false;
        this.StartCoroutine(this.RefreshChatMessage(chatLogInstance));
      }
      else
        this.mRequesting = false;
    }

    [DebuggerHidden]
    private IEnumerator RefreshChatMessage(ChatLog _chat_log)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChatWindow.\u003CRefreshChatMessage\u003Ec__IteratorEB() { _chat_log = _chat_log, \u003C\u0024\u003E_chat_log = _chat_log, \u003C\u003Ef__this = this };
    }

    private string FilterMessage(string message)
    {
      string[] strArray = this.isIllegalWord(message);
      if (strArray != null && strArray.Length > 0)
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          string oldValue = strArray[index];
          string newValue = new StringBuilder().Insert(0, "*", oldValue.Length).ToString();
          message = message.Replace(oldValue, newValue);
        }
      }
      if (this.mChatInspectionMaster != null && this.mChatInspectionMaster.Count > 0)
      {
        using (List<ChatUtility.ChatInspectionMaster>.Enumerator enumerator = this.mChatInspectionMaster.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ChatUtility.ChatInspectionMaster current = enumerator.Current;
            StringBuilder stringBuilder;
            if (!string.IsNullOrEmpty(current.word))
            {
              for (; message.ToLower().Contains(current.word.ToLower()); message = stringBuilder.ToString())
              {
                string str = new StringBuilder().Insert(0, "*", current.word.Length).ToString();
                int num = message.ToLower().IndexOf(current.word.ToLower());
                stringBuilder = new StringBuilder(message);
                stringBuilder.Remove(num, current.word.Length);
                stringBuilder.Insert(num, str);
              }
            }
          }
        }
      }
      return message;
    }

    private void setPosX(RectTransform rt, float px)
    {
      if (!UnityEngine.Object.op_Implicit((UnityEngine.Object) rt))
        return;
      Vector2 anchoredPosition = rt.get_anchoredPosition();
      anchoredPosition.x = (__Null) (double) px;
      rt.set_anchoredPosition(anchoredPosition);
    }

    private string[] isIllegalWord(string text)
    {
      List<string> stringList = new List<string>();
      MatchCollection matchCollection1 = new Regex("\\d{1,4}(-|ー|‐|－)\\d{1,4}(-|ー|‐|－)\\d{4}|\\d{10,11}").Matches(text);
      if (matchCollection1 != null && matchCollection1.Count > 0)
      {
        foreach (Match match in matchCollection1)
          stringList.Add(match.Value);
      }
      MatchCollection matchCollection2 = new Regex("(.)+@(.)+(\\.|どっと|ドット|dot)(.*)").Matches(text);
      if (matchCollection2 != null && matchCollection2.Count > 0)
      {
        foreach (Match match in matchCollection2)
          stringList.Add(match.Value);
      }
      return stringList.ToArray();
    }

    private void OnTapUnitIcon(SRPG_Button button)
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) button, (UnityEngine.Object) null))
        return;
      ChatLogItem componentInParent = (ChatLogItem) ((Component) button).GetComponentInParent<ChatLogItem>();
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) componentInParent, (UnityEngine.Object) null) || componentInParent.ChatLogParam == null || string.IsNullOrEmpty(componentInParent.ChatLogParam.uid))
        return;
      FlowNode_Variable.Set("SelectUserID", componentInParent.ChatLogParam.uid);
      FlowNode_Variable.Set("IsBlackList", "0");
      FlowNode_GameObject.ActivateOutputLinks((Component) this, 30);
    }

    private bool UpdateMaintenancePanel()
    {
      if (!GameUtility.Config_ChatState.Value)
      {
        this.MaintenancePanel.SetActive(true);
        this.NoUsedChatText.SetActive(true);
        this.MaintenanceText.SetActive(false);
        return true;
      }
      if (this.Maintenance)
      {
        this.MaintenancePanel.SetActive(true);
        this.MaintenanceText.SetActive(true);
        this.NoUsedChatText.SetActive(false);
        if (!string.IsNullOrEmpty(this.MaintenanceMsg))
          this.MaintenanceMsgText.set_text(this.MaintenanceMsg);
        return true;
      }
      this.MaintenancePanel.SetActive(false);
      this.NoUsedChatText.SetActive(false);
      this.MaintenanceText.SetActive(false);
      return false;
    }

    private void UpdateMessageBadgeState()
    {
      this.RefreshMainBadge(this.RefreshWorldChatBadge(), this.RefreshRoomChatBadge());
    }

    private void RefreshMainBadge(bool is_show_world_badge, bool is_show_room_badge)
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.UpdateMessageBadge, (UnityEngine.Object) null))
        return;
      if (this.mOpened)
      {
        this.UpdateMessageBadge.SetActive(false);
      }
      else
      {
        string str = string.Empty;
        if (is_show_world_badge)
        {
          if (this.mWorldChatLog != null && this.mWorldChatLog.messages.Count > 0)
          {
            ChatLogParam message = this.mWorldChatLog.messages[this.mWorldChatLog.messages.Count - 1];
            long id = message.id;
            if (message.fuid != this.gm.Player.FUID)
            {
              string source = this.FilterMessage(message.message);
              if (!source.All<char>((Func<char, bool>) (x =>
              {
                if ((int) x != 42)
                  return (int) x == 32;
                return true;
              })))
                str = "[" + message.name + "]:" + source;
              if (message.message == string.Empty)
                str = "[" + message.name + "] " + LocalizedText.Get("sys.CHAT_SEND_STAMP");
            }
          }
        }
        else if (is_show_room_badge && this.mRoomChatLog != null && this.mRoomChatLog.messages.Count > 0)
        {
          ChatLogParam message = this.mRoomChatLog.messages[this.mRoomChatLog.messages.Count - 1];
          long id = message.id;
          if (message.fuid == null)
            str = message.message;
          else if (this.gm.Player != null && message.fuid != this.gm.Player.FUID)
          {
            string source = this.FilterMessage(message.message);
            if (!source.All<char>((Func<char, bool>) (x =>
            {
              if ((int) x != 42)
                return (int) x == 32;
              return true;
            })))
              str = "[" + message.name + "]:" + source;
            if (message.message == string.Empty)
              str = "[" + message.name + "] " + LocalizedText.Get("sys.CHAT_SEND_STAMP");
          }
        }
        if (!is_show_world_badge && !is_show_room_badge)
          return;
        if (str != string.Empty && UnityEngine.Object.op_Inequality((UnityEngine.Object) this.ClosedShowMessage, (UnityEngine.Object) null))
          ((UnityEngine.UI.Text) ((Component) this.ClosedShowMessage.get_transform().Find("text")).GetComponent<UnityEngine.UI.Text>()).set_text(str);
        this.UpdateMessageBadge.SetActive(true);
      }
    }

    private bool RefreshWorldChatBadge()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.WordlChatBadge, (UnityEngine.Object) null))
        return false;
      bool isDirty = this.GetChatLogInstance(ChatWindow.eChatType.World).is_dirty;
      if (this.mCurrentChatType == ChatWindow.eChatType.World)
        this.WordlChatBadge.SetActive(false);
      else
        this.WordlChatBadge.SetActive(isDirty);
      return isDirty;
    }

    private bool RefreshRoomChatBadge()
    {
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) this.RoomChatBadge, (UnityEngine.Object) null))
        return false;
      bool isDirty = this.GetChatLogInstance(ChatWindow.eChatType.Room).is_dirty;
      if (this.mCurrentChatType == ChatWindow.eChatType.Room)
        this.RoomChatBadge.SetActive(false);
      else
        this.RoomChatBadge.SetActive(isDirty);
      return isDirty;
    }

    private void ResetCloseShowMessage()
    {
      if (!UnityEngine.Object.op_Inequality((UnityEngine.Object) this.ClosedShowMessage, (UnityEngine.Object) null))
        return;
      ((UnityEngine.UI.Text) ((Component) this.ClosedShowMessage.get_transform().Find("text")).GetComponent<UnityEngine.UI.Text>()).set_text(string.Empty);
    }

    private void CheckChannelUpdate()
    {
      if (this.mLastChannelID == -1)
        this.mLastChannelID = (int) GlobalVars.CurrentChatChannel;
      if (this.mLastChannelID != (int) GlobalVars.CurrentChatChannel && UnityEngine.Object.op_Inequality((UnityEngine.Object) this.FlowNodeSendChatMessage, (UnityEngine.Object) null))
        this.SetMessageDataToFlowNode(this.InputFieldMessage.get_text(), false);
      this.mLastChannelID = (int) GlobalVars.CurrentChatChannel;
    }

    private void CheckRoomMember()
    {
      if (!ChatWindow.room_info.IsActive)
      {
        ChatWindow.room_member_manager.Clear();
      }
      else
      {
        this.elapsed_time_for_photon_room_member -= Time.get_deltaTime();
        if ((double) this.elapsed_time_for_photon_room_member > 0.0)
          return;
        this.elapsed_time_for_photon_room_member = ChatWindow.SPAN_UPDATE_ROOM_MEMBER;
        if (PunMonoSingleton<MyPhoton>.Instance.IsConnectedInRoom())
          ChatWindow.room_member_manager.Refresh(PunMonoSingleton<MyPhoton>.Instance.GetRoomPlayerList());
        if (ChatWindow.room_member_manager.EntryMembers.Count <= 0 && ChatWindow.room_member_manager.LeaveMembers.Count <= 0)
          return;
        ChatLog chatLogInstance = this.GetChatLogInstance(ChatWindow.eChatType.Room);
        List<ChatLogParam> _message = new List<ChatLogParam>();
        ChatLogParam chatLogParam1 = (ChatLogParam) null;
        for (int index = 0; index < ChatWindow.room_member_manager.EntryMembers.Count; ++index)
        {
          ChatLogParam chatLogParam2 = new ChatLogParam();
          chatLogParam2.id = this.GenerateSystemMessageId();
          chatLogParam2.message_type = (byte) 3;
          chatLogParam2.posted_at = TimeManager.FromDateTime(TimeManager.ServerTime);
          if (MonoSingleton<GameManager>.Instance.DeviceId == ChatWindow.room_member_manager.EntryMembers[index].UID)
          {
            string format = LocalizedText.Get("sys.CHAT_SYSTEM_MESSAGE_ENTRY_ROOM_SELF");
            chatLogParam2.message = string.Format(format, (object) ChatWindow.room_info.QuestParam.name);
            chatLogParam1 = chatLogParam2;
          }
          else
          {
            string format = LocalizedText.Get("sys.CHAT_SYSTEM_MESSAGE_ENTRY_ROOM_OTHER");
            chatLogParam2.message = string.Format(format, (object) ChatWindow.room_member_manager.EntryMembers[index].Name);
          }
          _message.Add(chatLogParam2);
        }
        for (int index = 0; index < ChatWindow.room_member_manager.LeaveMembers.Count; ++index)
        {
          ChatLogParam chatLogParam2 = new ChatLogParam();
          chatLogParam2.id = this.GenerateSystemMessageId();
          chatLogParam2.message_type = (byte) 3;
          chatLogParam2.posted_at = TimeManager.FromDateTime(TimeManager.ServerTime);
          if (MonoSingleton<GameManager>.Instance.DeviceId == ChatWindow.room_member_manager.LeaveMembers[index].UID)
          {
            string format = LocalizedText.Get("sys.CHAT_SYSTEM_MESSAGE_LEAVE_ROOM_SELF");
            chatLogParam2.message = string.Format(format, (object) ChatWindow.room_info.QuestParam.name);
          }
          else
          {
            string format = LocalizedText.Get("sys.CHAT_SYSTEM_MESSAGE_LEAVE_ROOM_OTHER");
            chatLogParam2.message = string.Format(format, (object) ChatWindow.room_member_manager.LeaveMembers[index].Name);
          }
          _message.Add(chatLogParam2);
        }
        if (chatLogParam1 != null)
        {
          _message.Clear();
          _message.Add(chatLogParam1);
        }
        chatLogInstance.AddMessage(_message);
        this.UpdateMessageBadgeState();
        this.RefreshChatLogView(false);
        ChatWindow.room_member_manager.EntryMembers.Clear();
        ChatWindow.room_member_manager.LeaveMembers.Clear();
      }
    }

    public void ExitRoomSelf()
    {
      if (!ChatWindow.room_info.IsActive)
        return;
      ChatWindow.room_info.ExitRoom();
      ChatWindow.room_member_manager.Clear();
      if (ChatWindow.room_info.QuestParam != null)
      {
        ChatLog chatLogInstance = this.GetChatLogInstance(ChatWindow.eChatType.Room);
        ChatLogParam chatLogParam = new ChatLogParam();
        chatLogParam.id = this.GenerateSystemMessageId();
        chatLogParam.message_type = (byte) 3;
        chatLogParam.posted_at = TimeManager.FromDateTime(TimeManager.ServerTime);
        string format = LocalizedText.Get("sys.CHAT_SYSTEM_MESSAGE_LEAVE_ROOM_SELF");
        chatLogParam.message = string.Format(format, (object) ChatWindow.room_info.QuestParam.name);
        chatLogInstance.AddMessage(chatLogParam);
      }
      this.SaveRoomChatLogCache();
    }

    private long GenerateSystemMessageId()
    {
      --ChatWindow.system_message_local_id;
      return ChatWindow.system_message_local_id;
    }

    private void SaveRoomChatLogCache()
    {
      this.CopyChatLog(this.GetChatLogInstance(ChatWindow.eChatType.Room), ref ChatWindow.CacheRoomChatLog);
    }

    private void CopyChatLog(ChatLog _base, ref ChatLog _target)
    {
      ChatLog chatLog = new ChatLog();
      for (int index = 0; index < _base.messages.Count; ++index)
        chatLog.AddMessage(_base.messages[index]);
      _target = chatLog;
    }

    public enum eChatType : byte
    {
      None,
      World,
      Room,
    }

    public enum MessageTemplateType : byte
    {
      None,
      OtherUser,
      User,
      System,
    }

    private class State_NoUsed : State<ChatWindow>
    {
      public override void Begin(ChatWindow self)
      {
      }

      public override void Update(ChatWindow self)
      {
      }
    }

    private class State_Init : State<ChatWindow>
    {
      public override void Update(ChatWindow self)
      {
        if ((int) GlobalVars.CurrentChatChannel <= 0)
          return;
        self.CheckChannelUpdate();
        self.UpdateChannelPanel();
        self.mState.GotoState<ChatWindow.State_WaitClosed>();
      }
    }

    private class State_WaitOpened : State<ChatWindow>
    {
      public override void Begin(ChatWindow self)
      {
        if (!self.IsInitialized)
        {
          self.mRestTime_Opend_UpdateWorldChat = 0.0f;
          self.mRestTime_Opend_UpdateRoomChat = 0.0f;
        }
        self.IsInitialized = true;
      }

      public override void Update(ChatWindow self)
      {
        if (!self.IsOpened)
        {
          self.mState.GotoState<ChatWindow.State_WaitClosed>();
        }
        else
        {
          if (self.IsRequesting)
            return;
          if (self.mCurrentChatType == ChatWindow.eChatType.World && (double) self.mRestTime_Opend_UpdateWorldChat <= 0.0)
          {
            self.mRestTime_Opend_UpdateWorldChat = ChatWindow.SPAN_UPDATE_WORLD_MESSAGE_UIOPEN;
            self.RequestChatLog(self.mCurrentChatType, false);
          }
          else if (self.mCurrentChatType == ChatWindow.eChatType.Room && (double) self.mRestTime_Opend_UpdateRoomChat <= 0.0)
          {
            self.mRestTime_Opend_UpdateRoomChat = ChatWindow.SPAN_UPDATE_ROOM_MESSAGE_UIOPEN;
            self.RequestChatLog(self.mCurrentChatType, false);
          }
          else
          {
            self.mRestTime_Opend_UpdateWorldChat = Mathf.Max(0.0f, self.mRestTime_Opend_UpdateWorldChat - Time.get_deltaTime());
            self.mRestTime_Opend_UpdateRoomChat = Mathf.Max(0.0f, self.mRestTime_Opend_UpdateRoomChat - Time.get_deltaTime());
          }
        }
      }
    }

    private class State_WaitClosed : State<ChatWindow>
    {
      public override void Begin(ChatWindow self)
      {
        self.mRestTime_Closed_UpdateWorldChat = ChatWindow.SPAN_UPDATE_WORLD_MESSAGE_UICLOSE;
        self.mRestTime_Closed_UpdateRoomChat = ChatWindow.SPAN_UPDATE_ROOM_MESSAGE_UICLOSE;
      }

      public override void Update(ChatWindow self)
      {
        if (self.IsOpened)
        {
          self.mState.GotoState<ChatWindow.State_WaitOpened>();
        }
        else
        {
          if (self.IsRequesting)
            return;
          if (self.mCurrentChatType == ChatWindow.eChatType.World && (double) self.mRestTime_Closed_UpdateWorldChat <= 0.0)
          {
            self.mRestTime_Closed_UpdateWorldChat = ChatWindow.SPAN_UPDATE_WORLD_MESSAGE_UICLOSE;
            self.RequestChatLog(self.mCurrentChatType, false);
          }
          else if (self.mCurrentChatType == ChatWindow.eChatType.Room && (double) self.mRestTime_Closed_UpdateRoomChat <= 0.0)
          {
            self.mRestTime_Closed_UpdateRoomChat = ChatWindow.SPAN_UPDATE_ROOM_MESSAGE_UICLOSE;
            self.RequestChatLog(self.mCurrentChatType, false);
          }
          else
          {
            self.mRestTime_Closed_UpdateWorldChat = Mathf.Max(0.0f, self.mRestTime_Closed_UpdateWorldChat - Time.get_deltaTime());
            self.mRestTime_Closed_UpdateRoomChat = Mathf.Max(0.0f, self.mRestTime_Closed_UpdateRoomChat - Time.get_deltaTime());
          }
        }
      }
    }
  }
}
