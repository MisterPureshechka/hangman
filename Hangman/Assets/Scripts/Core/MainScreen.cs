using FSM.States;

using UnityEngine;

namespace Core
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private RulesState _rulesState;
        [SerializeField] private GameState _gameState;
        [SerializeField] private EndState _endState;

        private BaseState _currentState;

        public void ChangeToRulesState()
        {
            var data = new RulesState.Data(this, "someName", "SomeRules", "SomeButton");
            _currentState = _currentState.Change(_rulesState, data);
        }

        public void ChangeToGameState()
        {
            var data = new GameState.Data(this, "someName", "someWord");
            _currentState = _currentState.Change(_gameState, data);
        }

        public void ChangeToEndState()
        {
            var data = new EndState.Data(this, "someName", true);
            _currentState = _currentState.Change(_endState, data);
        }

        private void Start() => ChangeToRulesState();
    }
}