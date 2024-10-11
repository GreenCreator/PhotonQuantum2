using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gm;

    private void Start()
    {
        Debug.Log("Start connect ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        var roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("GameRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room!");
        gm.StartGame();
    }
}