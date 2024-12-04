using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Clicker.Core.RequestQueue
{
	public class RequestQueue : IRequestQueue
	{
		private readonly Dictionary<string, CancellationTokenSource> _activeRequests = new();
		private Queue<(string requestId, Func<Task> request)> _requests = new();
		private bool _isProcessing;

		public void AddRequest(Func<Task> request, string requestId = null)
		{
			requestId ??= Guid.NewGuid().ToString();
			_requests.Enqueue((requestId, request));
			ProcessQueue();
		}

		public void CancelRequest(string requestId)
		{
			if (_activeRequests.TryGetValue(requestId, out var tokenSource))
			{
				tokenSource.Cancel();
				_activeRequests.Remove(requestId);
			}
			else
			{
				_requests =
					new Queue<(string requestId, Func<Task> request)>(_requests.Where(r => r.requestId != requestId));
			}
		}

		public void ClearQueue()
		{
			_requests.Clear();
			foreach (var tokenSource in _activeRequests.Values)
			{
				tokenSource.Cancel();
			}

			_activeRequests.Clear();
		}

		private async void ProcessQueue()
		{
			if (_isProcessing || _requests.Count == 0)
			{
				return;
			}

			_isProcessing = true;

			while (_requests.Count > 0)
			{
				var (requestId, request) = _requests.Dequeue();
				var tokenSource = new CancellationTokenSource();
				_activeRequests[requestId] = tokenSource;

				try
				{
					await request.Invoke();
				}
				catch (Exception ex) when (ex is TaskCanceledException)
				{
					Debug.Log($"Запрос {requestId} отменён.");
				}
				catch (Exception ex)
				{
					Debug.LogError($"Ошибка при выполнении запроса {requestId}: {ex.Message}");
				}
				finally
				{
					_activeRequests.Remove(requestId);
				}
			}

			_isProcessing = false;
		}
	}
}