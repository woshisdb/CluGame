using System;
using UnityEngine;

public class IDModel
{
    public string id;
}

public interface IRegisterID
{
    string SetID(Func<string> id);
}

public static class IRegisterID_Ext
{
    public static string GetID(this IRegisterID obj)
    {
        var id = obj.SetID(() =>
        {
            return GameFrameWork.Instance.IdGenerator.Next().ToString();
        });
        return id;
    }
}

[System.Serializable]
public class ObjectRef<T> where T : IRegisterID
{
    [SerializeField]
    private string id;

    public string Id => id;

    public T Value
    {
        get
        {
            if (id==null)
                return default(T);

            var obj = GameFrameWork.Instance.GetObjById(id);
            return (T)obj;
        }
        set
        {
            id = value != null ? value.GetID() : "";
        }
    }

    public void SetNull()
    {
        id = null;
    }
    public bool IsValid => Value != null;
    public static implicit operator T(ObjectRef<T> reference)
    {
        return reference != null ? reference.Value : default(T);
    }
}

