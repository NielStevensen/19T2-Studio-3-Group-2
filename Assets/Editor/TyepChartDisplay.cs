using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TypeInteractions))]
public class TyepChartDisplay : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property ,GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);

        Rect newPos = position;
        newPos.y += 25f;
        SerializedProperty grid = property.FindPropertyRelative("matrix");
        for (int i = 0; i < 4; i++)
        {
            SerializedProperty intertaction = grid.GetArrayElementAtIndex(i).FindPropertyRelative("modifier");
            newPos.height = 20;
            if (intertaction.arraySize != 4)
                intertaction.arraySize = 4;
            newPos.width = 40;
            for (int j = 0; j < 4; j++)
            {
                EditorGUI.PropertyField(newPos, intertaction.GetArrayElementAtIndex(j), GUIContent.none);
                newPos.x = newPos.width;
            }
            newPos.x = position.x;
            newPos.y += 25;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 120;
    }
}
