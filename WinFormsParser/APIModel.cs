using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Parser
{

    //  public static class ObservableCollectionExtensions
    // {
    // It's not a common practice to sort ObservableCollection. MS did not implement it out of the box for a reason. Consider reconsidering your logic =)
    // public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
    // {
    //     var sortableList = new List<T>(collection);
    //    sortableList.Sort(comparison);
    //    for (int i = 0; i < sortableList.Count; i++)
    //    {
    // It's a bad practice to mix up items in ObsColl many times, especially if someone subscribes to its CollectionChanged event. Does Move() raise it?
    //      collection.Move(collection.IndexOf(sortableList[i]), i);
    //   }
    // }
    //  }----------------------------------------


    //! API is not a good name. API is a concept.
    // When we talk about a control's API, we mean its public properties and methods. 
    // I think that you can embed this code in the DataGrid class and stop handling events you raise on the same abstraction level.
    class APICore : INotifyPropertyChanged
    {
        public DataTypes DataTypes { get; set; } = new DataTypes();
        public SortDirections _SortDirection = SortDirections.None;
        public SortDirections SortDirection
        {
            get => _SortDirection;
            set
            {
                _SortDirection = value;
                OnPropertyChanged();
            }
        }
        public int SortedColumnIndex { get; set; } = -1;
        public TypeSelector TypeSelector { get; set; } = new TypeSelector();
        public Control EditorControl { get; set; } = new Control();
        public bool IsEditorOpened { get; set; }       
        public bool IsEditorUsed  { get; set; }
        public bool IsTypeSelectorOpened { get; set; } 
        public ObservableCollection<Column> Columns { get; } = new ObservableCollection<Column>();
        public event PropertyChangedEventHandler PropertyChanged;
        public APICore()
        {
            Columns.CollectionChanged += ColumnsChannge;
        }
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private void ColumnsChannge(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int index = 0;
                foreach (var item in Columns)
                {
                    item.Index = index;
                    index++;

                }
                SortDirection = SortDirections.None;
                SortedColumnIndex = -1;

            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int newIndex = e.NewStartingIndex;
                int columnsCount = 0;
                foreach (var item in Columns)
                {
                    if (item.Index == Columns[newIndex].Index)
                    {
                        columnsCount++;
                    }
                }
                if (columnsCount > 1)
                {
                    int maxColumnsIndex = Columns.Max(k => k.Index);
                    Columns[newIndex].Index = maxColumnsIndex + 1;
                }

            }
        }
         
    }

   
    public enum SortDirections
    {
        ASC,
        DESC,
        None
    }
    
    class DataTypes
    {  
        internal Dictionary<string, Type> TypesCollection { get; } = new Dictionary<string, Type>();
        public DataTypes()
        {
            TypesCollection.Add("String", typeof(String));
            TypesCollection.Add("Integer", typeof(Int32));
            TypesCollection.Add("Date/Time", typeof(DateTime));
        }
    
       
        public string GetKeyByValue(Type t)
        {
            string res = "";
            foreach (var item in TypesCollection)
            {
                if (item.Value == t)
                {
                    res = item.Key;
                    break;
                }
            }
            return res;
        }
    }

}
