using System.Collections.ObjectModel;

namespace DragAndDropToTreeView
{
  public class Folder
  {
    public string Name { get; set; }

    public ObservableCollection<Folder> Children { get; set; }

    public ObservableCollection<Element> Elements { get; set; }

    public Folder()
    {
      Children = new ObservableCollection<Folder>();
      Elements = new ObservableCollection<Element>();
    }
  }
}
