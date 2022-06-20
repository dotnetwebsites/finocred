using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.web.BusinessLayer.DTOs
{
    public class WaModel
    {
        public string Mobiles { get; set; }
        public string Templates { get; set; }
        public string ImageUrl { get; set; }

        public string Password { get; set; }
    }

    public class WhatsappDTO
    {
        public string messaging_product { get; set; }
        public string recipient_type { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public template template { get; set; }
    }

    public class template
    {
        public string name { get; set; }
        public language language { get; set; }
        public components[] components { get; set; }
    }

    public class language
    {
        public string code { get; set; }
    }

    public class components
    {
        public string type { get; set; }
        public parameters[] parameters { get; set; }
    }

    public class parameters
    {
        public string type { get; set; }
        public image image { get; set; }
    }

    public class image
    {
        public string link { get; set; }
    }


    public class WhatsappResponseDTO
    {
        public string messaging_product { get; set; }
        public contacts[] contacts { get; set; }
        public messages[] messages { get; set; }
    }

    public class contacts
    {
        public string input { get; set; }
        public string wa_id { get; set; }
    }

    public class messages
    {
        public string id { get; set; }
    }
}
