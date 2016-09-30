using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneric
{
    class SrcBean
    {
        public List<Dictionary<string, object>> DcDataPrint { get; set; }

        private List<Dictionary<String, Object>> DcDataPrintTemp;

        public void SetDcDataPrintTemp(List<Dictionary<String, Object>> DcDataPrintTemp)
        {
            this.DcDataPrintTemp = DcDataPrintTemp;
        }

        public List<Dictionary<String, Object>> GetDcDataPrintTemp()
        {
            return DcDataPrintTemp;
        }
    }
}
