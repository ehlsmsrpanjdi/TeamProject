using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResetHelper
{
    public static GameObject TryFindChild(this MonoBehaviour _parent, string _childName)
    {
        var child = ResetHelper.FindChild(_parent.transform, _childName);
        if (child == null) DebugHelper.Log($"{_parent.name}에 {_childName}이라는 자식 오브젝트는 존재하지 않음", _parent);
        return child;
    }

    private static GameObject FindChild(Transform _parent, string _childName)
    {
        GameObject findChild = null;

        for (int i = 0; i < _parent.transform.childCount; i++)
        {
            var child = _parent.transform.GetChild(i);
            findChild = child.name == _childName ? child.gameObject : FindChild(child, _childName);
            if (findChild != null) break;
        }

        return findChild;
    }
}
