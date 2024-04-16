using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wcng;
using Wcng.SkillEditor;

public class SkillAnimationEventDataInspectorHelper : ScriptableObjectSingleton<SkillAnimationEventDataInspectorHelper>
{
    public SkillAnimationEvent skillFrameEventBase;

    public void Inspector(SkillFrameEventBase skillFrameEventBase)
    {
        this.skillFrameEventBase = (SkillAnimationEvent)skillFrameEventBase;
        Selection.activeObject = this;
    }
}

public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = ScriptableObject.CreateInstance<T>();
            }
            return m_instance;
        }
    }

    private void OnDisable()
    {
        m_instance = null;
    }
}
