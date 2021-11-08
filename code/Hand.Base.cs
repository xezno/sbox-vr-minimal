using Sandbox;

namespace VrExample
{
	public partial class BaseHand : AnimEntity
	{
		[Net] public BaseHand Other { get; set; }

		protected virtual string ModelPath => "";

		public bool GripPressed => InputHand.Grip > 0.5f;
		public bool TriggerPressed => InputHand.Trigger > 0.5f;

		public virtual Input.VrHand InputHand { get; }

		public override void Spawn()
		{
			SetModel( ModelPath );

			Position = InputHand.Transform.Position;
			Rotation = InputHand.Transform.Rotation;

			Transmit = TransmitType.Always;
		}

		public override void FrameSimulate( Client cl )
		{
			base.FrameSimulate( cl );

			Transform = InputHand.Transform;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			Transform = InputHand.Transform;
			Animate();
		}

		private void Animate()
		{
			SetAnimFloat( "Index", InputHand.GetFingerCurl( 1 ) );
			SetAnimFloat( "Middle", InputHand.GetFingerCurl( 2 ) );
			SetAnimFloat( "Ring", InputHand.GetFingerCurl( 3 ) );
			SetAnimFloat( "Thumb", InputHand.GetFingerCurl( 0 ) );
		}
	}
}
