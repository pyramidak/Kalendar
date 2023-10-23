Class ppfLaunch

    Private wSetting As WpfSetting = Application.SettingWindow()
    Private myTask As New clsTaskScheduler

#Region " Loaded "

    Private Sub ppfLaunch_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If myTask.Exist = False Then Nastaveni.Spusteni = 0 'Exist načte nastavený čas v TaskCheduler
        txtTime.Text = myTask.Cas.ToShortTimeString

        Select Case Nastaveni.Spusteni
            Case 0
                rbnNever.IsChecked = True
            Case 1
                rbnEver.IsChecked = True
            Case 2
                rbnOnce.IsChecked = True
            Case Is > 2
                rbnReason.IsChecked = True
        End Select
        btnSave.IsEnabled = False

        If Application.winStore Then
            spCas.Visibility = Visibility.Collapsed
            lblHead.Text = "Plánovač úloh Windows neumí spouštět aplikace z Windows Store." + NR +
                "Pokud chcete spouštět Kalendář při přihlášení, klikněte na Autostart a v otevřeném seznamu aktivujte Stolní kalendář." + NR + NR +
                "Pokud používáte program STARTzjs, tak si jednoduše v TAPs nastavte: " + NR +
                "     Když nastane čas [18:00], tak spustit program [" + Chr(34) + "Kalendar" + Chr(34) + " -win]" + NR +
                "a níže si nastavte, jak se má kalendář zachovat."
        Else
            btnAutostart.Visibility = Visibility.Collapsed
        End If
    End Sub

#End Region

#Region " Uložit "

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim cas() As String = Split(txtTime.Text, ":")
        Try
            myTask.Cas = Today.AddHours(CInt(cas(0))).AddMinutes(CInt(cas(1)))
        Catch
        End Try

        Nastaveni.Spusteni = 0
        If rbnNever.IsChecked Then
            myTask.Delete()
            btnSave.IsEnabled = False
            Exit Sub
        End If
        myTask.Create()
        If rbnEver.IsChecked Then Nastaveni.Spusteni = 1
        If rbnOnce.IsChecked Then Nastaveni.Spusteni = 2
        If rbnReason.IsChecked Then Nastaveni.Spusteni = 3

        btnSave.IsEnabled = False
    End Sub
#End Region

#Region " RadioButtons "

    Private Sub btnAutostart_Click(sender As Object, e As RoutedEventArgs) Handles btnAutostart.Click
        myLink.Start(wSetting, "ms-settings:startupapps")
    End Sub

    Private Sub rbnEver_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles rbnEver.Checked, rbnNever.Checked, rbnOnce.Checked, rbnReason.Checked
        btnSave.IsEnabled = True
    End Sub

#End Region

End Class
