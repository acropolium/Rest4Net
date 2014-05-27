using System;
using System.Collections.Generic;
using System.Globalization;
using System.Json;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Rest4Net.ePochta.Responses;
using Rest4Net.ePochta.Responses.Implementation;
using Rest4Net.Exceptions;
using Rest4Net.Protocols;

namespace Rest4Net.ePochta
{
    /// <summary>
    /// This provider allows to access ePochta API through REST API as described here:
    /// http://www.epochta.ru/products/sms/v3.php
    /// </summary>
    public class ePochtaProvider : RestApiProvider
    {
        private class CustomProtocol : Http
        {
            public CustomProtocol(string host, int port = -1) : base(host, port)
            {
            }

            private static string CalculateMd5Hash(string input)
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hash = MD5.Create().ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var t in hash)
                    sb.Append(t.ToString("x2"));
                return sb.ToString();
            }

            public override CommandResult Execute(Command command)
            {
                var parameters = command.Parameters.OrderBy(x => x.Key);
                var sb = new StringBuilder();
                foreach (
                    var parameter in
                        parameters.Where(parameter => String.CompareOrdinal(parameter.Key, @"userapp") != 0))
                    sb.Append(parameter.Value);
                sb.Append(PrivateKey ?? "");
                return base.Execute(command.WithParameter("sum", CalculateMd5Hash(sb.ToString())));
            }

