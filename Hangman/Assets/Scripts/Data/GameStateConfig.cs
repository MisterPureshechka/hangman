using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameStateConfig", menuName = "ScriptableObjects/GameStateConfig")]
    public sealed class GameStateConfig : BaseStateConfig
    {
        [SerializeField] private string _resultStatus;
        public string ResultStatus => _resultStatus;
    }
}
