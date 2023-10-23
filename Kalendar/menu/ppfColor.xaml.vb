Imports System.Collections.ObjectModel

Class ppfColor

    Private uriImg As New Uri("/Kalendar;component/RGBpalette.png", UriKind.Relative)
    Private timPickup As New Windows.Threading.DispatcherTimer
    Private cbxSelected As ComboBox
    Private wSetting As wpfSetting = Application.SettingWindow()

#Region " Loaded "

    Private Sub ppfColor_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        timPickup.Interval = TimeSpan.FromMilliseconds(200)
        AddHandler timPickup.Tick, AddressOf timPickup_Tick

        Dim TT As New ToolTip
        TT.Content = "Levým klikem nastavíte uloženou barvu." + Chr(10) + "Pravým klikem nastavíte výchozí barvu."
        txtMesic.ToolTip = TT : txtDen.ToolTip = TT : txtHlava.ToolTip = TT : txtNazev.ToolTip = TT : txtNaroz.ToolTip = TT : txtJmenin.ToolTip = TT : txtPoznam.ToolTip = TT : txtSvatky.ToolTip = TT : txtPozadi.ToolTip = TT
        cbxDen.Foreground = If(mySystem.Framework >= 4, myColorConverter.StringToBrush("#FFDBDEDE"), Brushes.White)

        'Doplnění základních profilů barev
        cbxProfil.DisplayMemberPath = "Profil"
        cbxProfil.ItemsSource = Nastaveni.Barvy
        Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = Nastaveni.BarvyJmeno)
        If Barva Is Nothing Then
            cbxProfil.SelectedIndex = 0
        Else
            cbxProfil.SelectedItem = Barva
            SetTagColors(Barva)
        End If
        Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = "nový návrh")
        If Barva IsNot Nothing Then SetTagColors(Barva)
    End Sub

    Private Sub SetColorsOnMainWindow()
        DirectCast(Application.Current.MainWindow, WpfMain).proChangeColours(ColorsToClass(New clsSetting.clsBarva))
    End Sub

    Private Sub SetTagColors(Barva As clsSetting.clsBarva)
        txtMesic.Tag = Barva.ZahlaviDatum
        txtHlava.Tag = Barva.Zahlavi
        txtDen.Tag = Barva.Den
        txtNazev.Tag = Barva.DenJmeno
        txtNaroz.Tag = Barva.Narozeniny
        txtJmenin.Tag = Barva.Jmeniny
        txtPoznam.Tag = Barva.Poznamky
        txtSvatky.Tag = Barva.Svatky
        txtPozadi.Tag = Barva.Pozadi
    End Sub

#End Region

#Region " Color Picker "

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        myFile.Launch(wSetting, Chr(34) + "control" + Chr(34) + " " + "desk.cpl")
    End Sub

    Private Sub timPickup_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim BMP As New System.Drawing.Bitmap(1, 1)
        Dim GFX As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(BMP)
        Dim Pointer As Point = myWindow.PixelToPPI(myWindow.GetMousePosition(), False)
        GFX.CopyFromScreen(New System.Drawing.Point(CInt(Pointer.X), CInt(Pointer.Y)), New System.Drawing.Point(0, 0), BMP.Size)
        Dim dColor As System.Drawing.Color = BMP.GetPixel(0, 0)
        Dim mColor As Color = myColorConverter.ColorDrawingToMedia(dColor)
        cbxSelected.Text = mColor.ToString
        If mySystem.Framework >= 4 And mySystem.Current.Number > 7 Then
            cbxSelected.Foreground = myColorConverter.ColorToBrush(mColor)
        Else
            cbxSelected.Background = myColorConverter.ColorToBrush(mColor)
        End If
        SetColorsOnMainWindow()
    End Sub

#End Region

