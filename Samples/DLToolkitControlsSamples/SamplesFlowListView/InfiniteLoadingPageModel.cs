﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples.SamplesFlowListView
{
    public class InfiniteLoadingPageModel : BasePageModel
    {
		public InfiniteLoadingPageModel()
		{
			ItemTappedCommand = new BaseCommand((param) =>
			{

				var item = LastTappedItem as SimpleItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);

			});

			LoadingCommand = new BaseCommand(async (arg) =>
			{
				await LoadMoreAsync();
			});
		}

		public FlowObservableCollection<object> Items
		{
			get { return GetField<FlowObservableCollection<object>>(); }
			set { SetField(value); }
		}

		public ICommand LoadingCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public void ReloadData()
		{
			var exampleData = new FlowObservableCollection<object>();

			var howMany = 60;
			TotalRecords = 240;

			exampleData.BatchStart();

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
			}

			exampleData.BatchEnd();

			Items = exampleData;
		}

		public ICommand ItemTappedCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public object LastTappedItem
		{
			get { return GetField<object>(); }
			set { SetField(value); }
		}

		public bool IsLoadingInfinite
		{
			get { return GetField<bool>(); }
			set { SetField(value); }
		}

		public int TotalRecords
		{
			get { return GetField<int>(); }
			set { SetField(value); }
		}

		protected virtual async Task LoadMoreAsync()
		{
			var oldTotal = Items.Count;

			await Task.Delay(3000);

			var howMany = 60;

			Items.BatchStart();

			for (int i = oldTotal; i < oldTotal + howMany; i++)
			{
				Items.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
			}

			Items.BatchEnd();

			IsLoadingInfinite = false;
		}

		public class SimpleItem : BaseModel
		{
			string title;
			public string Title
			{
				get { return title; }
				set { SetField(ref title, value); }
			}

			public Color Color { get; private set; } = Colors.RandomColor;
		}
    }
}
