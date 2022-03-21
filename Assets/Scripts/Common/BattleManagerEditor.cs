using UnityEditor;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
[CustomEditor(typeof(csBattleManager))]
public class MyTypeEditor : Editor
{
	csBattleManager m_Instance;
	PropertyField[] m_fields;


	public void OnEnable()
	{
		m_Instance = target as csBattleManager;
		m_fields = ExposeProperties.GetProperties(m_Instance);
	}

	public override void OnInspectorGUI()
	{

		if (m_Instance == null)
			return;

		this.DrawDefaultInspector();

		ExposeProperties.Expose(m_fields);
	}
}
#endif
