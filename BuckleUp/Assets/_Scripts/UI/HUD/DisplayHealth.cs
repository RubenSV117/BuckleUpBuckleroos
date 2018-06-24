using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays Health left on the local player
/// 
/// Ruben Sanchez
/// 6/24/18
/// </summary>

public class DisplayHealth : MonoBehaviour
{
    private Text healthText;
    private Health health;
    
	void Start ()
	{
	    healthText = GetComponent<Text>();
	    health = GameManager.Instance.LocalPlayer.Health;
	}
	
	void Update ()
	{
	    healthText.text = "" +  health.currentHealth;
	}
}
