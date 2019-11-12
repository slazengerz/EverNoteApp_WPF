using EverNoteApp.Model;
using EverNoteLatest.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverNoteLatest.ViewModel
{
    class ExampleWindowVM
    {
        public ExampleCommand ExampleCommand { get; set; }
        public Users Users { get; set; }

        public ExampleWindowVM()
        {
            Users = new Users();
            ExampleCommand = new ExampleCommand(this);
        }

        public override string ToString()
        {
            Console.WriteLine("this is the end my friend");
            return "this is the end my friend";
        }
    }
}
