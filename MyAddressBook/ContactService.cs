using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MyAddressBook.Data;
using MyAddressBook.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MyAddressBook
{

    public class ContactService
    {
        // Redis cache initialization
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = KeyVaultService.CacheConnection;
            return ConnectionMultiplexer.Connect(cacheConnection);
        });
        IDatabase cache = lazyConnection.Value.GetDatabase();

        /// <summary>
        /// Gets all contacts from database
        /// </summary>
        /// <returns></returns>
        public List<Contacts> GetContacts()
        {
            var contactRepository = new ContactRepository();
            var contacts = contactRepository.GetContacts();
            return contacts;
        }

        /// <summary>
        /// Gets a specific contact from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contacts GetContact(int id)
        {
            var contactRepository = new ContactRepository();
            var contact = contactRepository.GetContact(id);

            return contact;
        }

        /// <summary>
        /// Gets a specific contact from Redis cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contacts GetContactFromCache(int id)
        {
            var cacheContent = cache.StringGet(id.ToString());
            if (!cacheContent.IsNull)
            {
                var contact = JsonConvert.DeserializeObject<Contacts>(cacheContent);
                return contact;
            }

            return null;
        }

        /// <summary>
        /// Adds a new contact to the database and cache
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public int AddContact(Contacts contact)
        {
            var contactRepository = new ContactRepository();
            contactRepository.AddContact(contact);

            var newId = contact.Id;

            // add new contact to cache
            cache.StringSet(newId.ToString(), JsonConvert.SerializeObject(contact));

            return newId;
        }

        /// <summary>
        /// deletes a given contact from database and cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteContact(int id)
        {
            var contactRepository = new ContactRepository();
            var success = contactRepository.DeleteContact(id);

            if (success)
            {
                // remove the item from cache
                cache.KeyDelete(id.ToString());
            }

            return success;
        }
    }
}