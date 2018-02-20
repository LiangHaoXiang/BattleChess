using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1_UI : MonoBehaviour
{
    public void onBeginClick()
    {
        SceneManager.LoadScene("scene2");
    }
}
