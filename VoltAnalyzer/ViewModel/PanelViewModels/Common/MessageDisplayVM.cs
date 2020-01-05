using GalaSoft.MvvmLight.Command;
using LanguageManager;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VoltAnalyzer.ValueObjects.Types.Common;

namespace ViewModels.PanelViewModels.Common
{
    public class MessageDisplayVM : ViewModelBase
    {
        #region "Properties"

        private String _messageDetail;
        public String MessageDetail
        {
            get
            {
                return _messageDetail;
            }
            set
            {
                _messageDetail = value;
                RaisePropertyChanged("MessageDetail");
            }
        }

        private String _message;
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        private bool _showMessageDetail;
        public bool ShowMessageDetail
        {
            get
            {
                return _showMessageDetail;
            }
            set
            {
                _showMessageDetail = value;
                RaisePropertyChanged("ShowMessageDetail");
            }
        }

        private Boolean _isDisplayingMessage = false;
        public Boolean IsDisplayingMessage
        {
            get
            {
                return _isDisplayingMessage;
            }
            set
            {
                _isDisplayingMessage = value;
                RaisePropertyChanged("IsDisplayingMessage");
            }
        }

        #endregion

        #region constructor
        public MessageDisplayVM()
        {
            CloseMessageCommand = new RelayCommand(CloseMessage);

            VoltAnalyzerMessage.Subscribe<string>(this, MessageConstants.ShowMessageView, (string _message) =>
            {
                IsDisplayingMessage = true;
                Message = _message;
            });
        }
        #endregion

        #region command execution

        private void CloseMessage()
        {
            IsDisplayingMessage = false;
        }

        #endregion

        #region Command

        private RelayCommand _closeMessageCommand;
        public RelayCommand CloseMessageCommand
        {
            get
            {
                return _closeMessageCommand;
            }
            set
            {
                _closeMessageCommand = value;
                RaisePropertyChanged("CloseMessageCommand");
            }
        }
        
        #endregion
    }
}
