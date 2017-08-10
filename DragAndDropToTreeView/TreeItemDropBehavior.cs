using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace DragAndDropToTreeView
{
  public class TreeItemDropBehavior
  {
    /// <summary>
    /// AssociatedObject Property
    /// </summary>
    public RadTreeViewItem AssociatedObject { get; set; }

    private static readonly Dictionary<RadTreeViewItem, TreeItemDropBehavior> Instances;

    static TreeItemDropBehavior()
    {
      Instances = new Dictionary<RadTreeViewItem, TreeItemDropBehavior>();
    }

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      var treeViewItem = obj.ParentOfType<RadTreeViewItem>();
      TreeItemDropBehavior behavior = GetAttachedBehavior(treeViewItem);

      behavior.AssociatedObject = treeViewItem;

      if (value)
      {
        behavior.Initialize();
      }
      else
      {
        behavior.CleanUp();
      }

      obj.SetValue(IsEnabledProperty, value);
    }

    // Using a DependencyProperty as the backing store for IsEnabled. This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof(bool),
      typeof(TreeItemDropBehavior),
      new PropertyMetadata(OnIsEnabledPropertyChanged));

    public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      SetIsEnabled(dependencyObject, (bool)e.NewValue);
    }

    private static TreeItemDropBehavior GetAttachedBehavior(RadTreeViewItem treeViewItem)
    {
      if (!Instances.ContainsKey(treeViewItem))
      {
        Instances[treeViewItem] = new TreeItemDropBehavior { AssociatedObject = treeViewItem };
      }

      return Instances[treeViewItem];
    }

    protected virtual void Initialize()
    {
      DragDropManager.AddGiveFeedbackHandler(AssociatedObject, OnGiveFeedback);
      DragDropManager.AddDropHandler(AssociatedObject, OnDrop);
    }

    protected virtual void CleanUp()
    {
      DragDropManager.RemoveGiveFeedbackHandler(AssociatedObject, OnGiveFeedback);
      DragDropManager.RemoveDropHandler(AssociatedObject, OnDrop);
    }

    private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs e)
    {
      Debug.WriteLine("TreeItemDropBehavior.OnGiveFeedback {0}", e.Effects);
      e.SetCursor(Cursors.Arrow);
      e.Handled = true;
    }

    private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      Debug.WriteLine("TreeItemDropBehavior.OnDrop: {0}", e.Effects);

      if (e.Effects != DragDropEffects.None)
      {
        var destinationItem = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
        var data = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData") as IList;
        var details = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;

        if (destinationItem != null)
        {
          foreach (var element in data)
          {
            (destinationItem.DataContext as Folder).Elements.Add(element as Element);
          }

          e.Handled = true;
        }
      }
    }
  }
}
