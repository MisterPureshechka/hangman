using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "RulesStateConfig", menuName = "ScriptableObjects/RulesStateConfig")]
    public sealed class RulesStateConfig : BaseStateConfig
    {
        [TextArea(5, 15)]
        [SerializeField] private string _rules;
        public string Rules => _rules;

        [SerializeField] private string _buttonLabel;
        public string ButtonLabel => _buttonLabel;
    }
}
