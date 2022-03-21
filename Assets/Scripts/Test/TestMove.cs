using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public Transform m_Target;
    public float m_Speed;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 MoveDir = (Vector3.forward * v) + (Vector3.right * h);
        m_Target.Translate(MoveDir.normalized * m_Speed * Time.deltaTime);
    }
}
