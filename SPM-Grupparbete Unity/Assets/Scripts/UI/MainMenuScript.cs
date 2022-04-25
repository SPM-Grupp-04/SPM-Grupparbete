using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    private PlayerStatistics playerStats = PlayerStatistics.Instance;

    [SerializeField] private GameObject startButton;
    [SerializeField] private int sceneToLoad = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton);
    }

  
        // För att senare ladda scenen som vi ska in i
        //SceneManager.LoadScene(playerStats.Scene);



       public void LoadStartScene()
        {
            
            SceneManager.LoadScene(sceneToLoad); 
            Debug.Log("Just nu laddar vi bara in i Scene på INDEX som vi sätter i inspectorn.");
        }

        public void Quit()
        {
            Debug.Log("Quitting game");
            Application.Quit();
        }
    
}
