using Core;

using Data;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    public static class BaseStateExt
    {
        public static BaseState Change(this BaseState currentState, BaseState newState, BaseState.Data data = null)
        {
            currentState?.OnEnd();
            newState.OnStart(data);
            return newState;
        }
    }

    public abstract class BaseState 
    {
        public class Data { }

        [SerializeField] protected Text _screenNameTextArea;
        [SerializeField] private GameObject _rootGO;

        protected MainScreen _mainScreen;

        protected abstract BaseStateConfig Config { get; }

        public void Init(MainScreen mainScreen) => _mainScreen = mainScreen;

        public virtual void OnStart(Data data)
        {
            _screenNameTextArea.text = Config.ScreenName;

            _rootGO.SetActive(true);
        }

        public virtual void OnEnd() => _rootGO.SetActive(false);
    }
}

