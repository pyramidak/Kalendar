Imports System.Collections.ObjectModel
Imports System.ComponentModel

#Region " Nastavení "

Public Class clsSetting

    Public Verze As Integer
    Public Aktualizovat As Boolean = True
    Public Width As Integer = 680
    Public Height As Integer = 675
    Public Jazyk As String = "CZ"
    Public FontSize As Integer = 16
    Public BarvyJmeno As String = "nový návrh"
    'Spuštění programu
    Public Spusteni As Integer
    Public Spusteno As Date = Today
    'Služba
    Public SluzbaPrvni As Date = New Date(Now.Year, 1, Now.Day)
    Public SluzbaZnova As Integer 'dnů
    'Public SerYear As Integer
    Public Obrazek As String = ""
    'Synchronizace
    Public DropBox As Boolean
    Public GoogleDrive As Boolean
    Public OneDrive As Boolean
    Public Sync As Boolean
    Public RemoteOneDrive As Boolean
    Public RefreshToken As String = ""

    Public Barvy As New ObservableCollection(Of clsBarva)

    Public Class clsBarva
        Implements INotifyPropertyChanged

        Private sProfil, sZahlaviDatum, sZahlavi, sDen, sDenJmeno, sJmeniny, sNarozeniny, sPoznamky, sSvatky, sPozadi As String

#Region " Get/Set "

        Public Property Profil() As String
            Get
                Return sProfil
            End Get
            Set(ByVal value As String)
                sProfil = value
                OnPropertyChanged("Profil")
            End Set
        End Property

        Public Property ZahlaviDatum() As String
            Get
                Return sZahlaviDatum
            End Get
            Set(ByVal value As String)
                sZahlaviDatum = value
                OnPropertyChanged("ZahlaviDatum")
            End Set
        End Property

        Public Property Zahlavi() As String
            Get
                Return sZahlavi
            End Get
            Set(ByVal value As String)
                sZahlavi = value
                OnPropertyChanged("Zahlavi")
            End Set
        End Property

        Public Property Den() As String
            Get
                Return sDen
            End Get
            Set(ByVal value As String)
                sDen = value
                OnPropertyChanged("Den")
            End Set
        End Property

        Public Property DenJmeno() As String
            Get
                Return sDenJmeno
            End Get
            Set(ByVal value As String)
                sDenJmeno = value
                OnPropertyChanged("DenJmeno")
            End Set
        End Property

        Public Property Jmeniny() As String
            Get
                Return sJmeniny
            End Get
            Set(ByVal value As String)
                sJmeniny = value
                OnPropertyChanged("Jmeniny")
            End Set
        End Property

        Public Property Narozeniny() As String
            Get
                Return sNarozeniny
            End Get
            Set(ByVal value As String)
                sNarozeniny = value
                OnPropertyChanged("Narozeniny")
            End Set
        End Property

        Public Property Poznamky() As String
            Get
                Return sPoznamky
            End Get
            Set(ByVal value As String)
                sPoznamky = value
                OnPropertyChanged("Poznamky")
            End Set
        End Property

        Public Property Svatky() As String
            Get
                Return sSvatky
            End Get
            Set(ByVal value As String)
                sSvatky = value
                OnPropertyChanged("Svatky")
            End Set
        End Property

        Public Property Pozadi() As String
            Get
                Return sPozadi
            End Get
            Set(ByVal value As String)
                sPozadi = value
                OnPropertyChanged("Pozadi")
            End Set
        End Property

#End Region

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Sub New()
        End Sub

        Sub New(Profil_ As String, ZahlaviDatum_ As String, Zahlavi_ As String, Den_ As String, DenJmeno_ As String, Jmenininy_ As String, Narozeniny_ As String, Poznamky_ As String, Svatky_ As String, Pozadi_ As String)
            sProfil = Profil_ : sZahlaviDatum = ZahlaviDatum_ : sZahlavi = Zahlavi_ : sDen = Den_ : sDenJmeno = DenJmeno_ : sJmeniny = Jmenininy_ : sNarozeniny = Narozeniny_ : sPoznamky = Poznamky_ : sSvatky = Svatky_ : sPozadi = Pozadi_
        End Sub

    End Class

End Class

#End Region

#Region " Data "

Public Class clsDBase

    Public Osoby As New ObservableCollection(Of clsOsoba)
    Public Poznamky As New ObservableCollection(Of clsPoznamka)

#Region " Osoba "

    Public Class clsOsoba
        Implements INotifyPropertyChanged

        Private sJmeno, sRC As String
        Private dNarozeni, dUmrti, dZmena As Date
        Private bSmazane As Boolean
        Public Uid As Long

