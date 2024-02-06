
using System.Text;

namespace CaseConvert
{
	public enum CaseStyle
	{
		/// <summary>
		/// Example: IPAddress
		/// </summary>
		PascalCase = 0,
		/// <summary>
		/// Example: ipAddress
		/// </summary>
		CamelCase,
		/// <summary>
		/// Example: ip_address
		/// </summary>
		SnakeCase,
		/// <summary>
		/// Example: ip-address
		/// </summary>
		DashCase,
		/// <summary>
		/// Example: IP_ADDRESS
		/// </summary>
		MacroCase
	}
	internal class CaseConverter
	{
		public static string Convert(string text, CaseStyle style)
		{
			if(string.IsNullOrEmpty(text))
			{
				return text;
			}
			var cracked = SplitCase(text);
			if (cracked.Count == 0) { return string.Empty; }
			var result = new StringBuilder();
			int i;
			switch (style)
			{
				case CaseStyle.PascalCase:
					for (i = 0; i < cracked.Count; ++i)
					{
						var c = cracked[i];
						if (c.Length <= 2)
						{
							result.Append(c.ToUpperInvariant());
						}
						else
						{
							if (char.IsSurrogatePair(c, 0))
							{
								var pre = c.Substring(0, 2);
								var post = c.Substring(2);
								result.Append(pre.ToUpperInvariant());
								result.Append(post.ToLowerInvariant());
							}
							else
							{
								result.Append(char.ToUpperInvariant(c[0]));
								result.Append(c.Substring(1).ToLowerInvariant());
							}
						}
					}
					break;
				case CaseStyle.CamelCase:
					i = 0;
					while (i < cracked.Count)
					{
						if (!char.IsDigit(cracked[i], 0))
						{
							break;
						}
						
						++i;
					}
					if (i < cracked.Count)
					{
						result.Append(cracked[i].ToLowerInvariant());
						for (i = 1; i < cracked.Count; ++i)
						{
							var c = cracked[i];
							if (c.Length <= 2)
							{
								result.Append(c.ToUpperInvariant());
							}
							else
							{
								if (char.IsSurrogatePair(c, 0))
								{
									var pre = c.Substring(0, 2);
									var post = c.Substring(2);
									result.Append(pre.ToUpperInvariant());
									result.Append(post.ToLowerInvariant());
								}
								else
								{
									result.Append(char.ToUpperInvariant(c[0]));
									result.Append(c.Substring(1).ToLowerInvariant());
								}
							}
						}
					}
					break;
				case CaseStyle.SnakeCase:
					result.Append(cracked[0].ToLowerInvariant());
					for (i = 1; i < cracked.Count; ++i)
					{
						result.Append('_');
						var c = cracked[i];
						result.Append(c.ToLowerInvariant());
					}
					break;
				case CaseStyle.DashCase:
					result.Append(cracked[0].ToLowerInvariant());
					for (i = 1; i < cracked.Count; ++i)
					{
						result.Append('-');
						var c = cracked[i];
						result.Append(c.ToLowerInvariant());
					}
					break;
				case CaseStyle.MacroCase:
					result.Append(cracked[0].ToUpperInvariant());
					for (i = 1; i < cracked.Count; ++i)
					{
						result.Append('_');
						var c = cracked[i];
						result.Append(c.ToUpperInvariant());
					}
					break;
			}
			return result.ToString();
		}
		public static IList<string> SplitCase(string text)
		{
			var result = new List<string>();
			var sb = new StringBuilder();
			var i = 0;
			var state = 0; // 0 = start
			var charcount = 0;
			var found = false;
			while(i< text.Length)
			{
				
				switch(state)
				{
					case 0:
						if(char.IsUpper(text,i))
						{
							_CapAdv(text, sb, ref i);
							++charcount;
							state = 1;

						} else if(char.IsLower(text,i))
						{
							_CapAdv(text, sb, ref i);
							++charcount;
							state = 2;
						}
						else if (char.IsDigit(text, i))
						{
							_CapAdv(text, sb, ref i);
							++charcount;
							state = 3;
						} else if(char.IsSymbol(text,i) || char.IsPunctuation(text,i))
						{
							if (char.IsSurrogatePair(text, i))
							{
								++i;
							}
							++i;
							continue;
						}
						else if (char.IsWhiteSpace(text, i))
						{
							if (char.IsSurrogatePair(text, i))
							{
								++i;
							}
							++i;
							continue;
						}
						else
						{
							throw new ArgumentException("Invalid characters in string", nameof(text));
                        }
						break;
					case 1: // upper case
						found = false;
						while(i < text.Length && char.IsLower(text,i))
						{
							found = true;
							_CapAdv(text, sb, ref i);
						}
						if(found)
						{
							while (i < text.Length && char.IsDigit(text, i))
							{
								_CapAdv(text, sb, ref i);
							}
							result.Add(sb.ToString());
							sb.Clear();
							charcount = 0;
							state = 0;
						} else if(i<text.Length && char.IsDigit(text,i))
						{
							while (i < text.Length && char.IsDigit(text, i))
							{
								_CapAdv(text, sb, ref i);
							}
							result.Add(sb.ToString());
							sb.Clear();
							charcount = 0;
							state = 0;
						} else {

							while(i < text.Length && char.IsUpper(text,i))
							{
								if(i<text.Length-1 && char.IsLower(text,i+1))
								{
									found = false;
									result.Add(sb.ToString());
									sb.Clear();
									charcount = 0;
									state = 0;
									break;
								}
								found = true;
								i = _CapAdv(text, sb, ref i);
							}
							if(found)
							{
								while (i < text.Length && char.IsDigit(text, i))
								{
									_CapAdv(text, sb, ref i);
								}
								result.Add(sb.ToString());
								sb.Clear();
								charcount = 0;
								state = 0;
							}
						}
						break;
					case 2: // lower case
						found = false;
						while (i < text.Length && char.IsLower(text, i))
						{
							found = true;
							_CapAdv(text, sb, ref i);
						}
						if (found)
						{
							while (i < text.Length && char.IsDigit(text, i))
							{
								_CapAdv(text, sb, ref i);
							}
							result.Add(sb.ToString());
							sb.Clear();
							charcount = 0;
							state = 0;
						}
						else if (i < text.Length && char.IsDigit(text, i))
						{
							while (i < text.Length && char.IsDigit(text, i))
							{
								_CapAdv(text, sb, ref i);
							}
							result.Add(sb.ToString());
							sb.Clear();
							charcount = 0;
							state = 0;
						}
						break;
					case 3: // digit
						while (i<text.Length && char.IsDigit(text, i))
						{
							_CapAdv(text, sb, ref i);
						}
						result.Add(sb.ToString());
						sb.Clear();
						charcount = 0;
						state = 0;
						break;						
				}
			}
			return result;
		}

		private static int _CapAdv(string text, StringBuilder sb, ref int i)
		{
			sb.Append(text[i]);
			if (char.IsSurrogatePair(text, i))
			{
				sb.Append(text[i + 1]);
				++i;
			}
			++i;
			return i;
		}
	}
}
