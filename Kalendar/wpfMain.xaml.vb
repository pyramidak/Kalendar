Imports System.Windows.Threading

Class WpfMain

#Region " declaration "

    Private Declare Function BitBlt Lib "gdi32.dll" Alias "BitBlt" (ByVal _
       hdcDest As IntPtr, ByVal nXDest As Integer, ByVal nYDest As _
       Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal _
       hdcSrc As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, _
       ByVal dwRop As System.Int32) As Long

    Dim memoryImage As System.Drawing.Bitmap
    Private ZJShareMem As New clsSharedMemory
    Private zjsSMok As Boolean = True
    Private WithEvents threadSynch As New System.ComponentModel.BackgroundWorker
    Private timZJS, timDate As DispatcherTimer
    Public Tyden As New clsWeek

#End Region

#Region " synchronizace "

#Region " postupné volání synch "

    Public Sub proSynchAll()
        For a As Integer = 1 To 3
            Dim itemMenu As MenuItem = CType(Menu1.Items(Menu1.Items.Count - 1), MenuItem)
            If itemMenu.Tag IsNot Nothing AndAlso itemMenu.Tag.ToString = "cloud" Then
                Menu1.Items.RemoveAt(Menu1.Items.Count - 1)
            End If
        Next

        Dim itemMenu2 As New MenuItem()
        itemMenu2.IsEnabled = False
        itemMenu2.Tag = "cloud"
        itemMenu2.Width = 100
        Menu1.Items.Add(itemMenu2)

        Dim Synchronized As Boolean
        If Nastaveni.DropBox And myCloud.DropBoxExist Then
            proSynchronize(myFile.Join(myCloud.DropBoxFolder, MySubFolder, DatName))
            Synchronized = True
            AddIcon("DropBox")
        End If
        If Nastaveni.GoogleDrive And myCloud.GoogleDriveExist Then
            proSynchronize(GetGoogleDuplicatedFile(DatName, MySubFolder))
            proSynchronize(GetGoogleDuplicatedFile(DatName, ""))
            Synchronized = True
            AddIcon("GoogleDrive")
        End If
        If Nastaveni.OneDrive And myCloud.OneDriveExist Then
            proSynchronize(myFile.Join(myCloud.OneDriveFolder, MySubFolder, DatName))
            Synchronized = True
            AddIcon("OneDrive")
        End If
        If Nastaveni.Sync And myCloud.SyncExist Then
            proSynchronize(myFile.Join(myCloud.SyncFolder, MySubFolder, DatName))
            Synchronized = True
            AddIcon("Sync")
        End If
        If Synchronized Then
            proPropocet()
            proLoadComboNarozenin()
        End If
    End Sub

    Private Function GetGoogleDuplicatedFile(filename As String, subfolder As String) As String
        Dim sFile As String
        For i = 1 To 3
            sFile = myFile.Join(myCloud.GoogleDriveFolder, subfolder, myFile.Name(filename, False) & " (" & 4 - i & ")" & myFile.Extension(filename))
            If myFile.Exist(sFile) Then Return sFile
        Next i
        Return myFile.Join(myCloud.GoogleDriveFolder, subfolder, filename)
    End Function

    Private Sub AddIcon(Sluzba As String)
        Dim imgIcon As New Image
        imgIcon.Source = CType(Me.FindResource(Sluzba), ImageSource)
        Dim itemMenu As New MenuItem()
        itemMenu.IsEnabled = False
        If mySystem.Current.Number = 7 Then itemMenu.Height = 15
        itemMenu.Icon = imgIcon
        itemMenu.Tag = "cloud"
        Menu1.Items.Add(itemMenu)
    End Sub

#End Region

