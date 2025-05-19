using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleStats : MonoBehaviour
{
    private string filePath;
    
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/puzzle_data.txt";
        filePath = GetUniqueFilePath(Application.persistentDataPath, filePath, ".txt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PuzzleUpdate(float timeTaken, int fails)
    {
        System.IO.File.AppendAllText(filePath, "Puzzle solved in " + timeTaken + " seconds with " + fails + " fails.\n");
    }
    
    public void FrameRateUpdate(float frameRate)
    {
        System.IO.File.AppendAllText(filePath, "Frame rate: " + frameRate + "\n");
    }
    
    private string GetUniqueFilePath(string directory, string pathName, string extension)
    {
        var fileName = Path.GetFileNameWithoutExtension(pathName) + "_";
        string candidate;
        int counter = 0;

        do {
            candidate = Path.Combine(directory, $"{fileName}{counter}{extension}");
            counter++;
        } while (File.Exists(candidate));

        return candidate;
    }
}
