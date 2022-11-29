using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OrderTracking
    {
        public int ID { get; set; }
        public Status status { get; set; }
        public List<Tuple<DateTime?, string>> TrackingList { get; set; } 
        public override string ToString() => $@"
        Order ID: {ID}, 
        Status: {status}";
    }     
}
