using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWF_Report_Updater
{
    class JsonResultTime
    {
        public string categoryname
        {
            get;
            set;
        }

        public string companyname
        {
            get;
            set;
        }
        public string projecttitle
        {
            get;
            set;
        }

        public string taskname
        {
            get;
            set;
        }

        public string contactname
        {
            get;
            set;
        }

        public decimal timetracked
        {
            get;
            set;
        }

        public DateTime starttime
        {
            get;
            set;
        }

        public string notes
        {
            get;
            set;
        }
    }
}
