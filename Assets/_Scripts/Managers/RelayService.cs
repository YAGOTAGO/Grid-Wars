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
using UnityEngine.UI;

public class RelayService : NetworkBehaviour
{
    private readonly int _maxPlayers = 1; //Doesn't include the host
    [SerializeField] private TMP_InputField _joinInput; 
    [SerializeField] private GameObject _buttons;
    [SerializeField] private TextMeshProUGUI _joinCodeTMP;
    [SerializeField] private TextMeshProUGUI _loadingTMP;

    private static bool _signedIn = false;
    private string _joinCode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            JoinRelay();
        }
    }

    private async void Start()
    {
        if(!_signedIn)
        {
            await AuthenticatePlayers(); //log in players annonymously
        }
    }

    private static async Task AuthenticatePlayers()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in");
            _signedIn = true;
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    public async void CreateRelay()
    {
        if(!_signedIn) { return; }

        _buttons.SetActive(false);
        _loadingTMP.gameObject.SetActive(true);
        try
        {
            //Show join on successful connection
            NetworkManager.Singleton.OnServerStarted += () => {
                _loadingTMP.gameObject.SetActive(false);
                _joinCodeTMP.gameObject.SetActive(true);
                _joinCodeTMP.text = "Join Code: " + _joinCode + "\n Waiting for player to join...";
            };

            //Make an allocation
            Allocation a = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(_maxPlayers);
            
            //Get the code
            _joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

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
        if (string.IsNullOrEmpty(_joinInput.text) || !_signedIn) { return; }
        
        _buttons.SetActive(false);
        _loadingTMP.gameObject.SetActive(true);
        try
        {
            JoinAllocation joinAllocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(_joinInput.text);
            RelayServerData relayServerData = new(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.OnClientConnectedCallback += (ulong clientId) =>
            {
                Debug.Log("On client connected swapping scene");
                LoadSceneServerRPC();
            };

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            _loadingTMP.gameObject.SetActive(false);
            _buttons.SetActive(true);
            Debug.Log(e);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoadSceneServerRPC()
    {
        //NetworkManager.Singleton.SceneManager.LoadScene("GameBoardScene", LoadSceneMode.Single);
        StartCoroutine(LoadBoardScene());
    }

    private IEnumerator LoadBoardScene()
    {
        yield return new WaitUntil(()=> NetworkManager.Singleton != null);
        NetworkManager.Singleton.SceneManager.LoadScene("GameBoardScene", LoadSceneMode.Single);
    }

}
