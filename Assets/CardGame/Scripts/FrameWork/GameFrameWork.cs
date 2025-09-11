using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Studio.OverOne.DragMe.Components;
using UnityEngine;

public class GameFrameWork : SerializedMonoBehaviour
{
    public static GameFrameWork instance;
    public static GameFrameWork Instance { get {
            if (instance == null)
            {
                instance = GameObject.Find("GameFrameWork").GetComponent<GameFrameWork>();
            }
            return instance;
    } }
    public GameConfig gameConfig;
    public GameObject taskPanel;
    public Transform Table;
    // Update is called once per frame
    public CardsManager cardsManager;
    public TaskManager taskManager;

    public ViewModelManager viewModelManager;


    void Awake()
    {
        cardsManager.Init();
        taskManager.Init();
        viewModelManager.Init();
    }
    public void AddCardByEnum(CardEnum cardEnum,Vector3 pos)
    {
        AddCardByCardData(gameConfig.CardMap[cardEnum],pos);
    }
    public void AddCardByCardData(CardData cardData, Vector3 pos)
    {
        //var cardTemplate = gameConfig.viewDic[cardData.viewType];
        AddCardByCardModel(cardData.CreateModel(),pos);
    }

    public GameObject AddCardByCardModel(CardModel cardModel,Vector3 pos)
    {
        var cardTemplate = gameConfig.viewDic[cardModel.cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        var cv = obj.GetComponent<CardView>();
        cv.BindModel(cardModel);
        obj.transform.GetComponent<DraggableCard>().transform.localPosition = (pos);
        return obj;
    }
    public GameObject AddTaskByData(TaskPanelModel model,Vector3 pos)
    {
        var taskTemplate = gameConfig.taskTemplate;
        var obj = GameObject.Instantiate(taskTemplate);
        var cv = obj.GetComponent<TaskView>();
        cv.BindModel(model);
        obj.transform.localPosition = (pos);
        //viewModelManager.Bind(model, cv);
        return obj;
    }

    public void Update()
    {
        cardsManager.Update();
        taskManager.Update();
    }
}
