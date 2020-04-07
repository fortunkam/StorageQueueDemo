using SharedModels.ProgressThread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Receiver
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            storageHelper = new StorageHelper();
        }

        private readonly StorageHelper storageHelper;

        private ICommand getCommand;
        private string name;
        private string message;
        private string time;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GetMessage
        {
            get
            {
                return getCommand ?? (getCommand = new RelayCommand(x =>
                {
                    var message = storageHelper.GetMessage();
                    Name = message.Name;
                    Message = message.Message;
                    Time = message.Time.ToString("HH:mm:ss.zzzz");
                }));
            }
        }

        public string Name
        {
            get => name; set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
        public string Message
        {
            get => message; set
            {
                message = value;
                NotifyPropertyChanged();
            }
        }
        public string Time { get => time; set
            {
                time = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
