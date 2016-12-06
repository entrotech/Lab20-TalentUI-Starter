using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Talent.DataAccess.Ado;
using Talent.Domain;
using UclaExt.Common.BaseClasses;

namespace Talent.WpfClient
{
    public class GenresViewModel : INotifyPropertyChanged
    {
        private GenreRepository _repo = new GenreRepository();
        private ObservableCollection<Genre> _items;
        private Genre _selectedItem;

        public GenresViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }

            SearchCommand = new RelayCommand(OnSearch, CanSearch);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAdd, CanAdd);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel, CanCancel);
        }

        #region Properties

        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public ObservableCollection<Genre> Items
        {
            get
            {
                return _items;
            }

            private set  // Can only set via private class methods
            {
                if (_items == value) return;
                _items = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedItem");
            }
        }

        public Genre SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged -= OnModelPropertyChanged;
                }
                _selectedItem = value;
                OnPropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                AddCommand.RaiseCanExecuteChanged();
                SearchCommand.RaiseCanExecuteChanged();
                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged += OnModelPropertyChanged;
                }
            }
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseAllCanExecuteChanged();
        }

        protected virtual void RaiseAllCanExecuteChanged()
        {
            SearchCommand.RaiseCanExecuteChanged();
            AddCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        #endregion // Properties

        #region Command Implementation

        private bool CanSearch()
        {
            return SelectedItem == null
                || !SelectedItem.IsDirty;
        }

        private void OnSearch()
        {
            Items = new ObservableCollection<Genre>(_repo.Fetch());
        }

        private bool CanAdd()
        {
            return Items != null
                && (SelectedItem == null
                || !SelectedItem.IsDirty);
        }

        public void OnAdd()
        {
            Items.Add(new Genre());
            SelectedItem = Items.LastOrDefault();
        }

        private bool CanDelete()
        {
            return SelectedItem != null;
        }

        private void OnDelete()
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsMarkedForDeletion = true;
                try
                {
                    _repo.Persist(SelectedItem);
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                }
                catch
                {
                    return;
                }
            }
        }

        private bool CanCancel()
        {
            return SelectedItem != null 
                && SelectedItem.IsDirty;
        }

        private void OnCancel()
        {
            var selectedItem = SelectedItem;
            OnSearch();
            SelectedItem = Items
                .Where(r => r == selectedItem)
                .FirstOrDefault();
        }

        private bool CanSave()
        {
            return SelectedItem != null 
                && SelectedItem.IsDirty;
        }

        private void OnSave()
        {
            try
            {
                SelectedItem = _repo.Persist(SelectedItem);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error saving changes: " 
                    + ex.Message, ex);
            }
        }

        #endregion // Commands



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "None")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
