using System;
using System.Collections.Generic;
using System.Threading;

namespace ironclad.Features.UI.Tools
{
	// ReaderWriterLock allows multiple concurrent reads but only one write at a time
	/// <summary> Class RwLockExtensions. </summary>
	public static class RwLockExtensions
	{
		/// <summary> The lock cookie </summary>
		[ThreadStatic] private static LockCookie _lockCookie;
		/// <summary> Uses the semiphore. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="semaphore"> The semaphore. </param>
		/// <param name="safeCall"> The safe call. </param>
		/// <returns> T. </returns>
		public static T UseSemiphore<T>(this Semaphore semaphore, Func<T> safeCall)
		{
			semaphore.WaitOne();
			try
			{
				return safeCall();
			}
			finally
			{
				semaphore.Release();
			}
		}

		/// <summary> Loads from cache. </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TValue"> The type of the t value. </typeparam>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="timeOut"> The time out. </param>
		/// <param name="cache"> The cache. </param>
		/// <param name="key"> The key. </param>
		/// <param name="addFunc"> The add function. </param>
		/// <returns> TValue. </returns>
		public static TValue LoadFromCache<TKey, TValue>(this ReaderWriterLock rwLock, int? timeOut,
			IDictionary<TKey, TValue> cache, TKey key, Func<TKey, TValue> addFunc) where TValue : class
		{
			return LoadFromCache(rwLock, timeOut, cache, key, null, null, addFunc);
		}

		/// <summary> Loads from cache. </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TValue"> The type of the t value. </typeparam>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="cache"> The cache. </param>
		/// <param name="key"> The key. </param>
		/// <param name="isItemValid"> The is item valid. </param>
		/// <param name="addFunc"> The add function. </param>
		/// <returns> TValue. </returns>
		public static TValue LoadFromCache<TKey, TValue>(this ReaderWriterLock rwLock,
			IDictionary<TKey, TValue> cache, TKey key, Func<TValue, bool> isItemValid, Func<TKey, TValue> addFunc) where TValue : class
		{
			return LoadFromCache(rwLock, null, cache, key, null, isItemValid, addFunc);
		}

		/// <summary> Loads from cache. </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TValue"> The type of the t value. </typeparam>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="timeOut"> The time out. </param>
		/// <param name="cache"> The cache. </param>
		/// <param name="key"> The key. </param>
		/// <param name="isCacheValid"> The is cache valid. </param>
		/// <param name="isItemValid"> The is item valid. </param>
		/// <param name="addFunc"> The add function. </param>
		/// <param name="getKeysFunc"> The get keys function. </param>
		/// <returns> TValue. </returns>
		/// <exception cref="System.Exception">  </exception>

		public static TValue LoadFromCache<TKey, TValue>(this ReaderWriterLock rwLock, int? timeOut,
			IDictionary<TKey, TValue> cache, TKey key, Func<bool, bool> isCacheValid,
			Func<TValue, bool> isItemValid, Func<TKey, TValue> addFunc, Func<TValue, IEnumerable<TKey>> getKeysFunc = null)
			where TValue : class
		{
			try
			{
				TValue value;
				rwLock.AcquireReaderLock(timeOut ?? 90000);
				if ((isCacheValid != null && !isCacheValid(false)) || !cache.TryGetValue(key, out value))
				{
					rwLock.UpgradeToWriterLock(timeOut ?? 90000);
					// if cache is not valid clear it and start over
					if (isCacheValid != null && !isCacheValid(true))
						cache.Clear();
					// double locking (checking map 2nd time) prevents race conditions where
					// TValue is added prior to entering write lock.
					else if (cache.TryGetValue(key, out value))
					{
						//check item returned for validity, replacing if invalid
						if (isItemValid == null || isItemValid(value))
							return value;
						var d = (value as IDisposable);
						if (d != null)
							d.Dispose();
					}
					value = addFunc(key);
					if (getKeysFunc == null)
						cache[key] = value;
					else
						getKeysFunc(value).ForEach(k => cache[k] = value);
				}
				return value;
			}
			catch (Exception e)
			{
				string msg = "Unable to retrieve value from cache for key = " + key;
				throw new Exception(msg, e);
			}
			finally
			{
				rwLock.ReleaseLock();
			}
		}

		/// <summary> Reloads if needed. </summary>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="expires"> The expires. </param>
		/// <param name="interval"> The interval. </param>
		/// <param name="test"> The test. </param>
		/// <param name="reload"> The reload. </param>
		public static void ReloadIfNeeded(this ReaderWriterLock rwLock, ref DateTime expires,
			TimeSpan interval, Func<bool> test, Action reload)
		{
			Func<string> reloadFunc = reload == null
				? (Func<string>)null
				: () =>
				{
					reload();
					return null;
				};
			ReloadIfNeeded(rwLock, ref expires, interval, test, reloadFunc);
		}

