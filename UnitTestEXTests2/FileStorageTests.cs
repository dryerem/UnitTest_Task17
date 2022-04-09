using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnitTestEX.exception;

namespace UnitTestEX.Tests
{
    [TestClass()]
    public class FileStorageTests
    {
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        public static IEnumerable<object[]> NewFilesData
        {
            get
            {
                yield return new object[] { new File(REPEATED_STRING, CONTENT_STRING) };
                yield return new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) };
                yield return new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) };
            }
        }

        public static IEnumerable<object[]> FilesForDeleteData
        {
            get
            {
                yield return new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING };
                yield return new object[] { null, TIC_TOC_TOE_STRING };
            }
        }

        public static IEnumerable<object[]> NewExceptionFileData
        {
            get
            {
                yield return new object[] { new File(REPEATED_STRING, CONTENT_STRING) };
            }
        }

        /* Тестирование записи дублирующегося файла */
        [DataTestMethod, DynamicData(nameof(NewExceptionFileData), DynamicDataSourceType.Property)]
        public void WriteExceptionTest(File file)
        {
            bool isException = false;
            try
            {
                storage.Write(file);
                Assert.IsFalse(storage.Write(file));
                storage.DeleteAllFiles();
            }
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.IsTrue(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [DataTestMethod, DynamicData(nameof(NewFilesData), DynamicDataSourceType.Property)]
        public void IsExistsTest(File file)
        {
            String name = file.GetFilename();
            Assert.IsFalse(storage.IsExists(name));
            try
            {
                storage.Write(file);
            }
            catch (FileNameAlreadyExistsException e)
            {
                Console.WriteLine(String.Format("Exception {0} in method {1}", e.GetBaseException(), MethodBase.GetCurrentMethod().Name));
            }
            Assert.IsTrue(storage.IsExists(name));
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        [DataTestMethod, DynamicData(nameof(FilesForDeleteData), DynamicDataSourceType.Property)]
        public void DeleteTest(File file, String fileName)
        {
            storage.Write(file);
            Assert.IsTrue(storage.Delete(fileName));
        }

        /* Тестирование получения файлов */
        [TestMethod]
        public void GetFilesTest()
        {
            foreach (File el in storage.GetFiles())
            {
                Assert.IsNotNull(el);
            }
        }

        /*
        * Тестирование получения всех файлов по заданному расширению
        */
        [TestMethod]
        public void GetFilesByExtensionTest_ReturnTrue()
        {
            File file1 = new File("C:\\Programs\\Engine\\log.journal", "[ERROR] - Engine init failed");
            File file2 = new File("C:\\Programs\\Engine\\log1.journal", "[INFO] - Engine init in processing");
            File file3 = new File("C:\\Programs\\Engine\\log2.journal", "[OK] - Engine init success");
            storage.Write(file1);
            storage.Write(file2);
            storage.Write(file3);

            List<File> files = storage.GetFilesByExtension("journal");
            bool flag = true;
            foreach (File file in storage.GetFiles())
			{
                string filename = file.GetFilename();
                if (filename.Split('.')[filename.Split('.').Length - 1] != "journal")
				{
                    flag = false;
				}
			}

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void GetFilesByExtensionTest_ReturnFalse()
        {
            File file1 = new File("C:\\my\\notes.txt", "bla bla bla");
            File file2 = new File("C:\\car\\BMW\\list.txt", "bla bla bla bla bla");
            File file3 = new File("C:\\colleage\\testing\\lection-2.pdf", "striiiiiiiiiiiiiiiiiiiiiiiiiiiiing");
            storage.Write(file1);
            storage.Write(file2);
            storage.Write(file3);

            List<File> files = storage.GetFilesByExtension("txt");
            bool flag = true;
            foreach (File file in storage.GetFiles())
            {
                string filename = file.GetFilename();
                if (filename.Split('.')[filename.Split('.').Length - 1] != "txt")
                {
                    flag = false;
                }
            }

            Assert.IsFalse(flag);
        }

        // Почти эталонный
        /* Тестирование получения файла */
        [DataTestMethod, DynamicData(nameof(NewFilesData), DynamicDataSourceType.Property)]
        public void GetFileTest(File expectedFile)
        {
            storage.Write(expectedFile);

            File actualfile = storage.GetFile(expectedFile.GetFilename());
            bool difference = actualfile.GetFilename().Equals(expectedFile.GetFilename()) && actualfile.GetSize().Equals(expectedFile.GetSize());

            Assert.IsTrue(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize()));
        }
    }
}