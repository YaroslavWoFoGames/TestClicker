using System;
using System.Threading.Tasks;

namespace Clicker.Core.RequestQueue
{
	public interface IRequestQueue
	{
		void AddRequest(Func<Task> request, string requestId = null);
		void CancelRequest(string requestId);
		void ClearQueue();
	}
}