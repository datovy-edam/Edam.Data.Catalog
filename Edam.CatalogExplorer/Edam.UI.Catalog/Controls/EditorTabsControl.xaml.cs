using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Edam.UI.Catalog.Models;
using Edam.Data.CatalogModel;
using Edam.Diagnostics;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
// https://learn.microsoft.com/en-us/windows/apps/design/controls/tab-view

namespace Edam.UI.Catalog.Controls;

public sealed partial class EditorTabsControl : UserControl
{
    private int _counter = 0;
    private Monaco.MonacoEditor _MonacoEditor;
    private MonacoEditorPage _SelectedEditor;
    private EditorTabsViewModel _viewModel = new EditorTabsViewModel();
    public EditorTabsViewModel ViewModel
    {
        get { return _viewModel; }
    }

    public EditorTabsControl()
    {
        this.InitializeComponent();
        this.DataContext = _viewModel;
    }

    /// <summary>
    /// Manage Notification Async.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public async Task ManageNotificationAsync(
        object? sender, ItemContentNotificationArgs args)
    {
        string content = null;
        string docName = string.Empty;
        CatalogPathItem pathItem = null;

        if (args.ItemContent == null || args.ItemContent.ItemInstance == null ||
            String.IsNullOrWhiteSpace(args.ItemContent.Content))
        {
            content = string.Empty;
            docName = "[editing document name]";
        }
        else
        {
            content = args.ItemContent.Content;
            var citem = args.ItemContent.ItemInstance as CatalogItemInfo;
            pathItem = citem.Tag as CatalogPathItem;
            docName = citem.Title;
        }

        args.Results.Clear();
        switch (args.Type)
        {
            case ItemContentNotificationType.GetContent:
                args.ItemContent.Content = await _SelectedEditor.GetContent();
                args.Results.Succeeded();
                break;
            case ItemContentNotificationType.SetContent:
                ViewModel.CurrentPathItem = pathItem;

                AddTab(pathItem, content);

                args.Results.Succeeded();
                break;
            default:
                args.Results.Failed(EventCode.Failed.ToString());
                break;
        }
    }

    private void AddTab(CatalogPathItem item, string content)
    {
        MonacoEditorViewModel model = new MonacoEditorViewModel();
        model.CurrentPathItem = item;
        model.Content = content;
        model.DocumentName = item.TreeItem.Title;
        ViewModel.EditorTabs.Add(model);
        EditorTabs.SelectedItem = model;
        model.ModelIndex = _counter;
        _counter++;
    }

    private void TabView_TabCloseRequested(
        TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        sender.TabItems.Remove(args.Tab);
    }

    private void TabView_SizeChanged(object sender, SizeChangedEventArgs args)
    {
        double asize = this.ActualHeight - (TabMenu.ActualHeight);
        EditorTabs.Height = asize;
    }

    private void MonacoEditor_Loaded(object sender, RoutedEventArgs e)
    {
        var model = EditorTabs.SelectedItem as MonacoEditorViewModel;
        var editor = sender as Monaco.MonacoEditor;
        if (editor == null || editor.Tag == null)
        {
            _MonacoEditor = editor;
            _MonacoEditor.Tag = model;
        }
    }

}
