using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPoolSpace : MonoBehaviour
{
    public Transform Transform { get; private set; }

    public void Settings()
    {
        Transform = GetComponent<Transform>();

        Transform.position = Vector3.zero;
    }
}
