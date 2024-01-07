using System.Collections;
using System.Collections.Generic;


namespace DDUKServer.HTML
{
    /// <summary>
    /// 서버사이드렌더링 요소.
    /// </summary>
    public abstract class Element : IEnumerable
    {
        public abstract string Tag { get; }
		public string Value { set; get; }

		public Dictionary<string, string> Attributes { set; get; } = new Dictionary<stirng, string>();
		public CSS CSS { set; get; }
		public List<Element> Children { set; get; } = new List<Element>();

		public Element()
        {
			OnInitialize();
		}

		protected virtual void OnInitialize()
		{
		}

        protected virtual void RefreshWidget()
        {
        }

		protected virtual void BuildLayout()
		{
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable)Children).GetEnumerator();
		}
	}
}