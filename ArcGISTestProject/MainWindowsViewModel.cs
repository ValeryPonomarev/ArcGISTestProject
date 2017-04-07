using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;

namespace ArcGISTestProject
{
    public class MainWindowsViewModel : INotifyPropertyChanged
    {
        private CollectionView maps;
        private CollectionView layers;
        private IList<string> mapsList;
        private IList<string> layersList;
        private string currentMap;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowsViewModel()
        {
            mapsList = new List<string>();
            maps = new CollectionView(mapsList);

            layersList = new List<string>();
            layers = new CollectionView(layersList);
        }

        #region properties
        public CollectionView Maps {
            get { return maps; }
        }
        
        public string CurrentMap
        {
            get
            {
                return currentMap;
            }

            set
            {
                currentMap = value;
                NotifyPropertyChanged("CurrentMap");
            }
        }

        public CollectionView Layers
        {
            get
            {
                return layers;
            }
        }
        #endregion

        public void AddMap(string map) {
            mapsList.Add(map);
            maps.Refresh();
        }

        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }
    }
}
