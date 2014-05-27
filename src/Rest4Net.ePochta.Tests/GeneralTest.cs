using System.Linq;
using NUnit.Framework;
using Rest4Net.ePochta.Responses;

namespace Rest4Net.ePochta.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string PublicKey = "24ad38cbd71d3ea035c398604fcd9a38";
        private const string PrivateKey = "f42e73b3df76daac375f887f974f3035";
        #endregion

        #region Test constants
        private const string Sender = "";
        private const string MyPhone1 = "";
        private const string MyPhone2 = "";
        private const string TestNewPhone1 = "+380501112233";
        private const string TestNewPhone2 = "+380501122233";
        private const string TestNewPhone3 = "+380501132233";
        private const string TestNewPhone11 = "+380531112233";
        private const string TestNewPhone12 = "+380531122233";
        private const string TestNewPhone13 = "+380531132233";
        private const string TestPhonePersonalization1 = "G1";
        private const string TestPhonePersonalization2 = "G2";
        private const string TestPhonePersonalization3 = "G3";
        private const string TestPhonePersonalization4 = "G4";
        private const string TestNewAddressbookName = "New book";
        private const string TestEditedAddressbookName = "Edited book";
        #endregion

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            using (var client = new ePochtaProvider(PublicKey, PrivateKey))
            {
                var balance = client.GetBalance();
                Assert.GreaterOrEqual(balance.Amount, 0);

                balance = client.GetBalance(Currency.GBP);
                Assert.AreEqual(balance.AmountCurrency, Currency.GBP);

                var id = client.CreateAddressbook(TestNewAddressbookName);
                Assert.Greater(id, 0);
                var r = client.EditAddressbook(id, TestEditedAddressbookName);
                Assert.True(r);
                var ab = client.GetAddressbook(id);
                Assert.AreEqual(ab.Name, TestEditedAddressbookName);

                var pid = client.CreatePhoneInAddressbook(id, TestNewPhone1);
                Assert.Greater(pid, 0);
                pid = client.CreatePhoneInAddressbook(id, TestNewPhone2, TestPhonePersonalization1,
                    TestPhonePersonalization2, TestPhonePersonalization3, TestPhonePersonalization4);
                Assert.Greater(pid, 0);
                pid = client.CreatePhoneInAddressbook(id, TestNewPhone3, TestPhonePersonalization2);
                Assert.Greater(pid, 0);

                Assert.IsTrue(client.EditPhone(pid, TestNewPhone13));
                var tempPhone = client.GetPhone(pid);
                Assert.AreEqual(tempPhone.Variables.Count, 1);

                var phone = client.GetPhone(pid);
                Assert.Greater(phone.Id, 0);

                Assert.IsTrue(client.CreatePhonesInAddressbook(id, new Phone(TestNewPhone11),
                    new Phone(TestNewPhone12, TestPhonePersonalization3),
                    new Phone(TestNewPhone13, TestPhonePersonalization3, TestPhonePersonalization1)));

                var phones = client.ListPhones();
                Assert.Greater(phones.Count, 0);

                phones = client.ListPhonesByPattern(TestNewPhone2);
                Assert.Greater(phones.Count, 0);

                phones = client.ListPhonesFromBook(id);
                Assert.AreEqual(phones.Count, 5);

                Assert.IsTrue(client.DeletePhone(pid));
                phones = client.ListPhonesFromBook(id);
                Assert.AreEqual(phones.Count, 4);

                Assert.IsTrue(client.DeleteAllPhonesFromAddressbook(id));
                Assert.Catch(() => client.ListPhonesFromBook(id), "");

                var books = client.ListAddressbooks();
                Assert.Greater(books.Count, 0);
                r = client.DeleteAddressbook(id);
                Assert.True(r);
            }
        }

        [Test(Description = "Sender info verification")]
        public void VerifySenderInfo()
        {
            using (var client = new ePochtaProvider(PublicKey, PrivateKey))
            {
                var senderNames = client.ListSenders();
                Assert.Greater(senderNames.Count, 0);
                var sender = client.GetSender(senderNames.Items.First().Id);
                var id = sender.Id;
                sender = client.GetSender(sender.Name, sender.Country);
                Assert.AreEqual(id, sender.Id);
            }
        }

        [Test(Description = "Camaign info verification")]
        public void VerifyCampaignInfo()
        {
            using (var client = new ePochtaProvider(PublicKey, PrivateKey))
            {
                var campaigns = client.ListCampaigns();
                Assert.Greater(campaigns.Count, 0);
                //var ids = campaigns.Select(x => x.Id).ToArray();
                //var detailed = client.ListCampaignsMessageStatuses(ids);
                //detailed = client.ListCampaignsDetailed(ids);
            }
        }

        [Test(Description = "Sender registration")]
        [Ignore("Ignore as critical")]
        public void SenderRegistration()
        {
            using (var client = new ePochtaProvider(PublicKey, PrivateKey))
            {
                Assert.Greater(client.RequestSenderRegistration(Sender, Country.Ukraine), 0);
            }
        }

        [Test(Description = "Send sms test")]
        [Ignore("Ignore as critical")]
        public void SendSmsTest()
        {
            using (var client = new ePochtaProvider(PublicKey, PrivateKey))
            {
                var r = client.SendSms(MyPhone1, new MessageInfo("Test message!", "Acropolium") {AlternativeSender = "Hola!"});
                Assert.Greater(r.Id, 0);
                Assert.Greater(r.Price, 0);
                var stats = client.GetCampaignStatistics(r.Id);
                Assert.Greater(stats.Count, 0);
            }
        }
    }
}