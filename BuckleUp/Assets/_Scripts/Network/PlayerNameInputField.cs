using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Gets and records Player name
/// 
/// Ruben Sanchez
/// 6/25/18
/// </summary>

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    private static string playerPrefNameKey = "PlayerName";
    private InputField inputField;

	void Start ()
	{
	    inputField = GetComponent<InputField>();

	    string playerName = "";

        // if a name has been previously created, use that as a default value
	    if (PlayerPrefs.HasKey(playerPrefNameKey))
	    {
	        playerName = PlayerPrefs.GetString(playerPrefNameKey);
	        inputField.text = playerName;
	    }
	        
	    PhotonNetwork.playerName = playerName;
	}

    public void SetPlayerName(string name)
    {
        PhotonNetwork.playerName = name + " ";
        PlayerPrefs.SetString(playerPrefNameKey, name);
    }
}
