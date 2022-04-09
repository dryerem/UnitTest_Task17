using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestEX.Tests
{
    [TestClass()]
    public class FileTests
    {
        public const string SIZE_EXCEPTION = "Wrong size";
        public const string NAME_EXCEPTION = "Wrong name";
        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public double lenght;

        /* ПРОВАЙДЕР */
        static object[] FilesData =
        {
            new object[] {new File(FILE_PATH_STRING, CONTENT_STRING), FILE_PATH_STRING, CONTENT_STRING},
            new object[] { new File(SPACE_STRING, SPACE_STRING), SPACE_STRING, SPACE_STRING}
        };

        /* Тестируем получение размера */
        [TestMethod, DataSource(nameof(FilesData))]
        public void GetSizeTest(File newFile, String name, String content)
        {
            lenght = content.Length / 2;

            Assert.AreEqual(newFile.GetSize(), lenght, SIZE_EXCEPTION);
        }

        /* Тестируем получение имени */
        [TestMethod, DataSource(nameof(FilesData))]
        public void GetFilenameTest(File newFile, String name, String content)
        {
            Assert.AreEqual(newFile.GetFilename(), name, NAME_EXCEPTION);
        }
    }
}