#Region " Combo "

    Private Sub cbxProfil_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cbxProfil.SelectionChanged
        If cbxProfil.SelectedIndex = -1 Then
            btnSave.IsEnabled = False
        Else
            btnSave.IsEnabled = True
            Dim Barva = CType(cbxProfil.SelectedItem, clsSetting.clsBarva)
            ClassToColors(Barva)
            SetColorsOnMainWindow()
        End If
    End Sub

    Private Sub cbxProfil_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)
        proCheckProfile()
    End Sub

    Private Sub cbxColor_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cbxMesic.SelectionChanged, cbxHlava.SelectionChanged, cbxDen.SelectionChanged, cbxNazev.SelectionChanged, cbxNaroz.SelectionChanged, cbxJmenin.SelectionChanged, cbxPoznam.SelectionChanged, cbxSvatky.SelectionChanged, cbxPozadi.SelectionChanged
        Dim cbx As ComboBox = CType(sender, ComboBox)
        cbx.SelectedIndex = -1
    End Sub

    Private Sub cbxColor_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)
        Dim cbx As ComboBox = CType(sender, ComboBox)
        If Not cbx.Text.StartsWith("#") And Not cbx.Text = "" Then
            If IsNothing(myColorConverter.NameToColor(cbx.Text)) = False Then
                Dim mColor As Color = myColorConverter.NameToColor(cbx.Text)
                If Not mColor.ToString = "#00000000" Then
                    cbx.Background = myColorConverter.ColorToBrush(mColor)
                    cbx.Text = mColor.ToString
                    SetColorsOnMainWindow()
                End If
            End If
        End If
    End Sub

    Private Sub cbxColor_DropDownClosed(sender As System.Object, e As System.EventArgs) Handles cbxMesic.DropDownClosed, cbxHlava.DropDownClosed, cbxDen.DropDownClosed, cbxNazev.DropDownClosed, cbxNaroz.DropDownClosed, cbxJmenin.DropDownClosed, cbxPoznam.DropDownClosed, cbxSvatky.DropDownClosed, cbxPozadi.DropDownClosed
        Dim cbx As ComboBox = CType(sender, ComboBox)
        timPickup.Stop()
        cbx.Text = cbx.Background.ToString
    End Sub

    Private Sub cbxColor_DropDownOpened(sender As System.Object, e As System.EventArgs) Handles cbxMesic.DropDownOpened, cbxHlava.DropDownOpened, cbxDen.DropDownOpened, cbxNazev.DropDownOpened, cbxNaroz.DropDownOpened, cbxJmenin.DropDownOpened, cbxPoznam.DropDownOpened, cbxSvatky.DropDownOpened, cbxPozadi.DropDownOpened
        cbxSelected = CType(sender, ComboBox)
        Dim Item As ComboBoxItem = CType(cbxSelected.Items(0), ComboBoxItem)
        For Each Control As Object In DirectCast(Item.Content, Grid).Children
            If TypeOf Control Is Image Then
                Dim imgFound As Image = CType(Control, Image)
                imgFound.Source = New BitmapImage(uriImg)
            End If
        Next
        timPickup.Start()
    End Sub

    Private Function ColorsToClass(Barva As clsSetting.clsBarva) As clsSetting.clsBarva
        Barva.ZahlaviDatum = myColorConverter.BrushToString(cbxMesic.Foreground)
        Barva.Zahlavi = myColorConverter.BrushToString(cbxHlava.Foreground)
        Barva.Den = myColorConverter.BrushToString(cbxDen.Foreground)
        Barva.DenJmeno = myColorConverter.BrushToString(cbxNazev.Foreground)
        Barva.Narozeniny = myColorConverter.BrushToString(cbxNaroz.Foreground)
        Barva.Jmeniny = myColorConverter.BrushToString(cbxJmenin.Foreground)
        Barva.Poznamky = myColorConverter.BrushToString(cbxPoznam.Foreground)
        Barva.Svatky = myColorConverter.BrushToString(cbxSvatky.Foreground)
        Barva.Pozadi = myColorConverter.BrushToString(cbxPozadi.Foreground)
        Return Barva
    End Function

    Private Sub ClassToColors(Barva As clsSetting.clsBarva)
        txtMesic.Background = myColorConverter.StringToBrush(Barva.ZahlaviDatum)
        cbxMesic.Text = Barva.ZahlaviDatum
        txtHlava.Background = myColorConverter.StringToBrush(Barva.Zahlavi)
        cbxHlava.Text = Barva.Zahlavi
        txtDen.Background = myColorConverter.StringToBrush(Barva.Den)
        cbxDen.Text = Barva.Den
        txtNazev.Background = myColorConverter.StringToBrush(Barva.DenJmeno)
        cbxNazev.Text = Barva.DenJmeno
        txtNaroz.Background = myColorConverter.StringToBrush(Barva.Narozeniny)
        cbxNaroz.Text = Barva.Narozeniny
        txtJmenin.Background = myColorConverter.StringToBrush(Barva.Jmeniny)
        cbxJmenin.Text = Barva.Jmeniny
        txtPoznam.Background = myColorConverter.StringToBrush(Barva.Poznamky)
        cbxPoznam.Text = Barva.Poznamky
        txtSvatky.Background = myColorConverter.StringToBrush(Barva.Svatky)
        cbxSvatky.Text = Barva.Svatky
        txtPozadi.Background = myColorConverter.StringToBrush(Barva.Pozadi)
        cbxPozadi.Text = Barva.Pozadi

        cbxDen.Foreground = txtDen.Background
        cbxMesic.Foreground = txtMesic.Background
        cbxHlava.Foreground = txtHlava.Background
        cbxNazev.Foreground = txtNazev.Background
        cbxNaroz.Foreground = txtNaroz.Background
        cbxJmenin.Foreground = txtJmenin.Background
        cbxPoznam.Foreground = txtPoznam.Background
        cbxSvatky.Foreground = txtSvatky.Background
        cbxPozadi.Foreground = txtPozadi.Background
    End Sub

