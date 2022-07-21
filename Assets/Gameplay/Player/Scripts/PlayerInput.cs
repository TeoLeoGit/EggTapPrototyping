using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public int playerId;
    public string playerName;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerManager>().AddPlayerToLeaderboard(playerId, playerName);
    }

}
