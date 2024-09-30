using UnityEngine;
using UnityEngine.Events;
using Movement.Data;
using Movement.Events;

namespace Movement.Provider
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementProvider : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent<PlayerMoveEvent> OnMove;
        public UnityEvent OnLand;

        [SerializeField]
        protected MovementData _data;

        // fields
		protected Vector2 _moveDir;
		protected float _speed;
		protected float _verticalVelocity;
		protected float _terminalVelocity = -20.0f;
		protected bool _prevGrounded;

        // Reference
        protected CharacterController _characterController;

		// Properties
		public Vector2 MoveDirection 
		{ 
			get
			{
				return _moveDir;
			}
			set 
			{
				_moveDir = value;
			}
		}

		public float VerticalVelocity
		{
			get
			{
				return _verticalVelocity;
			}
			set
			{
				_verticalVelocity = value;
			}
		}

        protected void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {
			UpdateGravity();
			GroundedCheck();
            Move();
        }

        protected void Move()
		{
			// 간단한 가속 및 감속 처리, 쉽게 제거, 대체 또는 반복 가능하도록 설계

			// 참고: Vector2의 == 연산자는 근사치를 사용하여 부동 소수점 오류가 발생하지 않으며, magnitude보다 저렴함
			// 입력이 없으면 목표 속도를 0으로 설정
            float targetSpeed = _moveDir == Vector2.zero ? 0.0f : _data.MoveSpeed;

			// 플레이어의 현재 수평 속도에 대한 참조
			float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

			float speedOffset = 0.1f;

			// 목표 속도에 가속하거나 감속
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// 선형 결과가 아닌 곡선 결과를 만들어 보다 자연스러운 속도 변화를 제공
				// 참고: Lerp의 T는 제한되어 있으므로 속도를 제한할 필요 없음
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * _data.SpeedChangeRate);

				// 속도를 소수점 3자리로 반올림
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// 입력 방향을 정규화
			Vector3 inputDirection = new Vector3(_moveDir.x, 0.0f, _moveDir.y).normalized;

			// 참고: Vector2의 != 연산자는 근사치를 사용하여 부동 소수점 오류가 발생하지 않으며, magnitude보다 저렴함
			// 이동 입력이 있을 경우 플레이어가 이동할 때 회전
			if (_moveDir != Vector2.zero)
			{
				// 이동
				inputDirection = transform.right * _moveDir.x + transform.forward * _moveDir.y;
			}

			// 플레이어 이동
            Vector3 displacement = inputDirection.normalized * _speed + new Vector3(0.0f, _verticalVelocity, 0.0f);
			_characterController.Move(displacement * Time.deltaTime);
            OnMove.Invoke(new PlayerMoveEvent(_speed, _verticalVelocity, _moveDir, _data.Grounded));
		}

		protected void UpdateGravity()
		{
            if (_data.Grounded)
			{
				// 지면에 닿아 있을 때 속도가 무한히 떨어지는 것을 방지
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				if (!_prevGrounded)
				{
					OnLand.Invoke();
				}
            }

            // 터미널 속도 이하일 경우 시간이 지남에 따라 중력 적용 (시간에 따라 선형적으로 가속되도록 두 번 델타 타임 곱함)
            if (_verticalVelocity > _terminalVelocity)
            {
                _verticalVelocity += _data.GravityAcceleration * Time.deltaTime;
            }
		}

		protected void GroundedCheck()
		{
			// 오프셋을 적용한 구체 위치 설정
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _data.GroundedOffset, transform.position.z);
			_prevGrounded = _data.Grounded;
			_data.Grounded = Physics.CheckSphere(spherePosition, _data.GroundedRadius, _data.GroundLayers, QueryTriggerInteraction.Ignore);
		}

		protected void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (_data.Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// 선택된 상태에서 지면 콜라이더의 위치와 반지름을 반영하는 기즈모를 그림
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _data.GroundedOffset, transform.position.z), _data.GroundedRadius);
		}
    }
}