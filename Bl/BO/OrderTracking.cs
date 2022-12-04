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
        public List<(DateTime?, string)>? TrackingList { get; set; }
        public override string ToString()
        {
            string str = "";
            foreach(var t in TrackingList)
                str += t.ToString() + "\n" + "      ";

            return $"      Order Id: {ID}\n" +
            $"      status: {status}\n" +
            $"      {str}";
        }
    }
}

