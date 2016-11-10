using System;
using System.Data.SQLite;

namespace PostExpressGaleb.Common
{
    public class Konekcija
    {
        public static SQLiteConnection VratiKonekciju()
        {
            string bazaIme = "DbPostExpressGaleb.sl3";
            string path = AppDomain.CurrentDomain.BaseDirectory + bazaIme;
                        
            SQLiteConnectionStringBuilder conString = new SQLiteConnectionStringBuilder();
            conString.DataSource = bazaIme;
            conString.DefaultTimeout = 5000;
            conString.SyncMode = SynchronizationModes.Off;
            conString.JournalMode = SQLiteJournalModeEnum.Memory;
            conString.PageSize = 65536;
            conString.CacheSize = 16777216;
            conString.FailIfMissing = false;
            conString.ReadOnly = false;

            SQLiteConnection con = new SQLiteConnection(conString.ToString());
            return con;
        }
    }
}
