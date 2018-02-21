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
        SceneManager.sceneLoaded += OnSceneLoaded;   //加载场景事件监听
        if (hadLoad == false)
        {
            hadLoad = true;
            DontDestroyOnLoad(gameObject);  //就算切换回场景1也不会执行，只执行最开始的那一遍
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.isLoaded)
        {
            if (scene.name.Equals("scene3(Main)"))  //若主场景加载完毕并切换到主场景
            {
                Debug.Log("主场景已加载完毕并切换到主场景");
            }
        }
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
