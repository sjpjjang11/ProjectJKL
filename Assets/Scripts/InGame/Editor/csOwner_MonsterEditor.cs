using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProjectK;

[CanEditMultipleObjects]
[CustomEditor(typeof(csOwner_Monster))]
public class csOwner_MonsterEditor : Editor
{
	protected ReorderableList m_List;
	protected SerializedProperty m_Target;
	//protected SerializedProperty m_CurrentState;
	//protected SerializedProperty _target;

	/*protected virtual void OnEnable()
	{
		m_List = new ReorderableList(serializedObject.FindProperty("m_States"));
		m_List.elementNameProperty = "States";
		m_List.elementDisplayType = ReorderableList.ElementDisplayType.Expandable;

		m_Target = serializedObject.FindProperty("m_Target");
		//m_CurrentState = serializedObject.FindProperty("m_CurrentState");
		//_target = serializedObject.FindProperty("Target");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		m_List.DoLayoutList();
		EditorGUILayout.PropertyField(m_Target);
		csOwner_Monster Target = (csOwner_Monster)target;
		//EditorGUILayout.LabelField("Current State", Target.CurrentState.m_BotState.ToString());
		//EditorGUILayout.PropertyField(m_CurrentState);
		//EditorGUILayout.PropertyField(_target);
		serializedObject.ApplyModifiedProperties();
	}*/
}
