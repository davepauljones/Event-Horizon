using System.IO;

namespace The_Oracle
{
    public class OracleDatabaseFileWatcher
    {
        public FileSystemWatcher watcher;

        public OracleDatabaseFileWatcher(string path, MainWindow.OnOracleDatabaseChanged OnChanged )
        {
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            //watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
            //   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.NotifyFilter = NotifyFilters.LastAccess;
            // Only watch text files.

            //switch (XMLReaderWriter.DatabaseSystem)
            //{
            //    case DatabaseSystems.AccessMDB:
            //        watcher.Filter = "*.mdb";
            //        break;
            //    case DatabaseSystems.SQLite:
            //        watcher.Filter = "*.db3";
            //        break;
            //}

            watcher.Filter = XMLReaderWriter.GlobalDatabaseString + XMLReaderWriter.GlobalDatabaseFileExtensionString;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        //private static void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    // Specify what is done when a file is changed, created, or deleted.
        //    Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        //}

        //private static void OnRenamed(object source, RenamedEventArgs e)
        //{
        //    // Specify what is done when a file is renamed.
        //    Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        //}
    }
}