using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Generic IAsyncResult implementation
	/// </summary>
	/// <remarks>
	/// http://msdn.microsoft.com/msdnmag/issues/05/10/WickedCode/
	/// http://msdn.microsoft.com/msdnmag/issues/07/03/WickedCode/
	/// http://msdn.microsoft.com/msdnmag/issues/03/06/Threading/#S5
	/// </remarks>
	public class AsyncResult : IAsyncResult
	{
		#region Constants

		private readonly object SyncLock = new object();

		#endregion Constants

		#region Fields

		private readonly IDictionary<string, object> Data = new Dictionary<string, object>();
		private readonly AsyncCallback callback;
		private readonly object asyncState;
		private ManualResetEvent waitHandle = null;
		private bool isCompleted = false;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="callback">Callback</param>
		/// <param name="state">Callback State</param>
		public AsyncResult(
			AsyncCallback callback,
			object state)
		{
			this.callback = callback;
			this.asyncState = state;
		}

		#endregion Init

		#region IAsyncResult Members

		/// <summary>
		/// Gets and sets extra place for storage of data
		/// </summary>
		public object this[string key]
		{
			get
			{
				if (!this.Data.ContainsKey(key))
				{
					return null;
				}
				return this.Data[key];
			}
			set
			{
				if (value == null)
				{
					if (this.Data.ContainsKey(key))
					{
						this.Data.Remove(key);
					}
					return;
				}
				this.Data[key] = value;
			}
		}

		/// <summary>
		/// Gets the state associated with the callback
		/// </summary>
		public object AsyncState
		{
			get { return this.asyncState; }
		}

		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get
			{
				lock (this.SyncLock)
				{
					if (this.waitHandle == null)
					{
						this.waitHandle = new ManualResetEvent(this.IsCompleted);
					}
					return this.waitHandle;
				}
			}
		}

		/// <summary>
		/// Signals completion and executes callback
		/// </summary>
		public void CompleteCall()
		{
			lock (this.SyncLock)
			{
				this.isCompleted = true;
				if (this.waitHandle != null)
				{
					this.waitHandle.Set();
				}
			}

			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		/// <summary>
		/// Determines if is completed
		/// </summary>
		public bool IsCompleted
		{
			get { return this.isCompleted; }
		}

		/// <summary>
		/// False
		/// </summary>
		bool IAsyncResult.CompletedSynchronously
		{
			get { return false; }
		}

		#endregion IAsyncResult Members
	}
}
