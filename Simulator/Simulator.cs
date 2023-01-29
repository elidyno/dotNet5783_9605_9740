using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public static class Simulator
    {
        //IBl instance variable used for accessing business logic methods
        static readonly BlApi.IBl? bl = BlApi.Factory.Get();
        private static volatile bool active;
        // Declare the event using the delegate.
        private static event Action? StopSimulator;            //??
        private static event Action<int, BO.Status, BO.Status, DateTime, DateTime>? UpdateReport;
        //private static event Action? SimulatorCompleted;
        public static void Activate()  // הפעלה 
        {        
            new Thread(() =>
            {
                active = true;
                BO.Order order;
                Random random = new Random();
                DateTime startTime;
                DateTime finishTime;
                while (active)
                {
                    int? orderid = bl?.Order.OldestOrder();
                    if (orderid != null)
                    {
                        order = bl!.Order.Get((int)orderid);
                        int delay = random.Next(2, 11);  // (3,10)
                        startTime = DateTime.Now;  //?
                        finishTime = startTime + new TimeSpan(0, 0, delay);
                        UpdateReport?.Invoke((int)orderid, (BO.Status)order.status, (BO.Status)order.status + 1, startTime, finishTime); //report to pl
                        Thread.Sleep(delay * 1000); // sleep until the handle is done ??
                        //event report to pl the handle is done in this order 
                        if (order.status == Status.APPROVED)
                            bl.Order.UpdateOrderSheep((int)orderid); 
                        else
                            bl.Order.UpdateOrderDelivery((int)orderid);
                        //SimulatorCompleted?.Invoke();
                        //UpdateReport?.Invoke((int)orderid, (BO.Status)order.status + 1, (BO.Status)order.status + 2, DateTime.Now, time); //report to pl

                    }
                    Thread.Sleep(1000);
                }
                //report finish 
                StopSimulator?.Invoke();
            }).Start();


        }

        public static void Deactivate() { active= false; }      
        public static void StopSimulatorRegister(Action observer)  // מקבל מתודה מתצוגה ורושם לאירוע המתאים
        {
            StopSimulator += observer;
        }

        //public static void SimulatorCompletedRegister(Action observer)  // מקבל מתודה מתצוגה ורושם לאירוע המתאים
        //{
        //    SimulatorCompleted += observer;
        //}

        public static void UpdateReportRegister(Action<int, BO.Status, BO.Status, DateTime, DateTime> observer)  // מקבל מתודה מתצוגה ורושם לאירוע המתאים
        {
            UpdateReport += observer;
        }
        public static void Unregister(Action observer1, Action<int, BO.Status, BO.Status, DateTime, DateTime> observer2) 
        {
            
            StopSimulator -= observer1;
            UpdateReport -= observer2;
        }

    }
}
