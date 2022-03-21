using UnityEngine;

[System.Serializable]
public class csInfo_Bugs
{
	public BugsHealth m_Health;
	public BugsPower m_Power;

	public float m_WalkSpeed;
	public float m_RunSpeed;

	public BugsAction[] m_BugsAction;
	
	public BugsCollider m_BugsCollider;

	public csInfo_Bugs()
	{

	}
}
