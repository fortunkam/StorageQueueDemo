using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Client
{
    public class ViewModel
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public ICommand SendMessage { get; } = new SendMessageCommand();
    }
}