#Region " synchronizace "

    Private Function proSynchronize(CloudFile As String) As Boolean
        If myFile.Exist(CloudFile) = False Then Return False
        Dim cloudSQL = New clsSQLite(CloudFile)
        Dim cloudDatabaze As New clsDBase
        cloudSQL.load(cloudDatabaze)

        For Each cloudOsoba In cloudDatabaze.Osoby
            Dim Osoba = Databaze.Osoby.FirstOrDefault(Function(x) x.Jmeno = cloudOsoba.Jmeno)
            If Osoba Is Nothing Then
                Dim osobaNew = New clsDBase.clsOsoba(cloudOsoba.Jmeno, cloudOsoba.Narozeni, cloudOsoba.Umrti, cloudOsoba.Zmena, cloudOsoba.RodneCislo, cloudOsoba.Smazane)
                osobaNew.Uid = mySQL.insertOsoba(osobaNew)
                Databaze.Osoby.Add(osobaNew)
            Else
                If cloudOsoba.Zmena > Osoba.Zmena Then
                    Osoba.Narozeni = cloudOsoba.Narozeni
                    Osoba.Umrti = cloudOsoba.Umrti
                    Osoba.Zmena = cloudOsoba.Zmena
                    Osoba.RodneCislo = cloudOsoba.RodneCislo
                    Osoba.Smazane = cloudOsoba.Smazane
                    mySQL.updateOsoba(Osoba)
                End If
            End If
        Next

        For Each cloudPoznamka In cloudDatabaze.Poznamky
            Dim Upominka = Databaze.Poznamky.FirstOrDefault(Function(x) x.Vznik = cloudPoznamka.Vznik)
            If Upominka Is Nothing Then
                Dim upominkaNew = New clsDBase.clsPoznamka(cloudPoznamka.Vznik, cloudPoznamka.Den, cloudPoznamka.Text, cloudPoznamka.Mesicne, cloudPoznamka.Rocne, cloudPoznamka.Zmena)
                upominkaNew.Uid = mySQL.insertUpominka(upominkaNew)
                Databaze.Poznamky.Add(upominkaNew)
            Else
                If Upominka.Zmena < cloudPoznamka.Zmena Then
                    Upominka.Den = cloudPoznamka.Den
                    Upominka.Text = cloudPoznamka.Text
                    Upominka.Mesicne = cloudPoznamka.Mesicne
                    Upominka.Rocne = cloudPoznamka.Rocne
                    mySQL.updateUpominka(Upominka)
                End If
            End If
        Next

        proSynchronize = True
    End Function

#End Region

#End Region

#Region " Timers"

    Private Sub timZJS_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If ZJShareMem.DataExists Then
                If ZJShareMem.Peek() = "ZJS:Kalendar:EXIT" Then Me.Close()
            End If
        Catch
            timZJS.Stop()
        End Try

        Dim ThisNewVersion As Integer = 395
        If Application.VersionNo = ThisNewVersion And Not Nastaveni.Verze = ThisNewVersion And Application.winStore = True Then
            Nastaveni.Verze = ThisNewVersion
            Dim wDialog = New wpfDialog(Me, "Vítejte v nové verzi Stolního kalendáře" + NR + NR +
                    "Synchronizace kalendáře mezi Windows a Android se" + NR +
                    "s ohledem na změnu podmínek Google Disku změnila." + NR + NR +
                    "Windows kalendář nemůže uložit data pro Android," + NR +
                    "ale může načíst data ze souboru pro Android." + NR + NR +
                    "V případě potřeby oboustrané synchronizace" + NR +
                    "využijte Windows subsystem pro Android (WSA)." + NR + NR +
                    "Návod instalace WSA na mém webu kalendáře." + NR +
                    "" + NR + NR, Me.Title, wpfDialog.Ikona.ok, "WSA", "Zavřít")
            If wDialog.ShowDialog() = True Then
                myLink.Start(Me, "https://app.jantac.net/wsa/") 'ms-windows-store://pdp/?productid=9P8T94MPJK4B
            Else
                'myLink.Start(Me, "https://play.google.com/store/apps/details?id=cz.pyramidak.kalendar")
            End If
            'OpenSetting(5)        
        End If
    End Sub

    Private Sub timDate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If datumTed <> datumNew Then
            datumTed = datumNew
            proPropocet()
        End If
        timDate.Stop()
    End Sub

#End Region

#Region " frmKalendar_events "

#Region " Open "

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.Title = "Stolní kalendář " & Application.Version

        timZJS = New DispatcherTimer
        timZJS.Interval = TimeSpan.FromSeconds(1)
        AddHandler timZJS.Tick, AddressOf timZJS_Tick
        timDate = New DispatcherTimer
        timDate.Interval = TimeSpan.FromSeconds(0.5)
        AddHandler timDate.Tick, AddressOf timDate_Tick

        If Nastaveni.Width > System.Windows.SystemParameters.WorkArea.Width Then Nastaveni.Width = CInt(System.Windows.SystemParameters.WorkArea.Width) - CInt(Me.Left)
        If Nastaveni.Height > System.Windows.SystemParameters.WorkArea.Height - 50 Then Nastaveni.Height = CInt(System.Windows.SystemParameters.WorkArea.Height) - CInt(Me.Top) - 50
        Me.Width = Nastaveni.Width
        Me.Height = Nastaveni.Height

        Me.DataContext = Tyden
        proChangeColours(Nastaveni.BarvyJmeno)
        Tyden.FontSize = Nastaveni.FontSize
        proSetDefPic()

        ZJShareMem.Open("ZJS")
        timZJS.Start()

        'Jazyk a Jmeniny
        LoadJazyk(Nastaveni.Jazyk)

        'Narozeniny
        cboNaroz.DisplayMemberPath = "Jmeno"
        proLoadComboNarozenin()

        'Propocet
        datumTed = Today
        proPropocet()

        'Synchronizace
        proSynchAll()
    End Sub

    Private Sub proLoadComboNarozenin()
        cboNaroz.ItemsSource = Databaze.Osoby.Where(Function(x) x.Smazane = False).OrderBy(Function(x) x.Jmeno)
        cboNaroz.IsEnabled = If(Databaze.Osoby.Count = 0, False, True)
    End Sub

    Private Sub proSetDefPic()
        Try
            If myFile.Exist(Nastaveni.Obrazek) Then
                Tyden.SluzbaImg = New BitmapImage(New Uri(Nastaveni.Obrazek, UriKind.Absolute))
            Else
                Tyden.SluzbaImg = CType(Application.Current.FindResource("Vykricnik"), ImageSource)
            End If
        Catch
            Tyden.SluzbaImg = CType(Application.Current.FindResource("Vykricnik"), ImageSource)
        End Try
    End Sub

