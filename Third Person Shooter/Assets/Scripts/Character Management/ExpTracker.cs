using UnityEngine;
using UnityEngine.UI;

public class ExpTracker: MonoBehaviour
{
    private Text uiExpText;
    private int totalExperienceEarned;
    private LevelProgress levels;

    public void Init(Text expText, Text levelText) {

        this.totalExperienceEarned = 0;
        this.uiExpText = expText;

        updateExpText();

        this.levels = ScriptableObject.CreateInstance(typeof(LevelProgress)) as LevelProgress;
        this.levels.Init(levelText);
    }
    

    public void ModifyExperience(int changeValue)
    {
        totalExperienceEarned += changeValue;
        updateExpText();
        levels.UpdateLevelIndex(totalExperienceEarned);
    }

    public int GetTotalExperience()
    {
        return totalExperienceEarned;
    }

    public int CheckLevel() {
        return levels.GetCurrentLevel();
    }

    public LevelProgress GetLevelProgress() {
        return levels;
    }

    private void updateExpText()
    {
        string newText = string.Format("Total Experience: {0}", totalExperienceEarned);
        uiExpText.text = newText;
    }

}
