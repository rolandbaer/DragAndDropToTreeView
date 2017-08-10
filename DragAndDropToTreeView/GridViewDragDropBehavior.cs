using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;

using GiveFeedbackEventArgs = Telerik.Windows.DragDrop.GiveFeedbackEventArgs;

namespace DragAndDropToTreeView
{
  public class GridViewDragDropBehavior
  {
    public RadGridView AssociatedControl { get; set; }

    private static readonly Dictionary<RadGridView, GridViewDragDropBehavior> Instances;

    static GridViewDragDropBehavior()
    {
      Instances = new Dictionary<RadGridView, GridViewDragDropBehavior>();
    }

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      GridViewDragDropBehavior behavior = GetAttachedBehavior(obj as RadGridView);

      behavior.AssociatedControl = obj as RadGridView;

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

    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof(bool),
      typeof(GridViewDragDropBehavior),
      new PropertyMetadata(OnIsEnabledPropertyChanged));

    public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      SetIsEnabled(dependencyObject, (bool)e.NewValue);
    }

    private static GridViewDragDropBehavior GetAttachedBehavior(RadGridView gridView)
    {
      if (!Instances.ContainsKey(gridView))
      {
        Instances[gridView] = new GridViewDragDropBehavior { AssociatedControl = gridView };
      }

      return Instances[gridView];
    }

    protected virtual void Initialize()
    {
      UnsubscribeFromDragDropEvents();
      SubscribeToDragDropEvents();
    }

    protected virtual void CleanUp()
    {
      UnsubscribeFromDragDropEvents();
    }

    private void SubscribeToDragDropEvents()
    {
      DragDropManager.AddDragInitializeHandler(AssociatedControl, OnDragInitialize);
      DragDropManager.AddGiveFeedbackHandler(AssociatedControl, OnGiveFeedback);
      DragDropManager.AddDragDropCompletedHandler(AssociatedControl, OnDragDropCompleted);
      DragDropManager.AddDragOverHandler(AssociatedControl, OnDragOver);
    }

    private void UnsubscribeFromDragDropEvents()
    {
      DragDropManager.RemoveDragInitializeHandler(AssociatedControl, OnDragInitialize);
      DragDropManager.RemoveGiveFeedbackHandler(AssociatedControl, OnGiveFeedback);
      DragDropManager.RemoveDragDropCompletedHandler(AssociatedControl, OnDragDropCompleted);
      DragDropManager.RemoveDragOverHandler(AssociatedControl, OnDragOver);
    }

    private void OnDragInitialize(object sender, DragInitializeEventArgs e)
    {
      DropIndicationDetails details = new DropIndicationDetails();
      var gridView = sender as RadGridView;
      details.DragSource = gridView.ItemsSource;

      var items = gridView.SelectedItems;
      details.CurrentDraggedItems = items;

      IDragPayload dragPayload = DragDropPayloadManager.GeneratePayload(null);

      dragPayload.SetData("DraggedData", items);
      dragPayload.SetData("DropDetails", details);

      e.Data = dragPayload;

      e.DragVisual = new DragVisual { Content = details, ContentTemplate = AssociatedControl.Resources["DraggedItemTemplate"] as DataTemplate };
      e.DragVisualOffset = new Point(e.RelativeStartPoint.X + 10, e.RelativeStartPoint.Y);
      e.AllowedEffects = DragDropEffects.All;
    }

    private void OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      Debug.WriteLine("GridViewDragDropBehavior.OnGiveFeedback {0}", e.Effects);
      e.SetCursor(Cursors.Arrow);
      e.Handled = true;
    }

    private void OnDragDropCompleted(object sender, DragDropCompletedEventArgs e)
    {
      Debug.WriteLine("GridViewDragDropBehavior.OnDragDropCompleted: {0}", e.Effects);
      var data = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData") as IList;
      var details = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails");
      Debug.WriteLine(e.Effects);

      // Remove Element from source list if drag drop effect is move
      /*if (e.Effects == DragDropEffects.Move)
      {
        var collection = (details as DropIndicationDetails).DragSource as IList;
        foreach(var element in data)
        {
          collection.Remove(element);
        }
      }*/
    }

    private void OnDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      Debug.WriteLine("GridViewDragDropBehavior.OnDragOver: {0}", e.Effects);
      e.Effects = DragDropEffects.None;
      e.Handled = true;
    }
  }
}
