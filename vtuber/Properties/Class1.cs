using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vtuber.Properties
{
    class Live_hollo_vtube
    {
        public string jpg;
        public string inter;
        public object Live_Time;
        public string name;
        public string vt_name;
        public Live_hollo_vtube(string jpg,string inter,string time)
        {
            this.jpg = jpg;
            this.inter = inter;
            if (time != "") { 
            long unixTimeStamp =Convert.ToInt64(time);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 當地時區
            DateTime dt = startTime.AddSeconds(unixTimeStamp);
            Live_Time = dt;
            }
            else
            {
                Live_Time = "直播中~~";
            }
        }
    }


    
}
