using UnityEngine;
using Utils.Singleton;

public class GameManager : DontDestroySingleton<GameManager>
{
    private int currentLevel = 0;
    private int lastLevel = 4;
    
    public void ChangeLevel(int nextLevel)
    {
        if (nextLevel > lastLevel)
        {
            Debug.Log("Need to implement what happens when end oof last level here!");
            // Not implemented yet
            return;
        }
        currentLevel = nextLevel;
    }
}
