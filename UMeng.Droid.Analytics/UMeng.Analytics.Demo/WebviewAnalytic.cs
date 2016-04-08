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
using Android.Webkit;
using Com.Umeng.Analytics;

namespace UMeng.Analytics.Demo
{
	[Activity(Label = "WebviewAnalytic")]
	public class WebviewAnalytic : Activity
	{
		private const string _pageName = "WebViewPage";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.umeng_example_analytics_webview);

			WebView webview = FindViewById<WebView>(Resource.Id.webview);
			new MobclickAgentJSInterface(this, webview, new WebChromeClient());
			webview.LoadUrl("file:///android_asset/demo.html");
		}

		protected override void OnPause()
		{
			base.OnPause();
			MobclickAgent.OnPageEnd(_pageName);
			MobclickAgent.OnPause(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			MobclickAgent.OnPageStart(_pageName);
			MobclickAgent.OnResume(this);
		}
	}
}