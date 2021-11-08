using Sandbox;

namespace VrExample
{
    public class LeftHand : BaseHand
	{
		protected override string ModelPath => "models/hands/handleft.vmdl";

		public override void Spawn()
		{
			base.Spawn();
			Log.Info( "VR Controller Left Spawned" );
			SetInteractsAs( CollisionLayer.LEFT_HAND );
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			Hand = Input.VR.LeftHand;
		}
	}
}
