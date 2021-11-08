using Sandbox;

namespace VrExample
{
    public partial class VrPlayer
	{
		public static ModelEntity CreatePuppet( Entity owner )
		{
			Host.AssertClient();

			var puppet = new ModelEntity();
			puppet.SetModel( "models/citizen/citizen.vmdl" );
			puppet.Owner = owner;
			puppet.Tags.Add( "player_puppet" );

			puppet.SetBodyGroup( "Head", 1 );
			puppet.SetBodyGroup( "Hands", 1 );

			return puppet;
		}

		public static void UpdatePuppetBones()
		{

		}
	}
}
