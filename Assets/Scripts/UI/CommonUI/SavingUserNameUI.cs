using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavingUserNameUI : MonoBehaviour
{

    [SerializeField] private InputField userNameField;


    void Awake()
    {
        String userName = PlayerPrefs.GetString("userName");
        if (userName.Trim().Equals(String.Empty)) return;
        //GoToScene();
    }

    public void SaveButtonClicked()
    {
        String userName = userNameField.text;
        if (userName.Length >= 3 && userName.Length <= 20)
        {
            PlayerPrefs.SetString("userName", userName);
            GoToScene();
        }
    }


    private void GoToScene()
    {
        SceneManager.LoadScene("FirstScene");
    }


}
