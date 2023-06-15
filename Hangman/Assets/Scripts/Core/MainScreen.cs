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

        public void ChangeToRulesState() =>
            _currentState = _currentState.Change(_rulesState, new BaseState.Data());

        public void ChangeToGameState()
        {
            var data = new GameState.Data("someWord");
            _currentState = _currentState.Change(_gameState, data);
        }

        public void ChangeToEndState(bool isWin, int wins, int fails)
        {
            var data = new EndState.Data(isWin, wins, fails);
            _currentState = _currentState.Change(_endState, data);
        }

        private void Start()
        {
            _rulesState.Init(this);
            _gameState.Init(this);
            _endState.Init(this);

            ChangeToRulesState();
        }
    }
}