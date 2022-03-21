using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Collector_Pool_Global
{
	public static Transform WaitingPool { get; private set; }
    public static Transform WaitingPool_Monster { get; private set; }
    public static Transform WaitingPool_Bullet { get; private set; }
	public static Transform WaitingPool_Effect { get; private set; }
	public static Transform WaitingPool_CommonEffect { get; private set; }
	public static Transform WaitingPool_Item { get; private set; }
    public static Transform OwnerPool_Local { get; private set; }
    public static Transform OwnerPool_Remote { get; private set; }
    public static Transform OwnerPool_AI { get; private set; }
    public static Transform OwnerPool_Monster { get; private set; }

    static Collector_Pool_Global()
    {
        CreatePool();
    }

    public static void Test()
    {
        /*Debug.LogError(WaitingPool_Monster.name);
        Debug.LogError(WaitingPool_Bullet.name);
        Debug.LogError(WaitingPool_Effect.name);
        Debug.LogError(WaitingPool_CommonEffect.name);
        Debug.LogError(WaitingPool_Item.name);

        Debug.LogError(OwnerPool_Local.name);
        Debug.LogError(OwnerPool_Remote.name);
        Debug.LogError(OwnerPool_AI.name);
        Debug.LogError(OwnerPool_Monster.name);*/
    }

    private static void CreatePool()
    {
        Scene Scene_InGame = SceneManager.GetSceneByName(csProjectManager.Instance.SceneType_InGame_InUse.ToString());

        csCreator_PoolSpace.CreatePool("OwnerPool/Local", Scene_InGame, out csPoolSpace _poolSpace);
        OwnerPool_Local = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("OwnerPool/Remote", Scene_InGame, out _poolSpace);
        OwnerPool_Remote = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("OwnerPool/AI", Scene_InGame, out _poolSpace);
        OwnerPool_AI = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("OwnerPool/Monster", Scene_InGame, out _poolSpace);
        OwnerPool_Monster = _poolSpace.Transform;

        csCreator_PoolSpace.CreatePool("WaitingPool/Monster", Scene_InGame, out _poolSpace);
        WaitingPool_Monster = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("WaitingPool/Bullet", Scene_InGame, out _poolSpace);
        WaitingPool_Bullet = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("WaitingPool/Effect", Scene_InGame, out _poolSpace);
        WaitingPool_Effect = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("WaitingPool/CommonEffect", Scene_InGame, out _poolSpace);
        WaitingPool_CommonEffect = _poolSpace.Transform;
        csCreator_PoolSpace.CreatePool("WaitingPool/Item", Scene_InGame, out _poolSpace);
        WaitingPool_Item = _poolSpace.Transform;        
    }
}
