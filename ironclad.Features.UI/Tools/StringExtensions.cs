using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace ironclad.Features.UI.Tools
{
	/// <summary>
	/// Class StringExtensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// To the stream.
		/// </summary>
		/// <param name="src">The source.</param>
		/// <returns>MemoryStream.</returns>
		public static MemoryStream ToStream(this string src)
		{
			return new MemoryStream(Encoding.Unicode.GetBytes(src));
		}

		/// <summary>
		/// Pads the number.
		/// </summary>
		/// <param name="buf">The buf.</param>
		/// <param name="val">The value.</param>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		private static void PadNumber(StringBuilder buf, string val, int start, int end)
		{
			if (start >= 0)
			{
				var len = end - start;
				if (len < 18)
					buf.Append('0', 18 - len);
				buf.Append(val, start, len);
			}
		}

		/// <summary>
		/// Normalizes the numbers.
		/// </summary>
		/// <param name="val">The value.</param>
		/// <returns>System.String.</returns>
		public static string NormalizeNumbers(this string val)
		{
			StringBuilder buf = null;
			if (!string.IsNullOrEmpty(val))
			{
				var numberStart = -1;
				for (var pos = 0; pos < val.Length; ++pos)
				{
					var ch = val[pos];
					if (char.IsDigit(ch))
					{
						if (numberStart == -1)
						{
							if (buf == null)
								buf = new StringBuilder(val.Length + 32).Append(val, 0, pos);
							numberStart = pos;
						}
						if (numberStart >= 0)
							continue;
					}
					else
					{
						PadNumber(buf, val, numberStart, pos);
						numberStart = (ch == '.' ? -2 : -1);
					}

					buf?.Append(ch);
				}

				PadNumber(buf, val, numberStart, val.Length);
			}

			return buf?.ToString() ?? val;
		}

		/// <summary>
		/// Filters the alpha number.
		/// </summary>
		/// <param name="src">The source.</param>
		/// <returns>System.String.</returns>
		public static string FilterAlphaNum(this string src)
		{
			return src.Filter("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", false);
		}

		/// <summary>
		/// Filters the specified valid chars.
		/// </summary>
		/// <param name="src">The source.</param>
		/// <param name="validChars">The valid chars.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
		/// <returns>System.String.</returns>
		public static string Filter(this string src, string validChars, bool ignoreCase)
		{
			if (src.IsNullOrEmpty()) return string.Empty;
			var buf = new StringBuilder();
			if (!ignoreCase)
				validChars = validChars.ToUpper() + validChars.ToLower();
			src.Where(ch => validChars.Contains(ch)).ForEach(ch => buf.Append(ch));
			return buf.ToString();
		}

		/// <summary>
		/// Removes all.
		/// </summary>
		/// <param name="src">The source.</param>
		/// <param name="invalidChars">The invalid chars.</param>
		/// <returns>System.String.</returns>
		public static string RemoveAll(this string src, string invalidChars)
		{
			if (src.IsNullOrEmpty()) return string.Empty;
			var buf = new StringBuilder();
			src.Where(ch => !invalidChars.Contains(ch)).ForEach(ch => buf.Append(ch));
			return buf.ToString();
		}

		/// <summary>
		/// Collapses the whitespace.
		/// </summary>
		/// <param name="src">The source.</param>
		/// <returns>System.String.</returns>
		public static string CollapseWhitespace(this string src)
		{
			const string wsChars = " \t\n\r";
			var buf = new StringBuilder();
			var isWS = false;
			foreach (var ch in src)
			{
				var ws = wsChars.Contains(ch);
				if (ws && isWS)
					continue;
				buf.Append(ws ? ' ' : ch);
				isWS = ws;
			}
			return buf.ToString();
		}

		/// <summary>
		/// return the first maxLen characters in a string
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <param name="maxLen">the maximum length.</param>
		/// <returns>System.String.</returns>
		public static string Head(this string value, int maxLen)
		{
			return value == null || maxLen < 1 || value.Length <= maxLen
				? value
				: value.Substring(0, maxLen);
		}

		/// <summary>
		/// return a string with the prefix value removed if match found
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <param name="prefix">The prefix to skip.</param>
		/// <returns>System.String.</returns>
		public static string IgnorePrefix(this string value, string prefix)
		{
			return (value != null && prefix != null && value.StartsWith(prefix)
				? value.Substring(prefix.Length)
				: value);
		}

		/// <summary>
		/// return a string with the sufix value removed if match found
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <param name="sufix">The sufix to skip.</param>
		/// <returns>System.String.</returns>
		public static string IgnoreSufix(this string value, string sufix)
		{
			return (value != null && sufix != null && value.EndsWith(sufix)
				? value.Substring(0, value.Length - sufix.Length)
				: value);
		}

		// ReSharper disable PossibleNullReferenceException

		/// <summary>
		/// return all characters up to first instance of seperator (or all if sep not found)
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="sep">The seperator.</param>
		/// <returns>System.String.</returns>
		public static string FirstWord(this string value, string sep)
		{
			var pos = (value == null || sep == null
				? -1
				: value.IndexOf(sep, StringComparison.Ordinal));
			return (pos < 0
				? value
				: value.Substring(0, pos));
		}

		/// <summary>
		/// return all characters after the last instance of seperator (or all if sep not found)
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="sep">The seperator.</param>
		/// <returns>System.String.</returns>
		public static string LastWord(this string value, string sep)
		{
			var pos = (value == null || sep == null
				? -1
				: value.LastIndexOf(sep, StringComparison.Ordinal));
			return (pos < 0
				? value
				: value.Substring(pos + sep.Length));
		}

		/// <summary>
		/// return all characters up to last instance of seperator (or empty string if seperator
		/// not found)
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="sep">The seperator.</param>
		/// <returns>System.String.</returns>
		public static string RemoveLastWord(this string value, string sep)
		{
			var pos = (value == null || sep == null
				? -1
				: value.LastIndexOf(sep, StringComparison.Ordinal));
			return (pos < 0
				? string.Empty
				: value.Substring(0, pos));
		}

		// ReSharper restore PossibleNullReferenceException

		/// <summary>
		/// convenience method for formating strings
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="args">A variable-length parameters list containing arguments.</param>
		/// <returns>System.String.</returns>
		[DebuggerHidden]
		public static string FormatStr(this string value, params object[] args)
		{
			return value == null || args == null || args.Length == 0
				? value
				: string.Format(value, args);
		}

		/// <summary>
		/// Formats the arguments.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>System.String.</returns>
		[DebuggerHidden]
		public static string FormatArgs<T>(this string value, IEnumerable<T> args)
		{
			return value == null
				? null
				: string.Format(value, args.Cast<object>().ToArray());
		}

		/// <summary>
		/// return string value or null if string is empty
		/// </summary>
		/// <param name="val">The value.</param>
		/// <returns>System.String.</returns>
		[DebuggerHidden]
		public static string IgnoreEmpty(this string val)
		{
			return (val == string.Empty
				? null
				: val);
		}

		/// <summary>
		/// Indicates if string is null or empty
		/// </summary>
		/// <param name="val">The value.</param>
		/// <returns><c>true</c> if [is null or empty] [the specified value]; otherwise, <c>false</c>.</returns>
		[DebuggerHidden]
		public static bool IsNullOrEmpty(this string val)
		{
			return string.IsNullOrEmpty(val);
		}

		/// <summary>
		/// attempts to parse string into specified primitive type and will return null if parsing
		/// fails for any reason
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="val">The value to parse.</param>
		/// <returns>System.Nullable&lt;T&gt;.</returns>
		[DebuggerStepThrough]
		public static T? SafeParse<T>(this object val) where T : struct
		{
			return (T?)SafeParse(val, typeof(T));
		}

		/// <summary>
		/// Url encode a string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <returns>System.String.</returns>
		[DebuggerStepThrough]
		public static string UrlEncode(this string str)
		{
			return HttpUtility.UrlEncode(str);
		}

		/// <summary>
		/// Url decode a string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <returns>System.String.</returns>
		[DebuggerStepThrough]
		public static string UrlDecode(this string str)
		{
			return HttpUtility.UrlDecode(str);
		}

		/// <summary>
		/// Html encode a string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <returns>System.String.</returns>
		[DebuggerStepThrough]
		public static string HtmlDecode(this string str)
		{
			return HttpUtility.HtmlDecode(str);
		}

		/// <summary>
		/// Html decode a string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <returns>System.String.</returns>
		[DebuggerStepThrough]
		public static string HtmlEncode(this string str)
		{
			return HttpUtility.HtmlEncode(str);
		}

		/// <summary>
		/// attempts to parse string into specified primitive type and will return null if parsing
		/// fails for any reason
		/// </summary>
		/// <param name="val">The value to parse.</param>
		/// <param name="type">type to parse into</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
		public static object SafeParse(this object val, Type type)
		{
			if (val is bool && type != typeof(bool))
				val = ((bool)val
					? 1
					: 0);
			return type == null || val == null
				? null
				: (!type.IsInstanceOfType(val))
					? SafeParse(val.ToString(), type)
					: (type == typeof(double) && double.IsNaN((double)val)
						? null
						: val);
		}

		/// <summary>
		/// attempts to parse string into specified primitive type and will return null if parsing
		/// fails for any reason
		/// </summary>
		/// <param name="val">The value to parse.</param>
		/// <param name="type">type to parse into</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="System.ArgumentException">can not parse type: " + type.Name</exception>
		/// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
		public static object SafeParse(this string val, Type type)
		{
			type = (type != null && type.IsGenericType
				? (Nullable.GetUnderlyingType(type) ?? type)
				: type);
			if (type == null)
				return null;

			switch (type.Name)
			{
				case "String":
					return val;
				case "Int32":
					return val.TryParseInt();
				case "Int64":
					return val.TryParseLong();
				case "Boolean":
					return val.TryParseBool();
				case "Int16":
					return val.TryParseShort();
				case "Double":
					return val.TryParseDouble();
				case "Single":
					return val.TryParseFloat();
				case "Decimal":
					return val.TryParseDecimal();
				case "DateTime":
					return val.TryParseDateTime();
				case "DateTimeOffset":
					return val.TryParseDateTimeOffset();
				case "TimeSpan":
					return val.TryParseTimeSpan();
				case "UInt32":
					return val.TryParseUInt();
				case "UInt64":
					return val.TryParseULong();
				case "Uint16":
					return val.TryParseUShort();
				case "Byte":
					return val.TryParseByte();
				case "SByte":
					return val.TryParseSByte();
				case "Guid":
					return val.TryParseGuid();
				default:
					if (type.IsEnum)
						return TryParseEnum(val, type);
					if (type == typeof(byte[]))
						return val.TryParseByteArray();
					throw new ArgumentException("can not parse type: " + type.Name);
			}
		}

		/// <summary>
		/// Tries to parse int.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Int32}.</returns>
		public static int? TryParseInt(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			int r;
			return (int.TryParse(val, out r)
				? r
				: (int?)null);
		}

		/// <summary>
		/// Tries to parse long.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Int64}.</returns>
		public static long? TryParseLong(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			long r;
			return (long.TryParse(val, out r)
				? r
				: (long?)null);
		}

		/// <summary>
		/// Tries to parse bool.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
		public static bool? TryParseBool(this string val)
		{
			val = (val ?? string.Empty).Trim();
			if (val.IsNullOrEmpty())
				return null;
			bool r;
			if (bool.TryParse(val, out r))
				return r;
			var idx = "0nNfF1yYtT".IndexOf(val[0]);
			return (idx < 0
				? (bool?)null
				: (idx > 4));
		}

		/// <summary>
		/// Tries to parse short.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Int16}.</returns>
		public static short? TryParseShort(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			short r;
			return (short.TryParse(val, out r)
				? r
				: (short?)null);
		}

		/// <summary>
		/// Tries to parse double.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Double}.</returns>
		public static double? TryParseDouble(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			double r;
			return (double.TryParse(val, out r)
				? (double.IsNaN(r)
					? (double?)null
					: r)
				: null);
		}

		/// <summary>
		/// The lock
		/// </summary>
		private static readonly ReaderWriterLock _lock = new ReaderWriterLock();

		/// <summary>
		/// Tries to parse float.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Single}.</returns>
		public static float? TryParseFloat(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			float r;
			return (float.TryParse(val, out r)
				? r
				: (float?)null);
		}

		/// <summary>
		/// Tries to parse decimal.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Decimal}.</returns>
		public static decimal? TryParseDecimal(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			decimal r;
			return (decimal.TryParse(val, out r)
				? r
				: (decimal?)null);
		}

		/// <summary>
		/// The enum map
		/// </summary>
		private static readonly IDictionary<string, long> _enumMap = new Dictionary<string, long>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Tries to parse date time.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{DateTime}.</returns>
		public static DateTime? TryParseDateTime(this string val)
		{
			val = (val ?? string.Empty).Trim();
			if (val.IsNullOrEmpty())
				return null;
			DateTime r;
			return (DateTime.TryParse(val, out r)
				? r
				: (DateTime?)null);
		}

		/// <summary>
		/// Tries to parse date time offset.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{DateTimeOffset}.</returns>
		public static DateTimeOffset? TryParseDateTimeOffset(this string val)
		{
			val = (val ?? string.Empty).Trim();
			if (val.IsNullOrEmpty())
				return null;
			DateTimeOffset r;
			return (DateTimeOffset.TryParse(val, out r)
				? r
				: (DateTimeOffset?)null);
		}

		/// <summary>
		/// The enum sep
		/// </summary>
		private static readonly char[] _enumSep = { '|', ',' };

		/// <summary>
		/// Tries to parse time span.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{TimeSpan}.</returns>
		public static TimeSpan? TryParseTimeSpan(this string val)
		{
			val = (val ?? string.Empty).Trim();
			if (val.IsNullOrEmpty())
				return null;
			TimeSpan r;
			return (TimeSpan.TryParse(val, out r)
				? r
				: (TimeSpan?)null);
		}

		/// <summary>
		/// Tries to parse Unsigned int.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.UInt32}.</returns>
		public static uint? TryParseUInt(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			uint r;
			return (uint.TryParse(val, out r)
				? r
				: (uint?)null);
		}

		/// <summary>
		/// The enum error
		/// </summary>
		private static readonly long _enumError = long.MinValue + 2;

		/// <summary>
		/// Tries to parse Unsigned long.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.UInt64}.</returns>
		public static ulong? TryParseULong(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			ulong r;
			return (ulong.TryParse(val, out r)
				? r
				: (ulong?)null);
		}

		/// <summary>
		/// Tries to parse Unsigned short.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.UInt16}.</returns>
		public static ushort? TryParseUShort(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			ushort r;
			return (ushort.TryParse(val, out r)
				? r
				: (ushort?)null);
		}

		/// <summary>
		/// Tries to parse byte.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.Byte}.</returns>
		public static byte? TryParseByte(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			byte r;
			return (byte.TryParse(val, out r)
				? r
				: (byte?)null);
		}

		/// <summary>
		/// Tries to parse Signed byte.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{System.SByte}.</returns>
		public static sbyte? TryParseSByte(this string val)
		{
			val = (val ?? string.Empty).Trim().RemoveAll(",$%");
			if (val.IsNullOrEmpty())
				return null;
			sbyte r;
			return (sbyte.TryParse(val, out r)
				? r
				: (sbyte?)null);
		}

		/// <summary>
		/// Tries to parse GUID.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Nullable{Guid}.</returns>
		public static Guid? TryParseGuid(this string val)
		{
			val = (val ?? string.Empty).Trim();
			if (val.IsNullOrEmpty())
				return null;
			Guid r;
			return (Guid.TryParse(val, out r)
				? r
				: (Guid?)null);
		}

		/// <summary>
		/// Tries to parse enum.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		public static object TryParseEnum(this string val, Type type)
		{
			long curItem, result = _enumError;
			try
			{
				val = (val ?? string.Empty).Trim();
				if (!val.IsNullOrEmpty())
				{
					var valNames = val.Split(_enumSep, StringSplitOptions.RemoveEmptyEntries);
					var enumTypeName = type.FullName + "/";
					result = 0;

					_lock.SetLockState(RWLock.State.ReadLock);
					if (!_enumMap.ContainsKey(enumTypeName))
					{
						var names = Enum.GetNames(type);
						var values = (IList)Enum.GetValues(type);
						_lock.SetLockState(RWLock.State.WriteLock);
						for (var loop = 0; loop < names.Length; ++loop)
						{
							curItem = Convert.ToInt64(values[loop]);
							_enumMap[enumTypeName + names[loop]] = curItem;
							_enumMap[enumTypeName + curItem] = curItem;
						}
						_enumMap[enumTypeName] = 0;
					}
					for (var loop = 0; result != _enumError && loop < valNames.Length; ++loop)
						result = _enumMap.TryGetValue(enumTypeName + valNames[loop], out curItem)
							? result | curItem
							: _enumError;
				}
			}
			catch
			{
				result = _enumError;
			}
			finally
			{
				_lock.SetLockState(RWLock.State.NoLock);
			}
			return result == _enumError ? null : Enum.ToObject(type, result);
		}

		/// <summary>
		/// Tries to parse byte array.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns>System.Byte[][].</returns>
		public static byte[] TryParseByteArray(this string val)
		{
			val = (val ?? string.Empty).Trim();
			if (val.IsNullOrEmpty())
				return null;
			try
			{
				return Convert.FromBase64String(val);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// retrieve an app setting and parse into correct type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The app setting name.</param>
		/// <returns>System.Nullable&lt;T&gt;.</returns>
		public static T? AppSetting<T>(this string name) where T : struct
		{
			return ConfigurationManager.AppSettings[name].SafeParse<T>();
		}

		/// <summary>
		/// retrieve an app setting
		/// </summary>
		/// <param name="name">The app setting name.</param>
		/// <returns>System.String.</returns>
		public static string AppSetting(this string name)
		{
			return ConfigurationManager.AppSettings[name].IgnoreEmpty();
		}

		/// <summary>
		/// Gets a query argument from the context and parse into correct type
		/// </summary>
		/// <typeparam name="T">.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="name">The name.</param>
		/// <returns>System.Nullable&lt;T&gt;.</returns>
		public static T? GetQueryArg<T>(this HttpContext context, string name) where T : struct
		{
			return context.Request[name].SafeParse<T>();
		}

		/// <summary>
		/// Formats the phone number.
		/// </summary>
		/// <param name="phoneNumber">The phone number.</param>
		/// <returns>System.String.</returns>
		public static string FormatPhoneNumber(this string phoneNumber)
		{
			if (phoneNumber.Length == 10)
				return "({0}) {1}-{2}".FormatStr(phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3), phoneNumber.Substring(6));

			if (phoneNumber.Length == 7)
				return "{1}-{2}".FormatStr(phoneNumber.Substring(0, 3), phoneNumber.Substring(4));

			return phoneNumber;
		}

		/// <summary>
		/// Lefts the specified original.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="characterCount">The character count.</param>
		/// <returns>String.</returns>
		public static string Left(this string original, int characterCount)
		{
			if (original.IsNullOrEmpty())
			{
				return "";
			}
			return characterCount > original.Length
				? original
				: original.Substring(0, characterCount);
		}

		/// <summary>
		/// Lefts the specified original.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="characterCount">The character count.</param>
		/// <returns>String.</returns>
		public static string Right(this string original, int characterCount)
		{
			if (string.IsNullOrEmpty(original))
			{
				return "";
			}
			return characterCount > original.Length
				? original.Substring(original.Length - characterCount)
				: original;
		}

		/// <summary>
		/// Splits the specified line length.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="lineLength">Length of the line.</param>
		/// <returns>System.String.</returns>
		public static string Split(this string original, int lineLength)
		{
			return Regex.Replace(original, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
		}

		/// <summary>
		/// Splits to array.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="lineLength">Length of the line.</param>
		/// <returns>System.String[].</returns>
		public static string[] SplitToArray(string text, int lineLength)
		{
			return Regex.Matches(text, ".{1," + lineLength + "}").Cast<Match>().Select(m => m.Value).ToArray();
		}

		/// <summary> Detects the text encoding. </summary>
		/// <param name="me"> Me. </param>
		/// <param name="text"> The text. </param>
		/// <param name="taster"> The taster. </param>
		/// <returns> Encoding. </returns>
		/// <remarks>
		/// Function to detect the encoding for UTF-7, UTF-8/16/32 (bom, no bom, little
		/// & big endian), and local default codepage, and potentially other codepages.
		/// 'taster' = number of bytes to check of the file (to save processing). Higher
		/// value is slower, but more reliable (especially UTF-8 with special characters
		/// later on may appear to be ASCII initially). If taster = 0, then taster
		/// becomes the length of the file (for maximum reliability). 'text' is simply
		/// the string with the discovered encoding applied to the file.
		/// </remarks>
		public static Encoding DetectTextEncoding(this string me, int taster = 1000)
		{
			var b = me;

			// First check the low hanging fruit by checking if a
			// BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
			if (b.Length >= 4 && b[0] == 0x00 && b[1] == 0x00 && b[2] == 0xFE && b[3] == 0xFF)
			{
				// UTF-32, big-endian
				//text = Encoding.GetEncoding("utf-32BE").GetString(b, 4, b.Length - 4);
				return Encoding.GetEncoding("utf-32BE");
			}

			if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xFE && b[2] == 0x00 && b[3] == 0x00)
			{
				// UTF-32, little-endian
				//text = Encoding.UTF32.GetString(b, 4, b.Length - 4);
				return Encoding.UTF32;
			}

			if (b.Length >= 2 && b[0] == 0xFE && b[1] == 0xFF)
			{
				// UTF-16, big-endian
				//text = Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2);
				return Encoding.BigEndianUnicode;
			}

			if (b.Length >= 2 && b[0] == 0xFF && b[1] == 0xFE)
			{
				// UTF-16, little-endian
				//text = Encoding.Unicode.GetString(b, 2, b.Length - 2);
				return Encoding.Unicode;
			}

			if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF)
			{
				// UTF-8
				//text = Encoding.UTF8.GetString(b, 3, b.Length - 3);
				return Encoding.UTF8;
			}

			if (b.Length >= 3 && b[0] == 0x2b && b[1] == 0x2f && b[2] == 0x76)
			{
				// UTF-7
				//text = Encoding.UTF7.GetString(b, 3, b.Length - 3);
				return Encoding.UTF7;
			}

			// If the code reaches here, no BOM/signature was found, so now
			// we need to 'taste' the file to see if can manually discover
			// the encoding. A high taster value is desired for UTF-8
			if (taster == 0 || taster > b.Length) taster = b.Length; // Taster size can't be bigger than the filesize obviously.

			// Some text files are encoded in UTF8, but have no BOM/signature. Hence
			// the below manually checks for a UTF8 pattern. This code is based off
			// the top answer at: http://stackoverflow.com/questions/6555015/check-for-invalid-utf8
			// For our purposes, an unnecessarily strict (and terser/slower)
			// implementation is shown at: http://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
			// For the below, false positives should be exceedingly rare (and would
			// be either slightly malformed UTF-8 (which would suit our purposes
			// anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
			var i = 0;
			var utf8 = false;
			while (i < taster - 4)
			{
				if (b[i] <= 0x7F)
				{
					i += 1;
					continue;
				} // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be treated as the default codepage of the computer). Hence, there's no "utf8 = true;" code unlike the next three checks.
				if (b[i] >= 0xC2 && b[i] <= 0xDF && b[i + 1] >= 0x80 && b[i + 1] < 0xC0)
				{
					i += 2;
					utf8 = true;
					continue;
				}

				if (b[i] >= 0xE0 && b[i] <= 0xF0 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0)
				{
					i += 3;
					utf8 = true;
					continue;
				}

				if (b[i] >= 0xF0 && b[i] <= 0xF4 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0 && b[i + 3] >= 0x80 && b[i + 3] < 0xC0)
				{
					i += 4;
					utf8 = true;
					continue;
				}

				utf8 = false;
				break;
			}

			if (utf8)
			{
				//text = Encoding.UTF8.GetString(b);
				return Encoding.UTF8;
			}

			// The next check is a heuristic attempt to detect UTF-16 without a BOM.
			// We simply look for zeroes in odd or even byte places, and if a certain
			// threshold is reached, the code is 'probably' UF-16.
			var threshold = 0.1d; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
			var count = 0;

			for (var n = 0; n < taster; n += 2)
				if (b[n] == 0)
					count++;

			if (((double)count) / taster > threshold)
			{
				//text = Encoding.BigEndianUnicode.GetString(b);
				return Encoding.BigEndianUnicode;
			}

			count = 0;
			for (var n = 1; n < taster; n += 2)
				if (b[n] == 0)
					count++;
			if (((double)count) / taster > threshold)
			{
				// (little-endian)
				//text = Encoding.Unicode.GetString(b);
				return Encoding.Unicode;
			}


			// Finally, a long shot - let's see if we can find "charset=xyz" or
			// "encoding=xyz" to identify the encoding:
			var b2 = string.Copy(b).ToLower();
			for (var n = 0; n < taster - 9; n++)
			{
				if (b2.Substring(n).StartsWith("charset") || b2.Substring(n).StartsWith("encoding"))
				{
					n += (b2[n + 0] == 'c') ? 8 : 9;
					if (b2[n] == '"' || b2[n] == '\'') n++;

					var oldn = n;

					while (n < taster && (b2[n] == '_' || b2[n] == '-' || (b2[n] >= '0' && b2[n] <= '9') || b2[n] >= 'a' && b2[n] <= 'z'))
						n++;

					var nb = new byte[n - oldn];

					try
					{
						var internalEnc = Encoding.ASCII.GetString(nb);
						//text = Encoding.GetEncoding(internalEnc).GetString(b2);
						return Encoding.GetEncoding(internalEnc);
					}
					catch
					{
						// If C# doesn't recognize the name of the encoding, break.
						break;
					}
				}
			}

			// If all else fails, the encoding is probably (though certainly not
			// definitely) the user's local codepage! One might present to the user a
			// list of alternative encodings as shown here: http://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
			// A full list can be found using Encoding.GetEncodings();
			//text = b;
			return Encoding.Default;
		}
	}
}
