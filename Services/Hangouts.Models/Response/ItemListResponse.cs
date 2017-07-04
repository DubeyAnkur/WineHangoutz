using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangout.Models
{
    public class ItemListResponse:BaseServiceResponse
    {
        public IList<Item> ItemList { get; set; }
    }
}
