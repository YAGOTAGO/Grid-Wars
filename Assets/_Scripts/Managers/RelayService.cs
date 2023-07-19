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

    private readonly int _maxPlayers = 1; //Doesn't include the host
    [SerializeField] private TMP_InputField _joinInput;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private TextMeshProUGUI _joinCodeTMP;
    [SerializeField] private TextMeshProUGUI _loadingTMP;
    private async void Start()
    {
        await AuthenticatePlayers();
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
            string _joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            //Display and update join code text
            _loadingTMP.gameObject.SetActive(false);
            _joinCodeTMP.gameObject.SetActive(true);
            _joinCodeTMP.text = "Join Code: " + _joinCode;

            //Join the the relay
            RelayServerData relayServerData = new(a, "dtls");
            NetworkManager.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.StartHost();
            
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
        
        try
        {
            JoinAllocation joinAllocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(_joinInput.text);

            RelayServerData relayServerData = new(joinAllocation, "dtls");
            NetworkManager.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.StartClient();
            StartCoroutine(WaitForClientConnect());
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator WaitForClientConnect()
    {
       
        while(!NetworkManager.Singleton.IsConnectedClient)
        {
            Debug.Log("Waiting for connection");
            yield return null;
        }
        Debug.Log("Connected");
       
        LoadSceneServerRPC();
    }
 
    [ServerRpc(RequireOwnership = false)]
    private void LoadSceneServerRPC()
    {

        NetworkManager.SceneManager.LoadScene("GameBoardScene", LoadSceneMode.Single);
        
    }
}
