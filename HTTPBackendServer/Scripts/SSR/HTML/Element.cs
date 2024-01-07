using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace DDUKServer.HTML
{
    /// <summary>
    /// 서버사이드렌더링 요소.
    /// </summary>
    public abstract class Element : DisposableObject, IEnumerable
    {
        public abstract string Name { get; }
		public string Value { set; get; }

		public Dictionary<string, string> Attributes { set; get; } = new Dictionary<string, string>();
		public Element Parent { set; get; }
		public ObservableCollection<Element> Children { set; get; } = new ObservableCollection<Element>();

		public Element() : base()
        {
			OnInitialize();

			Children.CollectionChanged += OnChildrenCollectionChanged;
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				// 추가.
				case NotifyCollectionChangedAction.Add:
					{
						foreach (Element child in e.NewItems)
						{
							if (child.Parent != null)
								continue;

							child.Parent = this;
							OnAddChild(child);
						}
						break;
					}

				case NotifyCollectionChangedAction.Remove:
					{
						foreach (Element child in e.OldItems)
						{
							if (child.Parent == this)
							{
								OnRemoveChild(child);
								child.Parent = null;								
							}
						}
						break;
					}
			}
		}

		protected virtual void OnInitialize()
		{
		}

		protected virtual void OnAddChild(Element child)
		{
		}

		protected virtual void OnRemoveChild(Element child)
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