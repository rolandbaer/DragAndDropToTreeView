using System.Collections;
using System.Windows;

namespace DragAndDropToTreeView
{
  public class DropIndicationDetails
  {
    public IList CurrentDraggedItems { get; set; }

    public object DragSource { get; set; }

    public DragDropEffects UsedEffects { get; set; }
  }
}
