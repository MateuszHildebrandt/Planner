#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(MonoBehaviour), true)] // Target all MonoBehaviours and descendants
public class ExposeMethodInEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Type type;
        Attribute attribute;

        foreach (object myTarget in targets)
        {
            type = myTarget.GetType();

            //Iterate over each private or public instance method(no static methods atm)
            foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                //Make sure it is decorated by our custom attribute
                attribute = method.GetCustomAttribute(typeof(ExposeMethodInEditorAttribute), true);

                if (attribute != null)
                {
                    if (GUILayout.Button(method.Name))
                        method.Invoke((MonoBehaviour)target, null);
                }
            }   
        }
    }
}
#endif