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
        public static IEnumerable<object[]> FilesData
        {
            get
            {
                yield return new object[] { new File(FILE_PATH_STRING, CONTENT_STRING), FILE_PATH_STRING, CONTENT_STRING };
                yield return new object[] { new File(SPACE_STRING, SPACE_STRING), SPACE_STRING, SPACE_STRING };
            }
        }

        /* Тестируем получение размера */
        [DataTestMethod, DynamicData(nameof(FilesData), DynamicDataSourceType.Property)]
        public void GetSizeTest(File newFile, String name, String content)
        {
            lenght = content.Length / 2;

            Assert.AreEqual(newFile.GetSize(), lenght, SIZE_EXCEPTION);
        }

        /* Тестируем получение имени */
        [DataTestMethod, DynamicData(nameof(FilesData), DynamicDataSourceType.Property)]
        public void GetFilenameTest(File newFile, String name, String content)
        {
            Assert.AreEqual(newFile.GetFilename(), name, NAME_EXCEPTION);
        }

        [TestMethod]
        public void EmptyTest_ReturnFalse()
		{
            File file = new File(FILE_PATH_STRING, CONTENT_STRING);

            Assert.IsFalse(file.Empty());
		}

        [TestMethod]
        public void EmptyTest_ReturnTrue()
        {
            File file = new File(FILE_PATH_STRING, "");

            Assert.IsTrue(file.Empty());
        }
    }
}