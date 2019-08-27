using Snappy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    static class WorkWithSQlite
    {
        static private string fileName; // переменная для хранения информации о файле
        static public string FileName { get; set; }

        static private string fileFormat; // переменная для хранения информации о файле
        static public string FileFormat { get; set; }

        static public void createOrOpenDB() // Создает базу данных, если не находит по указанному пути
        {
            string dbPath = ConfigurationSettings.AppSettings["PathToDB"]; // Извлекает из app.config путь к базе данных
            DirectoryInfo dirInfo = new DirectoryInfo(ConfigurationSettings.AppSettings["PathToFolder"]); // Извлекает из app.config путь к папке с базой данных
            if (!dirInfo.Exists) // Проверяет на наличие папки
            {
                dirInfo.Create(); // Создает необходимую папку
            }
            if (!File.Exists(dbPath)) // Проверяет на наличие базы данных по заданному пути и имени
            {
                SQLiteConnection.CreateFile(dbPath); // Создает файл базы данных по заданному пути и имени
            }
            createTableInDB();
        }

        static public void writeFileInfo(string filePath) // Записывает информацию о файле в переменные
        {
            FileFormat = Path.GetExtension(filePath).Replace(".", "").ToLower();
            FileName = Path.GetFileName(filePath).Replace(Path.GetExtension(filePath), "");
        }

        static public async void uploadFileToDBAsync(string filePath = null, string textFile = null) // Загружает данные в базу данных
        {
            try
            {
                byte[] _fileBytes = null; // Хранит информацию из файла
                if (!(filePath is null)) // Если путь к файлу указан выполняет чтение данных из файла
                {
                    FileInfo _fileInfo = new FileInfo(filePath);
                    long _numBytes = _fileInfo.Length;
                    FileStream _fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader _bRider = new BinaryReader(_fStream);
                    _fileBytes = _bRider.ReadBytes((int)_numBytes);
                    _bRider.Close();
                    _fStream.Close();
                }
                else if (!(textFile is null)) // Если данные поступают на прямую из текстового редактора
                {
                    _fileBytes = Encoding.UTF8.GetBytes(textFile);
                }
                else
                {
                    throw new Exception();
                }
                byte[] _fileBytesCompress = SnappyCodec.Compress(_fileBytes);
                string dbPath = "Data Source=" + ConfigurationSettings.AppSettings["PathToDB"]; // Путь к файлу базы данных
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {   // Написание строки команды для передачи базе данных на выполнение
                    string commandText = "INSERT INTO [dbFilesStore] ([file],[file_format],[file_name]) VALUES (@file, " +
                        "@format, @name)";
                    SQLiteCommand Command = new SQLiteCommand(commandText, conn);
                    Command.Parameters.AddWithValue("@file", _fileBytesCompress); // Сопоставление поля в базе данных с данными из файла
                    Command.Parameters.AddWithValue("@format", FileFormat); // Сопоставление поля в базе данных с форматом файла
                    Command.Parameters.AddWithValue("@name", FileName); // Сопоставление поля в базе данных с именем файла
                    conn.Open();
                    await Command.ExecuteNonQueryAsync(); // Выполнение запроса
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string strExeption = ex.Message.ToString();
            }
        }

        static public async Task<string> downloadFileFromDBAsync() // Загружает данные из базы данных
        {
            List<byte[]> _fileList = new List<byte[]>();
            List<string> _fileFormatList = new List<string>();
            List<string> _fileNameList = new List<string>();
            string dbPath = "Data Source=" + ConfigurationSettings.AppSettings["PathToDB"]; // указывает путь к файлу базы данных
            using (SQLiteConnection Conn = new SQLiteConnection(dbPath))
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand
                {
                    Connection = Conn,
                    CommandText = @"SELECT * FROM [dbFilesStore] WHERE [file_format] NOT NULL" // Запрос на выдачу файлов 
                };
                SQLiteDataReader sqlReader = Command.ExecuteReader();
                byte[] _dbFileByte = null;
                string _dbFileFormat = null;
                string _dbFileName = null;
                while (await sqlReader.ReadAsync())
                {
                    _dbFileByte = (byte[])sqlReader["file"];
                    _fileList.Add(_dbFileByte);
                    _dbFileFormat = sqlReader["file_format"].ToString().TrimStart().TrimEnd();
                    _fileFormatList.Add(_dbFileFormat);
                    _dbFileName = sqlReader["file_name"].ToString();
                    _fileNameList.Add(_dbFileName);
                }
                Conn.Close();
            }

            if (_fileList.Count == 0)
            {
                return null;
            }

            int i = _fileNameList.IndexOf(FileName);

            if (i < 0)
            {
                return null;
            }
            byte[] _fileBytesUncompress = SnappyCodec.Uncompress(_fileList[i]);
            FileFormat = _fileFormatList[i];
            FileName = _fileNameList[i];

            return Encoding.UTF8.GetString(_fileBytesUncompress);

        }

        static public void createTableInDB() // Создает таблицу в базе данных если таковой нету
        {
            string dbPath = "Data Source=" + ConfigurationSettings.AppSettings["PathToDB"]; // путь к файлу базы данных
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {   // Написание строки команды для передачи базе данных на выполнение
                string commandText = "CREATE TABLE IF NOT EXISTS [dbFilesStore] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT " +
                    "NOT NULL, [file] BINARY, [file_format] VARCHAR(10), [file_name] NVARCHAR(128))";
                SQLiteCommand Command = new SQLiteCommand(commandText, conn);
                conn.Open();
                Command.ExecuteNonQuery();
                conn.Close();
            }
        }

        static public Dictionary<string, string> findAllFilesInDB() // Находит все файлы записанные в базу данных
        {
            Dictionary<string, string> _fileInfo = new Dictionary<string, string>();
            string dbPath = "Data Source=" + ConfigurationSettings.AppSettings["PathToDB"];
            using (SQLiteConnection Conn = new SQLiteConnection(dbPath))
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand
                {
                    Connection = Conn,
                    CommandText = @"SELECT * FROM [dbFilesStore] WHERE [file_format] NOT NULL"
                };
                SQLiteDataReader sqlReader = Command.ExecuteReader();
                string _dbFileFormat = null;
                string _dbFileName = null;
                while (sqlReader.Read())
                {
                    _dbFileFormat = sqlReader["file_format"].ToString().TrimStart().TrimEnd();
                    _dbFileName = sqlReader["file_name"].ToString();
                    _fileInfo.Add(_dbFileName, _dbFileFormat);
                }
                Conn.Close();
            }

            if (_fileInfo.Count == 0)
            {
                return null;
            }
            return _fileInfo;
        }

        static public async void updateFileInDBAsync(string _fileText) // Обновляет ранее записаный файл в базе данных
        {
            byte[] _fileBytes = Encoding.UTF8.GetBytes(_fileText);
            byte[] _fileBytesCompress = SnappyCodec.Compress(_fileBytes);
            string dbPath = "Data Source=" + ConfigurationSettings.AppSettings["PathToDB"];
            using (SQLiteConnection Conn = new SQLiteConnection(dbPath))
            {

                string commandTe = "DELETE FROM [dbFilesStore] WHERE [file_name] = '" + FileName + "'";
                SQLiteCommand CommandDel = new SQLiteCommand(commandTe, Conn);

                string commandText = "UPDATE [dbFilesStore] SET [file] = @value WHERE [file_name] = '" + FileName + "'";
                SQLiteCommand command = new SQLiteCommand(commandText, Conn);
                command.Parameters.AddWithValue("@value", _fileBytesCompress); // новая начинка файла
                Conn.Open();
                await CommandDel.ExecuteNonQueryAsync();
                await command.ExecuteNonQueryAsync();
                Conn.Close();
            }
        }
    }
}
