/*Last modified by: Dale Spence
 * Last modified on: 5 / 15 / 25
 *
 * Basic PlaceHolder Main Menu 
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha9)) // dev key to delete save files
        {
            var saveSystem = FindObjectOfType<SaveSystem>();
            saveSystem.DeleteFiles();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) //dev key to delete all files in persistent data path
        {
            var path = Application.persistentDataPath;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
    
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
