using System;
using System.Threading;

namespace ironclad.Features.UI.Tools
{
	/// <summary>ReaderWriterLock allows multiple concurrent reads but only one write at a time.
	/// This class attempts to make ReaderWriterLocks easier to use.</summary>
	/// <remarks>Bagnes, Aug 2009.</remarks>
	public class RWLock : IDisposable
	{
		public enum State
		{
			NoLock,
			ReadLock,
			WriteLock
		}

		private readonly ReaderWriterLock lockObj;

		private LockCookie cookie;

		private State state;

		/// <summary>			 Constructor. </summary>
		/// <param name="rwLock">The rw lock.</param>
		public RWLock(ReaderWriterLock rwLock) : this(rwLock, State.ReadLock) { }

		/// <summary>				Constructor.</summary>
		/// <param name="rwLock">   The rw lock.</param>
		/// <param name="lockState">State of the lock.</param>
		public RWLock(ReaderWriterLock rwLock, State lockState)
		{
			lockObj = rwLock;
			Timeout = 90000;
			LockState = lockState;
		}

		/// <summary> the timeout.</summary>
		public int Timeout { get; set; }

		/// <summary>Gets or sets the state of the lock.</summary>
		public State LockState
		{
			get { return state; }
			set
			{
				if (value != state)
				{
					if (state == State.NoLock)
						if (value == State.ReadLock)
							lockObj.AcquireReaderLock(Timeout);
						else
							lockObj.AcquireWriterLock(Timeout);
					else if (state == State.WriteLock)
						if (value == State.NoLock)
							lockObj.ReleaseWriterLock();
						else
							lockObj.DowngradeFromWriterLock(ref cookie);
					else if (value == State.NoLock)
						lockObj.ReleaseReaderLock();
					else
						cookie = lockObj.UpgradeToWriterLock(Timeout);
					state = value;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		public void Dispose()
		{
			LockState = State.NoLock;
		}

		public static T GetInstance<T>(ref T instance) where T : class, new()
		{
			return GetInstance(ref instance, () => new T());
		}

		public static T GetInstance<T>(ref T instance, Func<T> create) where T : class
		{
			if (instance == null)
				lock (typeof(T))
				{
					if (instance == null)
						instance = create();
				}
			return instance;
		}
	}
}
