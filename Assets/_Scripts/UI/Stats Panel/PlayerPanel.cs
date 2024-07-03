using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : Singleton<PlayerPanel>
{
    public void SetPlayerUI(GameObject playerUI)
    {
        playerUI.transform.SetParent(transform);
    }
}
