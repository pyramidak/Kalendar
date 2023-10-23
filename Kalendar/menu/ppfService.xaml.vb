Class ppfService

    Private PicturePath As String
    Private wSetting As WpfSetting = Application.SettingWindow()
    Private wMain As WpfMain = CType(Application.Current.MainWindow, WpfMain)

#Region " Loaded "

    Private Sub ppfService_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtFirst.Text = Nastaveni.SluzbaPrvni.Date.ToShortDateString
        txtNext.Text = Nastaveni.SluzbaZnova.ToString
        PicturePath = Nastaveni.Obrazek

        If Not Nastaveni.Barvy.Count = 0 Then
            Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = Nastaveni.BarvyJmeno)
            If Barva IsNot Nothing Then
                Rectangle1.Fill = myColorConverter.StringToBrush(Barva.Den)
            Else
                Rectangle1.Fill = myColorConverter.StringToBrush(Nastaveni.Barvy(0).Den)
            End If
        End If
        proSetDefPic()
        btnSave.IsEnabled = False
    End Sub
#End Region

#Region " Picture "

    Private Sub picA_MouseLeftButtonUp(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles picA.MouseLeftButtonUp
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Title = "Vyber obrázek služby"
        dlg.Filter = "Obrázky *.bmp,jpg,png,gif,ico,tif|*.bmp;*.jpg;*.png*.gif;*.ico;*.dib;*.jpe;*.jpeg;*.tif;*.tiff"

        If dlg.ShowDialog = True Then
            If dlg.CheckFileExists = True Then
                PicturePath = dlg.FileName
                proSetDefPic()
            End If
        End If
    End Sub

    Private Sub picA_MouseRightButtonUp(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles picA.MouseRightButtonUp
        PicturePath = ""
        proSetDefPic()
    End Sub

    Private Sub proSetDefPic()
        Try
            If myFile.Exist(PicturePath) Then
                picA.Source = New BitmapImage(New Uri(PicturePath, UriKind.Absolute))
            Else
                picA.Source = CType(Application.Current.FindResource("Vykricnik"), ImageSource)
            End If
        Catch
            Call (New wpfDialog(wSetting, "Soubor není obrázek:" & NR & PicturePath, "Nastavení služby", wpfDialog.Ikona.varovani, "Zavřít")).ShowDialog()
            PicturePath = Nastaveni.Obrazek
        End Try
        btnSave.IsEnabled = True
    End Sub

#End Region

#Region " Uložit "

    Private Sub btnSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnSave.Click
        If IsDate(txtFirst.Text) = False Then
            Call (New wpfDialog(wSetting, "Zadejte platné datum prvního dne služby.", "Nastavení služby", wpfDialog.Ikona.varovani, "Zavřít")).ShowDialog()
            txtFirst.Focus()
            Exit Sub
        End If
        If IsNumeric(txtNext.Text) = False Then
            Call (New wpfDialog(wSetting, "Zadejte počet dní - číslo.", "Nastavení služby", wpfDialog.Ikona.varovani, "Zavřít")).ShowDialog()
            txtNext.Focus()
            Exit Sub
        End If

        Nastaveni.SluzbaPrvni = CDate(txtFirst.Text)
        Nastaveni.SluzbaZnova = CInt(txtNext.Text)
        Nastaveni.Obrazek = PicturePath

        btnSave.IsEnabled = False
        wMain.Tyden.SluzbaImg = picA.Source
        wMain.proPropocet()
    End Sub

    Private Sub txtFirst_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles txtFirst.TextChanged, txtNext.TextChanged
        btnSave.IsEnabled = True
    End Sub

#End Region

End Class
