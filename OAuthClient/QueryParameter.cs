using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth
{
	public class QueryParameter
	{
		public string Name { get; protected set; }
		public string Value { get; protected set; }

		public QueryParameter(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		public QueryParameter(OAuthParameter name, string value)
			: this(name.ToStringValue(), value) { }
	}

	public class QueryParameterComparer : IComparer<QueryParameter>
	{
		/// <summary>
		/// performs binary comparison on strings.
		/// </summary>
		public int Compare(QueryParameter x, QueryParameter y)
		{
			/* The parameters are sorted by name, using ascending byte value
			 * ordering.  If two or more parameters share the same name, they
			 * are sorted by their value.
			 */
			byte[] xNameBytes = System.Text.Encoding.UTF8.GetBytes(x.Name);
			byte[] yNameBytes = System.Text.Encoding.UTF8.GetBytes(y.Name);
			int namesComparison = Compare(xNameBytes, yNameBytes);
			if (namesComparison == 0)
			{
				byte[] xValueBytes = System.Text.Encoding.UTF8.GetBytes(x.Value);
				byte[] yValueBytes = System.Text.Encoding.UTF8.GetBytes(y.Value);
				return Compare(xValueBytes, yValueBytes);
			}
			else
			{
				return namesComparison;
			}
		}

		private static int Compare(byte[] x, byte[] y)
		{
			int max = Math.Min(x.Length, y.Length);
			for (int i = 0; i < max; i++)
			{
				int comparison = x[i].CompareTo(y[i]);
				if (comparison != 0) return comparison;
			}
			if (x.Length == y.Length) return 0;
			return x.Length > y.Length ? 1 : -1;
		}

	}
}