#End Region

#Region " Close "

    Private Sub WpfMain_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        proSynchAll()
        proSaveALL()
    End Sub

    Private Sub proSaveALL()
        Nastaveni.Width = CInt(Me.ActualWidth) : Nastaveni.Height = CInt(Me.ActualHeight)
        Call (New clsSerialization(Nastaveni, Me)).WriteXml(xmlNastaveni)
        If dataChanged = False Then Exit Sub

        'Smazat všechny prázdné poznámky starší měsíc
        Databaze.Poznamky.Where(Function(x) x.Text = "" And x.Zmena.AddMonths(1) < Now).ToList.ForEach(Sub(x)
                                                                                                           mySQL.deleteUpominka(x)
                                                                                                           Databaze.Poznamky.Remove(x)
                                                                                                       End Sub)
        'Smazat všechny smazané osoby starší měsíc
        Databaze.Osoby.Where(Function(x) x.Smazane = True And x.Zmena.AddMonths(1) < Now).ToList.ForEach(Sub(x)
                                                                                                             mySQL.deleteOsoba(x)
                                                                                                             Databaze.Osoby.Remove(x)
                                                                                                         End Sub)

        If Nastaveni.Sync And myCloud.SyncExist Then WriteDatabaze(myCloud.SyncFolder)
        If Nastaveni.DropBox And myCloud.DropBoxExist Then WriteDatabaze(myCloud.DropBoxFolder)
        If Nastaveni.OneDrive And myCloud.OneDriveExist Then WriteDatabaze(myCloud.OneDriveFolder)
        If Nastaveni.GoogleDrive And myCloud.GoogleDriveExist Then
            Dim cloudSQL = New clsSQLite(GetGoogleDuplicatedFile(DatName, MySubFolder))
            cloudSQL.save(Databaze)
            dataChanged = False
        End If
    End Sub

    Private Sub WriteDatabaze(CloudFolder As String)
        Dim cloudSQL = New clsSQLite(myFile.Join(CloudFolder, MySubFolder, DatName))
        cloudSQL.save(Databaze)
        dataChanged = False
    End Sub

#End Region

#End Region

