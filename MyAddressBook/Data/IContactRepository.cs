using System.Collections.Generic;
using MyAddressBook.Models;

namespace MyAddressBook.Data
{
    public interface IContactRepository
    {
        Contact GetContact(int id);

        int AddContact(Contact contact);

        bool DeleteContact(int id);

        List<Contact> GetContacts();
    }
}