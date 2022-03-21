using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator_Owner : MonoBehaviour
{
    public static csOwner_Hero CreateLocal()
    {
        return Instantiate(Resources.Load(Const.PATH_PREFAB_OWNER_LOCAL) as GameObject).GetComponent<csOwner_Hero>();
    }

    public void CreateRemote()
    {

    }

    public void CreateAI()
    {

    }

    public void CreateMonster()
    {

    }
}
