using Sandbox;
using System;

namespace VrExample;

public class PlayerAnimator : StandardPlayerAnimator
{
	float duck;

	public override void Simulate()
	{
		DoWalk();

		//
		// Let the animation graph know some shit
		//
		bool sitting = HasTag( "sitting" );
		bool noclip = HasTag( "noclip" ) && !sitting;

		SetAnimParameter( "b_grounded", GroundEntity != null || noclip || sitting );
		SetAnimParameter( "b_noclip", noclip );
		SetAnimParameter( "b_sit", sitting );
		SetAnimParameter( "b_swim", Pawn.WaterLevel > 0.5f && !sitting );

		Vector3 aimPos = Pawn.EyePosition + Rotation.Forward * 256;
		Vector3 lookPos = Input.VR.Head.Position + Input.VR.Head.Rotation.Forward * 256;

		//
		// Look in the direction what the player's input is facing
		//
		SetLookAt( "lookat_pos", lookPos ); // old
		SetLookAt( "aimat_pos", aimPos ); // old

		SetLookAt( "aim_eyes", lookPos );
		SetLookAt( "aim_head", lookPos );
		SetLookAt( "aim_body", aimPos );

		SetAnimParameter( "b_ducked", HasTag( "ducked" ) ); // old

		if ( HasTag( "ducked" ) ) duck = duck.LerpTo( 1.0f, Time.Delta * 10.0f );
		else duck = duck.LerpTo( 0.0f, Time.Delta * 5.0f );

		if ( (Pawn as Player)?.ActiveChild is BaseCarriable carry )
		{
			carry.SimulateAnimator( this );
		}
		else
		{
			SetAnimParameter( "holdtype", 0 );
			SetAnimParameter( "aimat_weight", 0.5f ); // old
			SetAnimParameter( "aim_body_weight", 0.5f );
		}
	}

	void DoWalk()
	{
		// Move Speed
		{
			var dir = Velocity;
			var forward = Rotation.Forward.Dot( dir );
			var sideward = Rotation.Right.Dot( dir );

			var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

			SetAnimParameter( "move_direction", angle );
			SetAnimParameter( "move_speed", Velocity.Length );
			SetAnimParameter( "move_groundspeed", Velocity.WithZ( 0 ).Length );
			SetAnimParameter( "move_y", sideward );
			SetAnimParameter( "move_x", forward );
			SetAnimParameter( "move_z", Velocity.z );
		}

		// Wish Speed
		{
			var dir = WishVelocity;
			var forward = Rotation.Forward.Dot( dir );
			var sideward = Rotation.Right.Dot( dir );

			var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

			SetAnimParameter( "wish_direction", angle );
			SetAnimParameter( "wish_speed", WishVelocity.Length );
			SetAnimParameter( "wish_groundspeed", WishVelocity.WithZ( 0 ).Length );
			SetAnimParameter( "wish_y", sideward );
			SetAnimParameter( "wish_x", forward );
			SetAnimParameter( "wish_z", WishVelocity.z );
		}
	}
}
