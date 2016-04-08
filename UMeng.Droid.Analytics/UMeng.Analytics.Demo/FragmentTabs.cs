using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Com.Umeng.Analytics;

namespace UMeng.Analytics.Demo
{
	[Activity(Label = "FragmentTabs")]
	public class FragmentTabs : FragmentActivity
	{
		private FragmentTabHost _tabHost;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.umeng_example_analytics_fragment_tabs);
			_tabHost = FindViewById<FragmentTabHost>(Android.Resource.Id.TabHost);
			_tabHost.Setup(this, SupportFragmentManager, Resource.Id.realtabcontent);

			_tabHost.AddTab(_tabHost.NewTabSpec("simple").SetIndicator("Simple"), FragmentSimple.NewInstance(1).Class, null);
			_tabHost.AddTab(_tabHost.NewTabSpec("contacts").SetIndicator("Contacts"), FragmentContacts.NewInstance(1).Class, null);
		}

		public class FragmentSimple : Android.Support.V4.App.Fragment
		{
			private const string _pageName = "FragmentSimple";

			public static FragmentSimple NewInstance(int num)
			{
				FragmentSimple f = new FragmentSimple();
				Bundle args = new Bundle();
				args.PutInt("num", num);
				f.Arguments = args;

				return f;
			}

			public override void OnPause()
			{
				base.OnPause();
				MobclickAgent.OnPageEnd(_pageName);
			}

			public override void OnResume()
			{
				base.OnResume();
				MobclickAgent.OnPageStart(_pageName);
			}

			public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
			{
				TextView tv = new TextView(Context);
				tv.Text = "Fragment Simple";
				return tv;
			}
		}

		public class FragmentContacts : Android.Support.V4.App.Fragment
		{
			private const string _pageName = "FragmentContacts";

			public static FragmentContacts NewInstance(int num)
			{
				FragmentContacts f = new FragmentContacts();

				Bundle args = new Bundle();
				args.PutInt("num", num);
				f.Arguments = args;

				return f;
			}

			public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
			{
				TextView tv = new TextView(Context);
				tv.Text = "Fragment Contacts";
				return tv;
			}

			public override void OnPause()
			{
				base.OnPause();
				MobclickAgent.OnPageEnd(_pageName);
			}

			public override void OnResume()
			{
				base.OnResume();
				MobclickAgent.OnPageStart(_pageName);
			}
		}
	}
}