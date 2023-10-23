
Namespace sqlite.models

    ''' <summary>
    ''' Models the Pragma Journal_Mode enumeration
    ''' - (<see cref="SQLiteDatabase.JournalMode(JournalMode)"/> And <see cref="SQLiteDatabase.JournalMode"/>).
    ''' 
    ''' See details described below And:
    ''' https//sqlite.org/pragma.html#pragma_journal_mode
    ''' https://sqlite.org/wal.html
    ''' </summary>
    Public Enum JournalMode

        ''' <summary>
        ''' The DELETE journaling mode Is the normal behavior. In the DELETE mode, the rollback journal Is
        ''' deleted at the conclusion of each transaction. Indeed, the delete operation Is the action that
        ''' causes the transaction to commit. (See the document titled Atomic Commit In SQLite for
        ''' additional detail.)
        ''' </summary>
        DELETE = 0

        ''' <summary>
        ''' The TRUNCATE journaling mode commits transactions by truncating the rollback journal
        ''' to zero-length instead of deleting it. On many systems, truncating a file Is much faster
        ''' than deleting the file since the containing directory does Not need to be changed.
        ''' </summary>
        TRUNCATE = 1

        ''' <summary>
        ''' The PERSIST journaling mode prevents the rollback journal from being deleted at the end
        ''' of each transaction. Instead, the header of the journal Is overwritten with zeros. This
        ''' will prevent other database connections from rolling the journal back. The PERSIST journaling
        ''' mode Is useful as an optimization on platforms where deleting Or truncating a file Is much
        ''' more expensive than overwriting the first block of a file with zeros. See also: PRAGMA
        ''' journal_size_limit And SQLITE_DEFAULT_JOURNAL_SIZE_LIMIT.
        ''' </summary>
        PERSIST = 2

        ''' <summary>
        ''' The MEMORY journaling mode stores the rollback journal in volatile RAM. This saves disk I/O
        ''' but at the expense of database safety And integrity. If the application using SQLite crashes
        ''' in the middle of a transaction when the MEMORY journaling mode Is set, then the database file
        ''' will very likely go corrupt.
        ''' </summary>
        MEMORY = 3

        ''' <summary>
        ''' Indicates the WAL Journal Mode
        ''' WAL mode Is persisted as documented here: https://sqlite.org/wal.html
        ''' </summary>
        WAL = 4

        ''' <summary>
        ''' The OFF journaling mode disables the rollback journal completely. No rollback journal Is ever
        ''' created And hence there Is never a rollback journal to delete. The OFF journaling mode disables
        ''' the atomic commit And rollback capabilities of SQLite. The ROLLBACK command no longer works; it
        ''' behaves in an undefined way. Applications must avoid using the ROLLBACK command when the journal
        ''' mode Is OFF. If the application crashes in the middle of a transaction when the OFF journaling
        ''' mode Is set, then the database file will very likely go corrupt.
        ''' </summary>
        OFF = 5
    End Enum
End Namespace
