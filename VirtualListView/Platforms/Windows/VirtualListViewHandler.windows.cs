﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using System;
using WStackLayout = Microsoft.UI.Xaml.Controls.StackLayout;

namespace Microsoft.Maui
{
	public partial class VirtualListViewHandler : ViewHandler<IVirtualListView, ItemsRepeaterScrollHost>
	{
		ItemsRepeaterScrollHost itemsRepeaterScrollHost;
		ScrollViewer scrollViewer;
		ItemsRepeater itemsRepeater;
		//IrDataTemplateSelector dataTemplateSelector;
		IrSource irSource;
		IrDataTemplateSelector templateSelector;

		internal PositionalViewSelector PositionalViewSelector { get; private set; }

		protected override ItemsRepeaterScrollHost CreateNativeView()
		{
			itemsRepeaterScrollHost = new ItemsRepeaterScrollHost();
			scrollViewer = new ScrollViewer();
			itemsRepeater = new ItemsRepeater();

			scrollViewer.Content = itemsRepeater;
			itemsRepeaterScrollHost.ScrollViewer = scrollViewer;

			return itemsRepeaterScrollHost;
		}

		protected override void ConnectHandler(ItemsRepeaterScrollHost nativeView)
		{
			base.ConnectHandler(nativeView);

			templateSelector = new IrDataTemplateSelector(VirtualView);
			itemsRepeater.ItemTemplate = templateSelector;
			itemsRepeater.Layout = new WStackLayout
			{
				Orientation = VirtualView.Orientation switch
				{
					ListOrientation.Vertical => Orientation.Vertical,
					ListOrientation.Horizontal => Orientation.Horizontal,
					_ => Orientation.Vertical
				}
			};

			PositionalViewSelector = new PositionalViewSelector(VirtualView);
			irSource = new IrSource(MauiContext, PositionalViewSelector, VirtualView);

			itemsRepeater.ItemsSource = irSource;
		}

		protected override void DisconnectHandler(ItemsRepeaterScrollHost nativeView)
		{
			itemsRepeater.ItemTemplate = null;
			//dataTemplateSelector.Dispose();
			//dataTemplateSelector = null;

			itemsRepeater.ItemsSource = null;
			irSource = null;

			base.DisconnectHandler(nativeView);
		}

		public void InvalidateData()
		{
			//dataTemplateSelector?.Reset();
			irSource?.Reset();
		}

		public static void MapAdapter(VirtualListViewHandler handler, IVirtualListView virtualListView)
			=> handler?.InvalidateData();

		public static void MapHeader(VirtualListViewHandler handler, IVirtualListView virtualListView)
			=> handler?.InvalidateData();

		public static void MapFooter(VirtualListViewHandler handler, IVirtualListView virtualListView)
			=> handler?.InvalidateData();

		public static void MapViewSelector(VirtualListViewHandler handler, IVirtualListView virtualListView)
			=> handler?.InvalidateData();

		public static void MapSelectionMode(VirtualListViewHandler handler, IVirtualListView virtualListView)
		{ }

		public static void MapInvalidateData(VirtualListViewHandler handler, IVirtualListView virtualListView)
			=> handler?.InvalidateData();

		public static void MapSetSelected(VirtualListViewHandler handler, IVirtualListView virtualListView, object? parameter)
		{
			if (parameter is ItemPosition[] items)
			{
				//
			}
		}

		public static void MapSetDeselected(VirtualListViewHandler handler, IVirtualListView virtualListView, object? parameter)
		{
			if (parameter is ItemPosition[] items)
			{
				//
			}
		}

		public static void MapOrientation(VirtualListViewHandler handler, IVirtualListView virtualListView)
		{
			handler.itemsRepeater.Layout = new WStackLayout
			{
				Orientation = virtualListView.Orientation switch
				{
					ListOrientation.Vertical => Orientation.Vertical,
					ListOrientation.Horizontal => Orientation.Horizontal,
					_ => Orientation.Vertical
				}
			};
			handler.InvalidateData();
		}

		internal static void AddLibraryResources(string key, string uri)
		{
			var resources = UI.Xaml.Application.Current?.Resources;
			if (resources == null)
				return;

			var dictionaries = resources.MergedDictionaries;
			if (dictionaries == null)
				return;

			if (!resources.ContainsKey(key))
			{
				dictionaries.Add(new UI.Xaml.ResourceDictionary
				{
					Source = new Uri(uri)
				});
			}
		}
	}
}
