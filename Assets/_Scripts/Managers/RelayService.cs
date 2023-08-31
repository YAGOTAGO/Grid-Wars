using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelayService : NetworkBehaviour
{

    private readonly int _maxPlayers = 2; //Doesn't include the host
    [SerializeField] private TMP_InputField _joinInput; 
    [SerializeField] private GameObject _buttons;
    [SerializeField] private TextMeshProUGUI _joinCodeTMP;
    [SerializeField] private TextMeshProUGUI _loadingTMP;
    private async void Start()
    {
        await AuthenticatePlayers(); //log in players annonymously
    }

    private static async Task AuthenticatePlayers()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in");
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        _buttons.SetActive(false);
        _loadingTMP.gameObject.SetActive(true);
        try
        {   
            //Make an allocation
            Allocation a = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(_maxPlayers);
            
            //Get the code
            string joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            //Display and update join code text
            _loadingTMP.gameObject.SetActive(false);
            _joinCodeTMP.gameObject.SetActive(true);
            _joinCodeTMP.text = "Join Code: " + joinCode + "\n Waiting for player to join...";

            //Join the the relay
            RelayServerData relayServerData = new(a, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }
    
    public async void JoinRelay()
    {
        if (string.IsNullOrEmpty(_joinInput.text)) { return; }
        
        _buttons.SetActive(false);
        _loadingTMP.gameObject.SetActive(true);
        try
        {
            JoinAllocation joinAllocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(_joinInput.text);

            RelayServerData relayServerData = new(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            StartCoroutine(WaitForClientConnect());

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    

    //Waits until client has connected and then loads scene for both players
    IEnumerator WaitForClientConnect()
    {
        
        while(!NetworkManager.Singleton.IsConnectedClient)
        {
            Debug.Log("Waiting for connection");
            yield return null;
        }
        _loadingTMP.gameObject.SetActive(false);
        Debug.Log("Connected");

        yield return new WaitForSeconds(.5f); //add a buffer for loading reasons
       
        LoadSceneServerRPC();
    }
 
    [ServerRpc(RequireOwnership = false)]
    private void LoadSceneServerRPC()
    {

        NetworkManager.Singleton.SceneManager.LoadScene("GameBoardScene", LoadSceneMode.Single);

    }
}