#Region " výpočet "

    Public Sub proPropocet()
        Dim datumFirst, datumLast As Date
        myKal.PrestupnyRok(datumTed) 'nastaví správně měsíc únor

        For a = -1 To 5
            datumFirst = DateAdd(DateInterval.Day, Not a, datumTed)
            If Weekday(datumFirst) = 2 Then Exit For
        Next a
        For a = 1 To 7
            datumLast = DateAdd(DateInterval.Day, a, datumTed)
            If Weekday(datumLast) = 1 Then Exit For
        Next a

        'Týdny
        '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        lblTyden.Content = " " & DatePart(DateInterval.WeekOfYear, datumFirst, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays) & ". " & If(Jazyk = "CZ", "týden", "týždeň")

        'Datum
        '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        lblMesic.Content = myKal.GetMesic(datumFirst.Month)
        lblRok.Content = CStr(Year(datumTed))
        If Month(datumFirst) <> Month(datumLast) Then lblMesic.Content = myKal.GetMesic(datumFirst.Month) & " / " & myKal.GetMesic(datumLast.Month)
        If Year(datumFirst) <> Year(datumLast) Then lblRok.Content = Year(datumFirst) & " / " & Strings.Right(CStr(Year(datumLast)), 2)

        For i = 0 To 6
            datumLast = DateAdd(DateInterval.Day, i, datumFirst)
            Tyden(i).Datum = datumLast
            'Služby
            '~~~~~~
            Tyden(i).Sluzba = Visibility.Hidden
            If Not Nastaveni.SluzbaZnova = 0 Then
                Dim datumDay As Date = datumFirst.AddDays(i)
                Dim iDays As Integer = CShort(DateDiff(DateInterval.Day, Nastaveni.SluzbaPrvni, datumDay))
                If iDays Mod Nastaveni.SluzbaZnova = 0 Then
                    Tyden(i).Sluzba = Visibility.Visible
                End If
            End If

            'Dny
            '~~~
            Tyden(i).Den = datumLast.Day
            Tyden(i).DenFontBrush = If(datumLast = datumTed, Brushes.Yellow, Brushes.White)

            'Narozeniny
            '~~~~~~~~~~
            Dim sText As String = ""
            Dim sTag As String = ""
            For Each Osoba In Databaze.Osoby.Where(Function(x) x.Smazane = False)
                If Osoba.Narozeni.Day = datumLast.Day And Osoba.Narozeni.Month = datumLast.Month Then
                    sText = sText & If(sText = "", "", "," & Chr(10)) _
                            & Osoba.Jmeno & " " & If(Osoba.Umrti = DateNull, "", "†" & CStr(Osoba.Umrti.Year - Osoba.Narozeni.Year) & " (") _
                            & datumTed.Year - Osoba.Narozeni.Year & If(Osoba.Umrti = DateNull, "", ") ")
                    sTag = Osoba.Jmeno
                End If

                If Not Osoba.Umrti = DateNull Then
                    If Osoba.Umrti.Day = datumLast.Day And Osoba.Umrti.Month = datumLast.Month Then
                        sText = sText & If(sText = "", "", "," & Chr(10)) & "†" _
                                & Osoba.Jmeno & " †" & CStr(Osoba.Umrti.Year - Osoba.Narozeni.Year)
                        sTag = Osoba.Jmeno
                    End If
                End If
            Next
            Tyden(i).Narozeniny = sText
            Tyden(i).TagNaroz = sTag

            'Jmeniny
            '~~~~~~~~~~~~~~~~
            sText = myKal.GetJmena(datumLast)
            sTag = ""
            For Each oneJmeno In Split(sText, "/")
                Dim Osoba = Databaze.Osoby.Where(Function(x) x.Smazane = False).FirstOrDefault(Function(x) x.Jmeno.Contains(oneJmeno))
                If Osoba IsNot Nothing Then
                    sTag = Osoba.Jmeno
                    Exit For
                End If
            Next
            Tyden(i).Jmeniny = sText
            Tyden(i).TagJmeno = sTag

            'Svátky
            '~~~~~~
            Tyden(i).Svatky = myKal.GetSvatek(datumLast)
            Tyden(i).Zavreno = myKal.GetZavreno(datumLast)

            'Poznámky
            '~~~~~~~~
            Dim poznamky As New List(Of clsDBase.clsPoznamka)
            poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Mesicne = False And x.Rocne = False And x.Den.Date = datumLast.Date).ToList)
            poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Mesicne And x.Den.Day = datumLast.Day And x.Den.Year = datumLast.Year).ToList)
            poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Rocne And x.Den.Day = datumLast.Day And x.Den.Month = datumLast.Month).ToList)
            poznamky.AddRange(Databaze.Poznamky.Where(Function(x) Not x.Text = "" And x.Mesicne And x.Rocne And x.Den.Day = datumLast.Day).ToList)
            Tyden(i).Poznamky = poznamky.OrderBy(Function(x) x.Den).ToList
        Next i
    End Sub

#End Region

