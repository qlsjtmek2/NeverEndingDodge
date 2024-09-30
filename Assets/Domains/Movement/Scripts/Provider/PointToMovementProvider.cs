using UnityEngine;
using UnityEngine.Events;
using Movement.Data;
using Movement.Events;

namespace Movement.Provider
{
    [RequireComponent(typeof(CharacterController))]
    public class PointToMovementProvider : MovementProvider
    {
		// fields
        private Vector3 _targetPosition;
        private bool _isMovingToTarget = false;

        protected override void Update()
        {
			UpdateGravity();
			GroundedCheck();

            // 목표 위치로 이동 중일 때 처리
            if (_isMovingToTarget)
            {
                MoveToTarget();
            }
            else
            {
                Move();
            }
        }

        // 새로운 MoveToTarget method for moving towards a target position
        private void MoveToTarget()
        {
            Vector3 currentPosition = transform.position;

            // 목표 지점에 도달했는지 확인 (거리가 아주 가까워지면 멈춤)
            if (Vector3.Distance(currentPosition, _targetPosition) < 0.1f)
            {
                _isMovingToTarget = false;
                return;
            }

            // 목표 지점으로 캐릭터 이동
            Vector3 direction = (_targetPosition - currentPosition).normalized;
            Vector3 displacement = direction * _data.MoveSpeed * Time.deltaTime;
            _characterController.Move(displacement);

            // OnMove 이벤트 호출 (필요시)
            OnMove.Invoke(new PlayerMoveEvent(_speed, _verticalVelocity, _moveDir, _data.Grounded));
        }

        // 새로운 메서드: 외부에서 호출하여 캐릭터를 특정 위치로 이동시킴
        public void MoveToPosition(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _isMovingToTarget = true;
        }

		public void Stop()
		{
			_isMovingToTarget = false;
		}
	}
}