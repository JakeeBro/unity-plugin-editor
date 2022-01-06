using System.Collections.Generic;
using PlayerCharacter;
using UnityEngine;

namespace SessionSystem
{
    public class Session : MonoBehaviour
    {
        public static Session Instance { get; set; }

        [SerializeField] private List<Player> players;
        private List<int> _invalidID = new List<int>();
        private void Awake()
        {
            InitializeSession();
            
            FindPlayers();
        }

        private void InitializeSession()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void FindPlayers()
        {
            var globalPlayers  = GlobalFunctions.GetPlayers();
            foreach (var player in globalPlayers)
                players.Add(player);
            
            AssignPlayerID();
        }

        private void AssignPlayerID()
        {
            for (var i = 0; i < players.Count; i++)
                if (_invalidID.Contains(players[i].preferredID))
                {
                    players[i].ID = i;
                    _invalidID.Add(players[i].ID);
                }
                else
                {
                    players[i].ID = players[i].preferredID;
                    _invalidID.Add(players[i].ID);
                }
        }
    }
}
