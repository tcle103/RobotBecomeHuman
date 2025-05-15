/*Last modified by: Dale Spence
 * Last modified on: 5 / 15 / 25
 *
 * Basic PlaceHolder Main Menu 
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("test1"); // Replace with your actual gameplay scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit requested"); // Only logs in editor
    }
}
