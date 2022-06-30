using Sandbox;

namespace VrExample;

public partial class Game : Sandbox.Game
{
	public Game()
	{
		if ( IsServer )
		{
			_ = new ExampleHudEntity();
		}
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var player = new Player();
		client.Pawn = player;

		player.Respawn();
	}
}
