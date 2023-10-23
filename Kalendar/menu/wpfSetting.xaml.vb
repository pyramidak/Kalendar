Public Class wpfSetting

    Public Property Oslavenec As String
    Public Property IndexPage As Integer = 0
    Private wMain As WpfMain = CType(Application.Current.MainWindow, WpfMain)

    Private Sub wpfSetting_Closed(sender As Object, e As System.EventArgs) Handles Me.Closed
        Call (New clsSerialization(Nastaveni, wndSetting)).WriteXml(xmlNastaveni)
        Call (New clsSerialization(Databaze, wndSetting)).WriteXml(dbDatabaze)
    End Sub

    Private Sub wpfSetting_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Me.KeyUp
        If e.Key = Key.Escape Then Me.Close()
    End Sub

    Private Sub wpfSetting_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.Icon = Application.Icon
        lbxMenu.SelectedIndex = IndexPage
        lbxMenu.Focus()
    End Sub

    Private Sub lbxMenu_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles lbxMenu.SelectionChanged
        Dim item As StackPanel = CType(lbxMenu.SelectedItem, StackPanel)

        If IsNothing(item) = False Then
            Select Case item.Tag.ToString
                Case "Birthday"
                    FramePage.Navigate(New ppfBirthday(Oslavenec))
                Case "Note"
                    FramePage.Navigate(New ppfNote)
                Case "Service"
                    FramePage.Navigate(New ppfService)
                Case "Launch"
                    FramePage.Navigate(New ppfLaunch)
                Case "Color"
                    FramePage.Navigate(New ppfColor)
                Case "Save"
                    FramePage.Navigate(New ppfSave)
                Case "Key"
                    FramePage.Navigate(New ppfKey)
                Case "About"
                    FramePage.Navigate(New ppfAbout)
            End Select
        End If
    End Sub

End Class
