namespace VrExample
{
	public static class TransformExtensions
	{
		public static Transform RotateAround( this Transform transform, Vector3 pivotPoint, Rotation rotation )
		{
			var resultTransform = transform;

			resultTransform.Position = rotation * (transform.Position - pivotPoint) + pivotPoint;
			resultTransform.Rotation = rotation * transform.Rotation;

			return resultTransform;
		}
	}
}
