using Sandbox;

namespace VrExample
{
	partial class VrPlayer : Player
	{
		[Net, Local] LeftHand LeftHand { get; set; }
		[Net, Local] RightHand RightHand { get; set; }

		private void CreateHands()
		{
			LeftHand?.Delete();
			RightHand?.Delete();

			LeftHand = new() { Owner = this };
			RightHand = new() { Owner = this };

			LeftHand.Other = RightHand;
			RightHand.Other = LeftHand;
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new WalkController(); // TODO
			Animator = new StandardPlayerAnimator(); // TODO
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			CreateHands();

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			SimulateActiveChild( cl, ActiveChild );
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EnableDrawing = false;
		}
	}
}
