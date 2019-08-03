using System.Collections.Generic;
using MyAddressBook.Models;

namespace MyAddressBook.Data
{
    public interface IContactRepository
    {
        Contacts GetContact(int id);

        int AddContact(Contacts contact);

        bool DeleteContact(int id);

        List<Contacts> GetContacts();
    }
}