using System;
using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Factory;
using Core.Handlers;
using Core.Interfaces;
using Core.Path;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Objects
{
    public class Unit : Entity, IProduct, IMobile, IAttackable, IAttacker
    {
        IEnumerator _movement;
        IEnumerator _attack;

        bool _isMooving = false;
        bool _isAlive = true;

        int _damage;
        int _attackRange;
        float _attackCoolDown;
        float _attackCoolDownTimer = float.PositiveInfinity;
        int _movementSpeed;
        Color _selectedColor;

        public bool IsMooving => _isMooving;
        public bool IsAlive => _isAlive;
        protected override void Awake()
        {
            base.Awake();
            if (_entityInfo is UnitInfo unitInfo)
            {
                _damage = unitInfo.damage;
                _attackRange = unitInfo.attackRange;
                _attackCoolDown = unitInfo.attackCooldown;
                _movementSpeed = unitInfo.movementSpeed;
                _selectedColor = unitInfo.selectedColor;
            }
            else
            {
                Debug.LogError("Set valid entity info! - Unit Info");
            }
        }

        public void SetSelectedColor(bool status)
        {
            if (status)
                _spriteRenderer.color = _selectedColor;
            else
                _spriteRenderer.color = Color.white;
        }

        public void Move(Vector2Int destination)
        {
            var destinationPath = Pathfinding.Instance.FindPath(transform.position, (Vector2)destination);

            if (destinationPath != null && destinationPath.Count > 1)
            {
                destinationPath.RemoveAt(0);
            }
            Move(_movementSpeed, 0, destinationPath);
        }

        private void Move(float speed, int currentPathIndex, List<Vector3> pathVectorList)
        {
            if (_movement != null) StopCoroutine(_movement);
            var movementHandler = new MovementHandler(speed, currentPathIndex, pathVectorList, () => { StopMovement(); });
            _movement = movementHandler.Movement(transform);
            _isMooving = true;
            StartCoroutine(_movement);
        }

        private void StopMovement()
        {
            _isMooving = false;
        }

        public void Attack(IAttackable attackable)
        {
            if (attackable == null) return;
            if (_attack != null) StopCoroutine(_attack);
            _attack = AttackCoroutine(attackable);
            StartCoroutine(_attack);
        }

        public void StopAttack()
        {
            if (_attack != null) StopCoroutine(_attack);
        }

        IEnumerator AttackCoroutine(IAttackable attackable)
        {
            yield return new WaitUntil(() => _isMooving == false);
            while (attackable != null)
            {
                if (!attackable.IsAlive) break;
                if (_attackCoolDownTimer >= _attackCoolDown)
                {
                    Vector2Int attackableSize = attackable.GetSize();
                    Vector2 attackablePosition = attackable.GetPosition();
                    var grid = Pathfinding.Instance.Grid;

                    attackable = AttackHandler.Attack(transform, attackable, _damage, grid, _attackRange, attackableSize, attackablePosition);
                    _attackCoolDownTimer = 0;

                    Debug.Log("Attack! - " + attackable);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    _attackCoolDownTimer += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
            yield return null;
        }

        public IAttackable GetDamage(int damage)
        {
            _currentHealth = _currentHealth - damage >= 0 ? _currentHealth - damage : 0;
            if (_currentHealth == 0)
            {
                Die();
                return null;
            }
            return this;
        }

        public void Die()
        {
            _isAlive = false;
            UnitFactory.Instance.RefuseProduct(_name, this);
            GameController.Instance.GridController.RelaseOccupition(GetPosition(), _width, _height);
        }

        public Vector2Int GetSize()
        {
            return new Vector2Int(_width, _height);
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }
}
