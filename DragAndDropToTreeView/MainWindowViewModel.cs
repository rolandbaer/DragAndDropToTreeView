using System.Collections.ObjectModel;
using System.Linq;

namespace DragAndDropToTreeView
{
  public class MainWindowViewModel
  {
    public ObservableCollection<Element> AllElements { get; private set; }

    public ReadOnlyObservableCollection<Folder> Folders { get; private set; }

    public Folder SelectedFolder { get; set; }

    public MainWindowViewModel()
    {
      CreateElements();
      CreateFolders();

      SelectedFolder = Folders.First();
    }

    public void CreateElements()
    {
      var elements = new ObservableCollection<Element>
                    {
                      new Element { Name = "Element 1", Description = "The First Element" },
                      new Element { Name = "Element 2", Description = "The Second Element" },
                      new Element { Name = "Element 3", Description = "The Third Element" },
                      new Element { Name = "Element 4", Description = "The Fourth Element" },
                      new Element { Name = "Element 5", Description = "The Fifth Element" },
                      new Element { Name = "Element 6", Description = "The Sixth Element" },
                      new Element { Name = "Element 7", Description = "The Seventh Element" },
                      new Element { Name = "Element 8", Description = "The Eighth Element" },
                    };
      AllElements = new ObservableCollection<Element>(elements);
    }

    public void CreateFolders()
    {
      var folders = new ObservableCollection<Folder>();
      var allUnitsFolder = new Folder { Name = "All Units", Elements = AllElements };
      folders.Add(allUnitsFolder);
      var departmentA = new Folder { Name = "Folder A" };
      departmentA.Children.Add(new Folder { Name = "Subfolder A1" });
      departmentA.Children.Add(new Folder { Name = "Subfolder A2" });
      departmentA.Children.Add(new Folder { Name = "Subfolder A3" });
      folders.Add(departmentA);
      var departmentB = new Folder { Name = "Folder B" };
      departmentB.Children.Add(new Folder { Name = "Subfolder B1" });
      departmentB.Children.Add(new Folder { Name = "Subfolder B2" });
      departmentB.Children.Add(new Folder { Name = "Subfolder B3" });
      folders.Add(departmentB);
      Folders = new ReadOnlyObservableCollection<Folder>(folders);
    }
  }
}
