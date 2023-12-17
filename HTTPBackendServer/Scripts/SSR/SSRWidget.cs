using System.Collections.Generic;


namespace DDUKServer
{
	public class SSRElement
	{
		
	}


	public class SSRWidget
	{
		public SSRWidget Child { protected set; get; }
		public List<SSRWidget> Children { protected set; get; }

		public SSRWidget()
		{
		}

		protected virtual void RefreshWidget()
		{
		}

		protected virtual SSRWidget BuildLayout()
		{
			return new SSRWidget
			{
				Child = new SSRWidget
				{
					Child = new SSRWidget
					{

					}
				}
			};
		}
	}
}