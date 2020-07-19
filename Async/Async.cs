using System;
using System.Threading.Tasks;

namespace HomidaUtility.Async
{
	public static class AsyncUtilities
	{
#pragma warning disable RECS0165
		public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore RECS0165
		{
			try
			{
				await task;
			}
			catch (Exception ex)
			{
				handler?.HandleError(ex);
			}
		}
	}
}