#Region " Get/Set "

        Public Property Jmeno As String
            Get
                Return sJmeno
            End Get
            Set(ByVal value As String)
                sJmeno = value
                OnPropertyChanged("Jmeno")
            End Set
        End Property

        Public Property Zmena As Date
            Get
                Return dZmena
            End Get
            Set(ByVal value As Date)
                dZmena = value
                OnPropertyChanged("Zmena")
            End Set
        End Property

        Public Property Narozeni As Date
            Get
                Return dNarozeni
            End Get
            Set(ByVal value As Date)
                dNarozeni = value
                OnPropertyChanged("Narozeni")
            End Set
        End Property

        Public Property Umrti As Date
            Get
                Return dUmrti
            End Get
            Set(ByVal value As Date)
                dUmrti = value
                OnPropertyChanged("Umrti")
            End Set
        End Property

        Public Property RodneCislo As String
            Get
                Return sRC
            End Get
            Set(ByVal value As String)
                sRC = value
                OnPropertyChanged("RodneCislo")
            End Set
        End Property

        Public Property Smazane As Boolean
            Get
                Return bSmazane
            End Get
            Set(ByVal value As Boolean)
                bSmazane = value
                OnPropertyChanged("Smazane")
            End Set
        End Property

#End Region

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Sub New()
        End Sub

        Sub New(Jmeno_ As String, Narozeni_ As Date, Umrti_ As Date, Zmena_ As Date, RodneCislo_ As String, Smazane_ As Boolean)
            sJmeno = Jmeno_ : dNarozeni = Narozeni_ : dUmrti = Umrti_ : dZmena = Zmena_ : sRC = RodneCislo_ : bSmazane = Smazane_
        End Sub

    End Class

#End Region

#Region " Upomínka "

    Public Class clsPoznamka
        Implements INotifyPropertyChanged

        Private brushPoznamka, brushTime As Brush
        Private iFontSize As Integer
        Private sText, sTime As String
        Private bRocne, bMesicne As Boolean
        Public Vznik, Den, Zmena, Alarm As Date
        Public Uid As Long
        Public SaveActive As Boolean

#Region " Get/Set "

        Public Property Time As String
            Get
                If sTime = "" Then
                    sTime = Den.ToShortTimeString
                    TimeBrush = myColorConverter.ColorToBrush(If(Den.ToShortTimeString = "0:00", Colors.Gray, Colors.Black))
                End If
                Return sTime
            End Get
            Set(ByVal value As String)
                Dim newColor As Color = Colors.Red
                If value.Contains(":") Then
                    Dim pulky = Split(value, ":")
                    Dim hodin = pulky(0)
                    Dim minut = pulky(1)
                    If IsNumeric(hodin) And IsNumeric(minut) OrElse minut = "" Then
                        If minut.Length = 2 Then
                            Dim hodina = CInt(hodin)
                            Dim minuta = CInt(minut)
                            If hodina >= 0 And hodina < 24 And minuta >= 0 And minuta < 60 Then
                                Den = New DateTime(Den.Year, Den.Month, Den.Day, hodina, minuta, 0)
                                Save()
                                newColor = If(hodina = 0 And minuta = 0, Colors.Gray, Colors.Black)
                            End If
                        End If
                        sTime = value
                        OnPropertyChanged("Time")
                    End If
                End If
                TimeBrush = myColorConverter.ColorToBrush(newColor)
            End Set
        End Property

        Public Property Text As String
            Get
                Return sText
            End Get
            Set(ByVal value As String)
                sText = value
                OnPropertyChanged("Text")
                Save()
            End Set
        End Property

        Public Property Mesicne As Boolean
            Get
                Return bMesicne
            End Get
            Set(ByVal value As Boolean)
                bMesicne = value
                OnPropertyChanged("Mesicne")
                Save()
            End Set
        End Property

        Public Property Rocne As Boolean
            Get
                Return bRocne
            End Get
            Set(ByVal value As Boolean)
                bRocne = value
                OnPropertyChanged("Rocne")
                Save()
            End Set
        End Property

        Public Property TimeBrush As Brush
            Get
                Return brushTime
            End Get
            Set(ByVal value As Brush)
                brushTime = value
                OnPropertyChanged("TimeBrush")
            End Set
        End Property

        Public Property PoznamkaBrush As Brush
            Get
                Return brushPoznamka
            End Get
            Set(ByVal value As Brush)
                brushPoznamka = value
                OnPropertyChanged("PoznamkaBrush")
            End Set
        End Property

        Public Property FontSize As Integer
            Get
                Return iFontSize
            End Get
            Set(ByVal value As Integer)
                iFontSize = value
                OnPropertyChanged("FontSize")
            End Set
        End Property

