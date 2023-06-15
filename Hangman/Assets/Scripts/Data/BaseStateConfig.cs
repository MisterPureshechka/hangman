using UnityEngine;

namespace Data
{
    public abstract class BaseStateConfig : ScriptableObject
    {
        [SerializeField] private string _screenName;
        public string ScreenName => _screenName;
    }
}