#End Region

#Region " Buttons "

    Private Sub btnApply_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        If Not cbxProfil.Items.Count = 0 Then
            If IsNothing(cbxProfil.SelectedItem) = False Then
                Dim Barva = CType(cbxProfil.SelectedItem, clsSetting.clsBarva)
                ColorsToClass(Barva)
                Nastaveni.BarvyJmeno = Barva.Profil
            End If
        End If
    End Sub

    Private Sub btnProfile_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnProfile.Click
        proCheckProfile()

        btnProfile.ContextMenu.PlacementTarget = btnProfile
        btnProfile.ContextMenu.IsOpen = True
    End Sub

    Private Sub proCheckProfile()
        miDel.IsEnabled = False
        miAdd.IsEnabled = False
        If Not cbxProfil.Text = "" Then
            Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = cbxProfil.Text)
            If Barva Is Nothing Then
                cbxProfil.SelectedItem = -1
                miAdd.IsEnabled = True
                btnSave.IsEnabled = False
            Else
                If Not cbxProfil.SelectedIndex = -1 Then
                    If Nastaveni.Barvy.Count > 1 Then miDel.IsEnabled = True
                    btnSave.IsEnabled = True
                End If
            End If
        End If
    End Sub

#End Region

#Region " TextBoxes "

    'cbxMesic., cbxHlava., cbxDen., cbxNazev., cbxNaroz., cbxJmenin., cbxPoznam., cbxSvatky., cbxPozadi.

    Private Sub txtMesic_MouseRightButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtMesic.MouseRightButtonDown
        cbxMesic.Foreground = myColorConverter.StringToBrush(txtMesic.Tag.ToString)
        cbxMesic.Text = txtMesic.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtMesic_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtMesic.MouseLeftButtonDown
        cbxMesic.Foreground = txtMesic.Background
        cbxMesic.Text = myColorConverter.BrushToString(txtMesic.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtHlava_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtHlava.MouseRightButtonDown
        cbxHlava.Foreground = myColorConverter.StringToBrush(txtHlava.Tag.ToString)
        cbxHlava.Text = txtHlava.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtHlava_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtHlava.MouseLeftButtonDown
        cbxHlava.Foreground = txtHlava.Background
        cbxHlava.Text = myColorConverter.BrushToString(txtHlava.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtDen_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtDen.MouseRightButtonDown
        cbxDen.Foreground = myColorConverter.StringToBrush(txtDen.Tag.ToString)
        cbxDen.Text = txtDen.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtDen_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtDen.MouseLeftButtonDown
        cbxDen.Foreground = txtDen.Background
        cbxDen.Text = myColorConverter.BrushToString(txtDen.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtNazev_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtNazev.MouseRightButtonDown
        cbxNazev.Foreground = myColorConverter.StringToBrush(txtNazev.Tag.ToString)
        cbxNazev.Text = txtNazev.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtNazev_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtNazev.MouseLeftButtonDown
        cbxNazev.Foreground = txtNazev.Background
        cbxNazev.Text = myColorConverter.BrushToString(txtNazev.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtNaroz_MouseRightButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtNaroz.MouseRightButtonDown
        cbxNaroz.Foreground = myColorConverter.StringToBrush(txtNaroz.Tag.ToString)
        cbxNaroz.Text = txtNaroz.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtNaroz_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtNaroz.MouseLeftButtonDown
        cbxNaroz.Foreground = txtNaroz.Background
        cbxNaroz.Text = myColorConverter.BrushToString(txtNaroz.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtJmenin_MouseRightButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtJmenin.MouseRightButtonDown
        cbxJmenin.Foreground = myColorConverter.StringToBrush(txtJmenin.Tag.ToString)
        cbxJmenin.Text = txtJmenin.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtJmenin_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtJmenin.MouseLeftButtonDown
        cbxJmenin.Foreground = txtJmenin.Background
        cbxJmenin.Text = myColorConverter.BrushToString(txtJmenin.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtPoznam_MouseRightButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtPoznam.MouseRightButtonDown
        cbxPoznam.Foreground = myColorConverter.StringToBrush(txtPoznam.Tag.ToString)
        cbxPoznam.Text = txtPoznam.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtPoznam_MouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtPoznam.MouseLeftButtonDown
        cbxPoznam.Foreground = txtPoznam.Background
        cbxPoznam.Text = myColorConverter.BrushToString(txtPoznam.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtSvatky_MouseRightButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtSvatky.MouseRightButtonDown
        cbxSvatky.Foreground = myColorConverter.StringToBrush(txtSvatky.Tag.ToString)
        cbxSvatky.Text = txtSvatky.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtSvatky_MouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtSvatky.MouseLeftButtonDown
        cbxSvatky.Foreground = txtSvatky.Background
        cbxSvatky.Text = myColorConverter.BrushToString(txtSvatky.Background)
        SetColorsOnMainWindow()
    End Sub

    Private Sub txtPozadi_MouseRightButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtPozadi.MouseRightButtonDown
        cbxPozadi.Foreground = myColorConverter.StringToBrush(txtPozadi.Tag.ToString)
        cbxPozadi.Text = txtPozadi.Tag.ToString
        SetColorsOnMainWindow()
    End Sub
    Private Sub txtPozadi_MouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles txtPozadi.MouseLeftButtonDown
        cbxPozadi.Foreground = txtPozadi.Background
        cbxPozadi.Text = myColorConverter.BrushToString(txtPozadi.Background)
        SetColorsOnMainWindow()
    End Sub

#End Region

#Region " Menu "

    Private Sub miAdd_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles miAdd.Click
        Dim Barva = New clsSetting.clsBarva
        Barva.Profil = cbxProfil.Text
        ColorsToClass(Barva)
        Nastaveni.Barvy.Add(Barva)
        cbxProfil.SelectedIndex = cbxProfil.Items.Count - 1
    End Sub

    Private Sub miDel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles miDel.Click
        Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = cbxProfil.Text)
        If Barva IsNot Nothing Then Nastaveni.Barvy.Remove(Barva)
    End Sub

    Private Sub miexport_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles miExport.Click
        Dim dlg As New Microsoft.Win32.SaveFileDialog
        dlg.Filter = "Extensible Markup Language|*.xml"
        dlg.Title = "Vyberte lokaci k uložení profilů barev:"
        dlg.FileName = "KalendarBarvy.xml"
        If dlg.ShowDialog = True Then
            Call (New clsSerialization(Nastaveni.Barvy, wSetting)).WriteXml(dlg.FileName)
        End If
    End Sub

    Private Sub miImport_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles miImport.Click
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Title = "Vyberte soubor s uloženými profily"
        dlg.Filter = "Extensible Markup Language|*.xml"

        If dlg.ShowDialog Then
            Dim iPocet As Integer
            Dim importBarvy As New Collection(Of clsSetting.clsBarva)
            importBarvy = CType(New clsSerialization(importBarvy, wSetting).ReadXml(dlg.FileName), Collection(Of clsSetting.clsBarva))

            For Each importBarva In importBarvy
                Dim Barva = Nastaveni.Barvy.FirstOrDefault(Function(x) x.Profil = importBarva.Profil)
                If Barva Is Nothing Then
                    iPocet += 1
                    Nastaveni.Barvy.Add(importBarva)
                End If
            Next
            Call (New wpfDialog(wSetting, "Počet nových profilů barev: " + iPocet.ToString, "Import barev", Nothing, "Zavřít")).ShowDialog()
        End If
    End Sub

#End Region


End Class
