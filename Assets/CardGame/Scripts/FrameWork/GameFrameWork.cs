using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Studio.OverOne.DragMe.Components;
using UnityEngine;

public class GameFrameWork : SerializedMonoBehaviour
{
    public static GameFrameWork Instance { get {
            return GameObject.Find("GameFrameWork").GetComponent<GameFrameWork>();
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
    public void AddCardByCardData(CardData cardData, Vector3 pos)
    {
        var cardTemplate = gameConfig.viewDic[cardData.viewType];
        AddCardByCardModel(cardData.CreateModel(),pos);
    }

    public GameObject AddCardByCardModel(CardModel cardModel,Vector3 pos)
    {
        var cardTemplate = gameConfig.viewDic[cardModel.cardData.viewType];
        var obj = GameObject.Instantiate(cardTemplate);
        var cv = obj.GetComponent<CardView>();
        cv.BindModel(cardModel);
        obj.transform.GetComponent<DragMe>().SetOriginPos(pos);
        viewModelManager.Bind(cardModel, cv);
        return obj;
    }
    public GameObject AddTaskByData(TaskPanelModel model,Vector3 pos)
    {
        var taskTemplate = gameConfig.taskTemplate;
        var obj = GameObject.Instantiate(taskTemplate);
        var cv = obj.GetComponent<TaskView>();
        cv.BindModel(model);
        obj.transform.GetComponent<DragMe>().SetOriginPos(pos);
        viewModelManager.Bind(model, cv);
        return obj;
    }

    public void Update()
    {
        cardsManager.Update();
        taskManager.Update();
    }
}
