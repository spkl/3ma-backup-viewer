using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LateNightStupidities.IIImaBackupReader;
using LateNightStupidities.IIImaBackupReader.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IIImaBackup backup = new IIImaBackup(@"C:\Users\Sebastian\Desktop\Threema\threema-backup_HVJNE9PD_1508343104568_1", "HVJNE9PD");
            backup.Read();
            backup.Contacts.Add(backup.Me, new Contact()
            {
                CustomDisplayName = "CS"
            });
            Conversation c = backup.Conversations.First(conv => conv.ConversationPartner == new Identity("UUS2VJKY"));
            
            List<string> strings = new List<string>();
            foreach (Message message in c)
            {
                string line = $"{message.CreatedAt.ToLocalTime()} {message.Creator.DisplayName}: ";
                if (message is TextMessage textMessage)
                {
                    line += textMessage.Text;
                }
                else
                {
                    line += message.Type;
                }

                strings.Add(line);
            }

            File.WriteAllLines(@"C:\Users\Sebastian\Desktop\Threema\test.txt", strings, Encoding.UTF8);
        }

        [TestMethod]
        public void TestMethod2()
        {
            IIImaBackup backup = new IIImaBackup(@"C:\Users\Sebastian\Desktop\Threema\threema-backup_HVJNE9PD_1508343104568_1", "HVJNE9PD");
            backup.Read();
            var ballotMessages = backup.Conversations[15].OfType<BallotMessage>().ToArray();
        }
    }
}
