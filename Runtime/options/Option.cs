using BeatThat.ConvertTypeExt;
using System;
using System.Collections.Generic;

namespace BeatThat
{
	/// <summary>
	/// Use when you are passing an options IDictionary<string, object> and you want to make some valid options visible in the API.
	/// Depends on ConvertTypeExt extension method.
	/// </summary>
	public class Option<T>
	{
		public Option(string k)
		{
			this.key = k;
		}

		/// <summary>
		/// The dictionary key the option sets
		/// </summary>
		/// <value>The key.</value>
		public string key { get; private set; }

		/// <summary>
		/// Accepted value type for the option.
		/// If an option allows multiple types, use object
		/// </summary>
		public Type type { get { return typeof(T); } }


		/// <summary>
		/// Set a value for the option onto an <c>IDictionary</c>.
		/// </summary>
		public void Set(T value, IDictionary<string, object> opts)
		{
			opts[this.key] = value;
		}

		/// <summary>
		/// Gets the current value for this option from a an <c>IDictionary</c>.
		/// If the value can't be found (or if it can't be converted to the required type)
		/// returns the provided default.
		/// </summary>
		public T Get(IDictionary<string, object> opts, T defaultVal = default(T))
		{
			if(opts == null) {
				return defaultVal;
			}

			object o;
			T v;
			return opts.TryGetValue(this.key, out o)?
				(o.TryConvertTo<T>(out v)? v: defaultVal) : defaultVal;
		}
	}

	public class Option : Option<object>
	{
		public Option(string k) : base(k) {}
	}

	namespace OptionsExtentions
	{
		public static class OptionsExt
		{
			public static void SetOpt<OptionType>(this IDictionary<string, object> opts, Option<OptionType> o, OptionType value = default(OptionType))
			{
				o.Set (value, opts);
			}

			public static OptionType GetOpt<OptionType>(this IDictionary<string, object> opts, Option<OptionType> o, OptionType defaultVal = default(OptionType))
			{
				return o.Get (opts, defaultVal);
			}
		}
	}
}
