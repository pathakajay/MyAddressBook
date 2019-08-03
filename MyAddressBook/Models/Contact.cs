using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAddressBook.Models
{
    public class Contacts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PictureName { get; set; }
    }
}