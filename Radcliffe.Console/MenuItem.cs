using Radcliffe.Core.EventSystem;
using System;
using System.Threading.Tasks;

namespace Radcliffe.ConsoleApp
{
    public class MenuItem
    {
        public string title { get { return _title; } }
        private string _title = "Item Title No Set";
        private Event _onSelect = new Event();

        public MenuItem(string title, Action onSelect)   
        {
            _title = title;
            RegisterOnSelect(onSelect);
        }

        public MenuItem(string title, Func<Task> asyncOnSelect)
        {
            _title = title;
            RegisterOnSelect(asyncOnSelect);
        }

        public void Select()
        {
            _onSelect.Raise();
        }

        public async Task AsyncSelect()
        {
            await _onSelect.AsyncRaise();
        }

        public void RegisterOnSelect(Action onSelect)
        {
            _onSelect.RegisterListener(onSelect);
        }

        public void RegisterOnSelect(Func<Task> asyncOnSelect)
        {
            _onSelect.RegisterAsyncListener(asyncOnSelect);
        }

        public void UnregisterOnSelect(Action onSelect)
        {
            _onSelect.UnregisterListener(onSelect);
        }

        public void UnregisterOnSelect(Func<Task> asyncOnSelect)
        {
            _onSelect.UnregisterAsyncListener(asyncOnSelect);
        }
    }
}
