﻿using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using EnemyStates;
using UnityEngine;
using UserInterface;

namespace EnemyControllers
{
	public class EnemyController : PhysicsObject
	{
		[Tooltip("The amount to be added to the high score when this enemy is killed")]
		public int pointValue = 10;
		
		[Tooltip("The list of states for this enemy. It MUST have the names of the state classes")]
		public StateHolder[] states;

		protected SpriteRenderer _spriteRenderer;
	
		protected Animator _animator;

		protected Collider2D _collider;
	
		protected int _direction;

		protected int _movementStop;
	
		protected IDictionary<States, IState> _stateObjects;
	
		protected States _currState;

		protected float _speed;

		protected bool _isPaused;

		protected bool _isDead;
		
		private bool _canAttack;

		protected int _lastMovingDirection;

		private IList<GameObject> _childObjects;

		private GameController _gameController;
		
	
		protected override void Init ()
		{
			_gameController = FindObjectOfType<GameController>();
			
			_movementStop = 1;
			
			_spriteRenderer = GetComponent<SpriteRenderer> ();

			if (gameObject.HasComponent<Animator>())
			{
				_animator = GetComponent<Animator>();
			}
			else
			{
				_animator = GetComponentInChildren<Animator>();
			}

			_collider = GetComponent<Collider2D>();
			
			_childObjects = new List<GameObject>();

			ChildAwake();
		}
		
		protected virtual void ChildAwake() {}

		public void OnPause()
		{
			_isPaused = true;
		
			foreach (var state in _stateObjects.Values)
			{
				state.OnPause();
			}

			_animator.enabled = false;
			_collider.enabled = false;
		}

		public void OnPlay()
		{
			_isPaused = false;
		
			foreach (var state in _stateObjects.Values)
			{
				state.OnPlay();
			}
		
			_animator.enabled = true;
			_collider.enabled = true;
		}

		public virtual bool IsPlayerWithinStoppingDistance()
		{
			return false;
		}

		public void ChangeState(States state)
		{
			_currState = state;
		}

		public void SetDirection(int direction)
		{
			if (direction != 0)
			{
				_lastMovingDirection = direction;
			}

			_direction = direction;
		}
		
		public void SetCanAttack(bool canAttack)
		{
			_canAttack = canAttack;
		}

		public bool CanAttack()
		{
			return _canAttack;
		}

		public void SetMovementStop(int movement)
		{
			_movementStop = movement;
		}
		
		public int GetDirection()
		{
			return _lastMovingDirection;
		}

		public void SetSpeed(float speed)
		{
			_speed = speed;
		}

		public virtual void DealDamage(int damage)
		{
		}

		public bool HasDied()
		{
			return _isDead;
		}

		public void MarkingAsDead()
		{
			_gameController.UpdateScore(pointValue);
			MarkAsDead();
		}

		public void MarkAsDead()
		{
			foreach (var state in _stateObjects.Values)
			{
				state.Delete();
			}
			_isDead = true;
		}
	}
}
