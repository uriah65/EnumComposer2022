#define FILE_CONFIG_NO
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using EnumComposer;


#if FILE_CONFIF
namespace TestComposer
{
    [TestClass]
    public class T02_ConfigurationFiles
    {
        ConfigReader _reader;
        Tuple<string, string> _connection;

        [TestInitialize()]
        public void Initialize()
        {
            _reader = new ConfigReader("", "", null);
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Test_DirectoryInner()
        { 
            Assert.AreEqual(false, _reader.IsInside(@"C:\a", @"D:\") , "Non equal start.");

            Assert.AreEqual(true, _reader.IsInside(@"C:\a", @"C:\") , "Correct inset.");
            Assert.AreEqual(true, _reader.IsInside(@"C:\aa\dddd\e\f", @"C:\aa\dddd\e"), "Correct inset. Parent without \\");
            Assert.AreEqual(true, _reader.IsInside(@"C:\aa\dddd\e\f\", @"C:\aa\dddd\e\"), "Correct inset. Parent with \\");

            Assert.AreEqual(false, _reader.IsInside(@"C:\aa\dddd\efe", @"C:\aa\dddd\e"), "Non equal tail.");
        }

        [TestMethod]
        public void Parsing_ConfigFile()
        {
            string inputFile = @"..\..\T02\App.config";
            string inputText = File.ReadAllText(inputFile);

            /* read valid connection */
            _connection = _reader.ExtractConnection("cnn1", inputText);
            ConstantsPR.AssertConnectionString("System.Data.ProviderName", "Valid Connection String;", _connection, "Expected equal.");

            /* read valid connection, case invariant */
            _connection = _reader.ExtractConnection("cNN1", inputText);
            ConstantsPR.AssertConnectionString("System.Data.ProviderName", "Valid Connection String;", _connection, "Expected equal.");

            /* read not valid connection */
            _connection = _reader.ExtractConnection("BAD_Cnn1", inputText);
            ConstantsPR.AssertConnectionString(null, "", _connection, "Expected equal.");


            //Helpers.ExtractConnectionString(inputFile, "cnn1");

        }

        [TestMethod]
        public void Visiting_ConfigFiles()
        {
            string startPath = @"..\..\T02\T02-1\T02-2";
            string endPath = @"..\..\T02";

            /* read connection all way to the top */
            _connection = _reader.GetConnection("NoExist", startPath, endPath);
            ConstantsPR.AssertConnectionString(null, null, _connection, "Expected equal.");

            /* read connection from the T02 */
            _connection = _reader.GetConnection("cNn1", startPath, endPath);
            ConstantsPR.AssertConnectionString("System.Data.ProviderName", "Valid Connection String;", _connection, "Expected equal.");

            /* read connection from the T02-2 */
            _connection = _reader.GetConnection("ZzZ", startPath, endPath);
            ConstantsPR.AssertConnectionString("providerZZ", "cnnZZcnn", _connection, "Expected equal.");
        }
    }
}
#endif