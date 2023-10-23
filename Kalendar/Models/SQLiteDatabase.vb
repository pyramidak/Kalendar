Imports System.Data.SQLite

Namespace sqlite.models

    ''' <summary>
    ''' Wraps around a Sqlite database to provide core functionality,
    ''' such as, create, open, close database etc...
    ''' </summary>
    Public Class SQLiteDatabase
#Region "fields"
        Private _DBFilePath As String = ".\"

        Private _DBFileName As String = "database.sqlite"

        Private _Connection As SQLiteConnection = Nothing

        Private _EnforceForeignKeys As Boolean = True

        Private _MutliThreadAccess As Boolean = False
#End Region

#Region "ctors"
        ''' <summary>
        ''' Class Constructor
        ''' </summary>
        Public Sub New(ByVal dbFileName As String, ByVal Optional enforceForeignKeys As Boolean = True, ByVal Optional mutliThreadAccess As Boolean = False)
            If String.IsNullOrEmpty(dbFileName) = False Then _DBFileName = dbFileName

            Status = "Not connected - no connection initialized."

            ExtendendStatus = String.Empty
            Me.Exception = Nothing
            _EnforceForeignKeys = enforceForeignKeys
            _MutliThreadAccess = mutliThreadAccess
        End Sub
#End Region

#Region "properties"
        ''' <summary>
        ''' Gets the name And path of the SQLite file in which the tree
        ''' model data Is to be stored.
        ''' </summary>
        Public ReadOnly Property DBFileNamePath As String
            Get
                Return System.IO.Path.Combine(_DBFilePath, _DBFileName)
            End Get
        End Property

        ''' <summary>
        ''' Gets whether Foreign Keys are:
        ''' - Enforced in the SQLite database file
        '''   (wrong values will result in exception on update Or insert) Or
        '''   
        ''' - Not enforced
        '''   (wrong values are ignored by SQLite).
        ''' </summary>
        Public ReadOnly Property EnforceForeignKeys As Boolean
            Get
                Return _EnforceForeignKeys
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the SQLite database file can be accessed from within multiple
        ''' threads And/Or connections Or Not.
        ''' </summary>
        Public ReadOnly Property MutliThreadAccess As Boolean
            Get
                Return _MutliThreadAccess
            End Get
        End Property

        ''' <summary>
        ''' Gets the Name of the SQLite file in which the tree
        ''' model data Is to be stored.
        ''' </summary>
        Public ReadOnly Property DBFileName As String
            Get
                Return _DBFileName
            End Get
        End Property

        ''' <summary>
        ''' Gets the Path of the SQLite file in which the tree
        ''' model data Is to be stored.
        ''' </summary>
        Public ReadOnly Property DBFilePath As String
            Get
                Return _DBFilePath
            End Get
        End Property

        ''' <summary>
        ''' Gets extended information on exceptions that might have
        ''' occurred to reach the current status.
        ''' </summary>
        Public Property Exception As Exception

        ''' <summary>
        ''' Gets a textual description of the current SQLite database status.
        ''' </summary>
        Public Property Status As String

        ''' <summary>
        ''' Gets/sets additional error/state information (if any).
        ''' </summary>
        Public Property ExtendendStatus As String

        ''' <summary>
        ''' Gets a connection object that can be uses
        ''' to interact with an existing And open SQLite database.
        ''' </summary>
        Public ReadOnly Property Connection As SQLiteConnection
            Get
                Return _Connection
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the database connection Is currently established (open), Or Not (false).
        ''' </summary>
        Public ReadOnly Property ConnectionState As Boolean
            Get
                Try
                    If _Connection IsNot Nothing Then
                        If _Connection.State = System.Data.ConnectionState.Open Then Return True
                    End If

                    Return False
                Catch exp As Exception
                    Status = exp.Message
                    Me.Exception = exp
                    Throw New Exception(exp.Message)
                End Try
            End Get
        End Property
#End Region

