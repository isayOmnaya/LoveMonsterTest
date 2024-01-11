using UnityEngine;
using TMPro;
using System.IO;

public class GuiManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]
    TMP_Text _roundDurationText = null;

    [SerializeField]
    TMP_Text _currentRoundText = null;

    [SerializeField]
    TMP_Text _monsterCountText = null;

    [SerializeField]
    GameObject _roundStats = null;

    [SerializeField]
    GameObject _pauseMenu = null;

    public TMP_Text RoundStatsText = null;

    [Header("Data")]
    string _savePath = "RoundDatas.json";
    
    public void UpdateRoundDuration(float roundDuration)
    {
        // Format the duration into minutes and seconds
        int minutes = Mathf.FloorToInt(roundDuration / 60F);
        int seconds = Mathf.FloorToInt(roundDuration % 60F);

        _roundDurationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateRound(int currentRound)
    {
        _currentRoundText.text = currentRound.ToString();
    }

    public void UpdateMonsterCount(int monsterCount)
    {
        _monsterCountText.text = monsterCount.ToString();
    }

    public void PauseMenu()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _roundStats.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _roundStats.SetActive(false);
    }

    public void TryCloseGame()
    {
        Application.Quit();
    }

    public void OpenLevelStats()
    {
        RoundStatsText.text = ""; // clear the data first
        _roundStats.SetActive(true);
        _pauseMenu.SetActive(false);
        LoadRoundData();
    }  

    public void LoadRoundData()
    {

        string saveFilePath = Path.Combine(Application.persistentDataPath, _savePath);
        if (File.Exists(saveFilePath))
        {
            // Read JSON file content
            string jsonData = File.ReadAllText(saveFilePath);

            // Deserialize JSON data
            RoundDataList roundDataList = JsonUtility.FromJson<RoundDataList>(jsonData);

            // Display each round's data on a new line
            foreach (RoundData roundData in roundDataList.roundsData)
            {
                string line = $"Round: {roundData.CurrentRound}, Duration: {roundData.RoundDuration:F2}, Monsters: {roundData.MonstersSpawned}";
                RoundStatsText.text += line + "\n";
            }
        }
        else
        {
            RoundStatsText.text = "No saved data found.";
        }
    }
}
