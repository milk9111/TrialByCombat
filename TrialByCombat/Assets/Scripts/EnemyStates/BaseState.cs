using UnityEngine;

namespace EnemyStates
{
    public class BaseState : MonoBehaviour, IState
    {
        protected EnemyController _controller;
        protected Animator _animator;

        public virtual void Init() { }
        
        public virtual void DoAction() { }

        public void SetupFields(EnemyController controller, Animator animator)
        {
            _controller = controller;
            _animator = animator;
        }
    }
}