#Region " Menu & Click "

    Private Sub smiSave_Click(sender As Object, e As RoutedEventArgs) Handles smiSave.Click
        If Nastaveni.OneDrive Or Nastaveni.GoogleDrive Or Nastaveni.DropBox Or Nastaveni.Sync Then
            proSaveALL()
        Else
            OpenSetting(5)
        End If
    End Sub

    Private Sub smuNapoveda_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles smuNapoveda.Click
        OpenSetting(6)
    End Sub

    Private Shared Function GetSettingWindow() As WpfSetting
        For Each wOne As Window In Application.Current.Windows
            If wOne.Name = "wndSetting" Then Return CType(wOne, WpfSetting)
        Next
        Return Nothing
    End Function

    Private Sub OpenSetting(Optional ByVal iIndexPage As Integer = 0, Optional ByVal sOslavenec As String = "")
        If IsNothing(GetSettingWindow) = False Then Exit Sub

        cboNaroz.SelectedItem = Nothing
        Dim Form As New WpfSetting
        Form.Owner = Me
        Form.IndexPage = iIndexPage
        Form.Oslavenec = sOslavenec
        Form.ShowDialog()

        proChangeColours(Nastaveni.BarvyJmeno)
        proPropocet()
        proLoadComboNarozenin()
    End Sub

    Private Sub wndMain_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs) Handles MyBase.KeyDown
        If Keyboard.IsKeyDown(Key.LeftCtrl) Or Keyboard.IsKeyDown(Key.RightCtrl) Then
            Select Case e.Key
                Case Key.S
                    datumTed = Today
                    proPropocet()
                Case Key.D
                    TydenDalsi()
                Case Key.A
                    TydenZpatky()
                Case Key.T
                    PrintGrid(gridPrint)
                Case Key.L
                    LoadJazyk(If(Jazyk = "CZ", "SK", "CZ"))
                    proPropocet()
            End Select
        End If
        If e.Key = Key.F1 Then OpenSetting(6)
    End Sub

    Private Sub smiDnesek_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles smiDnesek.Click
        datumTed = Today
        proPropocet()
    End Sub

    Private Sub smiDalsi_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles smiDalsi.Click
        TydenDalsi()
    End Sub

    Private Sub smiZpatky_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles smiZpatky.Click
        TydenZpatky()
    End Sub

    Private Sub smiKonec_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles smiKonec.Click
        Me.Close()
    End Sub

    Private Sub smuNastaveni_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles smuNastaveni.Click
        OpenSetting()
    End Sub

    Private Sub Narozeniny_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim tbk As TextBlock = CType(sender, TextBlock)
        If tbk.Tag IsNot Nothing Then
            If Not tbk.Tag.ToString = "" Then
                OpenSetting(0, tbk.Tag.ToString)
            End If
        End If
    End Sub
    Private Sub Narozeniny_TouchDown(sender As Object, e As TouchEventArgs)
        Dim tbk As TextBlock = CType(sender, TextBlock)
        If tbk.Tag IsNot Nothing Then
            If Not tbk.Tag.ToString = "" Then
                OpenSetting(0, tbk.Tag.ToString)
            End If
        End If
    End Sub

#End Region

#Region " Change Note "
    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim textBox As TextBox = TryCast(sender, TextBox)
        If textBox IsNot Nothing Then
            If Not textBox.Text = "" Then
                For Each den In Tyden
                    If den.Upominky.Any(Function(x) x.Text = "") = False Then
                        den.Upominky.Add(den.NewNote)
                    End If
                Next
            End If
        End If
    End Sub

#End Region

#Region " Change Date "

#Region " changeDateByWheel "

    Private Sub Window_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles MyBase.MouseDown
        If e.ChangedButton = MouseButton.Middle And e.MiddleButton = MouseButtonState.Pressed Then
            datumTed = Today
            proPropocet()
        End If
    End Sub
    Private Sub ChangeFontSize(Delta As Integer)
        If Keyboard.IsKeyDown(Key.LeftCtrl) Then
            For row As Integer = 1 To 7
                If Nastaveni.FontSize + Delta < 10 Then
                    Nastaveni.FontSize = 10
                ElseIf Nastaveni.FontSize + Delta > 50 Then
                    Nastaveni.FontSize = 50
                Else
                    Nastaveni.FontSize += Delta
                End If
                Tyden.FontSize = Nastaveni.FontSize
            Next
        Else
            If Delta < 0 Then
                TydenDalsi()
            ElseIf Delta > 0 Then
                TydenZpatky()
            End If
        End If
    End Sub

    Private Sub Poznamky_MouseWheel(sender As Object, e As MouseWheelEventArgs)
        ChangeFontSize(If(e.Delta > 0, 1, -1))
    End Sub

    Private Sub Window_MouseWheel(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseWheelEventArgs) Handles MyBase.MouseWheel
        ChangeFontSize(If(e.Delta > 0, 1, -1))
    End Sub

