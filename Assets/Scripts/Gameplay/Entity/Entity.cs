using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private string id;

    public string Id => id;

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }
    }

    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
