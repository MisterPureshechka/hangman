using Core;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    public static class BaseStateExt
    {
        public static BaseState Change(this BaseState currentState, BaseState newState, BaseState.Data data)
        {
            currentState?.OnEnd();
            newState.OnStart(data);
            return newState;
        }
    }

    public abstract class BaseState 
    {
        public class Data
        {
            private readonly MainScreen _mainScreen;
            public MainScreen MainScreen => _mainScreen;

            private readonly string _screenName;
            public string ScreenName => _screenName;

            public Data(MainScreen mainScreen, string screenName)
            {
                _mainScreen = mainScreen;
                _screenName = screenName;
            }
        }

        [SerializeField] protected Text _screenNameTextArea;

        public virtual void OnStart(Data data) => _screenNameTextArea.text = data.ScreenName;

        public abstract void OnEnd();
    }
}

