using Sandbox;

namespace VrExample
{
	public partial class BaseHand : AnimEntity
	{
		[Net] public BaseHand Other { get; set; }

		protected virtual string ModelPath => "";

		protected bool GripPressed => InputHand.Grip > 0.5f;
		protected bool TriggerPressed => InputHand.Trigger > 0.5f;

		public virtual Input.VrHand InputHand { get; }

		//
		// Offsets so that the controllers are in the right place
		//
		protected Vector3 PosOffset => InputHand.Transform.Rotation.Backward * 2f + InputHand.Transform.Rotation.Down * 4f;
		protected Rotation RotOffset => Rotation.FromPitch( 65 );

		public override void Spawn()
		{
			SetModel( ModelPath );

			Position = InputHand.Transform.Position;
			Rotation = Rotation.From( 0, 0, 0 );

			EnableDrawing = Local.Client == this.Client;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			Position = InputHand.Transform.Position;
			Rotation = InputHand.Transform.Rotation;
		}
	}
}
