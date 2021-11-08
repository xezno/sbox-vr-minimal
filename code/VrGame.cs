using Sandbox;

namespace VrExample
{
	public partial class VrGame : Game
	{
		public VrGame()
		{
			if ( IsServer )
			{
				_ = new ExampleHudEntity();
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new VrPlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}
}
