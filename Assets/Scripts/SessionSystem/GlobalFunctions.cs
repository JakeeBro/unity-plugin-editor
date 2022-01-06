using PlayerCharacter;
using Unity.VisualScripting;
using UnityEngine;

namespace SessionSystem
{
    public static class GlobalFunctions
    {
        public static GameObject GetPlayer(int index)
        {
            var players = Object.FindObjectsOfType<Player>();
            return players[index].gameObject;
        }

        public static Player[] GetPlayers()
        {
            var players = Object.FindObjectsOfType<Player>();
            return players;
        }

        public static Player CastToPlayer(Object obj)
        {
            if (obj.GetComponent<Player>())
                return obj.GetComponent<Player>();
            return null;
        }
    }
}