#End Region

        Private Sub Save()
            If SaveActive Then
                Zmena = Now
                If Uid = 0 Then
                    If Not sText = "" Then
                        Uid = mySQL.insertUpominka(Me)
                        Databaze.Poznamky.Add(Me)
                    End If
                Else
                    mySQL.updateUpominka(Me)
                End If
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Sub New()
        End Sub

        Sub New(Vznik_ As Date, Den_ As Date, Text_ As String, Mesicne_ As Boolean, Rocne_ As Boolean, Zmena_ As Date)
            Vznik = Vznik_ : Den = Den_ : sText = Text_ : bMesicne = Mesicne_ : bRocne = Rocne_ : Zmena = Zmena_
        End Sub

    End Class

#End Region

End Class

#End Region

#Region " Data Old "

Public Class clsDatabase

    Public Osoby As New ObservableCollection(Of clsOsoba)
    Public Poznamky As New ObservableCollection(Of clsPoznamka)

#Region " Osoba "

    Public Class clsOsoba
        Implements INotifyPropertyChanged

        Private sJmeno, sRC As String
        Private dNarozeni, dUmrti, dZmena As Date
        Private bSmazane As Boolean

#Region " Get/Set "

        Public Property Jmeno() As String
            Get
                Return sJmeno
            End Get
            Set(ByVal value As String)
                sJmeno = value
                OnPropertyChanged("Jmeno")
            End Set
        End Property

        Public Property Zmena() As Date
            Get
                Return dZmena
            End Get
            Set(ByVal value As Date)
                dZmena = value
                OnPropertyChanged("Zmena")
            End Set
        End Property

        Public Property Narozeni() As Date
            Get
                Return dNarozeni
            End Get
            Set(ByVal value As Date)
                dNarozeni = value
                OnPropertyChanged("Narozeni")
            End Set
        End Property

        Public Property Umrti() As Date
            Get
                Return dUmrti
            End Get
            Set(ByVal value As Date)
                dUmrti = value
                OnPropertyChanged("Umrti")
            End Set
        End Property

        Public Property RodneCislo() As String
            Get
                Return sRC
            End Get
            Set(ByVal value As String)
                sRC = value
                OnPropertyChanged("RodneCislo")
            End Set
        End Property

        Public Property Smazane() As Boolean
            Get
                Return bSmazane
            End Get
            Set(ByVal value As Boolean)
                bSmazane = value
                OnPropertyChanged("Smazane")
            End Set
        End Property

#End Region

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Sub New()
        End Sub

        Sub New(Jmeno_ As String, Narozeni_ As Date, Umrti_ As Date, Zmena_ As Date, RodneCislo_ As String, Smazane_ As Boolean)
            sJmeno = Jmeno_ : dNarozeni = Narozeni_ : dUmrti = Umrti_ : dZmena = Zmena_ : sRC = RodneCislo_ : bSmazane = Smazane_
        End Sub

    End Class

#End Region

#Region " Upomínka "

    Public Class clsPoznamka
        Implements INotifyPropertyChanged

        Private sPoznamka As String
        Private dDne, dZmena As Date
        Private bOpakovat, bNemazat As Boolean

#Region " Get/Set "

        Public Property Poznamka() As String
            Get
                Return sPoznamka
            End Get
            Set(ByVal value As String)
                sPoznamka = value
                OnPropertyChanged("Poznamka")
            End Set
        End Property

        Public Property Zmena() As Date
            Get
                Return dZmena
            End Get
            Set(ByVal value As Date)
                dZmena = value
                OnPropertyChanged("Zmena")
            End Set
        End Property

        Public Property Dne() As Date
            Get
                Return dDne
            End Get
            Set(ByVal value As Date)
                dDne = value
                OnPropertyChanged("Dne")
            End Set
        End Property

        Public Property Opakovat() As Boolean
            Get
                Return bOpakovat
            End Get
            Set(ByVal value As Boolean)
                bOpakovat = value
                OnPropertyChanged("Opakovat")
            End Set
        End Property

        Public Property Nemazat() As Boolean
            Get
                Return bNemazat
            End Get
            Set(ByVal value As Boolean)
                bNemazat = value
                OnPropertyChanged("Nemazat")
            End Set
        End Property

#End Region

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Sub New()
        End Sub

        Sub New(Poznamka_ As String, Dne_ As Date, Zmena_ As Date, Opakovat_ As Boolean, Nemazat_ As Boolean)
            sPoznamka = Poznamka_ : dDne = Dne_ : dZmena = Zmena_ : bOpakovat = Opakovat_ : bNemazat = Nemazat_
        End Sub

    End Class

#End Region

End Class

#End Region