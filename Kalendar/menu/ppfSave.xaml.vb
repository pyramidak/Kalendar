Class ppfSave

    Private useProgFolder As Boolean
    Private wMain As WpfMain = CType(Application.Current.MainWindow, WpfMain)
    Private wSetting As wpfSetting = Application.SettingWindow()

#Region " Loaded "

    Private Sub ppfSave_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'Detekce, zda lze použít uložení dat do složky exe souboru
        If myFolder.CheckAccess(Application.StartUpLocation) Then
            useProgFolder = True
        Else
            useProgFolder = False
            ckbProgFolder.IsEnabled = False
            ckbProgFolder.IsChecked = False
        End If

        txtLocal.Text = dbDatabaze

        myCloud.CheckClouds()
        ckbDropBox.IsEnabled = myCloud.DropBoxExist
        If myCloud.DropBoxExist Then
            txtDropBox.Text = myFile.Join(myCloud.DropBoxFolder, MySubFolder, DatName)
            txtDropBox.IsEnabled = Nastaveni.DropBox
            ckbDropBox.IsChecked = Nastaveni.DropBox
        Else
            txtDropBox.Text = "https://www.dropbox.com/register"
            txtDropBox.TextDecorations = TextDecorations.Underline
            txtDropBox.Foreground = Brushes.Blue
        End If

        ckbGoogleDrive.IsEnabled = myCloud.GoogleDriveExist
        If myCloud.GoogleDriveExist Then
            txtGoogleDrive.Text = myFile.Join(myCloud.GoogleDriveFolder, MySubFolder, DatName)
            txtGoogleDrive.IsEnabled = Nastaveni.GoogleDrive
            ckbGoogleDrive.IsChecked = Nastaveni.GoogleDrive
        Else
            txtGoogleDrive.Text = "https://www.google.com/drive/download/"
            txtGoogleDrive.TextDecorations = TextDecorations.Underline
            txtGoogleDrive.Foreground = Brushes.Blue
        End If

        ckbOneDrive.IsEnabled = myCloud.OneDriveExist
        If myCloud.OneDriveExist Then
            txtOneDrive.Text = myFile.Join(myCloud.OneDriveFolder, MySubFolder, DatName)
            txtOneDrive.IsEnabled = Nastaveni.OneDrive
            ckbOneDrive.IsChecked = Nastaveni.OneDrive
        Else
            txtOneDrive.Text = "https://onedrive.live.com/about/cs-cz/download/"
            txtOneDrive.TextDecorations = TextDecorations.Underline
            txtOneDrive.Foreground = Brushes.Blue
        End If

        ckbSync.IsEnabled = myCloud.SyncExist
        If myCloud.SyncExist Then
            txtSync.Text = myFile.Join(myCloud.SyncFolder, MySubFolder, DatName)
            txtSync.IsEnabled = Nastaveni.Sync
            ckbSync.IsChecked = Nastaveni.Sync
        Else
            txtSync.Text = "https://www.sync.com/install/"
            txtSync.TextDecorations = TextDecorations.Underline
            txtSync.Foreground = Brushes.Blue
        End If

        btnSave.IsEnabled = False
    End Sub
#End Region