#End Region

    Private Sub TydenZpatky()
        If Year(DateAdd(DateInterval.Day, -7, datumTed)) = 999 Then Exit Sub
        datumTed = DateAdd(DateInterval.Day, -7, datumTed)
        proPropocet()
    End Sub

    Private Sub TydenDalsi()
        If Year(DateAdd(DateInterval.Day, 7, datumTed)) = 5001 Then Exit Sub
        datumTed = DateAdd(DateInterval.Day, 7, datumTed)
        proPropocet()
    End Sub

    Private Sub lblRok_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lblRok.MouseLeftButtonDown
        If Year(DateAdd(DateInterval.Year, -1, datumTed)) = 999 Then Exit Sub
        datumTed = DateAdd(DateInterval.Year, -1, datumTed)
        proPropocet()
    End Sub

    Private Sub lblRok_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lblRok.MouseRightButtonDown
        If Year(DateAdd(DateInterval.Year, 1, datumTed)) = 5001 Then Exit Sub
        datumTed = DateAdd(DateInterval.Year, 1, datumTed)
        proPropocet()
    End Sub

    Private Sub lblTyden_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lblTyden.MouseLeftButtonDown
        TydenZpatky()
    End Sub

    Private Sub lblTyden_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lblTyden.MouseRightButtonDown
        TydenDalsi()
    End Sub

    Private Sub lblMesic_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lblMesic.MouseLeftButtonDown
        If Year(DateAdd(DateInterval.Month, -1, datumTed)) = 999 Then Exit Sub
        datumTed = DateAdd(DateInterval.Month, -1, datumTed)
        proPropocet()
    End Sub

    Private Sub lblMesic_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lblMesic.MouseRightButtonDown
        If Year(DateAdd(DateInterval.Month, 1, datumTed)) = 5001 Then Exit Sub
        datumTed = DateAdd(DateInterval.Month, 1, datumTed)
        proPropocet()
    End Sub
#End Region

#Region " Tisk "

    Private Sub smiTisk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles smiTisk.Click
        PrintGrid(gridPrint)
    End Sub

    Private Sub PrintGrid(ByVal grid As Grid)
        lblApp.Text = Application.CompanyName & "  " & Application.ProductName & "  verze " & Application.Version
        lblCop.Text = "copyright ©1997-" & mySystem.BuildYear.ToString & "  " & Application.LegalCopyright
        Dim iFontSize As Integer = Tyden.FontSize
        Tyden.FontSize = 11
        Dim iOkraj As Integer = 20
        Dim print As New PrintDialog()
        If print.ShowDialog() = False Then Exit Sub
        HideControlsFromPrint(Windows.Visibility.Hidden)
        Dim scale As Double
        If print.PrintTicket.PageOrientation = Printing.PageOrientation.Portrait Then
            scale = print.PrintableAreaWidth / grid.ActualWidth
        Else
            scale = print.PrintableAreaHeight / grid.ActualHeight
        End If

        Dim oldTransform As Transform = grid.LayoutTransform
        grid.LayoutTransform = New ScaleTransform(scale, scale)

        Dim oldSize As New Size(grid.ActualWidth, grid.ActualHeight)
        Dim newSize As New Size(print.PrintableAreaWidth - (2 * iOkraj), (grid.ActualHeight * scale) - (2 * iOkraj))
        grid.Measure(newSize)
        DirectCast(grid, UIElement).Arrange(New Rect(New Point(iOkraj, iOkraj), newSize))

        print.PrintVisual(grid, "Kalendář " + lblTyden.Content.ToString)
        grid.LayoutTransform = oldTransform
        grid.Measure(oldSize)

        DirectCast(grid, UIElement).Arrange(New Rect(New Point(0, 0), oldSize))

        Tyden.FontSize = iFontSize
        HideControlsFromPrint(Windows.Visibility.Visible)
    End Sub

    Private Sub HideControlsFromPrint(ByVal Visible As Windows.Visibility)
        Menu1.Visibility = Visible
        gbDatum.Visibility = Visible
        gbJmen.Visibility = Visible
        gbNaroz.Visibility = Visible
        gbUpon.Visibility = Visible
        If Visible = Windows.Visibility.Hidden Then
            Visible = Windows.Visibility.Visible
        Else
            Visible = Windows.Visibility.Hidden
        End If
        lblApp.Visibility = Visible
        lblCop.Visibility = Visible
    End Sub

#End Region

#Region " Hledání "

    Private Function setCheckCross(ByVal bFound As Boolean) As ImageSource
        Dim checkUri As ImageSource = CType(Application.Current.FindResource("Nalezeno"), ImageSource)
        Dim crossUri As ImageSource = CType(Application.Current.FindResource("Nenalezeno"), ImageSource)
        Return If(bFound, checkUri, crossUri)
    End Function

#Region " Datum "

    Private datumNew As Date

    Private Sub changeNewDate(ByVal Datum As Date)
        If datumTed <> Datum Then
            datumTed = Datum
            proPropocet()
        End If
    End Sub

    Private Sub txtDatum_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDatum.TextChanged
        Dim bDatum As Boolean = False
        If IsDate(txtDatum.Text) Then
            bDatum = True
            datumNew = CDate(txtDatum.Text)
            If datumNew.Year > 5000 Or datumNew.Year < 1000 Then bDatum = False
        End If
        picDatum.Source = setCheckCross(bDatum)
        timDate.IsEnabled = bDatum
    End Sub
#End Region