            internal string PrivateKey { private get; set; }
        }

        private readonly string _apiPublicKey;

        public ePochtaProvider(string apiPublicKey, string apiPrivateKey)
            : base(new CustomProtocol(@"atompark.com") {PrivateKey = apiPrivateKey})
        {
            _apiPublicKey = apiPublicKey;
        }

        private Command Run(string methodName)
        {
            return Cmd("/api/sms/3.0/" + methodName)
                .WithParameter("version", "3.0")
                .WithParameter("action", methodName)
                .WithParameter("key", _apiPublicKey);
        }

        private static JsonValue CheckForError(JsonValue arg)
        {
            if (arg == null || !arg.ContainsKey("error"))
                return arg;
            throw new ResultException(arg["error"].ReadAs<string>(), arg["code"].ReadAs<int>(), arg);
        }

        /// <summary>
        /// Create new address book
        /// </summary>
        /// <param name="name">Name for the new address book</param>
        /// <param name="description">Optional value for address book description</param>
        /// <returns>Id for the newly created address book</returns>
        public int CreateAddressbook(string name, string description = null)
        {
            var cmd = Run("addAddressbook").WithParameter("name", name);
            if (!String.IsNullOrWhiteSpace(description))
                cmd = cmd.WithParameter("description", description);
            return cmd.Execute().To<AddressbookCreateResultImpl>(CheckForError).result.Id;
        }

        /// <summary>
        /// Edit address book
        /// </summary>
        /// <param name="id">Id of the address book to edit</param>
        /// <param name="newName">New name for the address book</param>
        /// <param name="newDescription">New description for the address book</param>
        /// <returns></returns>
        public bool EditAddressbook(int id, string newName, string newDescription = null)
        {
            var cmd = Run("editAddressbook")
                .WithParameter("idAddressBook", id.ToString(CultureInfo.InvariantCulture))
                .WithParameter("newName", newName);
            if (!String.IsNullOrWhiteSpace(newDescription))
                cmd = cmd.WithParameter("newDescr", newDescription);
            return cmd.Execute().To<SuccessResultImpl>(CheckForError).result.successful;
        }

        /// <summary>
        /// Delete address book
        /// </summary>
        /// <param name="id">Id for the address book to delete</param>
        /// <returns>Operation success</returns>
        public bool DeleteAddressbook(int id)
        {
            var cmd = Run("delAddressbook").WithParameter("idAddressBook", id.ToString(CultureInfo.InvariantCulture));
            return cmd.Execute().To<SuccessResultImpl>(CheckForError).result.successful;
        }

        /// <summary>
        /// Fetch full information about one address book
        /// </summary>
        /// <param name="id">Id for the address book to get</param>
        /// <returns>Full information about address book</returns>
        public IAddressBook GetAddressbook(int id)
        {
            var cmd = Run("getAddressbook").WithParameter("idAddressBook", id.ToString(CultureInfo.InvariantCulture));
            return cmd.Execute().To<ResponseImpl<IAddressBook, AddressBookImpl>>(CheckForError).result;
        }

        /// <summary>
        /// Fetch full information about many address books
        /// </summary>
        /// <param name="offset">Array offset to get elements</param>
        /// <param name="count">Count of elements to fetch from server</param>
        /// <returns>Full information of many address books</returns>
        public IAddressBooks ListAddressbooks(uint offset = 0, uint count = 50)
        {
            var cmd = Run("getAddressbook")
                .WithParameter("from", offset.ToString(CultureInfo.InvariantCulture))
                .WithParameter("offset", count.ToString(CultureInfo.InvariantCulture));
            return cmd.Execute().To<ResponseImpl<IAddressBooks, AddressBooksImpl>>(RemakeJsonForList).result;
        }

        private static JsonValue RemakeJsonForList(JsonValue arg)
        {
            var oInitial = CheckForError(arg);
            if (oInitial == null || !oInitial.ContainsKey("result"))
                return oInitial;
            var o = oInitial["result"];
            if (o == null || !o.ContainsKey("fields") || !o.ContainsKey("data"))
                return o;
            var fieldsList = new List<string>();
            var arr = (JsonArray)(o["fields"]);
            foreach (var item in arr)
                fieldsList.Add(item.ReadAs<string>());
            arr = (JsonArray)(o["data"]);
            var result = new JsonArray();
            foreach (var item in arr)
            {
                var itm = (JsonArray) item;
                var cnt = itm.Count;
                var obj = new JsonObject();
                for (var i = 0; i < cnt; i++)
                    obj.SetValue(fieldsList[i], itm[i]);
                result.Add(obj);
            }
            o.SetValue("items", result);
            return oInitial;
        }

        /// <summary>
        /// Clone address book
        /// </summary>
        /// <param name="id">If for the address book to clone</param>
        /// <returns>Id for the cloned address book</returns>
        public int CloneAddressbook(int id)
        {
            var cmd = Run("cloneaddressbook").WithParameter("idAddressBook", id.ToString(CultureInfo.InvariantCulture));
            return cmd.Execute().To<ResponsePureImpl<AddressbookCloneImpl>>(CheckForError).result.Id;
        }

        /// <summary>
        /// Add new phone to address book
        /// </summary>
        /// <param name="addressbookId">Address book to add phone to</param>
        /// <param name="phone">Phone number to add</param>
        /// <param name="variables">Personalization variables list</param>
        /// <returns></returns>
        public int CreatePhoneInAddressbook(int addressbookId, string phone, params string[] variables)
        {
            var cmd = Run("addPhoneToAddressBook").WithParameter("idAddressBook", addressbookId.ToString(CultureInfo.InvariantCulture)).WithParameter("phone", phone);
            var vars = Phone.GetVariables(variables);
            if (!String.IsNullOrWhiteSpace(vars))
                cmd = cmd.WithParameter("variables", vars);
            return cmd.Execute().To<ResponsePureImpl<PhoneCreateImpl>>(CheckForError).result.Id;
        }

        private string PhonesToJson(Phone phone, Phone[] phones)
        {
            var items = new List<Phone> { phone };
            items.AddRange(phones ?? new Phone[0]);
            var array = new JsonArray();
            foreach (var item in items)
            {
                var itm = new JsonArray { new JsonPrimitive(item.PhoneNumber) };
                if (!String.IsNullOrWhiteSpace(item.Variables))
                    itm.Add(new JsonPrimitive(item.Variables));
                array.Add(itm);
            }
            return array.ToString();
        }

        /// <summary>
        /// Add many phones to address book
        /// </summary>
        /// <param name="addressbookId">Address book to add phone to</param>
        /// <param name="phone">Phone object to add</param>
        /// <param name="phones">Additional phone object to add</param>
        /// <returns></returns>
        public bool CreatePhonesInAddressbook(int addressbookId, Phone phone, params Phone[] phones)
        {
            

            var cmd =
                Run("addPhoneToAddressBook")
                    .WithParameter("idAddressBook", addressbookId.ToString(CultureInfo.InvariantCulture))
                    .WithParameter("data", PhonesToJson(phone, phones));
            return cmd.Execute().To<SuccessResultImpl>(CheckForError).result.successful;
        }

        /// <summary>
        /// Fetch full information about one phone
        /// </summary>
        /// <param name="id">Id for the phone to get</param>
        /// <returns>Full information about phone</returns>
        public IPhone GetPhone(int id)
        {
            var cmd = Run("getPhoneFromAddressBook").WithParameter("idPhone", id);
            return cmd.Execute().To<ResponseImpl<IPhone, PhoneImpl>>(CheckForError).result;
        }

        /// <summary>
        /// List all phones from all address books
        /// </summary>
        /// <param name="offset">Array offset to get elements</param>
        /// <param name="count">Count of elements to fetch from server</param>
        /// <returns>List of phones object</returns>
        public IPhones ListPhones(uint offset = 0, uint count = 50)
        {
            return ListPhonesFromBook(0, offset, count);
        }

        /// <summary>
        /// List all phones from specified address book
        /// </summary>
        /// <param name="addressbookId">Id of the address book to filter</param>
        /// <param name="offset">Array offset to get elements</param>
        /// <param name="count">Count of elements to fetch from server</param>
        /// <returns>List of phones object</returns>
        public IPhones ListPhonesFromBook(int addressbookId, uint offset = 0, uint count = 50)
        {
            return ListPhonesByPattern(null, addressbookId, offset, count);
        }

        /// <summary>
        /// List all phones from all address books
        /// </summary>
        /// <param name="phonePattern">Phone number to filter</param>
        /// <param name="addressbookId">Id of the address book to filter</param>
        /// <param name="offset">Array offset to get elements</param>
        /// <param name="count">Count of elements to fetch from server</param>
        /// <returns>List of phones object</returns>
        public IPhones ListPhonesByPattern(string phonePattern, int addressbookId = 0, uint offset = 0, uint count = 50)
        {
            var cmd = Run("getPhoneFromAddressBook")
                .WithParameter("from", offset)
                .WithParameter("offset", count);
            if (!String.IsNullOrWhiteSpace(phonePattern))
                cmd = cmd.WithParameter("phone", phonePattern);
            if (addressbookId > 0)
                cmd = cmd.WithParameter("idAddressBook", addressbookId);
            return cmd.Execute().To<ResponseImpl<IPhones, PhonesImpl>>(RemakeJsonForList).result;
        }

        /// <summary>
        /// Edit phone information
        /// </summary>
        /// <param name="id">Id for the phone record to edit</param>
        /// <param name="newPhone">New phone number</param>
        /// <param name="newVariables">Optional. New variables list. If not specified, old is remained</param>
        /// <returns></returns>
        public bool EditPhone(int id, string newPhone, params string[] newVariables)
        {
            var cmd = Run("editPhone")
                    .WithParameter("idPhone", id)
                    .WithParameter("phone", newPhone);
            var variables = Phone.GetVariables(newVariables);
            if (!String.IsNullOrWhiteSpace(variables))
                cmd = cmd.WithParameter("variables", variables);
            return cmd.Execute()
                .To<SuccessResultImpl>(CheckForError)
                .result.successful;
        }

        /// <summary>
        /// Delete phone number by id
        /// </summary>
        /// <param name="id">Id for the phone number to delete</param>
        /// <returns></returns>
        public bool DeletePhone(int id)
        {
            return
                Run("delPhoneFromAddressBook")
                    .WithParameter("idPhone", id)
                    .Execute()
                    .To<SuccessResultImpl>(CheckForError)
                    .result.successful;
        }
        
        /// <summary>
        /// Delete all phones from address book
        /// </summary>
        /// <param name="addressbookId">Address book id</param>
        /// <returns></returns>
        public bool DeleteAllPhonesFromAddressbook(int addressbookId)
        {
            return
                Run("delPhoneFromAddressBook")
                    .WithParameter("idAddressBook", addressbookId)
                    .Execute()
                    .To<SuccessResultImpl>(CheckForError)
                    .result.successful;
        }

        /// <summary>
        /// Delete phones by ids
        /// </summary>
        /// <param name="ids">List of ids to delete</param>
        /// <returns></returns>
        public bool DeletePhones(params int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return false;
            return
                Run("delphonefromaddressbookgroup")
                    .WithParameter("idPhones", String.Join(",", ids))
                    .Execute()
                    .To<SuccessResultImpl>(CheckForError)
                    .result.successful;
        }

        /// <summary>
        /// Simple balance getter with default currency
        /// </summary>
        public IBalance Balance
        {
            get { return GetBalance(); }
        }

        /// <summary>
        /// Get current balance
        /// </summary>
        /// <param name="currency">Currency in which to get balance</param>
        /// <returns>Balance information</returns>
        public IBalance GetBalance(Currency? currency = null)
        {
            var cmd = Run("getUserBalance");
            if (currency != null)
                cmd = cmd.WithParameter("currency", currency.Value.ToString());
            return cmd.Execute().To<ResponseImpl<IBalance, BalanceImpl>>(CheckForError).result;
        }

        /// <summary>
        /// Make a request to register sender name
        /// </summary>
        /// <param name="senderName">New sender name. For digits should be 14 chars maximum. For letters - limit is 11 characters</param>
        /// <param name="country">Country of origin for the name</param>
        /// <returns>Id for the sender name</returns>
        public int RequestSenderRegistration(string senderName, Country country)
        {
            return Run("registerSender")
                .WithParameter("name", senderName)
                .WithParameter("country", country.AsString())
                .Execute()
                .To<ResponsePureImpl<SenderRegisterImpl>>(CheckForError)
                .result.Id;
        }

        /// <summary>
        /// Get full information about sender name by id
        /// </summary>
        /// <param name="id">Id of sender name to retrieve</param>
        /// <returns>Sender name object with full information</returns>
        public ISender GetSender(int id)
        {
            return
                Run("getSenderStatus")
                    .WithParameter("idName", id)
                    .Execute()
                    .To<ResponseImpl<ISender, SenderImpl>>(CheckForError)
                    .result;
        }

        /// <summary>
        /// Get full information about sender name by name and country
        /// </summary>
        /// <param name="name">Name to search</param>
        /// <param name="country">Country to filter</param>
        /// <returns>Sender name object with full information</returns>
        public ISender GetSender(string name, Country country)
        {
            return
                Run("getSenderStatus")
                    .WithParameter("name", name)
                    .WithParameter("country", country.AsString())
                    .Execute()
                    .To<ResponseImpl<ISender, SenderImpl>>(CheckForError)
                    .result;
        }

        /// <summary>
        /// Retrieve list of sender names for current account
        /// </summary>
        /// <param name="offset">Array offset to get elements</param>
        /// <param name="count">Count of elements to fetch from server</param>
        /// <returns>List of sender name objects with full information</returns>
        public ISenders ListSenders(uint offset = 0, uint count = 50)
        {
            return Run("getSenderStatus")
                .WithParameter("from", offset)
                .WithParameter("offset", count).Execute().To<ResponseImpl<ISenders, SendersImpl>>(RemakeJsonForList).result;
        }

        /// <summary>
        /// Create a campaign only with one sms to send
        /// </summary>
        /// <param name="phoneNumber">Number for sms receiver</param>
        /// <param name="message">Message information object</param>
        /// <returns>New campaign information with id and price</returns>
        public ISendResult SendSms(string phoneNumber, MessageInfo message)
        {
            return
                message.FillCommand(Run("sendSMS").WithParameter("phone", phoneNumber))
                    .Execute()
                    .To<ResponseImpl<ISendResult, SendResultImpl>>(CheckForError)
                    .result;
        }

        /// <summary>
        /// Send sms to the list of phones from address book
        /// </summary>
        /// <param name="addressbookId">Id for the address book to send to</param>
        /// <param name="message">Message information object</param>
        /// <param name="watchdogPhoneNumber">Phone number to control the campaign send-out</param>
        /// <param name="portionSize">The count of sms to send at a time. Use this parameter if you would like to send sms by portions</param>
        /// <param name="portionInterval">Interval between portions in (TODO: seconds?)</param>
        /// <returns>New campaign information with id and price</returns>
        public ISendResult SendSmsBatch(int addressbookId, MessageInfo message, string watchdogPhoneNumber = null,
            uint portionSize = 0, uint portionInterval = 0)
        {
            var cmd = Run("createCampaign")
                .WithParameter("list_id", addressbookId)
                .WithParameter("batch", portionSize)
                .WithParameter("batchinterval", portionInterval);
            if (!String.IsNullOrWhiteSpace(watchdogPhoneNumber))
                cmd.WithParameter("control_phone", watchdogPhoneNumber);
            return
                message.FillCommand(cmd)
                    .Execute()
                    .To<ResponseImpl<ISendResult, SendResultImpl>>(CheckForError)
                    .result;
        }

        /// <summary>
        /// Send sms to the custom list of phones
        /// </summary>
        /// <param name="message">Message information object</param>
        /// <param name="phone">First custom phone</param>
        /// <param name="phones">Optional custom phones</param>
        /// <returns>New campaign information with id and price</returns>
        public ISendResult SendSmsBatch(MessageInfo message, Phone phone, params Phone[] phones)
        {
            return
                message.FillCommand(Run("sendsmsgroup").WithParameter("phones", PhonesToJson(phone, phones)))
                    .Execute()
                    .To<ResponseImpl<ISendResult, SendResultImpl>>(CheckForError)
                    .result;
        }
    }
}
