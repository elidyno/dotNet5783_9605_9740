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
        public static void Activate()  // הפעלה 
        {        
            new Thread(() =>
            {
                active = true;
                BO.Order order;
                Random random = new Random();
                DateTime time;
                while (active)
                {
                    int? orderid = bl?.Order.OldestOrder();
                    if (orderid != null)
                    {
                        order = bl!.Order.Get((int)orderid);
                        int delay = random.Next(2, 11);  // (3,10)
                        time = DateTime.Now + new TimeSpan(delay * 1000); //?
                        UpdateReport?.Invoke((int)orderid, (BO.Status)order.status, (BO.Status)order.status + 1, DateTime.Now, time); //report to pl
                        Thread.Sleep(delay * 1000); // sleep until the handle is done ??
                        //event report to pl the handle is done in this order 
                        if (order.status == Status.APPROVED)
                            bl.Order.UpdateOrderSheep((int)orderid); 
                        else
                            bl.Order.UpdateOrderDelivery((int)orderid); 
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

        public static void UpdateReportRegister(Action<int, BO.Status, BO.Status, DateTime, DateTime> observer)  // מקבל מתודה מתצוגה ורושם לאירוע המתאים
        {
            UpdateReport += observer;
        }
        public static void Unregister(Action observer, Action<int, BO.Status, BO.Status, DateTime, DateTime> observer1) 
        {
            StopSimulator -= observer;
            UpdateReport -= observer1;
        }

    }
}
