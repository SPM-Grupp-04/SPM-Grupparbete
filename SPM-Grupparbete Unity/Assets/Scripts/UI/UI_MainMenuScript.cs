 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_MainMenuScript : MonoBehaviour
{
    private PlayerStatistics playerStats = PlayerStatistics.Instance;

    [SerializeField] private GameObject firstSelected;
    [SerializeField] private int sceneToLoad = 0;

  

   // [SerializeField] private GameObject settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

  
        // För att senare ladda scenen som vi ska in i
        //SceneManager.LoadScene(playerStats.Scene);

        public void LoadGame()
        {
            if (GlobalControl.Instance.playerStatistics.drillLevel >= 1)
            {
             SceneManager.LoadScene(2);
                
            }
            else
            {
                SceneManager.LoadScene(1);
            }
            
        }

        public void LoadMain()
        {
            SceneManager.LoadScene(0);
        }
        

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