#Region " Jmeniny "

    Private Sub cboJmen_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboJmen.SelectionChanged
        picJmen.Source = setCheckCross(False)
        Dim Jmeno = TryCast(cboJmen.SelectedItem, clsKalendar.clsJmeno)
        If Jmeno IsNot Nothing Then
            If Jmeno.Mesic = 2 And Jmeno.Den = 29 AndAlso myKal.PrestupnyRok(datumTed) = False Then
                Call (New wpfDialog(Me, "Jméno " & If(Jazyk = "CZ", Jmeno.CZ, Jmeno.SK) & " je přístupné pouze v přestupném roce.", "Kalendář", Nothing, "Zavřít")).ShowDialog()
                Exit Sub
            End If
            picJmen.Source = setCheckCross(True)
            changeNewDate(New Date(datumTed.Year, Jmeno.Mesic, Jmeno.Den))
            Exit Sub
        End If
    End Sub

#End Region

#Region " Narozeniny "

    Private Sub cboNaroz_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboNaroz.SelectionChanged
        picNaroz.Source = setCheckCross(False)
        Dim Osoba = TryCast(cboNaroz.SelectedItem, clsDBase.clsOsoba)
        If Osoba IsNot Nothing Then
            picNaroz.Source = setCheckCross(True)
            changeNewDate(DateSerial(Year(datumTed), Osoba.Narozeni.Month, Osoba.Narozeni.Day))
        End If
    End Sub

#End Region

#Region " Upomínky - plány "
    Private dLastNote As Date

    Private Sub txtUpon_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtUpon.KeyUp
        If e.Key = Key.Enter Then FindFirst(True)
    End Sub

    Private Sub txtUpon_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUpon.TextChanged
        FindFirst()
    End Sub

    Private Sub FindFirst(Optional Jump As Boolean = False)
        If txtUpon.Text.Length < 4 Then Exit Sub
        dLastNote = New Date(3000, 1, 1)
        FindNext(Jump)
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        FindNext()
    End Sub

    Private Sub FindNext(Optional Jump As Boolean = False)
        picUpon.Source = setCheckCross(False)
        btnNext.IsEnabled = False

        Dim Poznamka = Databaze.Poznamky.OrderByDescending(Function(x) x.Den).FirstOrDefault(Function(y) y.Text.ToLower.Contains(txtUpon.Text.ToLower) And y.Den < dLastNote)
        If Poznamka IsNot Nothing Then
            dLastNote = Poznamka.Den
            picUpon.Source = setCheckCross(True)
            changeNewDate(Poznamka.Den)
            btnNext.IsEnabled = True
            If Jump Then btnNext.Focus()
        End If
    End Sub
#End Region

#End Region

#Region " ChangeColours "

    Public Sub proChangeColours(Barva As clsSetting.clsBarva)
        ChangeColors(Barva)
    End Sub

    Private Sub proChangeColours(ProfilJmeno As String)
        Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = ProfilJmeno)
        If Barva IsNot Nothing Then ChangeColors(Barva)
    End Sub

    Private Sub ChangeColors(Barva As clsSetting.clsBarva)
        lblTyden.Background = myColorConverter.StringToBrush(Barva.ZahlaviDatum)
        lblMesic.Background = myColorConverter.StringToBrush(Barva.ZahlaviDatum)
        lblRok.Background = myColorConverter.StringToBrush(Barva.ZahlaviDatum)
        lblDenNadpis.Background = myColorConverter.StringToBrush(Barva.Zahlavi)
        lblNarozeninyNadpis.Background = myColorConverter.StringToBrush(Barva.Zahlavi)
        lblUpominkyNadpis.Background = myColorConverter.StringToBrush(Barva.Zahlavi)
        Tyden.DenBrush = myColorConverter.StringToBrush(Barva.Den)
        Tyden.DenJmenoBrush = myColorConverter.StringToBrush(Barva.DenJmeno)
        Tyden.NarozeninyBrush = myColorConverter.StringToBrush(Barva.Narozeniny)
        Tyden.JmeninyBrush = myColorConverter.StringToBrush(Barva.Jmeniny)
        Tyden.PoznamkyBrush = myColorConverter.StringToBrush(Barva.Poznamky)
        Tyden.SvatkyBrush = myColorConverter.StringToBrush(Barva.Svatky)
        Me.Background = myColorConverter.StringToBrush(Barva.Pozadi)
    End Sub
#End Region