		/// <summary> Reloads if needed. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="expires"> The expires. </param>
		/// <param name="interval"> The interval. </param>
		/// <param name="test"> The test. </param>
		/// <param name="reload"> The reload. </param>
		/// <returns> T. </returns>
		/// <exception cref="System.Exception">  </exception>
		public static T ReloadIfNeeded<T>(this ReaderWriterLock rwLock, ref DateTime expires,
			TimeSpan interval, Func<bool> test, Func<T> reload) where T : class
		{
			T result = null;
			var now = DateTime.Now;
			try
			{
				rwLock.AcquireReaderLock(90000);
				if (now >= expires)
				{
					rwLock.UpgradeToWriterLock(90000);
					if (now >= expires)
					{
						expires = now.Add(interval);
						if (test == null || test())
							result = reload();
					}
				}
			}
			catch (Exception e)
			{
				const string msg = "Unable to refresh value";
				throw new Exception(msg, e);
			}
			finally
			{
				rwLock.ReleaseLock();
			}
			return result;
		}

		/// <summary> Create and use a ReaderWriterLock in read mode while delegate executes </summary>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="timeOut"> The time out. </param>
		/// <param name="safeCall"> The safe call. </param>
		public static void UseReadLock(this ReaderWriterLock rwLock, int? timeOut, Action safeCall)
		{
			UseReadLock(rwLock, timeOut, () =>
			{
				safeCall();
				return true;
			});
		}

		/// <summary> Create and use a ReaderWriterLock in read mode while delegate executes </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="timeOut"> The time out. </param>
		/// <param name="safeCall"> The safe call. </param>
		/// <returns> T. </returns>
		public static T UseReadLock<T>(this ReaderWriterLock rwLock, int? timeOut, Func<T> safeCall)
		{
			if (rwLock.IsReaderLockHeld || rwLock.IsWriterLockHeld)
				return safeCall();
			rwLock.AcquireReaderLock(timeOut ?? 90000);
			try
			{
				return safeCall();
			}
			finally
			{
				rwLock.ReleaseReaderLock();
			}
		}

		/// <summary> Create and use a ReaderWriterLock in write mode while delegate executes </summary>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="timeOut"> The time out. </param>
		/// <param name="safeCall"> The safe call. </param>
		public static void UseWriteLock(this ReaderWriterLock rwLock, int? timeOut, Action safeCall)
		{
			UseWriteLock(rwLock, timeOut, () =>
			{
				safeCall();
				return true;
			});
		}

		/// <summary> Create and use a ReaderWriterLock in write mode while delegate executes </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="rwLock"> The rw lock. </param>
		/// <param name="timeOut"> The time out. </param>
		/// <param name="safeCall"> The safe call. </param>
		/// <returns> T. </returns>
		public static T UseWriteLock<T>(this ReaderWriterLock rwLock, int? timeOut, Func<T> safeCall)
		{
			if (rwLock.IsWriterLockHeld)
				return safeCall();

			var oldState = rwLock.SetLockState(RWLock.State.WriteLock, null, timeOut);
			try
			{
				return safeCall();
			}
			finally
			{
				rwLock.SetLockState(oldState);
			}
		}
		/// <summary> Gets the state of the lock. </summary>
		/// <param name="lockObj"> The lock object. </param>
		/// <returns> RWLock.State. </returns>
		public static RWLock.State GetLockState(this ReaderWriterLock lockObj)
		{
			return (lockObj.IsWriterLockHeld
				? RWLock.State.WriteLock
				: (lockObj.IsReaderLockHeld
					? RWLock.State.ReadLock
					: RWLock.State.NoLock));
		}
		/// <summary> Sets the state of the lock. </summary>
		/// <param name="lockObj"> The lock object. </param>
		/// <param name="newState"> The new state. </param>
		/// <param name="oldState"> The old state. </param>
		/// <param name="timeout"> The timeout. </param>
		/// <returns> RWLock.State. </returns>
		public static RWLock.State SetLockState(this ReaderWriterLock lockObj, RWLock.State newState, RWLock.State? oldState = null,
			int? timeout = null)
		{
			oldState = oldState ?? lockObj.GetLockState();
			if (oldState.Value != newState)
				switch (oldState.Value)
				{
					case RWLock.State.NoLock:
						if (newState == RWLock.State.ReadLock)
							lockObj.AcquireReaderLock(timeout ?? 90000);
						else
							lockObj.AcquireWriterLock(timeout ?? 90000);
						break;
					case RWLock.State.WriteLock:
						if (newState == RWLock.State.NoLock)
							lockObj.ReleaseWriterLock();
						else
							lockObj.DowngradeFromWriterLock(ref _lockCookie);
						break;
					default:
						if (newState == RWLock.State.NoLock)
							lockObj.ReleaseReaderLock();
						else
							_lockCookie = lockObj.UpgradeToWriterLock(timeout ?? 90000);
						break;
				}
			return oldState.Value;
		}
	}
}
