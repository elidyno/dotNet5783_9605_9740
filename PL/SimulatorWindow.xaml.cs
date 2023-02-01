using PL.Order;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PL
{
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// The SimulatorWindow class represents the main window for the simulation.
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        // A background worker instance to run the simulation process.
        BackgroundWorker worker;
        // A stopwatch instance to keep track of the elapsed time of the simulation.
        Stopwatch stopWatch;
        // A boolean variable to prevent the window from closing.
        bool _preventClosing = true;

        // A Dependency Property to store the data of the simulation.
        public static readonly DependencyProperty ArrayListProperty = DependencyProperty.Register(
        "data", typeof(ArrayList), typeof(SimulatorWindow), new PropertyMetadata(default(ArrayList)));

        // Property for the Dependency Property "Order"
        public ArrayList data
        {
            get => (ArrayList)GetValue(ArrayListProperty);
            set => SetValue(ArrayListProperty, value);
        }

        /// <summary>
        /// Constructor for the SimulatorWindow class.
        /// Initializes the window and starts the background worker for the simulation.
        /// </summary>
        public SimulatorWindow()
        {
            InitializeComponent();
            // Initialize the background worker instance.
            worker = new BackgroundWorker();
            // Initialize the stopwatch instance.
            stopWatch = new Stopwatch();
            // Initialize the data for the simulation.
            data = new ArrayList();
            // Register the DoWork event handler for the background worker.
            worker.DoWork += Worker_DoWork;
            // Register the ProgressChanged event handler for the background worker.
            worker.ProgressChanged += Worker_ProgressChanged;
            // Register the RunWorkerCompleted event handler for the background worker.
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            // Set the workerReportsProgress property to true to enable progress updates.
            worker.WorkerReportsProgress = true;
            // Set the workerSupportsCancellation property to true to enable cancellation of the worker.
            worker.WorkerSupportsCancellation = true;
            // Start the background worker.
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// The StopSimulatorObserver method is used to stop the simulation process.
        /// </summary>
        void StopSimulatorObserver()
        {
            worker.CancelAsync();
        }


        /// <summary>
        /// Observer method that is registered to the Simulator's update event. 
        /// When the Simulator updates its order status, this method is called to report the updated information to the worker. 
        /// </summary>
        /// <param name="id">The ID of the updated order.</param>
        /// <param name="orderStatus">The current status of the order.</param>
        /// <param name="nextStatus">The next status of the order.</param>
        /// <param name="startTime">The start time of the order's current status.</param>
        /// <param name="finishTime">The finish time of the order's current status.</param>
        void SimulatorUpdatedObserver(int id, BO.Status orderStatus, BO.Status nextStatus, DateTime startTime, DateTime finishTime)
        {
            ArrayList data = new ArrayList() { id, orderStatus, nextStatus, startTime, finishTime };
            worker.ReportProgress(1, data);
        }

        /// <summary>
        /// The event handler for the worker's DoWork event.
        /// This method starts the Simulator and updates the stopwatch and order status as the Simulator runs.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            stopWatch.Start();
            Simulator.Simulator.StopSimulatorRegister(StopSimulatorObserver);
            Simulator.Simulator.UpdateReportRegister(SimulatorUpdatedObserver);
            Simulator.Simulator.Activate();

            while (worker.CancellationPending == false)
            {
                worker.ReportProgress(2);
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// The event handler for the worker's ProgressChanged event.
        /// This method updates the UI to display the updated order status and stopwatch.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double type = e.ProgressPercentage;
            if (type == 1)
            {
                data = (ArrayList)e.UserState!;
                DateTime startTime = (DateTime)data[3]!;
                DateTime finishTime = (DateTime)data[4]!;

                OrderIdText.Text = data[0]!.ToString();
                CurrentStatusText.Text = data[1]!.ToString();
                NextStatusText.Text = data[2]!.ToString();
                StartTimeText.Text = startTime.ToString();
                FinishTimeText.Text = finishTime.ToString();
            }
            else if (type == 2)
            {
                string timerText = stopWatch.Elapsed.ToString();
                timerText = timerText.Substring(0, 8);
                this.WatchText.Text = timerText;
            }
        }


        /// <summary>
        /// This method is the event handler for the RunWorkerCompleted event of the BackgroundWorker object.
        /// It unregisters the observers for the SimulatorUpdated and StopSimulator events and closes the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RunWorkerCompletedEventArgs that contains the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Simulator.Simulator.Unregister(StopSimulatorObserver, SimulatorUpdatedObserver);
            _preventClosing = false;//enable the cabcel close window
            this.Close();
        }

        /// <summary>
        /// This method is the event handler for the click event of the Stop button.
        /// It deactivates the Simulator by calling the Deactivate method of the Simulator class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            // Stop the simulator by calling the Deactivate method from the Simulator class and the stop observer will automatically perform the end of this process.
            Simulator.Simulator.Deactivate();
        }

        /// <summary>
        /// This method is the event handler for the Closing event of the Window.
        /// It prevents the window from closing if the _preventClosing flag is set to true.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The CancelEventArgs that contains the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_preventClosing)
                e.Cancel = true;
        }
    }
}