#Region " Checked Boxes "

    Private Sub txtDropBox_MouseDoubleClick(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtDropBox.MouseDoubleClick
        Try
            If myCloud.DropBoxExist Then
                Process.Start(myFolder.Path(txtDropBox.Text))
            Else
                myLink.Start(wSetting, txtDropBox.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub ckbDropBox_Checked(sender As System.Object, e As System.EventArgs) Handles ckbDropBox.Checked, ckbDropBox.Unchecked
        ckbDropBox.FontWeight = If(ckbDropBox.IsChecked, FontWeights.Bold, FontWeights.Normal)
        txtDropBox.IsEnabled = CBool(ckbDropBox.IsChecked)
        btnSave.IsEnabled = True
    End Sub

    Private Sub txtGoogleDrive_MouseDoubleClick(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtGoogleDrive.MouseDoubleClick
        Try
            If myCloud.GoogleDriveExist Then
                Process.Start(myFolder.Path(txtGoogleDrive.Text))
            Else
                myLink.Start(wSetting, txtGoogleDrive.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub ckbGoogleDrive_Checked(sender As System.Object, e As System.EventArgs) Handles ckbGoogleDrive.Checked, ckbGoogleDrive.Unchecked
        ckbGoogleDrive.FontWeight = If(ckbGoogleDrive.IsChecked, FontWeights.Bold, FontWeights.Normal)
        txtGoogleDrive.IsEnabled = CBool(ckbGoogleDrive.IsChecked)
        btnSave.IsEnabled = True
    End Sub

    Private Sub txtOneDrive_MouseDoubleClick(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtOneDrive.MouseDoubleClick
        Try
            If myCloud.OneDriveExist Then
                Process.Start(myFolder.Path(txtOneDrive.Text))
            Else
                myLink.Start(wSetting, txtOneDrive.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub ckbOneDrive_Checked(sender As System.Object, e As System.EventArgs) Handles ckbOneDrive.Checked, ckbOneDrive.Unchecked
        ckbOneDrive.FontWeight = If(ckbOneDrive.IsChecked, FontWeights.Bold, FontWeights.Normal)
        txtOneDrive.IsEnabled = CBool(ckbOneDrive.IsChecked)
        btnSave.IsEnabled = True
    End Sub

    Private Sub txtSync_MouseDoubleClick(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtSync.MouseDoubleClick
        Try
            If myCloud.SyncExist Then
                Process.Start(myFolder.Path(txtSync.Text))
            Else
                myLink.Start(wSetting, txtSync.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub ckbsync_Checked(sender As System.Object, e As System.EventArgs) Handles ckbSync.Checked, ckbSync.Unchecked
        ckbSync.FontWeight = If(ckbSync.IsChecked, FontWeights.Bold, FontWeights.Normal)
        txtSync.IsEnabled = CBool(ckbSync.IsChecked)
        btnSave.IsEnabled = True
    End Sub

    Private Sub txtLocal_MouseDoubleClick(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtLocal.MouseDoubleClick
        Try
            Process.Start(myFolder.Path(txtLocal.Text))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ckbLocal_Unchecked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles ckbLocal.Unchecked
        ckbLocal.IsChecked = True
    End Sub

    Private Sub ckbProgFolder_Checked(sender As System.Object, e As System.EventArgs) Handles ckbProgFolder.Checked
        txtLocal.Text = myFile.Join(Application.StartUpLocation, DatName)
        btnSave.IsEnabled = True
    End Sub

    Private Sub ckbProgFolder_Unchecked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles ckbProgFolder.Unchecked
        txtLocal.Text = myFile.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MySubFolder, DatName)
        btnSave.IsEnabled = True
    End Sub

#End Region

#Region " Button Nastavit "

    Private Sub onoff(ByVal SetOn As Boolean)
        btnSave.IsEnabled = SetOn
        wSetting.lbxMenu.IsEnabled = SetOn
        wSetting.Title = If(SetOn, "Nastavení Kalendáře", "Nastavení Kalendáře - probíhá test připojení k FTP serveru")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        btnSave.IsEnabled = False

        If CBool(ckbProgFolder.IsChecked) Then
            dbDatabaze = myFile.Join(Application.StartUpLocation, DatName)
            xmlNastaveni = myFile.Join(Application.StartUpLocation, SetName)
        Else
            myFile.Delete(myFile.Join(Application.StartUpLocation, DatName), True, False)
            myFile.Delete(myFile.Join(Application.StartUpLocation, SetName), True, False)
            Dim sDocuments As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim sAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            dbDatabaze = myFile.Join(sDocuments, MySubFolder, DatName)
            xmlNastaveni = myFile.Join(sAppData, MySubFolder, SetName)
            myFolder.Exist(myFolder.Join(sDocuments, MySubFolder), True)
            myFolder.Exist(myFolder.Join(sAppData, MySubFolder), True)
        End If

        Nastaveni.DropBox = CBool(ckbDropBox.IsChecked)
        Nastaveni.GoogleDrive = CBool(ckbGoogleDrive.IsChecked)
        Nastaveni.OneDrive = CBool(ckbOneDrive.IsChecked)
        Nastaveni.Sync = CBool(ckbSync.IsChecked)
        wMain.proSynchAll()
    End Sub
#End Region

End Class
