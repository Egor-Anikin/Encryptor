using Microsoft.VisualStudio.TestTools.UnitTesting;
using Encryptor.PageModels;
using System.IO;
using System.Linq;

namespace Encryptor.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var page = new MainPageModel();
            page.EditorText = "abcd";
            page.KeyEntry = "b";
            page.OnButtonClicked.Execute(null);
            Assert.IsTrue(page.LabelText == "bcde");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var page = new MainPageModel();
            page.EditorText = "bcde";
            page.KeyEntry = "b";
            page.OnButtonClicked2.Execute(null);
            Assert.IsTrue(page.LabelText == "abcd");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var page = new MainPageModel();
            page.EditorText = "Бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!";
            page.KeyEntry = "Скорпион";
            page.OnButtonClicked2.Execute(null);
            Assert.IsTrue(page.LabelText == "Поздравляю, ты получил исходный текст!!!");
        }

        [TestMethod]
        public void TestMethod4()
        {
            var page = new MainPageModel();
            page.EditorText = "Поздравляю, ты получил исходный текст!!!";
            page.KeyEntry = "Скорпион";
            page.OnButtonClicked.Execute(null);
            Assert.IsTrue(page.LabelText == "Бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!");
        }

        [TestMethod]
        public void TestMethod5()
        {
            var page = new MainPageModel();
            page.EditorText = "Поздравляю, ты получил исходный текст!!!";
            page.KeyEntry = "Скорпион";
            page.OnButtonClicked.Execute(null);

            int num1 = Directory.GetFiles(page.folderPath).Select(f => Path.GetFileName(f)).Count();

            File.WriteAllText(Path.Combine(page.folderPath, "a.txt"), page.LabelText); // OnButtonClickedSeve
            page.LabelTitel = "";
            page.LabelText = "";
            page.EditorText = "";

            int num2 = Directory.GetFiles(page.folderPath).Select(f => Path.GetFileName(f)).Count();
            Assert.IsTrue(num2 == num1 + 1);


            var file = "a.txt";                                 //OnButtonClickedOpen
            page.EditorText = File.ReadAllText(Path.Combine(page.folderPath, file));
            page.LabelBegin = $"Текст из файла {file}:";
            page.LabelTitel = "";
            page.LabelText = "";

            Assert.IsTrue(page.EditorText == "Бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!!");

            page.OnButtonClicked2.Execute(null);

            Assert.IsTrue(page.LabelText == "Поздравляю, ты получил исходный текст!!!");

            var file2 = "a.txt";                                //OnButtonClickedDel
            File.Delete(Path.Combine(page.folderPath, file2));
            page.LabelTitel = "";
            page.LabelText = "";
            page.EditorText = "";

            int num3 = Directory.GetFiles(page.folderPath).Select(f => Path.GetFileName(f)).Count();
            Assert.IsTrue(num3 == num1);
        }

        [TestMethod]
        public void TestMethod6()
        {
            string str = Cipher.Encrypt("1234567", "abcэюя", true);
            Assert.IsTrue(str == null);
        }
    }
}
