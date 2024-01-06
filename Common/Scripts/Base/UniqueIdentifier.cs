using System; // Random, BitConverter
using System.Collections.Generic;
using System.Runtime.InteropServices; // HashSet


namespace DDUKServer
{
	/// <summary>
	/// 유니크아이디 생성기.
	/// TNumber에는 정수만 지정할 것.
	/// </summary>
	public class UniqueIdentifier<TNumber> : DisposableObject
		where TNumber : struct, IComparable, IConvertible, IEquatable<TNumber>, IFormattable
	{
		private Random m_Random;
		private HashSet<TNumber> m_Identifiers;
		private byte[] m_TemporaryBuffer;
		private Dictionary<Type, Func<object>> m_BitConverters;
		private Type m_NumberType;
		private Func<object> m_BitConverter;

		public UniqueIdentifier(int randomSeed = 0) : base()
		{
			m_Random = new Random(randomSeed);
			m_Identifiers = new HashSet<TNumber>
			{
				default,
			};

			var temporaryBufferSize = Marshal.SizeOf<TNumber>();
			m_TemporaryBuffer = new byte[temporaryBufferSize];

			m_BitConverters = new Dictionary<Type, Func<object>>
			{
				{ typeof(sbyte), () => (object)m_TemporaryBuffer[0] },
				{ typeof(short), () => (object)BitConverter.ToInt16(m_TemporaryBuffer, 0) },
				{ typeof(int), () => (object)BitConverter.ToInt32(m_TemporaryBuffer, 0) },
				{ typeof(long), () => (object)BitConverter.ToInt64(m_TemporaryBuffer, 0) },
				{ typeof(byte), () => (object)m_TemporaryBuffer[0] },
				{ typeof(ushort), () => (object)BitConverter.ToUInt16(m_TemporaryBuffer, 0) },
				{ typeof(uint), () => (object)BitConverter.ToUInt32(m_TemporaryBuffer, 0) },
				{ typeof(ulong), () => (object)BitConverter.ToUInt64(m_TemporaryBuffer, 0) }
			};

			m_NumberType = typeof(TNumber);
			m_BitConverter = m_BitConverters[m_NumberType];
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			m_Random = null;

			m_Identifiers.Clear();
			m_Identifiers = null;

			m_TemporaryBuffer = null;

			m_BitConverters.Clear();
			m_BitConverters = null;

			base.OnDispose(explicitedDispose);
		}

		public TNumber Generate()
		{
			var identifier = default(TNumber);
			do
			{
				m_Random.NextBytes(m_TemporaryBuffer);
				identifier = (TNumber)m_BitConverter();
			}
			while (m_Identifiers.Contains(identifier));
			m_Identifiers.Add(identifier);

			return identifier;
		}

		public void Clear()
		{
			m_Identifiers.Clear();
		}

		public void Add(TNumber identifier)
		{
			m_Identifiers.Add(identifier);
		}

		public void Remove(TNumber identifier)
		{
			m_Identifiers.Remove(identifier);
		}

		public void UnionWith(HashSet<TNumber> identifiers)
		{
			m_Identifiers.UnionWith(identifiers);
		}

		public void ExceptWith(HashSet<TNumber> identifiers)
		{
			m_Identifiers.ExceptWith(identifiers);
		}

		public bool Contains(TNumber identifier)
		{
			return m_Identifiers.Contains(identifier);
		}
	}
}