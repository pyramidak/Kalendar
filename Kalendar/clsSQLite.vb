Imports System.Data.SQLite
Imports RootSpace.sqlite.models

Public Class clsSQLite
    Private db_file As String
    Private db As SQLiteDatabase
    Sub New(filename As String)
        db_file = filename
        db = New SQLiteDatabase(filename)
        create()
    End Sub

#Region " Close "
    Sub close()
        If db.ConnectionState Then db.CloseConnection()
    End Sub

#End Region

#Region " Create "

    Function create() As Boolean
        create = False
        Try
            db.OpenConnection()
            If db.ConnectionState = False Then
                MessageBox.Show("Databáze není přístupná." + NR + db_file, "Uložení databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
            Else
                Dim createQuery As String = "CREATE TABLE IF NOT EXISTS
                    [osoby] (
                    [uid] INTEGER PRIMARY KEY AUTOINCREMENT,                    
                    [jmeno] TEXT,
                    [rodne] TEXT,                    
                    [narozeni] TEXT,
                    [umrti] TEXT,
                    [zmena] TEXT,
                    [smazane] INTEGER
                    )"

                Using cmd As SQLiteCommand = New SQLiteCommand(db.Connection)
                    cmd.CommandText = createQuery
                    cmd.ExecuteNonQuery()
                End Using

                createQuery = "CREATE TABLE IF NOT EXISTS
                    [upominky] (
                    [uid] INTEGER PRIMARY KEY AUTOINCREMENT,                    
                    [vznik] TEXT,                    
                    [den] TEXT,                    
                    [text] TEXT,
                    [mesicne] INTEGER,
                    [rocne] INTEGER,
                    [zmena] TEXT
                    )"

                Using cmd As SQLiteCommand = New SQLiteCommand(db.Connection)
                    cmd.CommandText = createQuery
                    cmd.ExecuteNonQuery()
                End Using

                update()

                create = True
            End If
        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Vytvoření databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Function

    Private Sub update()
        If db.UserVersion() = 0 Then db.UserVersionIncrease()
        If db.UserVersion() = 1 Then
            Using cmd As SQLiteCommand = New SQLiteCommand(db.Connection)
                cmd.CommandText = "ALTER TABLE upominky ADD COLUMN [alarm] TEXT"
                cmd.ExecuteNonQuery()
            End Using
            db.UserVersionIncrease()
        End If
    End Sub

#End Region

#Region " Load "
    Sub load(database As clsDBase)
        Try
            'Čtení
            Dim query = "SELECT * FROM osoby"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using selectResult As SQLiteDataReader = cmd.ExecuteReader()
                    If selectResult.HasRows = True Then
                        database.Osoby.Clear()
                        While selectResult.Read()
                            Dim osoba = New clsDBase.clsOsoba()
                            osoba.Uid = getLng(selectResult("uid"))
                            osoba.Jmeno = getString(selectResult("jmeno"))
                            osoba.RodneCislo = getString(selectResult("rodne"))
                            osoba.Narozeni = getDate(selectResult("narozeni"))
                            osoba.Umrti = getDate(selectResult("umrti"))
                            osoba.Zmena = getDate(selectResult("zmena"))
                            osoba.Smazane = getBool(selectResult("smazane"))
                            database.Osoby.Add(osoba)
                        End While
                    End If
                End Using
            End Using

            query = "SELECT * FROM upominky"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using selectResult As SQLiteDataReader = cmd.ExecuteReader()
                    If selectResult.HasRows = True Then
                        database.Poznamky.Clear()
                        While selectResult.Read()
                            Dim upominka = New clsDBase.clsPoznamka
                            upominka.Uid = getLng(selectResult("uid"))
                            upominka.Vznik = getDate(selectResult("vznik"))
                            upominka.Den = getDate(selectResult("den"))
                            upominka.Text = getString(selectResult("text"))
                            upominka.Mesicne = getBool(selectResult("mesicne"))
                            upominka.Rocne = getBool(selectResult("rocne"))
                            upominka.Zmena = getDate(selectResult("zmena"))
                            upominka.Alarm = getDate(selectResult("alarm"))
                            database.Poznamky.Add(upominka)
                        End While
                    End If
                End Using
            End Using

        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Načtení databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

    Private Function getDate(val As Object) As Date
        If IsDBNull(val) Then Return Nothing
        If val.ToString = "" Then Return Nothing
        Return DateTime.ParseExact(val.ToString, "yyyy-MM-ddTHH:mm:ss", Globalization.CultureInfo.InvariantCulture)
    End Function
    Private Function getString(val As Object) As String
        If IsDBNull(val) Then Return ""
        Return val.ToString
    End Function
    Private Function getInt(val As Object) As Integer
        If IsDBNull(val) Then Return 0
        Return CInt(val)
    End Function
    Private Function getBool(val As Object) As Boolean
        Return CBool(getInt(val))
    End Function
    Private Function getLng(val As Object) As Long
        If IsDBNull(val) Then Return 0
        Return CLng(val)
    End Function

#End Region

#Region " Save "
    Sub save(database As clsDBase)
        Try
            'Mazání
            Dim cmdDeleteRecords = New SQLiteCommand("DELETE FROM osoby", db.Connection)
            cmdDeleteRecords.ExecuteNonQuery()

            'Reset autoincrement
            Dim cmdResetAutoincrement = New SQLiteCommand("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='osoby'", db.Connection)
            cmdResetAutoincrement.ExecuteNonQuery()

            'Ukládání
            Dim query As String = "INSERT INTO osoby (
                                    [jmeno],[rodne],[narozeni],[umrti],[zmena],[smazane])
                                    VALUES(@jmeno,@rodne,@narozeni,@umrti,@zmena,@smazane)"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Dim result As Integer = 0
                Using transaction = cmd.Connection.BeginTransaction()
                    For Each osoba In database.Osoby
                        cmd.Parameters.AddWithValue("@jmeno", osoba.Jmeno)
                        cmd.Parameters.AddWithValue("@rodne", osoba.RodneCislo)
                        cmd.Parameters.AddWithValue("@narozeni", setDate(osoba.Narozeni))
                        cmd.Parameters.AddWithValue("@umrti", setDate(osoba.Umrti))
                        cmd.Parameters.AddWithValue("@zmena", setDate(osoba.Zmena))
                        cmd.Parameters.AddWithValue("@smazane", osoba.Smazane)
                        result += cmd.ExecuteNonQuery()
                    Next
                    transaction.Commit()
                End Using
            End Using

            'Mazání
            cmdDeleteRecords = New SQLiteCommand("DELETE FROM upominky", db.Connection)
            cmdDeleteRecords.ExecuteNonQuery()

            'Reset autoincrement
            cmdResetAutoincrement = New SQLiteCommand("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='upominky'", db.Connection)
            cmdResetAutoincrement.ExecuteNonQuery()

            'Ukládání
            query = "INSERT INTO upominky (
                                    [vznik],[den],[text],[mesicne],[rocne],[zmena],[alarm])
                                    VALUES(@vznik,@den,@text,@mesicne,@rocne,@zmena,@alarm)"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Dim result As Integer = 0
                Using transaction = cmd.Connection.BeginTransaction()
                    For Each upominka In database.Poznamky
                        cmd.Parameters.AddWithValue("@vznik", setDate(upominka.Vznik))
                        cmd.Parameters.AddWithValue("@den", setDate(upominka.Den))
                        cmd.Parameters.AddWithValue("@text", upominka.Text)
                        cmd.Parameters.AddWithValue("@mesicne", upominka.Mesicne)
                        cmd.Parameters.AddWithValue("@rocne", upominka.Rocne)
                        cmd.Parameters.AddWithValue("@zmena", setDate(upominka.Zmena))
                        cmd.Parameters.AddWithValue("@alarm", setDate(upominka.Alarm))
                        result += cmd.ExecuteNonQuery()
                    Next
                    transaction.Commit()
                End Using
            End Using

        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Uložení databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

    Private Function setDate(datum As Date) As String
        If datum.Year = 1 Then Return ""
        Return datum.ToString("s", Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Function setInt(state As Boolean) As Integer
        Return CInt(state)
    End Function

#End Region

#Region " Insert "
    Function insertOsoba(osoba As clsDBase.clsOsoba) As Long
        Try
            'Insert
            Dim query As String = "INSERT INTO osoby (
                                    [jmeno],[rodne],[narozeni],[umrti],[zmena],[smazane])
                                    VALUES(@jmeno,@rodne,@narozeni,@umrti,@zmena,@smazane)"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using transaction = cmd.Connection.BeginTransaction()
                    cmd.Parameters.AddWithValue("@jmeno", osoba.Jmeno)
                    cmd.Parameters.AddWithValue("@rodne", osoba.RodneCislo)
                    cmd.Parameters.AddWithValue("@narozeni", setDate(osoba.Narozeni))
                    cmd.Parameters.AddWithValue("@umrti", setDate(osoba.Umrti))
                    cmd.Parameters.AddWithValue("@zmena", setDate(osoba.Zmena))
                    cmd.Parameters.AddWithValue("@smazane", osoba.Smazane)
                    cmd.ExecuteNonQuery()
                    insertOsoba = cmd.Connection.LastInsertRowId
                    transaction.Commit()
                End Using
            End Using
            dataChanged = True

        Catch exp As System.Exception
            insertOsoba = 0
            MessageBox.Show(exp.Message, "Vložení do databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Function

    Function insertUpominka(upominka As clsDBase.clsPoznamka) As Long
        Try
            'Insert
            Dim query As String = "INSERT INTO upominky (
                                    [vznik],[den],[text],[mesicne],[rocne],[zmena],[alarm])
                                    VALUES(@vznik,@den,@text,@mesicne,@rocne,@zmena,@alarm)"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using transaction = cmd.Connection.BeginTransaction()
                    cmd.Parameters.AddWithValue("@vznik", setDate(upominka.Vznik))
                    cmd.Parameters.AddWithValue("@den", setDate(upominka.Den))
                    cmd.Parameters.AddWithValue("@text", upominka.Text)
                    cmd.Parameters.AddWithValue("@mesicne", upominka.Mesicne)
                    cmd.Parameters.AddWithValue("@rocne", upominka.Rocne)
                    cmd.Parameters.AddWithValue("@zmena", setDate(upominka.Zmena))
                    cmd.Parameters.AddWithValue("@alarm", setDate(upominka.Alarm))
                    cmd.ExecuteNonQuery()
                    insertUpominka = cmd.Connection.LastInsertRowId
                    transaction.Commit()
                End Using
            End Using
            dataChanged = True

        Catch exp As System.Exception
            insertUpominka = 0
            MessageBox.Show(exp.Message, "Vložení do databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Function

#End Region

#Region " Update "
    Sub updateOsoba(osoba As clsDBase.clsOsoba)
        Try
            'Update
            Dim query As String = "UPDATE osoby
                                    SET jmeno = @jmeno,
                                        rodne = @rodne,
                                        narozeni = @narozeni,
                                        umrti = @umrti,
                                        zmena = @zmena,
                                        smazane = @smazane
                                   WHERE uid = @uid"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using transaction = cmd.Connection.BeginTransaction()
                    cmd.Parameters.AddWithValue("@uid", osoba.Uid)
                    cmd.Parameters.AddWithValue("@jmeno", osoba.Jmeno)
                    cmd.Parameters.AddWithValue("@rodne", osoba.RodneCislo)
                    cmd.Parameters.AddWithValue("@narozeni", setDate(osoba.Narozeni))
                    cmd.Parameters.AddWithValue("@umrti", setDate(osoba.Umrti))
                    cmd.Parameters.AddWithValue("@zmena", setDate(osoba.Zmena))
                    cmd.Parameters.AddWithValue("@smazane", osoba.Smazane)
                    cmd.ExecuteNonQuery()
                    transaction.Commit()
                End Using
            End Using
            dataChanged = True

        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Aktualizace do databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

    Sub updateUpominka(upominka As clsDBase.clsPoznamka)
        Try
            'Update
            Dim query As String = "UPDATE upominky
                                   SET vznik = @vznik, 
                                       den = @den,
                                       text = @text,
                                       mesicne = @mesicne,
                                       rocne = @rocne,
                                       zmena = @zmena,
                                       alarm = @alarm
                                   WHERE uid = @uid"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using transaction = cmd.Connection.BeginTransaction()
                    cmd.Parameters.AddWithValue("@uid", upominka.Uid)
                    cmd.Parameters.AddWithValue("@vznik", setDate(upominka.Vznik))
                    cmd.Parameters.AddWithValue("@den", setDate(upominka.Den))
                    cmd.Parameters.AddWithValue("@text", upominka.Text)
                    cmd.Parameters.AddWithValue("@mesicne", upominka.Mesicne)
                    cmd.Parameters.AddWithValue("@rocne", upominka.Rocne)
                    cmd.Parameters.AddWithValue("@zmena", setDate(upominka.Zmena))
                    cmd.Parameters.AddWithValue("@alarm", setDate(upominka.Alarm))
                    cmd.ExecuteNonQuery()
                    transaction.Commit()
                End Using
            End Using
            dataChanged = True

        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Aktualizace do databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

#End Region

#Region " Delete "
    Sub deleteOsoba(osoba As clsDBase.clsOsoba)
        Try
            'Update
            Dim query As String = "DELETE FROM osoby
                                   WHERE [uid] = @uid"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using transaction = cmd.Connection.BeginTransaction()
                    cmd.Parameters.AddWithValue("@uid", osoba.Uid)
                    cmd.ExecuteNonQuery()
                    transaction.Commit()
                End Using
            End Using
            dataChanged = True

        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Odebrání z databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

    Sub deleteUpominka(upominka As clsDBase.clsPoznamka)
        Try
            'Update
            Dim query As String = "DELETE FROM upominky
                                   WHERE [uid] = @uid"
            Using cmd As SQLiteCommand = New SQLiteCommand(query, db.Connection)
                Using transaction = cmd.Connection.BeginTransaction()
                    cmd.Parameters.AddWithValue("@uid", upominka.Uid)
                    cmd.ExecuteNonQuery()
                    transaction.Commit()
                End Using
            End Using
            dataChanged = True

        Catch exp As System.Exception
            MessageBox.Show(exp.Message, "Odebrání z databáze", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

#End Region

End Class
