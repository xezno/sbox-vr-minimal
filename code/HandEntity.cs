using Sandbox;
using Sandbox.Diagnostics;

namespace MyGame;

/// <summary>
/// This represents a hand. You should use prefabs to create these and attach
/// models, snap points, etc. depending on what sort of game you're going for.
/// </summary>
[Prefab, Title( "VR Hand Entity" )]
public partial class HandEntity : Entity
{
	/// <summary>
	/// This should either be Hands.Left or Hands.Right and determines the
	/// hand used for input (i.e. transform)
	/// </summary>
	[Net] public Hands InputHand { get; set; }

	public override void Spawn()
	{
		Transmit = TransmitType.Always;

		EnableDrawing = true;
	}

	public override void Simulate( IClient cl )
	{
		Assert.True( InputHand == Hands.Left || InputHand == Hands.Right, "InputHand should be Left or Right." );

		var input = (InputHand == Hands.Left) ? Input.VR.LeftHand : Input.VR.RightHand;
		Transform = input.Transform.WithScale( 1.0f );

		DebugOverlay.Sphere( Position, 4.0f, Color.Blue, 0f, false );
	}
}
