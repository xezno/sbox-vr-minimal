using Sandbox;

namespace VrExample
{
	public class VrCamera : FirstPersonCamera
	{
		public override void Update()
		{
			base.Update();

			// You will probably need to tweak these depending on your use case
			ZNear = 1;
			ZFar = 25000;
		}
	}
}
