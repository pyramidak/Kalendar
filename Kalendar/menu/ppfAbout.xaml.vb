Class ppfAbout

    Private PicturePath As String
    Private wSetting As WpfSetting = Application.SettingWindow()

#Region " Loaded "

    Private Sub ppfAbout_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        lblApp.Text = Application.CompanyName & " Stolní kalendář verze " & Application.Version
        lblCop.Text = "copyright ©1997-" & mySystem.BuildYear.ToString & "  " & Application.LegalCopyright
        txtLicense.Text = myString.FromBytes(myFile.ReadEmbeddedResource("License.txt"))
    End Sub

#End Region

#Region " Hyperlinks "

    Private Sub lbl_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles txtMail.MouseLeftButtonUp, txtWeb.MouseLeftButtonUp
        Dim lbl As TextBlock = CType(sender, TextBlock)
        myLink.Start(wSetting, lbl.Text)
    End Sub

    Private Sub txtPlay_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles txtPlay.MouseLeftButtonUp
        Dim lbl As TextBlock = CType(sender, TextBlock)
        myLink.Start(wSetting, lbl.Tag.ToString)
    End Sub

    Private Sub lbl_MouseEnter(sender As System.Object, e As System.Windows.Input.MouseEventArgs) Handles txtMail.MouseEnter, txtWeb.MouseEnter, txtPlay.MouseEnter
        Me.Cursor = Cursors.Hand
    End Sub

    Private Sub lbl_MouseLeave(sender As System.Object, e As System.Windows.Input.MouseEventArgs) Handles txtMail.MouseLeave, txtWeb.MouseLeave, txtPlay.MouseLeave
        Me.Cursor = Cursors.Arrow
    End Sub

#End Region


End Class
