public class ViewModelManager
{
    public Dictionary<IModel, List<IView>> mapper;
    public ViewModelManager()
    {
        mapper = new Dictionary<IModel, List<IView>>();
    }

    public void RefreshView(IModel model)
    {
        if (mapper.ContainKey(model))
        {
            foreach (var x in mapper[model])
            {
                x.Refresh();
            }
        }
    }

    public void Bind(IModel model, IView view)
    {
        if (!mapper.ContainKey(model))
        {
            mapper[model] = new List<IView>();
        }
    }
    
    public void ReleaseView(IView view)
    {
        var model = view.GetModel();
        mapper[model].remove(view);
    }

    public void ReleaseModel(IModel model)
    {
        foreach (var x in mapper[model])
        {
            x.Release();
        }
    }
}