using Sandbox;

namespace VrExample
{
	public partial class BaseHand : AnimEntity
	{
		[Net] public BaseHand Other { get; set; }

		protected virtual string ModelPath => "";

		protected bool GripPressed => Hand.Grip > 0.5f;
		protected bool TriggerPressed => Hand.Trigger > 0.5f;

		public Input.VrHand Hand { get; protected set; }

		//
		// Offsets so that the controllers are in the right place
		//
		protected Vector3 PosOffset => Hand.Transform.Rotation.Backward * 2f + Hand.Transform.Rotation.Down * 4f;
		protected Rotation RotOffset => Rotation.FromPitch( 65 );

		public override void Spawn()
		{
			SetModel( ModelPath );

			Position = Hand.Transform.Position;
			Rotation = Rotation.From( 0, 0, 0 );

			EnableDrawing = Local.Client == this.Client;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			Position = Hand.Transform.Position;
			Rotation = Hand.Transform.Rotation;
		}
	}
}
