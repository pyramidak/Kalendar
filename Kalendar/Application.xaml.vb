Imports System.Windows.Threading

Class Application

    Public Shared StartUpLocation As String = myFolder.Path(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Public Shared VersionNo As Integer = CInt(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMajorPart & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMinorPart & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileBuildPart)
    Public Shared Version As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMajorPart & "." & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMinorPart & "." & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileBuildPart
    Public Shared LegalCopyright As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).LegalCopyright
    Public Shared CompanyName As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).CompanyName
    Public Shared ProductName As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductName
    Public Shared ExeName As String = myFile.Name(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).InternalName, False)
    Public Shared ProcessName As String = Diagnostics.Process.GetCurrentProcess.ProcessName
    Public Shared selType As Integer = 0
    Public Shared winStore As Boolean = True

    Public Class myGlobal
        Public Shared NR As String = Chr(13) & Chr(10)
        Public Shared DateNull As New Date(1, 1, 1)
        Public Shared mySQL As clsSQLite
        Public Shared Databaze As New clsDBase
        Public Shared Nastaveni As New clsSetting
        Public Shared xmlNastaveni, dbDatabaze As String
        Public Shared mySystem As New clsSystem
        Public Shared datumTed As Date
        Public Shared myKal As New clsKalendar
        Public Shared Jazyk As String = "CZ"
        Public Shared myCloud As New clsCloud()
        Public Shared MySubFolder As String = "pyramidak"
        Public Shared DatName As String = "kalendar.db"
        Public Shared SetName As String = "KalendarSetS.xml"
        Public Shared UninsPath As String
        Public Shared dataChanged As Boolean
    End Class

#Region " error handle "
    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Private bError As Boolean

    Private Sub App_DispatcherUnhandledException(ByVal sender As Object, ByVal e As DispatcherUnhandledExceptionEventArgs) Handles MyClass.DispatcherUnhandledException
        ' Process unhandled exception
        If bError Then Exit Sub
        bError = True
        e.Handled = True

        Dim Form As New wpfError
        Form.myError = e
        Form.ShowDialog()

        End
    End Sub

    Private Sub App_StartUp(ByVal sender As Object, ByVal e As StartupEventArgs) Handles MyClass.Startup
        If mySystem.isAppRunning(ProcessName, mySystem.User) Then End
        FrameworkElement.LanguageProperty.OverrideMetadata(GetType(FrameworkElement), New FrameworkPropertyMetadata(Markup.XmlLanguage.GetLanguage(Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)))

        If ContinueStart() Then
            Nastaveni.Spusteno = Now
            Dim mainWindow As New WpfMain
            mainWindow.Show()
        Else
            End
        End If
    End Sub
#End Region

