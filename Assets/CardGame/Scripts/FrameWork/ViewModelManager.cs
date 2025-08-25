using System.Collections.Generic;
using UnityEngine;

public class ViewModelManager
{
    public Dictionary<IModel, List<IView>> mapper;
    public ViewModelManager()
    {
        mapper = new Dictionary<IModel, List<IView>>();
    }

    public void RefreshView(IModel model)
    {
        if (mapper.ContainsKey(model))
        {
            foreach (var x in mapper[model])
            {
                x.Refresh();
            }
        }
    }
    public void Init()
    {
        //var obj = GameObject.Find("Task");
        //Bind(obj.GetComponent<TaskView>().model, obj.GetComponent<TaskView>());
    }
    public void Bind(IModel model, IView view)
    {
        if (!mapper.ContainsKey(model))
        {
            mapper[model] = new List<IView>();
        }
        mapper[model].Add(view);
    }
    
    public void ReleaseView(IView view)
    {
        var model = view.GetModel();
        mapper[model].Remove(view);
    }

    public void ReleaseModel(IModel model)
    {
        foreach (var x in mapper[model])
        {
            x.Release();
        }
    }
}