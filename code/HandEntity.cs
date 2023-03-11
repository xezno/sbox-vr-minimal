using Sandbox;
using Sandbox.Diagnostics;

namespace MyGame;

public partial class HandEntity : ModelEntity
{
	[Net] public Hands NetInputHand { get; set; }

	public Hands InputHand
	{
		get => NetInputHand;
		set
		{
			NetInputHand = value;
			SetHandModel();
		}
	}

	public override void Spawn()
	{
		Transmit = TransmitType.Always;

		EnableDrawing = true;
	}

	private void SetHandModel()
	{
		var model = InputHand == Hands.Left ? "models/hands/alyx_hand_left.vmdl" : "models/hands/alyx_hand_right.vmdl";
		SetModel( model );
	}

	public override void Simulate( IClient cl )
	{
		Assert.True( InputHand == Hands.Left || InputHand == Hands.Right, "InputHand should be Left or Right." );

		var input = (InputHand == Hands.Left) ? Input.VR.LeftHand : Input.VR.RightHand;
		Transform = input.Transform.WithScale( 1.0f );

		DebugOverlay.Sphere( Position, 4.0f, Color.Blue, 0f, false );
	}
}
