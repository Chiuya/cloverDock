using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneToLoad;
    public Animator transitionAnim;
    [SerializeField]
    private float transitionTime = 1f;

    public void loadNextScene()
    {
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        //play animation
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