#Region "methods"
        ''' <summary>
        ''' Opens a connection to a SQLite database if there Is none already open.
        ''' 
        ''' The previously existing database Is deleted if <paramref name="overwriteFile"/>
        ''' Is true.
        ''' </summary>
        ''' <param name="overwriteFile"></param>
        Public Sub OpenConnection(ByVal Optional overwriteFile As Boolean = False)
            Try
                If _Connection IsNot Nothing Then
                    If _Connection.State = System.Data.ConnectionState.Open Then Return
                Else
                    ConstructConnection(overwriteFile)
                End If

                _Connection.Open()
            Catch exp As Exception
                Status = exp.Message
                Me.Exception = exp
                Throw New Exception(exp.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Closes any open connections to the SQLite database.
        ''' </summary>
        Public Sub CloseConnection()
            Try
                If ConnectionState = True Then _Connection.Close()
                _Connection = Nothing
            Catch exp As Exception
                Status = exp.Message
                Me.Exception = exp
                Throw New Exception(exp.Message)
            End Try
        End Sub

#Region "Pragma UserVersion"
        ''' <summary>
        ''' Gets the current user version of the currently
        ''' opened database (Or throws an exception if database was unavailable).
        ''' </summary>
        ''' <returns></returns>
        Public Function UserVersion() As Long
            Try
                Using cmd As SQLiteCommand = New SQLiteCommand(_Connection)
                    cmd.CommandText = "pragma user_version;"
                    Return CLng(cmd.ExecuteScalar())
                End Using
            Catch exp As Exception
                Status = exp.Message
                Me.Exception = exp
                Throw New Exception(exp.Message)
            End Try
        End Function

        ''' <summary>
        ''' Method increases the current user version of the currently
        ''' opened database (Or throw an exception if database was unavailable).
        ''' </summary>
        ''' <returns></returns>
        Public Function UserVersionIncrease() As Long
            Dim version = UserVersion()
            Try
                Using cmd As SQLiteCommand = New SQLiteCommand(_Connection)
                    cmd.CommandText = String.Format("pragma user_version = {0};", version + 1)
                    cmd.ExecuteNonQuery()
                End Using

                Return UserVersion()
            Catch exp As Exception
                Status = exp.Message
                Me.Exception = exp
                Throw New Exception(exp.Message)
            End Try
        End Function
#End Region

#Region "Pragma Pragma JournalMode"
        ''' <summary>
        ''' Gets the current journal mode of the currently
        ''' opened database (Or throws an exception if database was unavailable).
        ''' </summary>
        ''' <returns></returns>
        Public Function JournalMode() As String
            Try
                Using cmd As SQLiteCommand = New SQLiteCommand(_Connection)
                    cmd.CommandText = "pragma journal_mode;"
                    Dim result = cmd.ExecuteScalar()
                    Return TryCast(result, String)
                End Using
            Catch exp As Exception
                Status = exp.Message
                Me.Exception = exp
                Throw New Exception(exp.Message)
            End Try
        End Function

        ''' <summary>
        ''' Method sets the journal mode of the currently
        ''' opened database (Or throws an exception if database was unavailable).
        ''' </summary>
        Public Sub JournalMode(ByVal journalMode As JournalMode)
            Try
                Using cmd As SQLiteCommand = New SQLiteCommand(_Connection)
                    cmd.CommandText = String.Format("pragma journal_mode = {0};", journalMode.ToString())
                    cmd.ExecuteNonQuery()
                End Using

                Return
            Catch exp As Exception
                Status = exp.Message
                Me.Exception = exp
                Throw New Exception(exp.Message)
            End Try
        End Sub
#End Region

        Private Sub ConstructConnection(ByVal Optional overWriteFile As Boolean = False)

            Dim connectString As SQLiteConnectionStringBuilder = New SQLiteConnectionStringBuilder()

            connectString.DataSource = DBFileNamePath
            connectString.ForeignKeys = EnforceForeignKeys
            connectString.JournalMode = GetJournalMode()

            _Connection = New SQLiteConnection(connectString.ToString())
            If System.IO.File.Exists(DBFileNamePath) = False Then

                ' Overwrites a file if it Is already there
                SQLiteConnection.CreateFile(DBFileNamePath)
                Status = "Created New Database."
            Else
                If overWriteFile = False Then
                    Status = "Using exsiting Database."
                Else
                    ' Overwrites a file if it Is already there
                    SQLiteConnection.CreateFile(DBFileNamePath)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Determines the journal model of the SQLite database - this Is
        ''' required to at conneciton/database open time.
        ''' </summary>
        ''' <returns></returns>
        Private Function GetJournalMode() As SQLiteJournalModeEnum
            Return (If(Me.MutliThreadAccess, SQLiteJournalModeEnum.Wal, SQLiteJournalModeEnum.[Default]))
        End Function
#End Region
    End Class
End Namespace
