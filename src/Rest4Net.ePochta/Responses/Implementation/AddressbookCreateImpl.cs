namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class AddressbookCreateImpl
    {
#pragma warning disable 649
        // ReSharper disable once InconsistentNaming
        public int addressbook_id;
#pragma warning restore 649

        public int Id
        {
            get { return addressbook_id; }
        }
    }

    internal class AddressbookCloneImpl
    {
#pragma warning disable 649
        // ReSharper disable once InconsistentNaming
        public int idAddressBook;
#pragma warning restore 649

        public int Id
        {
            get { return idAddressBook; }
        }
    }
}
