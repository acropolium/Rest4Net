namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class AddressbookCreateImpl
    {
        public int addressbook_id;

        public int Id
        {
            get { return addressbook_id; }
        }
    }

    internal class AddressbookCloneImpl
    {
        public int idAddressBook;

        public int Id
        {
            get { return idAddressBook; }
        }
    }
}
