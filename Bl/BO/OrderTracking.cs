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
        public Status? status { get; set; }
        public List<(DateTime?, string?)>? TrackingList { get; set; }
        public override string ToString()
        {
           
            var query = from t in TrackingList
                        select t.ToString();

            var concatenatedString = string.Join("\n      ", query);


            return $"      Order Id: {ID}\n" +
            $"      status: {status}\n" +
            $"      {concatenatedString}";
        }
    }
}

