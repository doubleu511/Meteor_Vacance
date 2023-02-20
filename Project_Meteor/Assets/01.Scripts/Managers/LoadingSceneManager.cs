using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingSceneManager
{
    private float delayTime = 2f;
    private AsyncOperation operation;

    private string nextScene;

    private CanvasGroup cg;
    private bool isIntroLoading = false;

    public void Init()
    {
        GameObject canvas = Resources.Load<GameObject>("UI/LoadingCanvas");

        canvas = Object.Instantiate(canvas, null);
        cg = canvas.GetComponent<CanvasGroup>();
        Object.DontDestroyOnLoad(canvas);
    }

    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        isIntroLoading = true;
        DOTween.KillAll();

        cg.DOFade(1, 1f).OnComplete(() => isIntroLoading = false).SetUpdate(true);
        cg.interactable = true;
        cg.blocksRaycasts = true;
        Global.Pool.Clear();

        cg.gameObject.SetActive(true);
        cg.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(LoadAsynSceneCoroutine());
    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        yield return new WaitUntil(() => !isIntroLoading);
        Time.timeScale = 1;
        while (!operation.isDone)
        {
            float time = Time.timeSinceLevelLoad;

            if (time > delayTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        PlayerController.Interactable = true;
        cg.DOFade(0, 1f).OnComplete(() => cg.gameObject.SetActive(false)).SetUpdate(true);
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
