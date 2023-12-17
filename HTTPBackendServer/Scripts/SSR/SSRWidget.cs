namespace DDUKServer
{
	public class SSRContext
	{
	}

	public class SSRWidget
	{
		public SSRWidget Child;

		public SSRWidget()
		{
		}

		public SSRWidget Build(SSRContext context)
		{
			return this;
		}
	}
}