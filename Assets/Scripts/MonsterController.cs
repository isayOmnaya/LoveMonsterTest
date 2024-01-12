using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class MonsterController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField]
    MonsterManager _monsterManager = null;

    [SerializeField]
    RoundManager _roundManager = null;

    [SerializeField]
    MonsterObjectPool _monsterObjectPool = null;

    // Start is called before the first frame update
    void Start()
    {
        _monsterManager.Initialize(_monsterObjectPool);
        _roundManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        _monsterManager.UpdateExternal();
        _roundManager.UpdateExternal(_monsterManager);
        TryCloseGame();

    #if UNITY_EDITOR
        if(!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            _roundManager.DeleteSaveFile(); // Temporary Delete when you quit From Inspector
        }
#endif
    }

    void TryCloseGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            _roundManager.DeleteSaveFile();
            Application.Quit();
        }
    }
}
