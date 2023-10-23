Class ppfBirthday

    Private RadioButtonInitialized As Boolean
    Private sOslavenec As String
    Private wSetting As wpfSetting = Application.SettingWindow()

#Region " Loaded "

    Private Sub ppfBirthday_Initialized(sender As Object, e As System.EventArgs) Handles Me.Initialized
        AddHandler rbnPocLet.Checked, AddressOf rbnPocLet_Checked
        AddHandler rbnRokNar.Checked, AddressOf rbnRokNar_Checked
        AddHandler rbnSmrtLet.Checked, AddressOf rbnSmrtLet_Checked
        AddHandler rbnSmrtRok.Checked, AddressOf rbnSmrtRok_Checked
        rbnRokNar.IsChecked = True
        cboNarozenin.DisplayMemberPath = "Jmeno"
        RefreshNarozeniny()
    End Sub

    Private Sub ppfBirthday_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If Not sOslavenec = "" Then
            cboNarozenin.Text = sOslavenec
            proDopln()
        End If
    End Sub

    Public Sub New(ByVal Oslavenec As String)
        InitializeComponent()
        sOslavenec = Oslavenec
    End Sub

    Private Sub proEmptyFields()
        txtDenU.Text = "" : txtMesicU.Text = "" : txtRokLet.Text = "" : cboNarozenin.Text = ""
        txtDenSmrt.Text = "" : txtMesicSmrt.Text = "" : txtRokSmrt.Text = "" : txtRC.Text = "" : txtRC.Text = ""
        cmdSave.IsEnabled = False : cmdRemove.IsEnabled = False
    End Sub

    Private Sub RefreshNarozeniny()
        cboNarozenin.ItemsSource = Databaze.Osoby.Where(Function(x) x.Smazane = False).OrderBy(Function(x) x.Jmeno)
        proEmptyFields()
    End Sub

#End Region

#Region " Objekty "

    Private Sub Umrti_checked(sender As Object, e As RoutedEventArgs) Handles ckbUmrti.Checked, ckbUmrti.Unchecked
        proUmrtiObjekty(CBool(ckbUmrti.IsChecked))
    End Sub

    Private Sub proUmrtiObjekty(bChecked As Boolean)
        If Not CBool(ckbUmrti.IsChecked) = bChecked Then ckbUmrti.IsChecked = bChecked : Exit Sub
        rbnSmrtLet.IsEnabled = bChecked
        rbnSmrtRok.IsEnabled = bChecked
        txtRokSmrt.IsEnabled = bChecked
        txtMesicSmrt.IsEnabled = bChecked
        txtDenSmrt.IsEnabled = bChecked
        If bChecked = False Then
            txtRokSmrt.Text = ""
            txtMesicSmrt.Text = ""
            txtDenSmrt.Text = ""
        End If
    End Sub

    Private Sub proDopln()
        If cboNarozenin.SelectedItem Is Nothing Then
            cmdRemove.IsEnabled = False
            Exit Sub
        End If

        Dim Osoba = TryCast(cboNarozenin.SelectedItem, clsDBase.clsOsoba)
        If Osoba Is Nothing Then
            cmdRemove.IsEnabled = False
            Exit Sub
        End If

        cboNarozenin.Text = Osoba.Jmeno
        txtDenU.Text = Osoba.Narozeni.Day.ToString
        txtMesicU.Text = Osoba.Narozeni.Month.ToString
        If rbnRokNar.IsChecked Then
            txtRokLet.Text = Osoba.Narozeni.Year.ToString
        Else
            txtRokLet.Text = CStr(Now.Year - Osoba.Narozeni.Year)
        End If

        proUmrtiObjekty(Not Osoba.Umrti = DateNull)
        If Not Osoba.Umrti = DateNull Then
            txtDenSmrt.Text = Osoba.Umrti.Day.ToString
            txtMesicSmrt.Text = Osoba.Umrti.Month.ToString
            If rbnSmrtRok.IsChecked Then
                txtRokSmrt.Text = Osoba.Umrti.Year.ToString
            Else
                txtRokSmrt.Text = CStr(Osoba.Umrti.Year - Osoba.Narozeni.Year)
            End If
        End If

        If Osoba.RodneCislo = "" Then
            Dim isGirl As Boolean = If(Osoba.Jmeno.EndsWith("ova") Or Osoba.Jmeno.EndsWith("ová"), True, False)
            txtRC.Text = Osoba.Narozeni.Year.ToString.Substring(2, 2) + If(isGirl, CStr(Osoba.Narozeni.Month + 50),
            If(Osoba.Narozeni.Month.ToString.Length = 1, "0" + Osoba.Narozeni.Month.ToString, Osoba.Narozeni.Month.ToString)) +
            If(Osoba.Narozeni.Day.ToString.Length = 1, "0" + Osoba.Narozeni.Day.ToString, Osoba.Narozeni.Day.ToString) + "/"
        Else
            txtRC.Text = Osoba.RodneCislo
        End If

        cmdRemove.IsEnabled = True
    End Sub

    Private Sub cboNarozenin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If cboNarozenin.Text = "" Then
            cboNarozenin.SelectedIndex = -1
            cmdSave.IsEnabled = False
            Exit Sub
        Else
            cmdSave.IsEnabled = True
        End If
        Dim Exist = Databaze.Osoby.Any(Function(x) x.Smazane = False And x.Jmeno.ToLower = cboNarozenin.Text.ToLower)
        cmdSave.Content = If(Exist, "Změnit", "Přidat")
        cmdSave.ToolTip = If(Exist, "Uložit změny.", "Přidat jméno.")
    End Sub

    Private Sub cboNarozenin_SelectionChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboNarozenin.SelectionChanged
        proDopln()
    End Sub

    Private Sub cboNarozenin_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs) Handles cboNarozenin.KeyUp
        If e.Key = Key.Enter Then proDopln()
    End Sub

    Private Sub countRokLet()
        If txtRokLet.Text = "" Then Exit Sub
        If IsNumeric(txtRokLet.Text) = False Then
            txtRokLet.Focus()
            Call (New wpfDialog(wSetting, "Toto není číslo.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")).ShowDialog()
            Exit Sub
        End If
        txtRokLet.Text = CStr(Year(Now) - CInt(txtRokLet.Text))
    End Sub

    Private Sub rbnPocLet_Checked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        countRokLet()
    End Sub

    Private Sub rbnRokNar_Checked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        countRokLet()
    End Sub

    Private Sub rbnSmrtRok_Checked(sender As System.Object, e As System.EventArgs)
        If IsNumeric(txtRokSmrt.Text) = False Then Exit Sub
        Dim iRokNar As Integer = If(rbnRokNar.IsChecked, CInt(txtRokLet.Text), Year(Now) - CInt(txtRokLet.Text))
        txtRokSmrt.Text = CStr(iRokNar + CInt(txtRokSmrt.Text))
    End Sub

    Private Sub rbnSmrtLet_Checked(sender As System.Object, e As System.EventArgs)
        If IsNumeric(txtRokSmrt.Text) = False Then Exit Sub
        Dim iRokNar As Integer = If(rbnRokNar.IsChecked, CInt(txtRokLet.Text), Year(Now) - CInt(txtRokLet.Text))
        txtRokSmrt.Text = CStr(CInt(txtRokSmrt.Text) - iRokNar)
    End Sub
