namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class AddressbookCreateImpl
    {
        // ReSharper disable once InconsistentNaming
        public int addressbook_id;

        public int Id
        {
            get { return addressbook_id; }
        }
    }

    internal class AddressbookCloneImpl
    {
        // ReSharper disable once InconsistentNaming
        public int idAddressBook;

        public int Id
        {
            get { return idAddressBook; }
        }
    }
}
