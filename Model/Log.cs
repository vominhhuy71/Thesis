using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model
{
    public enum LogTarget
    {
        ITEM,
        ORDER,
        SUPPLIER
    }

    public enum LogAction
    {
        ADD,
        DELETE,
        UPDATE
    }

    public class Log
    {
        public string Name { get; set; }

        public LogTarget Target { get; set; }

        public LogAction Action { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
