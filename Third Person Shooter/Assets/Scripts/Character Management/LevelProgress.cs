using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress: ScriptableObject
{

    private Text levelText;
    private List<Level> levels;
    private int curLevelIndex;

    public void Init(Text levelText) {

        this.curLevelIndex = 0;
        this.levelText = levelText;

        updateLevelText();

        int sumExp = 0;

        this.levels = new List<Level>();

        for (int i = 0; i < 5; i++)
        {
            sumExp += i + 1;
            Level newLevel = new Level();
            newLevel.requiredExp = sumExp;
            levels.Add(newLevel);
        }
    }

    public bool UpdateLevelIndex(int totalExp) {
        int nextLevelIndex = curLevelIndex + 1;

        if (nextLevelIndex >= levels.Count) {
            return false;
        }

        else{
            Level nextLevel = levels[nextLevelIndex];
            if (totalExp < nextLevel.requiredExp)
            {
                return false;
            }
            else {
                curLevelIndex++;
                updateLevelText();
                return true;
            }
        }
    }

    public int GetCurrentLevel() {
        return curLevelIndex + 1;
    }

    private void updateLevelText() {
        string newText = string.Format(
            "Level: {0}",
            GetCurrentLevel());
        levelText.text = newText;
    }
   
}

public struct Level {
    public int requiredExp;
    //ToDo: Flush out information for level perks
}
