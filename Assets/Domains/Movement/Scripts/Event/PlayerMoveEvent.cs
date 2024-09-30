using UnityEngine;

namespace Movement.Events
{
    [System.Serializable]
	public class PlayerMoveEvent
	{
		public float Speed;
		public float VerticalVelocity;
	    public Vector2 MoveDirection;
		public bool Grounded;
	    public bool IsSprint;

		public PlayerMoveEvent(float speed, float verticalVelocity, Vector2 moveDirection, bool grounded)
		{
			Speed = speed;
			VerticalVelocity = verticalVelocity;
			MoveDirection = moveDirection;
			Grounded = grounded;
		}
	}
}