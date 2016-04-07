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
using Com.Umeng.Analytics;
using Android.Graphics;

namespace UMeng.Analytics.Demo
{
	[Activity(Label = "FragmentStack")]
	public class FragmentStack : Activity
	{
		private int _stackLevel = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.umeng_example_analytics_fragment_stack);

			Button button = FindViewById<Button>(Resource.Id.new_fragment);
			button.Click += (e, s) =>
			{
				AddFragmentToStack();
			};

			if (savedInstanceState == null)
			{
				Fragment newFragment = CountingFragment.NewInstance(this, _stackLevel);
				FragmentManager.BeginTransaction().Add(Resource.Id.simple_fragment, newFragment).Commit();
			}
			else
			{
				_stackLevel = savedInstanceState.GetInt("level");
			}
		}

		public override void OnSaveInstanceState(Bundle outState, PersistableBundle outPersistentState)
		{
			base.OnSaveInstanceState(outState, outPersistentState);
			outState.PutInt("level", _stackLevel);
		}

		protected override void OnPause()
		{
			base.OnPause();
			MobclickAgent.OnPause(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			MobclickAgent.OnResume(this);
		}

		void AddFragmentToStack()
		{
			_stackLevel++;

			Fragment newFragment = CountingFragment.NewInstance(this, _stackLevel);
			FragmentManager.BeginTransaction().Replace(Resource.Id.simple_fragment, newFragment)
				.SetTransition(FragmentTransit.FragmentOpen)
				.AddToBackStack(null)
				.Commit();
		}

		public class CountingFragment : Fragment
		{
			private string _pageName;
			private int _num;
			private Context _context;

			public CountingFragment(Context context)
			{
				_context = context;
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

			public static CountingFragment NewInstance(Context context, int num)
			{
				CountingFragment f = new CountingFragment(context);

				Bundle args = new Bundle();
				args.PutInt("num", num);
				f.Arguments = args;
				return f;
			}

			public override void OnCreate(Bundle savedInstanceState)
			{
				base.OnCreate(savedInstanceState);
				_num = Arguments == null ? 1 : Arguments.GetInt("num");
				_pageName = $"fragment {_num}";
			}

			public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
			{
				FrameLayout fl = new FrameLayout(_context);
				fl.LayoutParameters = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent,
					FrameLayout.LayoutParams.MatchParent);
				fl.SetBackgroundColor(Color.Black);
				TextView tv = new TextView(_context);
				tv.Text = $"Fragment #{_num}";
				tv.SetTextColor(Color.Black);
				fl.AddView(tv);
				return fl;
			}
		}
	}
}