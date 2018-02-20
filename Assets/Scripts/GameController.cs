using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public static bool hadLoad = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        if (hadLoad == false)
        {
            hadLoad = true;
            DontDestroyOnLoad(gameObject);  //就算切换回场景1也不会执行，只执行最开始的那一遍
        }
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