#Region " before initialize "

    Private Function ContinueStart() As Boolean
        'Uložení lokace programu do registru
        myRegister.CreateValue(HKEY.CURRENT_USER, "Software\pyramidak\Kalendar", "Location", System.Reflection.Assembly.GetExecutingAssembly.Location)
        'Nastavení cest k souborům
        Dim xmlDatOld As String
        If myFile.Exist(myFile.Join(StartUpLocation, DatName)) Then
            dbDatabaze = myFile.Join(StartUpLocation, DatName)
            xmlNastaveni = myFile.Join(StartUpLocation, SetName)
            xmlDatOld = myFile.Join(StartUpLocation, "KalendarDatS.xml")
        Else
            Dim sDocuments As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim sAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            dbDatabaze = myFile.Join(sDocuments, MySubFolder, DatName)
            xmlNastaveni = myFile.Join(sAppData, MySubFolder, SetName)
            xmlDatOld = myFile.Join(sDocuments, MySubFolder, "KalendarDatS.xml")
            myFolder.Exist(myFolder.Join(sDocuments, MySubFolder), True)
            myFolder.Exist(myFolder.Join(sAppData, MySubFolder), True)
        End If

        'Načtení nastavení
        If myFile.Exist(xmlNastaveni) Then
            Nastaveni = CType(New clsSerialization(Nastaveni).ReadXml(xmlNastaveni), clsSetting)
        End If
        'Přidání výchozích barev
        Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = "nový návrh")
        If Barva Is Nothing Then
            Nastaveni.Barvy.Add(New clsSetting.clsBarva("nový návrh", "#FFF5767C", "#FFF9A4A7", "#FF439C52", "#FFB7D882", "#FFFBCA8E", "#FFFAB461", "#FF80C6D8", "#FFB3E7FB", "#FFF3F3F3"))
        End If
        Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = "starý návrh")
        If Barva Is Nothing Then
            Nastaveni.Barvy.Add(New clsSetting.clsBarva("starý návrh", Colors.Coral.ToString, Color.FromRgb(255, 192, 128).ToString, Colors.Gray.ToString, Color.FromRgb(255, 192, 192).ToString,
                Color.FromRgb(192, 255, 192).ToString, Color.FromRgb(192, 192, 255).ToString, Color.FromRgb(255, 255, 192).ToString, Color.FromRgb(128, 255, 255).ToString, Colors.WhiteSmoke.ToString))
        End If

        'Načtení databáze
        mySQL = New clsSQLite(dbDatabaze)
        If myFile.Exist(xmlDatOld) Then
            Dim dbOld As New clsDatabase
            dbOld = CType(New clsSerialization(dbOld).ReadXml(xmlDatOld), clsDatabase)
            For Each stary In dbOld.Osoby
                If stary.Smazane = False Then
                    Databaze.Osoby.Add(New clsDBase.clsOsoba(stary.Jmeno, stary.Narozeni, stary.Umrti, stary.Zmena, stary.RodneCislo, False))
                End If
            Next
            For Each stary In dbOld.Poznamky
                If Not stary.Poznamka = "" Then
                    Dim pocet As Integer = 0
                    Do While stary.Poznamka.Contains(":")
                        Dim index = stary.Poznamka.LastIndexOf(":")
                        If stary.Poznamka.Length > index + 3 AndAlso IsNumeric(stary.Poznamka.Substring(index + 1, 2)) Then
                            Dim posun = -1
                            If index - 2 >= 0 AndAlso IsNumeric(stary.Poznamka.Substring(index - 2, 2)) Then
                                posun = 0
                            ElseIf index - 1 >= 0 AndAlso IsNumeric(stary.Poznamka.Substring(index - 1, 1)) Then
                                posun = 1
                            End If
                            If posun = -1 Then Exit Do
                            Dim time = stary.Poznamka.Substring(index - 2 + posun, 5 - posun)
                            Dim hodina, minuta As Integer
                            Dim text = ""
                            If IsNumeric(time.Substring(0, 2 - posun)) And IsNumeric(time.Substring(3 - posun, 2)) Then
                                hodina = Math.Abs(CInt(time.Substring(0, 2 - posun)))
                                minuta = Math.Abs(CInt(time.Substring(3 - posun, 2)))
                                text = stary.Poznamka.Substring(index + 4, stary.Poznamka.Length - index - 4)
                            Else
                                hodina = 0
                                minuta = 0
                                text = stary.Poznamka.Substring(index - 2 + posun, stary.Poznamka.Length - index + 2 - posun)
                            End If
                            pocet += 1
                            Dim novy = New clsDBase.clsPoznamka(stary.Dne, stary.Dne, stary.Poznamka, False, stary.Opakovat, Now)
                            novy.Vznik = New Date(stary.Dne.Year, stary.Dne.Month, stary.Dne.Day, 0, 0, pocet)
                            novy.Den = New Date(stary.Dne.Year, stary.Dne.Month, stary.Dne.Day, hodina, minuta, 0)
                            novy.Text = If(text = "", "-", text)
                            Databaze.Poznamky.Add(novy)
                            stary.Poznamka = stary.Poznamka.Substring(0, index - 2 + posun)
                        Else
                            Exit Do
                        End If
                    Loop
                    If pocet = 0 Or stary.Poznamka.Length > 4 Then
                        Databaze.Poznamky.Add(New clsDBase.clsPoznamka(stary.Dne, stary.Dne, stary.Poznamka, False, stary.Opakovat, stary.Zmena))
                    End If
                End If
            Next
            mySQL.save(Databaze)
            myFile.Move(xmlDatOld, xmlDatOld & ".bak")
        Else
            If myFile.Exist(dbDatabaze) Then mySQL.load(Databaze)
        End If

        'Kontrola spuštění, podmínek autostartu
        Dim Arg As String = ""
        Dim Args() As String = Environment.GetCommandLineArgs
        If UBound(Args) > 0 Then Arg = Args(1)
        If Arg = "-win" Or Arg = "/win" Then
            If Nastaveni.Spusteni = 0 Then Return False
            If Nastaveni.Spusteni = 1 Then Return True
            If Nastaveni.Spusteni = 2 Then Return Not Nastaveni.Spusteno.Date = Today
            If Nastaveni.Spusteni > 2 Then
                Dim poznamky As New List(Of clsDBase.clsPoznamka)
                poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Mesicne = False And x.Rocne = False And x.Den.Date = Today.Date).ToList)
                poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Mesicne And x.Den.Day = Today.Day And x.Den.Year = Today.Year).ToList)
                poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Rocne And x.Den.Day = Today.Day And x.Den.Month = Today.Month).ToList)
                poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Mesicne And x.Rocne And x.Den.Day = Today.Day).ToList)
                Dim bNarozky = Databaze.Osoby.Any(Function(x) x.Narozeni.Month = Today.Month And x.Narozeni.Day = Today.Day And x.Smazane = False)
                Return (bNarozky Or poznamky.Count > 0) And Not Nastaveni.Spusteno.Date = Today
            End If
        End If

        Return True
    End Function

#End Region

#Region " window setting "

    Public Shared ReadOnly Property Icon As ImageSource
        Get
            Return myBitmap.UriToImageSource(New Uri("/" + ExeName + ";component/" + ExeName + ".ico", UriKind.Relative))
        End Get
    End Property

    Public Shared Function SettingWindow() As wpfSetting
        For Each wOne As Window In Application.Current.Windows
            If wOne.Name = "wndSetting" Then Return CType(wOne, wpfSetting)
        Next
        Return Nothing
    End Function

    Public Shared Function Title() As String
        Return ProductName + " " + Version
    End Function

#End Region

End Class
