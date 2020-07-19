using System;

namespace HomidaUtility.Async
{
	public class AsyncErrorHandler : IErrorHandler
	{
		public void HandleError(Exception ex)
		{
			UnityEngine.Debug.LogError($"Failed async method. {ex.Message}.");
		}
	}
}