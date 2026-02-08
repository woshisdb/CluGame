using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Studio.OverOne.DragMe.Components;
using UnityEngine;

public class GameFrameWork : SerializedMonoBehaviour,ISendEvent,IUISelector
{
    public static GameFrameWork instance;
    public static GameFrameWork Instance { get {
            if (instance == null)
            {
                instance = GameObject.Find("GameFrameWork").GetComponent<GameFrameWork>();
            }
            return instance;
    } }

    public SaveData data
    {
        get
        {
            return gameConfig.saveData;
        }
    }

    public UniqueIdGenerator IdGenerator
    {
        get
        {
            return data.saveFile.IdGenerator;
        }
    }
    public UIManager UIManager;
    public RollSystem rollSystem;
    public GameConfig gameConfig;
    public GameObject taskPanel;
    public Transform Table;
    // Update is called once per frame
    public CardsManager cardsManager;
    public TaskManager taskManager;
    public PlayerManager playerManager;
    public ViewModelManager viewModelManager;
    public GameHandUI gameHandUI;
    public Camera mainCamera;
    public GPTSystem GptSystem;
    public MonoManager MonoManager;
    public GameTimeManager GameTimeManager;
    public WorldMapSystem WorldMapSystem;
    public GameGenerate GameGenerate;
    public ChatPanel ChatPanel;
    public Dictionary<SpaceEnum, SpaceConfig> spaces;
    // public KPSystem KpSystem;
    // public KPStory KpStory;
    public KPFilterStory KpFilterStory;
    public KPBuildNpc KpBuildNpc;
    void Awake()
    {
        cardsManager.Init();
        taskManager.Init();
        viewModelManager.Init();
        playerManager.Init();
        // rollSystem.Init();
        GameTimeManager.Init();
    }

    public IRegisterID GetObjById(string id)
    {
        if (id == null)
        {
            return null;
        }
        if (data.saveFile.idMap.ContainsKey(id))
        {
            return data.saveFile.idMap[id];
        }
        else if(MonoManager.IDMap.ContainsKey(id))
        {
            return MonoManager.IDMap[id];
        }
        else if(gameConfig.ScriptableDatabase.ContainsKey(id))
        {
            return gameConfig.ScriptableDatabase[id];
        }
        else
        {
            return null;
        }
    }
    public NpcCardModel FindNpcById(string id)
    {
        return data.saveFile.npcs.Find(e =>
        {
            return e.npcId == id;
        });
    }

    public GameObject AddCardByCardModel(CardModel cardModel,Vector3 pos,bool pureSlotCard=false)
    {
        var cardTemplate = gameConfig.viewDic[cardModel.cardData.viewType];
        var obj = Instantiate(cardTemplate, pos, Quaternion.identity);
        var cv = obj.GetComponent<CardView>();
        cv.pureSlotCard = pureSlotCard;
        cv.BindModel(cardModel);
        return obj;
    }
    public GameObject AddTaskByData(TaskPanelModel model,Vector3 pos)
    {
        var taskTemplate = gameConfig.taskTemplate;
        var obj = GameObject.Instantiate(taskTemplate, pos, Quaternion.identity);
        var cv = obj.GetComponent<TaskView>();
        cv.BindModel(model);
        obj.transform.localPosition = (pos);
        //viewModelManager.Bind(model, cv);
        return obj;
    }

    public TaskPanelView OpenTaskPanel(TaskPanelModel model,Vector3 pos)
    {
        var view = taskPanel.GetComponent<TaskPanelView>();
        view.BindModel(model);
        view.gameObject.SetActive(true);
        view.transform.position = pos;
        return view;
    }

    public TaskPanelView RemoveTaskAndClosePanel(TaskPanelModel model)
    {
        taskManager.RemoveTask(model);
        var view = taskPanel.GetComponent<TaskPanelView>();
        view.gameObject.SetActive(false);
        return view;
    }
    public void Update()
    {
        cardsManager.Update();
        taskManager.Update();
    }
    // /// <summary>
    // /// 前往指定场景的处理
    // /// </summary>
    // /// <param name="spaceType"></param>
    // public void GoToSpace(SpaceEnum spaceType)
    // {
    //     var ret = spaces[spaceType];
    //     mainCamera.transform.position = ret.pos.position + new Vector3(0,40,0);
    // }
    public void GoToSpace(BaseGoToSpaceDelegate data)
    {
        data.GoTo();
    }
    /// <summary>
    /// 前往卡片
    /// </summary>
    public void CameraGoTo(Transform pos)
    {
        var aimPos = new Vector3(pos.position.x,40,pos.position.z);
        mainCamera.transform.position = aimPos;
    }
    public void GetNpcUI()
    {
        this.SendEvent(new RefreshViewEvent(this));
    }

    public List<UIItemBinder> GetUI()
    {
        var ret = new List<UIItemBinder>();
        foreach (var npc in playerManager.allNpc)
        {
            var bd = new List<UIItemBinder>();
            if (npc.GetComponent<BelongComponent>().belong.Value!=null)
            {
                bd.Add(new ButtonBinder(() =>
                {
                    return "位置";
                }, () =>
                {
                    CameraGoTo(npc.GetComponent<BelongComponent>().belong.Value.GetTransform());
                }));
            }
            var ui = new TableItemBinder(() =>
            {
                return npc.npcId;
            },bd);
            ret.Add(ui);
        }
        return ret;
    }
}
