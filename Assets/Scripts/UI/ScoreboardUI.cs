using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject scoreboardEntryPrefab;
    private void Start()
    {
        KillTracker.Instance.PlayerKills.OnListChanged += UpdateScoreboard;
    }
    
    private void UpdateScoreboard(NetworkListEvent<int> changeEvent)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        var scoreboardData = new List<(string, int)>();
        for (int i = 0; i < KillTracker.Instance.PlayerKills.Count; i++)
        {
            string playerName = KillTracker.Instance.PlayerNames[i].ToString();
            int playerKills = KillTracker.Instance.PlayerKills[i];
            scoreboardData.Add((playerName, playerKills));
        }

        var sortedScoreboardData = scoreboardData.OrderByDescending(data => data.Item2).ToList();

        for (int i = 0; i < sortedScoreboardData.Count; i++)
        {
            GameObject entry = Instantiate(scoreboardEntryPrefab, transform);
            entry.transform.position += new Vector3(0, i * -15);

            entry.GetComponent<TextMeshProUGUI>().text = $"{i+1}. {sortedScoreboardData[i].Item1}: {sortedScoreboardData[i].Item2}";
        }
    }
}
