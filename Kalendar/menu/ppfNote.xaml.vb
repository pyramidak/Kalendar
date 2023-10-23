Class ppfNote

    Private FromDate As Date
    Private wMain As WpfMain = CType(Application.Current.MainWindow, WpfMain)
    Private wSetting As wpfSetting = Application.SettingWindow()

#Region " Loaded "

    Private Sub Objekty(ByVal Stav As Boolean)
        btnClear.IsEnabled = Stav
        cboDatum.IsEnabled = Stav
        btnImport.IsEnabled = Stav
    End Sub

    Private Sub StavUpominek()
        Objekty(If(Databaze.Poznamky.Count = 0, False, True))
        txtInfo.Text = "plánů celkem: " + Databaze.Poznamky.Count.ToString
    End Sub

    Private Sub ppfNote_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Dim datumFirst As Date
        cboDatum.Items.Clear()
        cboDatum.Items.Add(datumTed & " (tento den)")
        For a = -1 To 5
            datumFirst = DateAdd(Microsoft.VisualBasic.DateInterval.Day, Not a, datumTed)
            If Weekday(datumFirst) = 2 Then Exit For
        Next a
        cboDatum.Items.Add(datumFirst & " (tento týden)")
        cboDatum.Items.Add("1." & Month(datumTed) & "." & Year(datumTed) & " (tento měsíc)")
        If Month(datumTed) - 6 > 1 Then cboDatum.Items.Add("1." & Month(datumTed) - 6 & "." & Year(datumTed) & " (6 měsíců)")
        cboDatum.Items.Add("1.1." & Year(datumTed) & " (tento rok)")
        cboDatum.Items.Add("1.1." & Year(datumTed) - 2 & " (dva roky)")
        cboDatum.Items.Add("1.1." & Year(datumTed) - 5 & " (pět let)")
        For a As Integer = 10 To 50 Step 4
            cboSize.Items.Add(a)
            If a = Nastaveni.FontSize Or a + 2 = Nastaveni.FontSize Then cboSize.SelectedItem = a
        Next
        cboSize.SelectedItem = CInt(Nastaveni.FontSize)

        StavUpominek()
    End Sub

#End Region

#Region " Info "

    Private Sub cboDatum_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)
        Dim sDatum As String = RemoveBrackets(cboDatum.Text)
        If IsDate(sDatum) Then
            FromDate = CDate(sDatum)
            ShowInfo()
        End If
    End Sub

    Private Sub cboDatum_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cboDatum.SelectionChanged
        If cboDatum.SelectedItem Is Nothing Then Exit Sub
        FromDate = CDate(RemoveBrackets(cboDatum.SelectedItem.ToString))
        ShowInfo()
    End Sub

    Private Function RemoveBrackets(ByVal sText As String) As String
        Dim i As Integer = sText.IndexOf("(")
        If i = -1 Then
            Return sText
        Else
            Return sText.Substring(0, i - 1)
        End If
    End Function

    Private Sub ShowInfo()
        txtInfo.Text = Databaze.Poznamky.Where(Function(x) x.Den < FromDate).Count.ToString + " nalezených plánů připravených ke smazání. Klikněte na Odstranit."
    End Sub

#End Region

#Region " Remove Notes "

    Private Sub btnClear_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnClear.Click
        Objekty(False)
        RemoveOldNotes()
        StavUpominek()
    End Sub

    Private Sub RemoveOldNotes()
        Databaze.Poznamky.Where(Function(x) x.Mesicne = False And x.Rocne = False And x.Den < FromDate).ToList.ForEach(Sub(y)
                                                                                                                           mySQL.deleteUpominka(y)
                                                                                                                           Databaze.Poznamky.Remove(y)
                                                                                                                       End Sub)
    End Sub

#End Region

#Region " Add Notes "

    Private Sub btnUpominky_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        ' Configure open file dialog box 
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Title = "Najděte databázový soubor Kalendáře"
        dlg.Filter = "Databáze Kalendáře|KalendarDatS.xml;kalendar.db"

        If dlg.ShowDialog Then
            If dlg.FileName.ToLower = dbDatabaze.ToLower Then
                Call (New wpfDialog(wSetting, "Do této databáze se budou přidávat data." & NR & dbDatabaze, "Správa plánů", wpfDialog.Ikona.varovani, "Zavřít")).ShowDialog()
                Exit Sub
            End If
            Objekty(False)
            Dim Pocet As Integer = Databaze.Poznamky.Count
            If myFile.Name(dlg.FileName, False) = "KalendarDatS" Then
                AddXmlSNotes(dlg.FileName)
            Else
                AddDbNotes(dlg.FileName)
            End If
            Call (New wpfDialog(wSetting, "Počet přidaných plánů: " & Databaze.Poznamky.Count - Pocet, "Správa plánů", wpfDialog.Ikona.ok, "Zavřít")).ShowDialog()
            Objekty(True)
            StavUpominek()
        End If
    End Sub

    Private Sub AddXmlSNotes(ByVal addFile As String)
        Dim oldDatabaze As New clsDatabase
        oldDatabaze = CType(New clsSerialization(oldDatabaze, wSetting).ReadXml(addFile), clsDatabase)
        For Each one In oldDatabaze.Poznamky
            Dim Poznamka = Databaze.Poznamky.FirstOrDefault(Function(x) x.Den.Date = one.Dne.Date)
            If Poznamka Is Nothing Then
                Dim poznamkaNew = New clsDBase.clsPoznamka(Now, one.Dne, one.Poznamka, False, one.Opakovat, one.Zmena)
                poznamkaNew.Uid = mySQL.insertUpominka(poznamkaNew)
                Databaze.Poznamky.Add(poznamkaNew)
            Else
                If one.Zmena > Poznamka.Zmena Then
                    Poznamka.Text = one.Poznamka
                    Poznamka.Zmena = Now
                    Poznamka.Rocne = one.Opakovat
                    mySQL.updateUpominka(Poznamka)
                End If
            End If
        Next
    End Sub

    Private Sub AddDbNotes(ByVal addFile As String)
        Dim exDatabaze As New clsDBase
        Dim exSQL = New clsSQLite(addFile)
        exSQL.load(exDatabaze)

        For Each one In exDatabaze.Poznamky
            Dim poznamka = Databaze.Poznamky.FirstOrDefault(Function(x) x.Vznik = one.Vznik)
            If poznamka Is Nothing Then
                Dim poznamkaNew = New clsDBase.clsPoznamka(one.Vznik, one.Den, one.Text, one.Mesicne, one.Rocne, one.Zmena)
                poznamkaNew.Uid = mySQL.insertUpominka(poznamkaNew)
                Databaze.Poznamky.Add(poznamkaNew)
            Else
                If one.Zmena > poznamka.Zmena Then
                    one.Den = one.Den
                    poznamka.Text = one.Text
                    poznamka.Mesicne = one.Mesicne
                    poznamka.Rocne = one.Rocne
                    poznamka.Zmena = Now
                    mySQL.updateUpominka(poznamka)
                End If
            End If
        Next
    End Sub

#End Region

    Private Sub cboSize_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSize.SelectionChanged
        Nastaveni.FontSize = CInt(cboSize.SelectedItem)
        wMain.Tyden.FontSize = Nastaveni.FontSize
    End Sub

End Class
