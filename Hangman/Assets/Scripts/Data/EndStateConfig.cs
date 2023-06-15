using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EndStateConfig", menuName = "ScriptableObjects/EndStateConfig")]
    public sealed class EndStateConfig : BaseStateConfig
    {
        [SerializeField] private string _matchResultWin;
        [SerializeField] private string _matchResultFail;

        [SerializeField] private string _buttonLabel;
        public string ButtonLabel => _buttonLabel;

        [SerializeField] private string _resultStatus;
        public string ResultStatus => _resultStatus;

        public string GetMatchResult(bool isWin) => isWin ? _matchResultWin : _matchResultFail;
    }
}
