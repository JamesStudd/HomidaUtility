using System;
namespace HomidaUtility.Async
{
	public interface IErrorHandler
	{
		void HandleError(Exception ex);
	}
}