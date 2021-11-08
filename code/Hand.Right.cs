using Sandbox;

namespace VrExample
{
	public class RightHand : BaseHand
	{
		protected override string ModelPath => "models/hands/handright.vmdl";
		public override Input.VrHand InputHand => Input.VR.RightHand;

		public override void Spawn()
		{
			base.Spawn();
			Log.Info( "VR Controller Right Spawned" );
			SetInteractsAs( CollisionLayer.RIGHT_HAND );
		}
	}
}
