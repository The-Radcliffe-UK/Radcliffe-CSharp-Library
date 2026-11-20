using System;
using System.Collections.Generic;

namespace Radcliffe.ConsoleApp
{
    public class Menu
    {
        private List<MenuItem> _items = new();

        public Menu(List<MenuItem> items)
        {
            _items.AddRange(items);
        }

        public void AddItem(MenuItem item)
        {
            _items.Add(item); 
        }

        public void Display(string selectionMessage)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                IO.Print(i + 1.ToString() + " : " + _items[i]);
            }

            int? selection = null;
            bool _validInput = false;
            while (!_validInput)
            {
                selection = IO.IntInput(selectionMessage);
                _validInput = selection != null || IO.IntRangeCheck(selection, 1, _items.Count);
                if (!_validInput)
                {
                    IO.Print("Please make a valid selection");
                }
            }
            _items[selection.GetValueOrDefault()].Select();
        }
    }
}
