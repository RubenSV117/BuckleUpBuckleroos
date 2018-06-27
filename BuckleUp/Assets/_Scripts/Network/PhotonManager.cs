using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages photon networking
/// 
/// Ruben Sanchez
/// 6/26/18
/// </summary>

public class PhotonManager : Photon.PunBehaviour
{
    public static PhotonManager Instance;

    [SerializeField] private GameObject player;

    private string versionName = "0.1"; // game that players will be connecting to
    

	void Awake ()
	{
	    if (Instance == null)
	    {
	        DontDestroyOnLoad(this);
	        Instance = this;
        }

        else
	        Destroy(gameObject);
	    

	    PhotonNetwork.automaticallySyncScene = true; // have all the players in the same room load the same scene
	    PhotonNetwork.ConnectUsingSettings(versionName);

	    SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TopDown")
            PhotonNetwork.Instantiate(player.name, player.transform.position, player.transform.rotation, 0);
    }

    #region Photon Overrides

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        print("@@@@@@@@@@@@@@@@joined lobby");
    }

    public override void OnDisconnectedFromPhoton()
    {
        print("@@@@@@@@@@@Disconnected From Photon");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("TopDown");
    }

    #endregion

    #region Public UI methods

    public void JoinRoom(InputField nameField)
    {
        if (nameField.text.Length > 0)
            PhotonNetwork.JoinOrCreateRoom(nameField.text, new RoomOptions() {MaxPlayers = 8}, TypedLobby.Default);
    }

    public void CreateRoom(InputField nameField)
    {
        if (nameField.text.Length > 0)
            PhotonNetwork.CreateRoom(nameField.text, new RoomOptions() {MaxPlayers = 8}, null);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    

    #endregion


}
