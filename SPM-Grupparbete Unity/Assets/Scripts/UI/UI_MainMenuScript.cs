 using System.Collections;
using System.Collections.Generic;
 using System.IO;
 using TMPro;
 using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
 using UnityEngine.UI;

 public class UI_MainMenuScript : MonoBehaviour
{
    private PlayerStatistics playerStats = PlayerStatistics.Instance;

    [SerializeField] private Button firstSelected;
    [SerializeField] private int sceneToLoad = 0;

    public GameObject loadingScreen;
    public Slider Slider;
    public TextMeshProUGUI progressText;

   // [SerializeField] private GameObject settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
       // gameObject.SetActive(false);
        firstSelected.Select();
    }

  
        // För att senare ladda scenen som vi ska in i
        //SceneManager.LoadScene(playerStats.Scene);

        public void LoadGame()
        {
            if (Directory.Exists("Saves"))
            {
               StartCoroutine(LoadAsynchronusly(2));

            }
            else
            {
               StartCoroutine( LoadAsynchronusly(1));
            }
            
        }

        public void LoadMain()
        {
            SceneManager.LoadScene(0);
        }
        

       public void LoadStartScene()
        {
            
            StartCoroutine(LoadAsynchronusly(sceneToLoad));
            playerStats.ResetPlayerStatistics();
        }

        public void Quit()
        {
            Debug.Log("Quitting game");
            Application.Quit();
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
