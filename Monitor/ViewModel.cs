using SharedModels;
using SharedModels.ProgressThread;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Monitor
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            storageHelper = new StorageHelper();
            storageHelper.MessagesArrived += StorageHelper_MessageArrived;
            storageHelper.IsRunning += StorageHelper_IsRunning;
        }

        private void StorageHelper_IsRunning(bool IsRunning)
        {
            this.IsRunning = IsRunning;
        }

        private void StorageHelper_MessageArrived(DemoMessage[] messages)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                Messages.Clear();
                foreach (var m in messages)
                {
                    Messages.Add(m);
                }
            });
            
            
        }

        public ObservableCollection<DemoMessage> Messages { get; }
            = new ObservableCollection<DemoMessage>();

        private readonly StorageHelper storageHelper;

        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                NotifyPropertyChanged("IsRunning");
            }
        }

        private ICommand startCommand;
        public ICommand Start
        {
            get
            {
                return startCommand ?? (startCommand = new RelayCommand(x =>
                {
                    storageHelper.Start();
                }, y => true));
            }
        }

        private ICommand stopCommand;
        private bool isRunning;

        public ICommand Stop
        {
            get
            {
                return stopCommand ?? (stopCommand = new RelayCommand(x =>
                {
                    storageHelper.Stop();
                }, y => true));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }





    }
}
