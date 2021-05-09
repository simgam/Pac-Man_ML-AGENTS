using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public Button NewGame,Exit;
    // Start is called before the first frame update
    void Start()
    {
        NewGame.onClick.AddListener(TaskOnClick);
        Exit.onClick.AddListener(EscOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("Livello_Player");
    }

    void EscOnClick()
    {
        Application.Quit();
    }
}
