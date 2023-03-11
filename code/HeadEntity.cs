namespace Sandbox;

/// <summary>
/// This represents the player's head. It mainly exists so that other players
/// can see what you're looking at.
/// </summary>
public class HeadEntity : ModelEntity
{
	public override void Spawn()
	{
		SetModel( "models/editor/camera.vmdl" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void Simulate( IClient cl )
	{
		Transform = Input.VR.Head;
	}
}
