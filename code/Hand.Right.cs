using Sandbox;

namespace VrExample
{
    public class RightHand : BaseHand
	{
		protected override string ModelPath => "models/hands/handright.vmdl";

		public override void Spawn()
		{
			base.Spawn();
			Log.Info( "VR Controller Right Spawned" );
			SetInteractsAs( CollisionLayer.RIGHT_HAND );
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			Hand = Input.VR.RightHand;
		}
	}
}
