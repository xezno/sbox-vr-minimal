using Sandbox;
using Sandbox.UI;

namespace VrExample
{
	public class ExampleHudEntity : HudEntity<RootPanel>
	{
		public ExampleHudEntity()
		{
			if ( IsClient )
			{
				if ( Input.VR.IsActive )
				{
					// Use a world panel - we're in VR
					_ = new VrHudEntity();
				}
				else
				{
					// Just display the HUD on-screen
					RootPanel.SetTemplate( "/Code/UI/ExampleHud.html" );
				}
			}
		}
	}
}
