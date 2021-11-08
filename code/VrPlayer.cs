using Sandbox;

namespace VrExample
{
	partial class VrPlayer : Player
	{
		[Net, Local] public LeftHand LeftHand { get; set; }
		[Net, Local] public RightHand RightHand { get; set; }

		private void CreateHands()
		{
			LeftHand?.Delete();
			RightHand?.Delete();

			LeftHand = new() { Owner = this };
			RightHand = new() { Owner = this };

			LeftHand.Other = RightHand;
			RightHand.Other = LeftHand;
		}

		private void AnimateVr()
		{
			// TODO: Make this line up with the hands properly
			SetAnimBool( "b_vr", true );
			var leftHand = Transform.ToLocal( LeftHand.Transform );
			var rightHand = Transform.ToLocal( RightHand.Transform );
			SetAnimVector( "left_hand_ik.position", leftHand.Position );
			SetAnimVector( "right_hand_ik.position", rightHand.Position );

			SetAnimRotation( "left_hand_ik.rotation", leftHand.Rotation * Rotation.From( 65, 0, 90 ) );
			SetAnimRotation( "right_hand_ik.rotation", rightHand.Rotation * Rotation.From( 65, 0, 90 ) );

			float height = Input.VR.Head.Position.z - Position.z;
			SetAnimFloat( "duck", 1.0f - ((height - 32f) / 32f) );
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

			SetBodyGroup( "Hands", 1 );

			CreateHands();

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			SimulateActiveChild( cl, ActiveChild );

			LeftHand.Simulate( cl );
			RightHand.Simulate( cl );

			AnimateVr();
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EnableDrawing = false;
		}
	}
}
