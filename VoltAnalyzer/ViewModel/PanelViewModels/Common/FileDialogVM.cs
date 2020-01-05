using GalaSoft.MvvmLight.Command;
using LanguageManager;
using VoltAnalyzer.Utils.FileDialog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.ViewModel.PanelViewModels.Common
{
    public class FileDialogVM: ViewModelBase
    {
        public FileDialogVM()
        {
            this.SaveCommand = new RelayCommand(this.SaveFile);
            this.OpenCommand = new RelayCommand(this.OpenFile);
            this.BrowseFolderCommand = new RelayCommand(this.FolderBrowser);
        }

        #region Properties

        public Stream Stream
        {
            get;
            set;
        }

        public string SelectedFolder
        {
            get;
            set;
        }

        public string Extension
        {
            get;
            set;
        }

        public string Filter
        {
            get;
            set;
        }

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand;
            }
            set
            {
                _openCommand = value;
                RaisePropertyChanged("OpenCommand");
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand;
            }
            set
            {
                _saveCommand = value;
                RaisePropertyChanged("SaveCommand");
            }
        }

        private RelayCommand _browseFolderCommand;
        public RelayCommand BrowseFolderCommand
        {
            get
            {
                return _browseFolderCommand;
            }
            set
            {
                _browseFolderCommand = value;
                RaisePropertyChanged("BrowseFolderCommand");
            }
        }
        #endregion

        private void OpenFile()
        {
            FileDialog fileServices = new FileDialog();
            this.Stream = fileServices.OpenFile(this.Extension, this.Filter);
        }

        private void SaveFile()
        {
            FileDialog fileServices = new FileDialog();
            this.Stream = fileServices.SaveFile(this.Extension, this.Filter);
        }

        private void FolderBrowser()
        {
            FileDialog fileServices = new FileDialog();
            this.SelectedFolder = fileServices.BrowseFolder();
        }
    }
}