#End Region

#Region " Kontrola "

    Private Function proKontrolaDat() As Boolean
        proKontrolaDat = False

        If txtDenU.Text = "" Then
            txtDenU.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Nezadal(a) jste žádný den.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        ElseIf txtMesicU.Text = "" Then
            txtMesicU.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Nezadal(a) jste žádný měsíc.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        ElseIf IsNumeric(txtDenU.Text) = False Then
            txtDenU.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Zadejte správný den, v tomto je chyba.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        ElseIf IsNumeric(txtMesicU.Text) = False Then
            txtMesicU.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Zadejte správný měsíc, v tomto je chyba.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        End If

        If IsNumeric(txtRokLet.Text) = False Then
            txtRokLet.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Toto není číslo.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        End If
        If rbnRokNar.IsChecked = True Then
            If CInt(txtRokLet.Text) < 1101 Or CInt(txtRokLet.Text) > Year(Now) Then
                txtRokLet.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Zadat lze rok větší než 1100 až po současný rok " & Year(Now) & ".", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            End If
        ElseIf rbnPocLet.IsChecked = True Then
            If CInt(txtRokLet.Text) < 0 Or CInt(txtRokLet.Text) > 150 Then
                txtRokLet.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Nesmysl v počtu let.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            End If
        End If

        If CInt(txtMesicU.Text) < 0 Or CInt(txtMesicU.Text) > 12 Then
            txtMesicU.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Rok má jen 12 měsíců.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        End If

        If CInt(txtDenU.Text) < 0 Or CInt(txtDenU.Text) > myKal.GetMesicMaDnu(CInt(txtMesicU.Text)) Then
            txtDenU.Focus()
            Dim wDialog = New wpfDialog(wSetting, myKal.GetMesic(CInt(txtMesicU.Text)) & " má max. " & myKal.GetMesicMaDnu(CInt(txtMesicU.Text)).ToString & " dnů.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        End If

        Return True
    End Function

    Private Function proKontrolaDat2() As Boolean
        If Len(Trim(cboNarozenin.Text)) < 3 Then
            cboNarozenin.Focus()
            Dim wDialog = New wpfDialog(wSetting, "Tak krátké jméno s příjmením není.", "Narozeniny", wpfDialog.Ikona.varovani, "Zavřít")
            wDialog.ShowDialog()
            Return False
        End If
        Return True
    End Function

    Private Function proKontrolaDat3() As Boolean
        proKontrolaDat3 = False

        If Not txtRokSmrt.Text = "" Or Not txtDenSmrt.Text = "" Or Not txtMesicSmrt.Text = "" Then
            If txtDenSmrt.Text = "" Then
                txtDenSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Nezadal(a) jste žádný den.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            ElseIf txtMesicSmrt.Text = "" Then
                txtMesicSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Nezadal(a) jste žádný měsíc.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            ElseIf IsNumeric(txtDenSmrt.Text) = False Then
                txtDenSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Zadejte správný den, v tomto je chyba.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            ElseIf IsNumeric(txtMesicSmrt.Text) = False Then
                txtMesicSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Zadejte správný měsíc, v tomto je chyba.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            End If

            If IsNumeric(txtRokSmrt.Text) = False Then
                txtRokSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Toto není číslo.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            End If
            If rbnSmrtRok.IsChecked = True Then
                If CInt(txtRokSmrt.Text) < 1101 Or CInt(txtRokSmrt.Text) > Year(Now) Then
                    txtRokSmrt.Focus()
                    Dim wDialog = New wpfDialog(wSetting, "Zadat lze rok větší než 1100 až po současný rok " & Year(Now) & ".", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                    wDialog.ShowDialog()
                    Return False
                End If
            ElseIf rbnSmrtLet.IsChecked = True Then
                If CInt(txtRokSmrt.Text) < 0 Or CInt(txtRokSmrt.Text) > 150 Then
                    txtRokSmrt.Focus()
                    Dim wDialog = New wpfDialog(wSetting, "Nesmysl v počtu let.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                    wDialog.ShowDialog()
                    Return False
                End If
            End If

            If CInt(txtMesicSmrt.Text) < 0 Or CInt(txtMesicSmrt.Text) > 12 Then
                txtMesicSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, "Rok má jen 12 měsíců.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            End If

            If CInt(txtDenSmrt.Text) < 0 Or CInt(txtDenSmrt.Text) > myKal.GetMesicMaDnu(CInt(txtMesicSmrt.Text)) Then
                txtDenSmrt.Focus()
                Dim wDialog = New wpfDialog(wSetting, myKal.GetMesic(CInt(txtMesicSmrt.Text)) & " má max. " & myKal.GetMesicMaDnu(CInt(txtMesicSmrt.Text)).ToString & " dnů.", "Úmrtí", wpfDialog.Ikona.varovani, "Zavřít")
                wDialog.ShowDialog()
                Return False
            End If
        Else
            ckbUmrti.IsChecked = False
        End If

        Return True
    End Function
#End Region

#Region " Data "

    Private Sub Objekty(ByVal Stav As Boolean)
        cboNarozenin.IsEnabled = Stav : txtRokLet.IsEnabled = Stav
        txtDenU.IsEnabled = Stav : txtMesicU.IsEnabled = Stav
        rbnRokNar.IsEnabled = Stav : rbnPocLet.IsEnabled = Stav
        cmdSave.IsEnabled = Stav : btnBackup.IsEnabled = Stav
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        cmdRemove.IsEnabled = False
        If cboNarozenin.SelectedItem Is Nothing Then Exit Sub
        Dim Osoba = TryCast(cboNarozenin.SelectedItem, clsDBase.clsOsoba)
        If Osoba IsNot Nothing Then
            Osoba.Smazane = True
            mySQL.updateOsoba(Osoba)
        End If
        RefreshNarozeniny()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        If proKontrolaDat() = False Or proKontrolaDat2() = False Or proKontrolaDat3() = False Then Exit Sub
        Dim bUmrti As Boolean = True
        If txtRokSmrt.Text = "" And txtDenSmrt.Text = "" And txtMesicSmrt.Text = "" Then bUmrti = False
        Objekty(False)
        datumTed = DateSerial(datumTed.Year, CInt(txtMesicU.Text), CInt(txtDenU.Text))
        Dim iRokLet As Integer = CInt(If(rbnRokNar.IsChecked, CInt(txtRokLet.Text), Now.Year - CInt(txtRokLet.Text)))
        Dim dNarozen As New Date(iRokLet, datumTed.Month, datumTed.Day)
        Dim dUmrti As Date = DateNull
        If bUmrti Then
            Dim iRokSmrt As Integer = CInt(If(rbnSmrtRok.IsChecked, CInt(txtRokSmrt.Text), iRokLet + CInt(txtRokSmrt.Text)))
            dUmrti = New Date(iRokSmrt, CInt(txtMesicSmrt.Text), CInt(txtDenSmrt.Text))
        End If

        Dim Osoba = Databaze.Osoby.FirstOrDefault(Function(x) x.Jmeno.ToLower = cboNarozenin.Text.ToLower)
        If Osoba Is Nothing Then
            Dim osobaNew = New clsDBase.clsOsoba(cboNarozenin.Text.Trim, dNarozen, dUmrti, Now, txtRC.Text, False)
            osobaNew.Uid = mySQL.insertOsoba(osobaNew)
            Databaze.Osoby.Add(osobaNew)
        Else
            Osoba.Jmeno = cboNarozenin.Text.Trim
            Osoba.Narozeni = dNarozen
            Osoba.Umrti = dUmrti
            Osoba.Zmena = Now
            Osoba.RodneCislo = txtRC.Text
            Osoba.Smazane = False
            mySQL.updateOsoba(Osoba)
        End If

        Objekty(True)
        RefreshNarozeniny()
    End Sub

#End Region

#Region " Backup "

    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        ' Configure open file dialog box 
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Title = "Najděte databázový soubor Kalendáře"
        dlg.Filter = "Databáze Kalendáře|KalendarDatS.xml;kalendars.db"

        If dlg.ShowDialog Then
            If dlg.FileName.ToLower = dbDatabaze.ToLower Then
                Call (New wpfDialog(wSetting, "Do této databáze se budou přidávat data." & NR & dbDatabaze, "Editace narozenin", wpfDialog.Ikona.varovani, "Zavřít")).ShowDialog()
                Exit Sub
            End If
            Objekty(False)
            Dim Pocet As Integer = Databaze.Osoby.Count
            If myFile.Name(dlg.FileName, False) = "KalendarDatS" Then
                AddXmlSNaroz(dlg.FileName)
            Else
                AddDbNaroz(dlg.FileName)
            End If
            Call (New wpfDialog(wSetting, "Počet přidaných jmen: " & Databaze.Osoby.Count - Pocet, "Editace narozenin", wpfDialog.Ikona.ok, "Zavřít")).ShowDialog()
            Objekty(True)
            RefreshNarozeniny()
        End If
    End Sub

    Private Sub AddXmlSNaroz(ByVal addFile As String)
        Dim oldDatabaze As New clsDatabase
        oldDatabaze = CType(New clsSerialization(oldDatabaze, wSetting).ReadXml(addFile), clsDatabase)
        For Each one In oldDatabaze.Osoby
            Dim Osoba = Databaze.Osoby.FirstOrDefault(Function(x) x.Jmeno.ToLower = one.Jmeno.ToLower)
            If Osoba Is Nothing Then
                Dim osobaNew = New clsDBase.clsOsoba(one.Jmeno, one.Narozeni, one.Umrti, Now, one.RodneCislo, False)
                osobaNew.Uid = mySQL.insertOsoba(osobaNew)
                Databaze.Osoby.Add(osobaNew)
            Else
                If one.Zmena > Osoba.Zmena Then
                    Osoba.Narozeni = one.Narozeni
                    Osoba.Umrti = one.Umrti
                    Osoba.Zmena = Now
                    Osoba.RodneCislo = one.RodneCislo
                    mySQL.updateOsoba(Osoba)
                End If
            End If
        Next
    End Sub

    Private Sub AddDbNaroz(ByVal addFile As String)
        Dim exDatabaze As New clsDBase
        Dim exSQL = New clsSQLite(addFile)
        exSQL.load(exDatabaze)

        For Each one In exDatabaze.Osoby
            Dim Osoba = Databaze.Osoby.FirstOrDefault(Function(x) x.Jmeno.ToLower = one.Jmeno.ToLower)
            If Osoba Is Nothing Then
                Dim osobaNew = New clsDBase.clsOsoba(one.Jmeno, one.Narozeni, one.Umrti, Now, one.RodneCislo, False)
                osobaNew.Uid = mySQL.insertOsoba(osobaNew)
                Databaze.Osoby.Add(osobaNew)
            Else
                If one.Zmena > Osoba.Zmena Then
                    Osoba.Narozeni = one.Narozeni
                    Osoba.Umrti = one.Umrti
                    Osoba.Zmena = Now
                    Osoba.RodneCislo = one.RodneCislo
                    mySQL.updateOsoba(Osoba)
                End If
            End If
        Next
    End Sub

#End Region

End Class
