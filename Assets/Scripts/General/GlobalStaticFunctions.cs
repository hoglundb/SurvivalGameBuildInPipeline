using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStaticFunctions
{
    static Transform childFound = null;
    public static Transform CustomFindChild(string key, Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == key)
            {
                childFound = child;
            }
            else
            {
                if (child.childCount > 0)
                {
                    if (childFound == null)
                    {
                        CustomFindChild(key, child);
                    }
                }
            }
        }

        return childFound;
    }
}
