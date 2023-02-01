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
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        BackgroundWorker worker;
        Stopwatch stopWatch;
       

        public static readonly DependencyProperty ArrayListProperty = DependencyProperty.Register(
        "data", typeof(ArrayList), typeof(SimulatorWindow), new PropertyMetadata(default(ArrayList)));
        //Dependency Property "Order" for holding order data
        public ArrayList data
        {
            get => (ArrayList)GetValue(ArrayListProperty);
            set => SetValue(ArrayListProperty, value);
        }

        public SimulatorWindow()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            stopWatch = new Stopwatch();
            data= new ArrayList();
            worker.DoWork += Worker_DoWork!;
            worker.ProgressChanged += Worker_ProgressChanged!;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted!;  
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();     //הפעלת התהליך 
        }

        void StopSimulatorObserver()
        {
            worker.CancelAsync();
        }

       
        void SimulatorUpdatedObserver(int id, BO.Status orderStatus, BO.Status nextStatus, DateTime startTime, DateTime finishTime)
        {
            ArrayList data = new ArrayList() { id, orderStatus, nextStatus, startTime, finishTime };
            worker.ReportProgress(1, data); //ושולח לו נתונים , והאירוע הזה יפעיל את הפונקציה שנרשמה אליו ProgressChanged מפעיל את האירוע   
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //BackgroundWorker worker = sender as BackgroundWorker;

            Simulator.Simulator.StopSimulatorRegister(StopSimulatorObserver); // רישום משקיף לאירוע עצירה
            Simulator.Simulator.UpdateReportRegister(SimulatorUpdatedObserver);

            Simulator.Simulator.Activate(); // הפעלת הסימולטור

            while(worker.CancellationPending == false)
            {
               
                worker.ReportProgress(2);  // עדכן שעון
                Thread.Sleep(1000);
            }

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            double type = e.ProgressPercentage; //?
            if (type == 1) // בקשת עדכון הזמנה
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
            else if(type == 2) // בקשת עדכון שעון 
            {
                string timerText = stopWatch.Elapsed.ToString(); // עדכון פקד שמציג שעון
                timerText = timerText.Substring(0, 8);
                this.WatchText.Text = timerText;
            }
           
        }

        
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Simulator.Simulator.Unregister(StopSimulatorObserver, SimulatorUpdatedObserver);
            this.Close();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            // עצירת הסימולטור ע"י זימון מתודה מסימולטור ואז אוטומטית המשקיף על עצירה יבצע סיום תהליכון הזה.
            Simulator.Simulator.Deactivate();
        }
    }
}
