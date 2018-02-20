using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingByAsync : MonoBehaviour
{
    AsyncOperation async;   //异步对象

    public static LoadingByAsync instance;
    public Slider loadingSlider;

    void Awake()
    {
        instance = this;
        loadingSlider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
    }

    void Start()
    {
        StartCoroutine(DelayLoading());
    }

    void Update()
    {
        if (async == null)
            return;
        int toProcess;
        //async.progress 的取值范围在0.1 - 1之间， 但是它不会等于1，加载完也就等于0.9
        if (async.progress < 0.9f)
        {
            toProcess = (int)async.progress * 100;
        }
        else
        {
            toProcess = 100;
        }
        if (loadingSlider.value < toProcess)
        {
            loadingSlider.value = toProcess;
        }
        if (toProcess == 100)
        {
            async.allowSceneActivation = true;
        }
    }

    public IEnumerator DelayLoading()
    {
        yield return new WaitForSeconds(0.1f);
        yield return Load_Scene("scene3(Main)");
    }

    //注意这里返回值一定是 IEnumerator
    public IEnumerator Load_Scene(string sceneName)
    {
        //异步读取场景。
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return async;
    }
}

