using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MultiplayerScriptableObjetc", order = 1)]
    public class MultiplayerSO : ScriptableObject
    {
        public bool IsHost;

    }
}