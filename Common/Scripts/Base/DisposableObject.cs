using System; // GC, IDisposable


namespace DDUKServer
{
	/// <summary>
	/// 해제 가능한 오브젝트 (일반 클래스 전용).
	/// 참조가 전부 날아가면 당연히 자동으로 제거되겠지만 명시적으로 해제해줌으로서 좀더 상황을 명확히 통제할 수 있음.
	/// new로 할당하고 Dispose() 로 삭제한다.
	/// 이를 상속받은 클래스는 public void Dispose()에 대한 구현을 필요로 한다.
	/// </summary>
	public class DisposableObject : IDisposable
	{
		/// <summary>
		/// 객체가 해제되었는지 여부.
		/// </summary>
		public bool IsDisposed { private set; get; }

		/// <summary>
		/// 생성.
		/// </summary>
		public DisposableObject()
		{
			IsDisposed = false;
		}

		/// <summary>
		/// 소멸.
		/// </summary>
		~DisposableObject()
		{
			if (IsDisposed)
				return;

			IsDisposed = true;
			OnDispose(false);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected virtual void OnDispose(bool explicitedDispose)
		{
		}

		/// <summary>
		/// IDisposable 상속.
		/// </summary>
		void IDisposable.Dispose()
		{
			if (IsDisposed)
				return;

			IsDisposed = true;
			OnDispose(true);

			// 소멸자 호출 금지.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 박싱 회피용 비교 함수.
		/// </summary>
		public bool Equals(DisposableObject target)
		{
			return this == target;
		}

		/// <summary>
		/// 해제.
		/// 해당 오브젝트의 모든 상속자는 명시적 해제 기능을 직접 구현해야함 (자세한건 상속된 오브젝트 참고).
		/// </summary>
		protected static void Dispose(DisposableObject target)
		{
			if (!DisposableObject.IsValid(target))
				return;

			var disposable = target as IDisposable;
			if (disposable != null)
				disposable.Dispose();
		}

		/// <summary>
		/// 객체가 유효한지 여부.
		/// </summary>
		public static bool IsValid<TDisposableObject>(TDisposableObject target) where TDisposableObject : DisposableObject
		{
			return target != null && !target.IsDisposed;
		}

		/// <summary>
		/// 안전한 해제.
		/// </summary>
		public static bool SafeDispose<TDisposableObject>(ref TDisposableObject target) where TDisposableObject : DisposableObject
		{
			if (target == null)
			{
				return false;
			}
			else if (target.IsDisposed)
			{
				target = null;
				return false;
			}
			else
			{
				DisposableObject.Dispose(target);
				target = null;
				return true;
			}
		}

		/// <summary>
		/// 객체가 유효한지 여부.
		/// </summary>
		public static implicit operator bool(DisposableObject target)
		{
			return DisposableObject.IsValid(target);
		}
	}
}