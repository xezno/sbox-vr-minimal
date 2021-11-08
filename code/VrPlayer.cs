﻿using Sandbox;

namespace VrExample
{
	partial class VrPlayer : Player
	{
		[Net, Local] public LeftHand LeftHand { get; set; }
		[Net, Local] public RightHand RightHand { get; set; }

		private void CreateHands()
		{
			DeleteHands();

			LeftHand = new() { Owner = this };
			RightHand = new() { Owner = this };

			LeftHand.Other = RightHand;
			RightHand.Other = LeftHand;
		}

		private void DeleteHands()
		{
			LeftHand?.Delete();
			RightHand?.Delete();
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new VrWalkController();
			Animator = new VrPlayerAnimator();
			Camera = new VrCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			CreateHands();

			SetBodyGroup( "Hands", 1 ); // Hide hands

			base.Respawn();
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			SimulateActiveChild( cl, ActiveChild );

			LeftHand?.Simulate( cl );
			RightHand?.Simulate( cl );

			CheckRotate();
			SetVrAnimProperties();
		}

		public void SetVrAnimProperties()
		{
			if ( LifeState != LifeState.Alive )
				return;

			SetAnimBool( "b_vr", true );
			var leftHandLocal = Transform.ToLocal( LeftHand.GetBoneTransform( 0 ) );
			var rightHandLocal = Transform.ToLocal( RightHand.GetBoneTransform( 0 ) );

			var handOffset = Vector3.Zero;
			SetAnimVector( "left_hand_ik.position", leftHandLocal.Position + (handOffset * leftHandLocal.Rotation) );
			SetAnimVector( "right_hand_ik.position", rightHandLocal.Position + (handOffset * rightHandLocal.Rotation) );

			SetAnimRotation( "left_hand_ik.rotation", leftHandLocal.Rotation * Rotation.From( 65, 0, 90 ) );
			SetAnimRotation( "right_hand_ik.rotation", rightHandLocal.Rotation * Rotation.From( 65, 0, 90 ) );

			float height = Input.VR.Head.Position.z - Position.z;
			SetAnimFloat( "duck", 1.0f - ((height - 32f) / 32f) ); // This will probably need tweaking depending on height
		}

		private TimeSince timeSinceLastRotation;
		private void CheckRotate()
		{
			const float deadzone = 0.2f;
			const float angle = 45f;
			const float delay = 0.25f;

			if ( timeSinceLastRotation > delay )
			{
				var rotate = Input.VR.RightHand.Joystick.Value.x;

				if ( rotate > deadzone )
				{
					Rotation = Rotation.RotateAroundAxis( Vector3.Up, -angle );
					timeSinceLastRotation = 0;
				}
				else if ( rotate < -deadzone )
				{
					Rotation = Rotation.RotateAroundAxis( Vector3.Up, angle );
					timeSinceLastRotation = 0;
				}
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EnableDrawing = false;
			DeleteHands();
		}
	}
}
