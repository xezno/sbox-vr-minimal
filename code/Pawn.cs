using Sandbox;

namespace MyGame;

public partial class Pawn : Entity
{
	[ClientInput] public Vector3 InputDirection { get; set; }
	[ClientInput] public Angles ViewAngles { get; set; }

	private BBox _bbox = new BBox(
		new Vector3( -16, -16, 0 ),
		new Vector3( 16, 16, 64 )
	);

	public BBox Hull { get; set; }

	private BBox GetHull()
	{
		var headLocal = Transform.ToLocal( Input.VR.Head );

		var mins = _bbox.Mins + (headLocal.Position.WithZ( 0.0f ) * Rotation);
		var maxs = _bbox.Maxs + (headLocal.Position.WithZ( 0.0f ) * Rotation);

		return new BBox( mins, maxs );
	}

	[BindComponent] public PawnController Controller { get; }

	[Net] public HandEntity LeftHand { get; set; }
	[Net] public HandEntity RightHand { get; set; }
	[Net] public HeadEntity Head { get; set; }

	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		Head = new HeadEntity();
		Head.Owner = this;
		Head.SetParent( this );

		PrefabLibrary.TrySpawn<HandEntity>( "prefabs/hands/left.prefab", out var leftHand );
		leftHand.InputHand = Hands.Left;
		leftHand.Owner = this;
		leftHand.SetParent( this );
		LeftHand = leftHand;

		PrefabLibrary.TrySpawn<HandEntity>( "prefabs/hands/right.prefab", out var rightHand );
		rightHand.InputHand = Hands.Right;
		rightHand.Owner = this;
		rightHand.SetParent( this );
		RightHand = rightHand;
	}

	public void Respawn()
	{
		Components.Create<PawnController>();
	}

	public override void Simulate( IClient cl )
	{
		SimulateRotation();

		Hull = _bbox;

		LeftHand?.Simulate( cl );
		RightHand?.Simulate( cl );

		Controller?.Simulate( cl );
	}

	public override void BuildInput()
	{
		var analogValue = Input.VR.LeftHand.Joystick.Value;
		var headAngles = Input.VR.Head.Rotation.Angles();

		var moveRotation = Rotation.From( 0, headAngles.yaw, 0 );
		InputDirection = new Vector3( analogValue.y, -analogValue.x, 0 ) * moveRotation;

		if ( Input.StopProcessing )
			return;

		var look = Angles.Zero;

		if ( ViewAngles.pitch > 90f || ViewAngles.pitch < -90f )
		{
			look = look.WithYaw( look.yaw * -1f );
		}

		var viewAngles = ViewAngles;
		viewAngles += look;
		viewAngles.pitch = viewAngles.pitch.Clamp( -89f, 89f );
		viewAngles.roll = 0f;
		ViewAngles = viewAngles.Normal;
	}

	public override void FrameSimulate( IClient cl )
	{
		Camera.FirstPersonViewer = this;
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, float liftFeet = 0.0f )
	{
		return TraceBBox( start, end, Hull.Mins, Hull.Maxs, liftFeet );
	}

	private TraceResult TraceBBox( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs, float liftFeet = 0.0f )
	{
		if ( liftFeet > 0 )
		{
			start += Vector3.Up * liftFeet;
			maxs = maxs.WithZ( maxs.z - liftFeet );
		}

		var tr = Trace.Ray( start, end )
					.Size( mins, maxs )
					.WithAnyTags( "solid", "playerclip", "passbullets" )
					.Ignore( this )
					.Run();

		return tr;
	}

	private TimeSince _timeSinceLastRotation;
	private void SimulateRotation()
	{
		const float Deadzone = 0.2f;
		const float Angle = 45f;
		const float Delay = 0.25f;

		float rotate = Input.VR.RightHand.Joystick.Value.x;

		if ( _timeSinceLastRotation > Delay )
		{
			if ( rotate > Deadzone )
			{
				Transform = Transform.RotateAround(
					Input.VR.Head.Position.WithZ( Position.z ),
					Rotation.FromAxis( Vector3.Up, -Angle )
				);

				_timeSinceLastRotation = 0;
			}
			else if ( rotate < -Deadzone )
			{
				Transform = Transform.RotateAround(
					Input.VR.Head.Position.WithZ( Position.z ),
					Rotation.FromAxis( Vector3.Up, Angle )
				);

				_timeSinceLastRotation = 0;
			}
		}

		if ( rotate > -Deadzone && rotate < Deadzone )
		{
			_timeSinceLastRotation = 10;
		}
	}
}
