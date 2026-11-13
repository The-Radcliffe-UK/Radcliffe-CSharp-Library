using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace Radcliffe.WinCore.Printing
{
    public class Printer : IDisposable
    {
        public static List<string> GetAvailablePrinters()
        {
            // Display available printers
            List<string> availablePrinters = new List<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                availablePrinters.Add(printer);
            }

            return availablePrinters;
        }

        public PageSettings PageSettings => _printDocument.DefaultPageSettings;
        public bool valid { get { return _valid; } }
        public List<string> errorLog { get { return _errorLog; } }
        public bool printing { get { return _printing; } }

        private PrintDocument _printDocument = new();
        private string _printerName = "";
        private bool _valid = true;
        private List<string> _errorLog = new();
        private Queue<Action<Graphics>> _printQueue = new();
        private Action<Graphics> _currentJob;
        private bool _printing = false;

        public Printer(string printerName)
        {
            try
            {
                _printDocument.PrinterSettings.PrinterName = printerName;
                _printerName = printerName;
                _printDocument.PrintPage += PrintHandler;
                _valid = _printDocument.PrinterSettings.IsValid;
            }
            catch (Exception ex)
            {
                _errorLog.Add(ex.ToString());
                _valid = false; 
            }
        }

        public void Print(Action<Graphics> page) {
            if (!_valid) { throw new InvalidOperationException($"Printer '{_printerName}' is not valid."); }

            _printQueue.Enqueue(page);
            if (!_printing)
            {
                ProcessQueue();
            }
        }

        private void ProcessQueue()
        {
            _printing = true;
            try
            {
                while (_printQueue.Count > 0)
                {
                    _currentJob = _printQueue.Dequeue();

                    try
                    {
                        _printDocument.Print();
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Add(ex.ToString());
                    }
                    finally
                    {
                        _currentJob = null;
                    }
                }
            }
            finally
            {
                _printing = false;
            }
        }

        private void PrintHandler(object sender, PrintPageEventArgs e)
        {
            if(e.Graphics != null)
            {
                _currentJob(e.Graphics);
            }

            e.HasMorePages = false;
        }

        public void Dispose()
        {
            _printDocument.Dispose();
        }

    }
}