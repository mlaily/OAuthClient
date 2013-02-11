using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth
{
	/// <summary>
	/// Provides an UrlDecode(string) method without depending on System.Web.HttpUtility,
	/// which requires to target the full .Net 4 profile...
	/// </summary>
	/// <remarks>
	/// This implementation was extracted from the .Net Framework using ILSpy.
	/// </remarks>
	class UrlDecoder
	{
		private int _bufferSize;
		private int _numChars;
		private char[] _charBuffer;
		private int _numBytes;
		private byte[] _byteBuffer;
		private Encoding _encoding;
		private void FlushBytes()
		{
			if (this._numBytes > 0)
			{
				this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
				this._numBytes = 0;
			}
		}
		internal UrlDecoder(int bufferSize, Encoding encoding)
		{
			this._bufferSize = bufferSize;
			this._encoding = encoding;
			this._charBuffer = new char[bufferSize];
		}
		internal void AddChar(char ch)
		{
			if (this._numBytes > 0)
			{
				this.FlushBytes();
			}
			this._charBuffer[this._numChars++] = ch;
		}
		internal void AddByte(byte b)
		{
			if (this._byteBuffer == null)
			{
				this._byteBuffer = new byte[this._bufferSize];
			}
			this._byteBuffer[this._numBytes++] = b;
		}
		internal string GetString()
		{
			if (this._numBytes > 0)
			{
				this.FlushBytes();
			}
			if (this._numChars > 0)
			{
				return new string(this._charBuffer, 0, this._numChars);
			}
			return string.Empty;
		}

		public static string UrlDecode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return UrlDecodeStringFromStringInternal(str, Encoding.UTF8);
		}

		private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
		{
			int length = s.Length;
			UrlDecoder urlDecoder = new UrlDecoder(length, e);
			int i = 0;
			while (i < length)
			{
				char c = s[i];
				if (c == '+')
				{
					c = ' ';
					goto IL_106;
				}
				if (c != '%' || i >= length - 2)
				{
					goto IL_106;
				}
				if (s[i + 1] == 'u' && i < length - 5)
				{
					int num = HexToInt(s[i + 2]);
					int num2 = HexToInt(s[i + 3]);
					int num3 = HexToInt(s[i + 4]);
					int num4 = HexToInt(s[i + 5]);
					if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
					{
						goto IL_106;
					}
					c = (char)(num << 12 | num2 << 8 | num3 << 4 | num4);
					i += 5;
					urlDecoder.AddChar(c);
				}
				else
				{
					int num5 = HexToInt(s[i + 1]);
					int num6 = HexToInt(s[i + 2]);
					if (num5 < 0 || num6 < 0)
					{
						goto IL_106;
					}
					byte b = (byte)(num5 << 4 | num6);
					i += 2;
					urlDecoder.AddByte(b);
				}
			IL_120:
				i++;
				continue;
			IL_106:
				if ((c & 0x0000ff80) == '\0') // 'ﾀ' == 0x0000ff80
				{
					urlDecoder.AddByte((byte)c);
					goto IL_120;
				}
				urlDecoder.AddChar(c);
				goto IL_120;
			}
			return urlDecoder.GetString();
		}


		private static int HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return (int)(h - '0');
			}
			if (h >= 'a' && h <= 'f')
			{
				return (int)(h - 'a' + '\n');
			}
			if (h < 'A' || h > 'F')
			{
				return -1;
			}
			return (int)(h - 'A' + '\n');
		}
	}

}
