using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Umeng.Analytics;
using System.Collections.Generic;
using Com.Umeng.Analytics.Social;

namespace UMeng.Analytics.Demo
{
	[Activity(Label = "UMeng.Analytics.Demo", MainLauncher = true, Icon = "@drawable/icon")]
	public class AnalyticsHome : Activity
	{
		private Context _context;
		private const string _pageName = "AnalyticsHome";

		protected override void OnCreate(Bundle bundle)
		{
			Action<View> t = OnButtonClick;
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.umeng_example_analytics);
			_context = this;
			MobclickAgent.SetDebugMode(true);
		}

		protected override void OnResume()
		{
			base.OnResume();
			MobclickAgent.OnPageStart(_pageName);
			MobclickAgent.OnResume(_context);
		}

		protected override void OnPause()
		{
			base.OnPause();
			MobclickAgent.OnPageEnd(_pageName);
			MobclickAgent.OnPause(_context);
		}

		[Java.Interop.Export("onButtonClick")]
		public void OnButtonClick(View view)
		{
			int id = view.Id;
			switch (id)
			{
				case Resource.Id.umeng_example_analytics_event:
					{
						MobclickAgent.OnEvent(_context, "click");
						MobclickAgent.OnEvent(_context, "click", "button");
					}
					break;
				case Resource.Id.umeng_example_analytics_ekv:
					{
						var dic = new Dictionary<string, string>();
						dic.Add("type", "popular");
						dic.Add("artist", "JJLin");
						MobclickAgent.OnEvent(_context, "music", dic);
					}
					break;
				case Resource.Id.umeng_example_analytics_duration:
					{
						var dic = new Dictionary<string, string>();
						dic.Add("type", "popular");
						dic.Add("artist", "JJLin");
						MobclickAgent.OnEventValue(this, "music", dic, 12000);
					}
					break;
				case Resource.Id.umeng_example_analytics_event_begin:
					{
						MobclickAgent.OnEventBegin(_context, "music");
						MobclickAgent.OnEventBegin(_context, "music", "one");
						var dic = new Dictionary<string, string>();
						dic.Add("type", "popular");
						dic.Add("artist", "JJLin");
						MobclickAgent.OnKVEventBegin(_context, "music", dic, "flag0");
					}
					break;
				case Resource.Id.umeng_example_analytics_event_end:
					{
						MobclickAgent.OnEventEnd(_context, "music");
						MobclickAgent.OnEventEnd(_context, "music", "one");

						MobclickAgent.OnKVEventEnd(_context, "music", "flag0");
					}
					break;
				case Resource.Id.umeng_example_analytics_make_crash:
					{
						"123".Substring(10);
					}
					break;
				case Resource.Id.umeng_example_analytics_js_analytic:
					{
						StartActivity(new Intent(this, typeof(WebviewAnalytic)));
					}
					break;
				case Resource.Id.umeng_example_analytics_fragment_stack:
					{
						StartActivity(new Intent(this, typeof(FragmentStack)));
					}
					break;
				case Resource.Id.umeng_example_analytics_fragment_tabs:
					{
						StartActivity(new Intent(this, typeof(FragmentTabs)));
					}
					break;
				case Resource.Id.umeng_example_analytics_social:
					{
						UMPlatformData platform = new UMPlatformData(UMPlatformData.UMedia.SinaWeibo, "user_id");
						platform.Gender = UMPlatformData.GENDER.Male;
						platform.WeiboId = "weiboId";

						MobclickAgent.OnSocialEvent(this, platform);
					}
					break;
				case Resource.Id.umeng_example_analytics_signin:
					{
						MobclickAgent.OnProfileSignIn("example_id");
					}
					break;
				case Resource.Id.umeng_example_analytics_signoff:
					{
						MobclickAgent.OnProfileSignOff();
					}
					break;
			}
		}

		public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
		{
			if(keyCode == Keycode.Back)
			{
				Hook();
				return true;
			}
			return base.OnKeyDown(keyCode, e);
		}

		private void Hook()
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(_context);
			builder.SetPositiveButton("退出应用", (e, s) =>
			{
				MobclickAgent.OnKillProcess(_context);
				int pid = Process.MyPid();
				Process.KillProcess(pid);
			});
			builder.SetNeutralButton("退出", (e, s) =>
			{
				Finish();
			});
			builder.SetNegativeButton("取消", (e, s) =>
			{

			});
			builder.Show();
		}
	}
}

