Class wpfBrowser

    Private scope As String = "wl.skydrive_update%20wl.offline_access"
    Public client_id As String
    Public auth_code As String

    Private Sub BrowserWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim signInUrl As New Uri([String].Format("https://login.live.com/oauth20_authorize.srf?client_id={0}&redirect_uri=https://login.live.com/oauth20_desktop.srf&response_type=code&scope={1}", client_id, scope))
        webBrowser.Navigate(signInUrl)
    End Sub

    Private Sub webBrowser_LoadCompleted(sender As Object, e As System.Windows.Navigation.NavigationEventArgs)
        If e.Uri.AbsoluteUri.Contains("code=") Then
            Me.auth_code = Text.RegularExpressions.Regex.Split(e.Uri.AbsoluteUri, "code=")(1)
            Me.Close()
        ElseIf e.Uri.AbsoluteUri.Contains("error=access_denied") Then
            Me.Close()
        End If
    End Sub

End Class
