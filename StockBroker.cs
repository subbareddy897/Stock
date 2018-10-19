using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
/// <summary>
/// Namespace is used to organize the classes
/// </summary>
namespace Assignment2
{
    /// <summary>
    /// Contains method addStock, Filehandler and event handlers.
    /// </summary>
    class StockBroker
    {
        /// <summary>
        /// Declare the mutex lock.
        /// </summary>
        private static Mutex mut = new Mutex();
        /// <summary>
        /// Directory of the text file to be created.
        /// </summary>
        string path = @"C:\Users\subba\Downloads\dotnet\EventLog.txt";
        /// <summary>
        /// Variable Name stores the brokers name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Variable StockList stores the names of the stocks that the broker manages.
        /// </summary>
        public List<Stock> StockList { get; set; }

        /// <summary>
        /// Constructor sets the name and creates a new StockList.
        /// </summary>
        /// <param name="name">
        /// Consists of the name of the stock broker.
        /// </param>
        public StockBroker(string name)
        {
            Name = name;
            StockList = new List<Stock>();
        } // end constructor StockBroker

        /// <summary>
        /// This method adds the stocks to the stock list.
        /// </summary>
        /// <param name="s">
        /// Object of the stock is passed as a parameter.
        /// </param>
        public void AddStock(Stock s)
        {
            StockList.Add(s);
            s.StockEvent += MyEventHandler;
        } // end method AddStock
        /// <summary>
        /// This method handles the event that is raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">
        /// Object of the class that contains the event data.
        /// </param>
        void MyEventHandler(Object sender, EventData e)
        {
            // Wait until safe to run
            mut.WaitOne();
            // Writing data to console
           Console.WriteLine(Name.PadRight(15) + e.stockName.PadRight(15) +
                            e.currentValue.ToString().PadRight(15) +
                             e.numOfChange.ToString());
            if (!File.Exists(path))
            {
                // Create a file to write 
                using (StreamWriter sw = File.CreateText(path))
                {
                    // Write the table header to the file.
                    sw.WriteLine("Broker".PadRight(10) + "Stock".PadRight(10) +
                                 "Value".PadRight(10) + "Changes".PadRight(10) +
                                 "\r\n");
                }
            }
            using (StreamWriter sw = File.AppendText(path))
            {
                //Write the event data to the file
                sw.WriteLine(Name.PadRight(10) + e.stockName.PadRight(10) +
                              e.currentValue.ToString().PadRight(10) +
                              e.numOfChange.ToString());
            }
            // Release lock
            mut.ReleaseMutex();
        } // End method MyEventHandler
    } // End class StockBroker
}//End namespace