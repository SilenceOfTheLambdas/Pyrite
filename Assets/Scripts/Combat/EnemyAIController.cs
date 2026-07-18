using System;
using UnityEngine;
using UnityEngine.AI;

namespace Combat
{
    [RequireComponent(typeof(Combatant))]
    [RequireComponent(typeof(EnemyStats))]
    public class EnemyAIController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform target;
        
        private Combatant _combatant;
        private EnemyStats _stats;
        private NavMeshAgent _agent;

        private Vector3 _spawnPosition;
        private float _nextAttackTime;
        
        public CombatState CurrentState { get; private set; } = CombatState.Idle;

        private void Awake()
        {
            _combatant = GetComponent<Combatant>();
            _stats = GetComponent<EnemyStats>();
            _agent = GetComponent<NavMeshAgent>();
            
            _spawnPosition = transform.position;
        }

        private void Start()
        {
            _combatant.SetMaxHealth(_stats.maxHealth);

            if (_agent != null)
            {
                _agent.speed = _stats.moveSpeed;
                _agent.stoppingDistance = _stats.attackRange * 0.9f;
            }

            _combatant.OnDeath += HandleDeath;
        }

        private void OnDestroy()
        {
            if (_combatant != null)
                _combatant.OnDeath -= HandleDeath;
        }

        private void Update()
        {
            if (!_combatant.IsAlive) return;

            switch (CurrentState)
            {
                case CombatState.Idle:
                    UpdateIdle();
                    break;
                case CombatState.Chasing:
                    UpdateChasing();
                    break;
                case CombatState.Attacking:
                    UpdateAttacking();
                    break;
                case CombatState.Returning:
                    UpdateReturning();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateIdle()
        {
            Transform player = FindPlayerInAggroRange();

            if (player == null) return;

            target = player.transform;
            SetState(CombatState.Chasing);
        }

        private void UpdateChasing()
        {
            if (target == null)
            {
                SetState(CombatState.Returning);
                return;
            }

            if (IsOutsideLeashRadius())
            {
                target = null;
                SetState(CombatState.Returning);
                return;
            }

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= _stats.attackRange)
            {
                StopMoving();
                SetState(CombatState.Attacking);
                return;
            }

            MoveTo(target.position);
        }

        private void UpdateAttacking()
        {
            if (target == null)
            {
                SetState(CombatState.Returning);
                return;
            }

            if (IsOutsideLeashRadius())
            {
                target = null;
                SetState(CombatState.Returning);
                return;
            }
            
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > _stats.attackRange)
            {
                SetState(CombatState.Chasing);
                return;
            }
            
            StopMoving();
            FaceTarget(target.position);

            if (Time.time >= _nextAttackTime)
            {
                AttackTarget();
                _nextAttackTime = Time.time + _stats.attackCooldown;
            }
        }

        private void UpdateReturning()
        {
            target = null;
            float distanceToSpawn = Vector3.Distance(transform.position, _spawnPosition);

            if (distanceToSpawn <= 0.5f)
            {
                StopMoving();
                SetState(CombatState.Idle);
                return;
            }
            
            MoveTo(_spawnPosition);
        }

        private Transform FindPlayerInAggroRange()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                return null;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > _stats.aggroRadius) return null;
            
            Combatant playerCombatant = player.GetComponent<Combatant>();
            
            if (playerCombatant == null || !playerCombatant.IsAlive) return null;
            
            return player.transform;
        }

        private void AttackTarget()
        {
            Combatant targetCombatant = target.GetComponent<Combatant>();

            if (targetCombatant == null || !targetCombatant.IsAlive) return;

            DamageInfo damageInfo = new DamageInfo
            {
                Source = gameObject,
                Target = target.gameObject,
                PhysicalDamage = _stats.physicalDamage,
                ElementalDamage = _stats.elementalDamage,
                CanCrit = true,
                CritChance = _stats.criticalChance,
                CritMultiplier = _stats.criticalDamageMultiplier
            };
            
            targetCombatant.TakeDamage(damageInfo);
        }

        private void SetState(CombatState newState)
        {
            CurrentState = newState;
        }

        private bool IsOutsideLeashRadius()
        {
            return Vector3.Distance(transform.position, _spawnPosition) > _stats.leashRadius;
        }

        private void MoveTo(Vector3 destination)
        {
            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.isStopped = false;
                _agent.SetDestination(destination);
                return;
            }
            
            Vector3 direction = destination - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.001f) return;
            
            direction.Normalize();
            transform.position += direction * _stats.moveSpeed * Time.deltaTime;
            FaceTarget(destination);
        }

        private void StopMoving()
        {
            if (_agent != null && _agent.isOnNavMesh)
                _agent.isStopped = true;
        }
        
        private void FaceTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;
            
            if (direction.sqrMagnitude <= 0.001f) return;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _stats.rotationSpeed * Time.deltaTime
            );
        }
        
        private void HandleDeath(Combatant deadCombatant)
        {
            StopMoving();
            SetState(CombatState.Dead);

            Collider enemyCollider = GetComponent<Collider>();

            if (enemyCollider != null)
                enemyCollider.enabled = false;
            
            if (_agent != null)
                _agent.enabled = false;
        }
    }
}