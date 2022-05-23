using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{


    public GameObject loadingScreen;
    public Slider Slider;
    public Text progressText;
    public void loadLevel(int scenIndex)
    {

        StartCoroutine(LoadAsynchronusly(scenIndex));

    }

    IEnumerator LoadAsynchronusly(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);
        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
    
}
