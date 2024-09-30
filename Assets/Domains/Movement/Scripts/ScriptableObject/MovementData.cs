using UnityEngine;
using UnityEngine.Events;
using Movement.Events;

namespace Movement.Data
{
    [CreateAssetMenu(fileName = "Movement Data", menuName = "Scriptable Object/Movement Data", order = int.MaxValue)]
    public class MovementData : ScriptableObject
    {
        [Tooltip("캐릭터의 이동 속도 (m/s)")]
        public float MoveSpeed = 4.0f;
        [Tooltip("가속 및 감속")]
        public float SpeedChangeRate = 10.0f;
        [Tooltip("캐릭터가 사용하는 중력 값. 엔진의 기본값은 -9.81f입니다.")]
        public float GravityAcceleration = -15.0f;

        [Header("Player Grounded")]
        [Tooltip("캐릭터가 지면에 닿아 있는지 여부. CharacterController에 내장된 지면 체크의 일부가 아닙니다.")]
        public bool Grounded = true;
        [Tooltip("거친 지면에 유용합니다.")]
        public float GroundedOffset = -0.14f;
        [Tooltip("지면 체크의 반지름입니다. CharacterController의 반지름과 일치해야 합니다.")]
        public float GroundedRadius = 0.5f;
        [Tooltip("캐릭터가 지면으로 인식하는 레이어")]
        public LayerMask GroundLayers;
    }
}