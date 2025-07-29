using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Singleton;
public class Player_Polar_Bear_Controller : Singleton<Player_Polar_Bear_Controller>/*, IDamageable*/
{
    // // Input fields
    // private PlayerPolarBearActionsAsset _playerPolarBearActionsAsset;
    // private Animator _animator;
    // private InputAction _move;
    //
    // // Movement fields
    // [SerializeField] private float _speed = 8f;
    //
    // // Health
    // public bool _isDead { get; private set; }
    // private int _health = 1;
    //
    // // Attack
    // [SerializeField] private Transform _attackPoint;
    // [SerializeField] private Pool _attackPoolItem;
    // [SerializeField] private float _attackCooldownTime;
    // private bool _canAttack = true;
    //
    // // Power
    // [SerializeField] private Power _powerScript;
    //
    // LevelController _levelController => LevelController.I;
    // private PoolManager _poolManager => PoolManager.I;
    //
    // protected override void Awake()
    // {
    //     base.Awake();
    //     
    //     _animator = GetComponent<Animator>();
    //     _playerPolarBearActionsAsset = new Player_Polar_Bear();
    //     _move = _playerPolarBearActionsAsset.Player.Move;
    // }
    //
    // private void Start()
    // {
    //     _levelController.beginLevelEvent += EnableInputs;
    //
    //     _levelController.timeUpEvent += DisableInputs;
    //     _levelController.pauseEvent += DisableInputs;
    //     
    //     _levelController.blessingsRandomizedEvent += () => SetPower(_levelController._levelPolarBearBlessing);
    // }
    //
    // private void FixedUpdate()
    // {
    //     Movement();
    // }
    //
    // #region Movement
    //
    // private void Movement()
    // {
    //     float speedX = _move.ReadValue<Vector2>().x;
    //     MovementAnimationControl(speedX);
    //     this.transform.position += new Vector3(speedX, 0) * _speed * Time.deltaTime;
    //     BodyRotate(speedX);
    // }
    //
    // private void BodyRotate(float speedX)
    // {
    //     if (speedX > 0f)
    //         this.transform.localScale = new Vector3(1, 1, 1);
    //     else if(speedX < 0f)
    //         this.transform.localScale = new Vector3(-1, 1, 1);
    // }
    //
    // #endregion
    //
    // #region Animation
    //
    // private void MovementAnimationControl(float speedX)
    // {
    //     _animator.SetBool("Walking", (speedX == 0 ? false : true));
    // }
    //
    // private void AnimationTrigger(string trigger)
    // {
    //     _animator.SetTrigger(trigger);
    // }
    //
    // #endregion
    //
    // #region Input
    //
    // public void EnableInputs()
    // {
    //     _playerPolarBearActionsAsset.Player.Power.started += DoPowerControl;
    //     _playerPolarBearActionsAsset.Player.Attack.started += DoAttackControl;
    //
    //     _playerPolarBearActionsAsset.Player.Enable();
    // }
    //
    // public void DisableInputs()
    // {
    //     _playerPolarBearActionsAsset.Player.Power.started -= DoPowerControl;
    //     _playerPolarBearActionsAsset.Player.Attack.started -= DoAttackControl;
    //
    //     _playerPolarBearActionsAsset.Player.Disable();
    // }
    //
    // #endregion
    //
    // #region Power
    //
    // private void DoPowerControl(InputAction.CallbackContext obj)
    // {
    //     _powerScript.UsePower();
    // }
    //
    // #endregion
    //
    // #region Health
    //
    // public void ModifyHealth(int value)
    // {
    //     if (_isDead)
    //     {
    //         return;
    //     }
    //
    //     _health += value;
    //
    //     if (_health <= 0)
    //     {
    //         _isDead = true;
    //         TimeCountManager.I.SetTimeUp();
    //     }
    // }
    //
    // #endregion
    //
    // #region Attack 
    //
    // private void DoAttackControl(InputAction.CallbackContext obj)
    // {
    //     if (!_canAttack)
    //     {
    //         return;
    //     }
    //     _canAttack = false;
    //     AnimationTrigger("Attack");
    //     StartCoroutine(AttackCoolDown());
    // }
    //
    // public void SpawnSnowball()
    // {
    //     GameObject snowball = _poolManager.GetObject(_attackPoolItem.tagPool, _attackPoint.position, Quaternion.identity);
    // }
    //
    // private IEnumerator AttackCoolDown()
    // {
    //     yield return new WaitForSeconds(_attackCooldownTime);
    //
    //     _canAttack = true;
    // }
    //
    // #endregion
    //
    // #region Set
    // private void SetPower(Power power)
    // {
    //     _powerScript = power;
    // }
    //
    // #endregion
    //
    // #region Get
    // public Power GetPower()
    // {
    //     return _powerScript;
    // }
    //
    // #endregion
}
