using System;
using System.Threading;
/// <summary>
/// Namespace is used to organize the classes
/// </summary>
namespace Assignment2
{
    /// <summary>
    /// Class stock consists of constructor and methods required to monitor changes in the stock.
    /// </summary>
    class Stock
    {
        /// <summary>
        /// variable name consists of the name of the stock.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Variable initialValue consists of the initial value of the stock.
        /// </summary>
        public double initialValue { get; set; }
        /// <summary>
        /// Variable currentValue consists of the current value of the stock after the changes.
        /// </summary>
        public double currentValue { get; set; }
        /// <summary>
        /// Variable maxChange specifies the limit of the change a stock can undergo
        /// </summary>
        public int maxChange { get; set; }
        /// <summary>
        /// Specifies the total number of changes made.
        /// </summary>
        public int numberChanges { get; set; }
        /// <summary>
        /// The threshold above or below  which the collection of brokers who control the stock must be notified.
        /// </summary>
        public double priceTreshold { get; set; }
        /// <summary>
        /// Create a new thread.
        /// </summary>
        private Thread thread;
        /// <summary>
        /// Declare an event.
        /// </summary>
        public event EventHandler<EventData> StockEvent;

        /// <summary>
        /// Constructor for the class Stock which sets the values and starts the thread.
        /// </summary>
        /// <param name="name">
        /// Sets the name of the stock.
        /// </param>
        /// <param name="value">
        /// Sets the initial value of the stock.
        /// </param>
        /// <param name="change">
        /// sets the maximum value of the stock.
        /// </param>
        /// <param name="priceTreshold">
        /// sets the threshold of the stock.
        /// </param>
        public Stock(string name, double value, int change, double priceTreshold)
        {
            this.name = name;
            this.initialValue = value;
            this.currentValue = this.initialValue;
            this.maxChange = change;
            this.numberChanges = 0;
            this.priceTreshold = priceTreshold;
            thread = new Thread(new ThreadStart(Update));
            thread.Start();
        } 
        /// <summary>
        /// randomstock creates a new random object.
        /// </summary>
        Random randomstock = new Random();
        /// <summary>
        /// This method updates the stocks and calls the method ChangeStockValue.
        /// </summary>
        public void Update()
        {
            for (int c= 0;c< 15 ;c++ )
            {
                // Thread sleep for 500 ms
                Thread.Sleep(500);
                // Method that changes the stock value.
                ChangeStockValue();
            }
        }
        /// <summary>
        /// This method assigns a random value to the stock and raises the event.
        /// </summary>
        public void ChangeStockValue()
        {
            // Assign random integer between 1 - maxChange
            currentValue += randomstock.Next(1, (maxChange + 1));
            //Increase the number of change by 1
            numberChanges++;
            //Check whether the threshold is reached
            if ((currentValue - initialValue) > priceTreshold)
            {
                //Raise the event
                EventData temp = new EventData();
                temp.stockName = name;
                temp.currentValue = currentValue;
                temp.numOfChange = numberChanges;
                OnStockEvent(temp);
            }
        } // end method ChangeStockValue
        /// <summary>
        /// Defines a delegate.
        /// </summary>
        /// <param name="stockName">
        /// Pass stock name to the variable.
        /// </param>
        /// <param name="currentValue">
        /// Pass the present stock value to the stock name.
        /// </param>
        /// <param name="numberChanges">
        /// pass the number of changes 
        /// </param>
        public delegate void StockNotification(String stockName, int currentValue, int numberChanges);
        /// <summary>
        /// Define an event based on that delegate.
        /// </summary>
        public event StockNotification StockThresholdReachedEvent;
        /// <summary>
        /// This method is used to raise the event
        /// </summary>
        /// <param name="e">
        /// EventData object is passed as a parameter.
        /// </param>
        protected virtual void OnStockEvent(EventData e)
        {
            EventHandler<EventData> handler = StockEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        } // end method OnStockEvent

       
        
    } // end class Stock
    /// <summary>
    /// Class EventData stores the event data when the event is raised.
    /// </summary>
    public class EventData : EventArgs
    {
        public String stockName { get; set; }
        public double currentValue { get; set; }
        public int numOfChange { get; set; }
    }//end class EventData
}//end Assignment2