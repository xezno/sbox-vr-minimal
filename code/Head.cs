namespace Sandbox;

public class Head : ModelEntity
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
