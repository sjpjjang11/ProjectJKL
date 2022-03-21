using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csManager_InGame : Singleton<csManager_InGame>
{
    // 임시
    public Transform[] m_Array_SpawnPoint;

    public csBattleUIManager BattleUIManager { get; private set; }

    public csPlayerCamera PlayerCamera { get; private set; }

    public const int Type_Hero = 0;
    public const int Type_Monster = 1;

    protected override void Awake()
    {
        base.Awake();

        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (PlayerCamera == null)
        {
            PlayerCamera = Instantiate(Resources.Load("Prefabs/HeroRelated/PlayerCamera") as GameObject).GetComponent<csPlayerCamera>();
            SceneManager.MoveGameObjectToScene(PlayerCamera.gameObject, gameObject.scene);
        }

        if (BattleUIManager == null)
        {
            BattleUIManager = Instantiate(Resources.Load("Prefabs/UI/BattleUIManager") as GameObject).GetComponent<csBattleUIManager>();
            SceneManager.MoveGameObjectToScene(BattleUIManager.gameObject, gameObject.scene);
        }
    }

    private void Start()
    {
        Settings();
    }
 
    private void Settings()
    {
        //CreatePool();

        //CreateLocal();
        //CreateMonster();
    }

    private void PrepareGame()
    {

    }

    private void CreatePool()
    {
        csCreator_PoolSpace.CreatePool("WaitingPool/Monster", gameObject.scene);
        csCreator_PoolSpace.CreatePool("WaitingPool/Bullet", gameObject.scene);
        csCreator_PoolSpace.CreatePool("WaitingPool/Effect", gameObject.scene);
        csCreator_PoolSpace.CreatePool("WaitingPool/CommonEffect", gameObject.scene);
        csCreator_PoolSpace.CreatePool("WaitingPool/Item", gameObject.scene);

        csCreator_PoolSpace.CreatePool("OwnerPool/Hero", gameObject.scene);
        csCreator_PoolSpace.CreatePool("OwnerPool/Remote", gameObject.scene);
        csCreator_PoolSpace.CreatePool("OwnerPool/AI", gameObject.scene);
        csCreator_PoolSpace.CreatePool("OwnerPool/Monster", gameObject.scene);
    }

    private void CreateLocal()
    {
        Creator_Owner.CreateLocal();
    }

    private void CreateMonster()
    {

    }
}
