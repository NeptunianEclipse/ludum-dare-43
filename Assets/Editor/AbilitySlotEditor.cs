using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilitySlot), true)]
public class AbilitySlotEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Update"))
		{
			var slot = (AbilitySlot)target;
			slot.UpdateInternalData();
		}
	}
}
