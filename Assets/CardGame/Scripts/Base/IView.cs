using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    void BindModel(IModel model);
    IModel GetModel();
    void Refresh();
}
