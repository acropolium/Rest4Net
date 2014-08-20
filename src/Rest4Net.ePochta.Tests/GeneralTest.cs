using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Rest4Net.ePochta.Responses;

namespace Rest4Net.ePochta.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        private readonly dynamic _a = new ExpandoObject();

        [TestFixtureSetUp]
        public void LoadConfigurationValuesAndConstants()
        {
            //var configPath = Path.GetFullPath("./../../Assets/Config.xml");
            var configPath = "./../../Assets/Config.xml";
            //if(!File.Exists(configPath))
            //    throw new FileNotFoundException("Test configuration file not found. Prepare it from _Config.xml", configPath);
            var w = (IDictionary<string, object>)_a;
            var doc = XDocument.Load(configPath);
            foreach (var item in doc.Descendants("Item"))
                w[item.Attribute("name").Value] = item.Attribute("value").Value;
        }

        private ePochtaProvider CreateProvider()
        {
            return new ePochtaProvider(_a.PublicKey, _a.PrivateKey);
        }

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            using (var client = CreateProvider())
            {
                var balance = client.GetBalance();
                Assert.GreaterOrEqual(balance.Amount, 0);

                balance = client.GetBalance(Currency.GBP);
                Assert.AreEqual(balance.AmountCurrency, Currency.GBP);

                var id = client.CreateAddressbook((string)_a.TestNewAddressbookName);
                Assert.Greater(id, 0);
                var r = client.EditAddressbook(id, (string)_a.TestEditedAddressbookName);
                Assert.True(r);
                var ab = client.GetAddressbook(id);
                Assert.AreEqual(ab.Name, (string) _a.TestEditedAddressbookName);

                var pid = client.CreatePhoneInAddressbook(id, (string)_a.TestNewPhone1);
                Assert.Greater(pid, 0);
                pid = client.CreatePhoneInAddressbook(id, (string)_a.TestNewPhone2, (string)_a.TestPhonePersonalization1,
                    (string)_a.TestPhonePersonalization2, (string)_a.TestPhonePersonalization3, (string)_a.TestPhonePersonalization4);
                Assert.Greater(pid, 0);
                pid = client.CreatePhoneInAddressbook(id, (string)_a.TestNewPhone3, (string)_a.TestPhonePersonalization2);
                Assert.Greater(pid, 0);

                Assert.IsTrue(client.EditPhone(pid, (string)_a.TestNewPhone13));
                var tempPhone = client.GetPhone(pid);
                Assert.AreEqual(tempPhone.Variables.Count, 1);

                var phone = client.GetPhone(pid);
                Assert.Greater(phone.Id, 0);

                Assert.IsTrue(client.CreatePhonesInAddressbook(id, new Phone((string)_a.TestNewPhone11),
                    new Phone((string)_a.TestNewPhone12, (string)_a.TestPhonePersonalization3),
                    new Phone((string)_a.TestNewPhone13, (string)_a.TestPhonePersonalization3, (string)_a.TestPhonePersonalization1)));

                var phones = client.ListPhones();
                Assert.Greater(phones.Count, 0);

                phones = client.ListPhonesByPattern(_a.TestNewPhone2);
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
            using (var client = CreateProvider())
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
            using (var client = CreateProvider())
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
            using (var client = CreateProvider())
            {
                Assert.Greater(client.RequestSenderRegistration((string)_a.Sender, Country.Ukraine), 0);
            }
        }

        [Test(Description = "Send sms test")]
        [Ignore("Ignore as critical")]
        public void SendSmsTest()
        {
            using (var client = CreateProvider())
            {
                var r = client.SendSms((string)_a.MyPhone1, new MessageInfo(_a.Message, _a.Sender) { AlternativeSender = _a.AlternativeSender });
                Assert.Greater(r.Id, 0);
                Assert.Greater(r.Price, 0);
                var stats = client.GetCampaignStatistics(r.Id);
                Assert.Greater(stats.Count, 0);
            }
        }
    }
}