using TMPro;
using Unity.Collections;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private Name nameScript;

    private void Awake()
    {
        nameScript.playerName.OnValueChanged += ChangeText;
    }

    private void Start()
    {
        ChangeText("", nameScript.playerName.Value);
    }

    private void ChangeText(FixedString64Bytes oldString, FixedString64Bytes newString)
    {
        textUI.text = newString.ToString();
    }
}