#Region " Dotykové ovládání "

    Private wDiff As Integer = 175
    Private wTouchActive As Boolean
    Private ts1, id1 As Integer
    Private p1, wTouchPoint As Point

    Private Sub lblRok_TouchDown(sender As Object, e As TouchEventArgs) Handles lblRok.TouchDown
        If Math.Abs(ts1 - e.Timestamp) < 20 And e.TouchDevice.Id > id1 Then
            If Year(DateAdd(DateInterval.Year, 2, datumTed)) = 5001 Then Exit Sub
            datumTed = DateAdd(DateInterval.Year, 2, datumTed)
            proPropocet()
        End If

        id1 = e.TouchDevice.Id
        ts1 = e.Timestamp
    End Sub

    Private Sub lblMesic_TouchDown(sender As Object, e As TouchEventArgs) Handles lblMesic.TouchDown
        If Math.Abs(ts1 - e.Timestamp) < 20 And e.TouchDevice.Id > id1 Then
            If Year(DateAdd(DateInterval.Month, 2, datumTed)) = 5001 Then Exit Sub
            datumTed = DateAdd(DateInterval.Month, 2, datumTed)
            proPropocet()
        End If

        id1 = e.TouchDevice.Id
        ts1 = e.Timestamp
    End Sub

    Private Sub lblTyden_TouchDown(sender As Object, e As TouchEventArgs) Handles lblTyden.TouchDown
        If Math.Abs(ts1 - e.Timestamp) < 20 And e.TouchDevice.Id > id1 Then
            If Year(DateAdd(DateInterval.Day, 14, datumTed)) = 5001 Then Exit Sub
            datumTed = DateAdd(DateInterval.Day, 14, datumTed)
            proPropocet()
        End If

        id1 = e.TouchDevice.Id
        ts1 = e.Timestamp
    End Sub

    Private Sub WpfMain_PreviewTouchDown(sender As Object, e As TouchEventArgs) Handles Me.PreviewTouchDown
        wTouchPoint = e.GetTouchPoint(Me).Position
        wTouchActive = True
    End Sub

    Private Sub WpfMain_PreviewTouchMove(sender As Object, e As TouchEventArgs) Handles Me.PreviewTouchMove
        If wTouchActive = False Then Exit Sub
        Dim yDiff As Double = wTouchPoint.Y - e.GetTouchPoint(Me).Position.Y
        Dim xDiff As Double = wTouchPoint.X - e.GetTouchPoint(Me).Position.X
        If yDiff > wDiff Then
            wTouchActive = False
            TydenDalsi()
        ElseIf yDiff < Not wDiff Then
            wTouchActive = False
            TydenZpatky()
        End If
        If xDiff > wDiff Or xDiff < Not wDiff Then
            datumTed = Today
            proPropocet()
        End If
    End Sub

    Private Sub Day_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Dim tbk As TextBlock = CType(sender, TextBlock)
        datumTed = CType(tbk.Tag, Date)
        proPropocet()
    End Sub

    Private Sub Day_TouchDown(sender As Object, e As TouchEventArgs)
        Dim tbk As TextBlock = CType(sender, TextBlock)
        datumTed = CType(tbk.Tag, Date)
        proPropocet()
    End Sub


#End Region

#Region " Jazky "

    Private Sub smiCZ_Click(sender As Object, e As RoutedEventArgs) Handles smiCZ.Click
        Nastaveni.Jazyk = "CZ"
        LoadJazyk(Nastaveni.Jazyk)
        proPropocet()
    End Sub

    Private Sub smiSK_Click(sender As Object, e As RoutedEventArgs) Handles smiSK.Click
        Nastaveni.Jazyk = "SK"
        LoadJazyk(Nastaveni.Jazyk)
        proPropocet()
    End Sub

    Private Sub LoadJazyk(Lge As String)
        Jazyk = Lge
        imgJazyk.Source = CType(Me.FindResource("flag" + Jazyk + "bw"), ImageSource)
        cboJmen.Items.SortDescriptions.Clear()
        cboJmen.DisplayMemberPath = Jazyk
        cboJmen.SelectedValuePath = Jazyk
        cboJmen.SelectedValue = New Binding(Jazyk)
        cboJmen.ItemsSource = myKal.Jmena
        cboJmen.Items.Filter = Function(a) a.GetType.GetProperty(Jazyk).GetValue(a, Nothing).ToString <> ""
        cboJmen.Items.SortDescriptions.Add(New ComponentModel.SortDescription(Jazyk, ComponentModel.ListSortDirection.Ascending))
        For i = 0 To 6
            Tyden(i).DenJmeno = myKal.GetDenJmeno(i + 1)
        Next
        lblDenNadpis.Content = If(Jazyk = "CZ", "Den", "Deň")
        lblNarozeninyNadpis.Content = If(Jazyk = "CZ", "Narozeniny / Jmeniny", "Narodeniny / Meniny")
        lblUpominkyNadpis.Content = If(Jazyk = "CZ", "Plány / Svátky", "Plány / Sviatky")
    End Sub

#End Region

End Class
