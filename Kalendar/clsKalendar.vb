Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public Class clsKalendar
    Private Dny As New Collection(Of clsDen)
    Private Mesice As New Collection(Of clsMesic)
    Public Jmena As New Collection(Of clsJmeno)
    Private Svatky As New Collection(Of clsSvatek)
    Private Sezona As New Collection(Of Date)

#Region " Sub Classes "

    Private Class clsDen
        'MĚSÍC	DNŮ	CZ	SK
        Property Den As Integer
        Property CZ As String
        Property SK As String

        Sub New(iDen As Integer, sCZ As String, sSK As String)
            Den = iDen : CZ = sCZ : SK = sSK
        End Sub
    End Class

    Private Class clsMesic
        'MĚSÍC	DNŮ	CZ	SK
        Property Mesic As Integer
        Property Dnu As Integer
        Property CZ As String
        Property SK As String

        Sub New(iMesic As Integer, iDnu As Integer, sCZ As String, sSK As String)
            Mesic = iMesic : Dnu = iDnu : CZ = sCZ : SK = sSK
        End Sub
    End Class

    Public Class clsJmeno
        'DEN	MĚSÍC	CZ	SK
        Property Den As Integer
        Property Mesic As Integer
        Property CZ As String
        Property SK As String

        Sub New(iDen As Integer, iMesic As Integer, sCZ As String, sSK As String)
            Den = iDen : Mesic = iMesic : CZ = sCZ : SK = sSK
        End Sub
    End Class

    Private Class clsSvatek
        'DEN	MĚSÍC	CZ	SK
        Property Den As Integer
        Property Mesic As Integer
        Property CZ As String
        Property SK As String
        Property CZzavreno As Boolean

        Sub New(iDen As Integer, iMesic As Integer, sCZ As String, sSK As String, Optional bCZzavreno As Boolean = False)
            Den = iDen : Mesic = iMesic : CZ = sCZ : SK = sSK : CZzavreno = bCZzavreno
        End Sub
    End Class

    Enum Obdobi
        Jaro = 3
        Leto = 6
        Podzim = 9
        Zima = 12
    End Enum

#End Region

#Region " Load Data "

    Sub New()
        Dny.Add(New clsDen(1, "Pondělí", "Pondelok"))
        Dny.Add(New clsDen(2, "Úterý", "Utorok"))
        Dny.Add(New clsDen(3, "Středa", "Streda"))
        Dny.Add(New clsDen(4, "Čtvrtek", "Štvrtok"))
        Dny.Add(New clsDen(5, "Pátek", "Piatok"))
        Dny.Add(New clsDen(6, "Sobota", "Sobota"))
        Dny.Add(New clsDen(7, "Neděle", "Nedeľa"))

        Mesice.Add(New clsMesic(1, 31, "Leden", "Január"))
        Mesice.Add(New clsMesic(2, 28, "Únor", "Február"))
        Mesice.Add(New clsMesic(3, 31, "Březen", "Marec"))
        Mesice.Add(New clsMesic(4, 30, "Duben", "Apríl"))
        Mesice.Add(New clsMesic(5, 31, "Květen", "Máj"))
        Mesice.Add(New clsMesic(6, 30, "Červen", "Jún"))
        Mesice.Add(New clsMesic(7, 31, "Červenec", "Júl"))
        Mesice.Add(New clsMesic(8, 31, "Srpen", "August"))
        Mesice.Add(New clsMesic(9, 30, "Září", "Septembra"))
        Mesice.Add(New clsMesic(10, 31, "Říjen", "Október"))
        Mesice.Add(New clsMesic(11, 30, "Listopad", "November"))
        Mesice.Add(New clsMesic(12, 31, "Prosinec", "December"))

        Jmena.Add(New clsJmeno(2, 1, "", "Alexandra"))
        Jmena.Add(New clsJmeno(2, 1, "Karina", "Karina"))
        Jmena.Add(New clsJmeno(3, 1, "Radmila", "Daniela"))
        Jmena.Add(New clsJmeno(4, 1, "Diana", "Drahoslav"))
        Jmena.Add(New clsJmeno(5, 1, "Dalimil", "Andrea"))
        Jmena.Add(New clsJmeno(6, 1, "", "Antónia"))
        Jmena.Add(New clsJmeno(7, 1, "Vilma", "Bohuslava"))
        Jmena.Add(New clsJmeno(8, 1, "Čestmír", "Severín"))
        Jmena.Add(New clsJmeno(9, 1, "Vladan", "Alexej"))
        Jmena.Add(New clsJmeno(10, 1, "Břetislav", "Dáša"))
        Jmena.Add(New clsJmeno(11, 1, "Bohdana", "Malvína"))
        Jmena.Add(New clsJmeno(12, 1, "Pravoslav", "Ernest"))
        Jmena.Add(New clsJmeno(13, 1, "Edita", "Rastislav"))
        Jmena.Add(New clsJmeno(14, 1, "Radovan", "Radovan"))
        Jmena.Add(New clsJmeno(15, 1, "Alice", "Dobroslav"))
        Jmena.Add(New clsJmeno(16, 1, "Ctirad", "Kristína"))
        Jmena.Add(New clsJmeno(17, 1, "Drahoslav", "Nataša"))
        Jmena.Add(New clsJmeno(18, 1, "Vladislav", "Bohdana"))
        Jmena.Add(New clsJmeno(19, 1, "Doubravka", "Drahomíra"))
        Jmena.Add(New clsJmeno(19, 1, "", "Mário"))
        Jmena.Add(New clsJmeno(20, 1, "Ilona", "Dalibor"))
        Jmena.Add(New clsJmeno(21, 1, "Běla", "Vincent"))
        Jmena.Add(New clsJmeno(22, 1, "Slavomír", "Zora"))
        Jmena.Add(New clsJmeno(23, 1, "Zdeněk", "Miloš"))
        Jmena.Add(New clsJmeno(24, 1, "Milena", "Timotej"))
        Jmena.Add(New clsJmeno(25, 1, "Miloš", "Gejza"))
        Jmena.Add(New clsJmeno(26, 1, "Zora", "Tamara"))
        Jmena.Add(New clsJmeno(27, 1, "Ingrid", "Bohuš"))
        Jmena.Add(New clsJmeno(28, 1, "Otýlie", "Alfonz"))
        Jmena.Add(New clsJmeno(29, 1, "Zdislava", "Gašpar"))
        Jmena.Add(New clsJmeno(30, 1, "Robin", "Ema"))
        Jmena.Add(New clsJmeno(31, 1, "Marika", "Emil"))
        Jmena.Add(New clsJmeno(1, 2, "Hynek", "Tatiana"))
        Jmena.Add(New clsJmeno(2, 2, "Nela", "Erika, Erik"))
        Jmena.Add(New clsJmeno(3, 2, "Blažej", "Blažej"))
        Jmena.Add(New clsJmeno(4, 2, "Jarmila", "Veronika"))
        Jmena.Add(New clsJmeno(5, 2, "Dobromila", "Agáta"))
        Jmena.Add(New clsJmeno(6, 2, "Vanda", "Dorota"))
        Jmena.Add(New clsJmeno(7, 2, "Veronika", "Vanda"))
        Jmena.Add(New clsJmeno(8, 2, "Milada", "Zoja"))
        Jmena.Add(New clsJmeno(9, 2, "Apolena", "Zdenko"))
        Jmena.Add(New clsJmeno(10, 2, "Mojmír", "Gabriela"))
        Jmena.Add(New clsJmeno(11, 2, "Božena", "Dezider"))
        Jmena.Add(New clsJmeno(12, 2, "Slavěna", "Perla"))
        Jmena.Add(New clsJmeno(13, 2, "Věnceslav", "Arpád"))
        Jmena.Add(New clsJmeno(14, 2, "VALENTÝN", "VALENTÝN"))
        Jmena.Add(New clsJmeno(15, 2, "Jiřina", "Pravoslav"))
        Jmena.Add(New clsJmeno(16, 2, "Ljuba", "Ida, Liana"))
        Jmena.Add(New clsJmeno(17, 2, "Miloslava", "Miloslava"))
        Jmena.Add(New clsJmeno(18, 2, "Gizela", "Jaromír"))
        Jmena.Add(New clsJmeno(19, 2, "Patrik", "Vlasta"))
        Jmena.Add(New clsJmeno(20, 2, "Oldřich", "Lívia"))
        Jmena.Add(New clsJmeno(21, 2, "Lenka", "Eleonóra"))
        Jmena.Add(New clsJmeno(22, 2, "Petr", "Etela"))
        Jmena.Add(New clsJmeno(23, 2, "Svatopluk", "Roman"))
        Jmena.Add(New clsJmeno(23, 2, "", "Romana"))
        Jmena.Add(New clsJmeno(24, 2, "Matěj", "Matej"))
        Jmena.Add(New clsJmeno(25, 2, "Liliana", "Frederik"))
        Jmena.Add(New clsJmeno(25, 2, "", "Frederika"))
        Jmena.Add(New clsJmeno(26, 2, "Dorota", "Viktor"))
        Jmena.Add(New clsJmeno(27, 2, "Alexandr", "Alexander"))
        Jmena.Add(New clsJmeno(28, 2, "Lumír", "Zlatica"))
        Jmena.Add(New clsJmeno(29, 2, "Horymír", "Radomír"))
        Jmena.Add(New clsJmeno(1, 3, "Bedřich", "Albín"))
        Jmena.Add(New clsJmeno(2, 3, "Anežka", "Anežka"))
        Jmena.Add(New clsJmeno(3, 3, "Kamil", "Bohumil"))
        Jmena.Add(New clsJmeno(3, 3, "", "Bohumila"))
        Jmena.Add(New clsJmeno(4, 3, "Stela", "Kazimír"))
        Jmena.Add(New clsJmeno(5, 3, "Kazimír", "Fridrich"))
        Jmena.Add(New clsJmeno(6, 3, "Miroslav", "Radoslav"))
        Jmena.Add(New clsJmeno(6, 3, "", "Radoslava"))
        Jmena.Add(New clsJmeno(7, 3, "Tomáš", "Tomáš"))
        Jmena.Add(New clsJmeno(8, 3, "Gabriela", "Alan, Alana"))
        Jmena.Add(New clsJmeno(9, 3, "Františka", "Františka"))
        Jmena.Add(New clsJmeno(9, 3, "Rebeka", ""))
        Jmena.Add(New clsJmeno(10, 3, "Viktorie", "Bruno, Branislav"))
        Jmena.Add(New clsJmeno(11, 3, "Anděla", "Angela"))
        Jmena.Add(New clsJmeno(11, 3, "", "Angelika"))
        Jmena.Add(New clsJmeno(12, 3, "Řehoř", "Gregor"))
        Jmena.Add(New clsJmeno(13, 3, "Růžena", "Vlastimil"))
        Jmena.Add(New clsJmeno(14, 3, "Rút", "Matilda"))
        Jmena.Add(New clsJmeno(14, 3, "Matilda", ""))
        Jmena.Add(New clsJmeno(15, 3, "Ida", "Svetlana"))
        Jmena.Add(New clsJmeno(16, 3, "Elena", "Boleslav"))
        Jmena.Add(New clsJmeno(16, 3, "Ella", ""))
        Jmena.Add(New clsJmeno(16, 3, "Herbert", ""))
        Jmena.Add(New clsJmeno(17, 3, "Vlastimil", "Ľubica"))
        Jmena.Add(New clsJmeno(18, 3, "Eduard", "Eduard"))
        Jmena.Add(New clsJmeno(19, 3, "Josef", "Jozef"))
        Jmena.Add(New clsJmeno(20, 3, "Světlana", "Víťazoslav"))
        Jmena.Add(New clsJmeno(20, 3, "", "Klaudius"))
        Jmena.Add(New clsJmeno(21, 3, "Radek", "Blahoslav"))
        Jmena.Add(New clsJmeno(22, 3, "Leona", "Beňadik"))
        Jmena.Add(New clsJmeno(23, 3, "Ivona", "Adrián"))
        Jmena.Add(New clsJmeno(24, 3, "Gabriel", "Gabriel"))
        Jmena.Add(New clsJmeno(25, 3, "Marián", "Marián"))
        Jmena.Add(New clsJmeno(26, 3, "Emanuel", "Emanuel"))
        Jmena.Add(New clsJmeno(27, 3, "Dita", "Alena"))
        Jmena.Add(New clsJmeno(28, 3, "Soňa", "Soňa"))
        Jmena.Add(New clsJmeno(29, 3, "Taťána", "Miroslav"))
        Jmena.Add(New clsJmeno(30, 3, "Arnošt", "Vieroslava"))
        Jmena.Add(New clsJmeno(31, 3, "Kvído", "Benjamín"))
        Jmena.Add(New clsJmeno(1, 4, "Hugo", "Hugo"))
        Jmena.Add(New clsJmeno(2, 4, "Erika", "Zita"))
        Jmena.Add(New clsJmeno(3, 4, "Richard", "Richard"))
        Jmena.Add(New clsJmeno(4, 4, "Ivana", "Izidor"))
        Jmena.Add(New clsJmeno(5, 4, "Miroslava", "Miroslava"))
        Jmena.Add(New clsJmeno(6, 4, "Vendula", "Irena"))
        Jmena.Add(New clsJmeno(7, 4, "Heřman", "Zoltán"))
        Jmena.Add(New clsJmeno(7, 4, "Hermína", ""))
        Jmena.Add(New clsJmeno(8, 4, "Ema", "Albert"))
        Jmena.Add(New clsJmeno(9, 4, "Dušan", "Milena"))
        Jmena.Add(New clsJmeno(10, 4, "Darja", "Igor"))
        Jmena.Add(New clsJmeno(11, 4, "Izabela", "Július"))
        Jmena.Add(New clsJmeno(12, 4, "Julius", "Estera"))
        Jmena.Add(New clsJmeno(13, 4, "Aleš", "Aleš"))
        Jmena.Add(New clsJmeno(14, 4, "Vincenc", "Justina"))
        Jmena.Add(New clsJmeno(15, 4, "Anastázie", "Fedor"))
        Jmena.Add(New clsJmeno(16, 4, "Irena", "Dana"))
        Jmena.Add(New clsJmeno(16, 4, "Bernadeta", "Danica"))
        Jmena.Add(New clsJmeno(17, 4, "Rudolf", "Rudolf"))
        Jmena.Add(New clsJmeno(18, 4, "Valérie", "Valér"))
        Jmena.Add(New clsJmeno(19, 4, "Rostislav", "Jela"))
        Jmena.Add(New clsJmeno(20, 4, "Marcela", "Marcel"))
        Jmena.Add(New clsJmeno(21, 4, "Alexandra", "Ervín"))
        Jmena.Add(New clsJmeno(22, 4, "Evženie", "Slavomír"))
        Jmena.Add(New clsJmeno(23, 4, "Vojtěch", "Vojtech"))
        Jmena.Add(New clsJmeno(24, 4, "Jiří", "Juraj"))
        Jmena.Add(New clsJmeno(25, 4, "Marek", "Marek"))
        Jmena.Add(New clsJmeno(26, 4, "Oto", "Jaroslava"))
        Jmena.Add(New clsJmeno(27, 4, "Jaroslav", "Jaroslav"))
        Jmena.Add(New clsJmeno(28, 4, "Vlastislav", "Jarmila"))
        Jmena.Add(New clsJmeno(29, 4, "Robert", "Lea"))
        Jmena.Add(New clsJmeno(30, 4, "Blahoslav", "Anastázia"))
        Jmena.Add(New clsJmeno(2, 5, "Zikmund", "Žigmund"))
        Jmena.Add(New clsJmeno(3, 5, "Alexej", "Galina"))
        Jmena.Add(New clsJmeno(4, 5, "Květoslav", "Florián"))
        Jmena.Add(New clsJmeno(5, 5, "Klaudie", "Lesia"))
        Jmena.Add(New clsJmeno(5, 5, "", "Lesana"))
        Jmena.Add(New clsJmeno(6, 5, "Radoslav", "Hermína"))
        Jmena.Add(New clsJmeno(7, 5, "Stanislav", "Monika"))
        Jmena.Add(New clsJmeno(8, 5, "", "Ingrida"))
        Jmena.Add(New clsJmeno(9, 5, "Ctibor", "Roland"))
        Jmena.Add(New clsJmeno(10, 5, "Blažena", "Viktória"))
        Jmena.Add(New clsJmeno(11, 5, "Svatava", "Blažena"))
        Jmena.Add(New clsJmeno(12, 5, "Pankrác", "Pankrác"))
        Jmena.Add(New clsJmeno(13, 5, "Servác", "Servác"))
        Jmena.Add(New clsJmeno(14, 5, "Bonifác", "Bonifác"))
        Jmena.Add(New clsJmeno(15, 5, "Žofie", "Žofia"))
        Jmena.Add(New clsJmeno(15, 5, "", "Sofia"))
        Jmena.Add(New clsJmeno(16, 5, "Přemysl", "Svetozár"))
        Jmena.Add(New clsJmeno(17, 5, "Aneta", "Gizela"))
        Jmena.Add(New clsJmeno(18, 5, "Nataša", "Viola"))
        Jmena.Add(New clsJmeno(19, 5, "Ivo", "Gertrúda"))
        Jmena.Add(New clsJmeno(20, 5, "Zbyšek", "Bernard"))
        Jmena.Add(New clsJmeno(21, 5, "Monika", "Zina"))
        Jmena.Add(New clsJmeno(22, 5, "Emil", "Júlia"))
        Jmena.Add(New clsJmeno(22, 5, "", "Juliana"))
        Jmena.Add(New clsJmeno(23, 5, "Vladimír", "Želmíra"))
        Jmena.Add(New clsJmeno(24, 5, "Jana", "Ela"))
        Jmena.Add(New clsJmeno(24, 5, "Vanesa", ""))
        Jmena.Add(New clsJmeno(25, 5, "Viola", "Urban"))
        Jmena.Add(New clsJmeno(26, 5, "Filip", "Dušan"))
        Jmena.Add(New clsJmeno(27, 5, "Valdemar", "Iveta"))
        Jmena.Add(New clsJmeno(28, 5, "Vilém", "Viliam"))
        Jmena.Add(New clsJmeno(29, 5, "Maxmilián", "Vilma"))
        Jmena.Add(New clsJmeno(30, 5, "Ferdinand", "Ferdinand"))
        Jmena.Add(New clsJmeno(31, 5, "Kamila", "Petrana"))
        Jmena.Add(New clsJmeno(31, 5, "", "Petronela"))
        Jmena.Add(New clsJmeno(1, 6, "Laura", "Žaneta"))
        Jmena.Add(New clsJmeno(2, 6, "Jarmil", "Xénia"))
        Jmena.Add(New clsJmeno(2, 6, "", "Oxana"))
        Jmena.Add(New clsJmeno(3, 6, "Tamara", "Karolína"))
        Jmena.Add(New clsJmeno(4, 6, "Dalibor", "Lenka"))
        Jmena.Add(New clsJmeno(5, 6, "Dobroslav", "Laura"))
        Jmena.Add(New clsJmeno(6, 6, "Norbert", "Norbert"))
        Jmena.Add(New clsJmeno(7, 6, "Iveta", "Róbert"))
        Jmena.Add(New clsJmeno(7, 6, "Slavoj", ""))
        Jmena.Add(New clsJmeno(8, 6, "Medard", "Medard"))
        Jmena.Add(New clsJmeno(9, 6, "Stanislava", "Stanislava"))
        Jmena.Add(New clsJmeno(10, 6, "Gita", "Margaréta"))
        Jmena.Add(New clsJmeno(11, 6, "Bruno", "Dobroslava"))
        Jmena.Add(New clsJmeno(12, 6, "Antonie", "Zlatko"))
        Jmena.Add(New clsJmeno(13, 6, "Antonín", "Anton"))
        Jmena.Add(New clsJmeno(14, 6, "Roland", "Vasil"))
        Jmena.Add(New clsJmeno(15, 6, "Vít", "Vít"))
        Jmena.Add(New clsJmeno(16, 6, "Zbyněk", "Blanka"))
        Jmena.Add(New clsJmeno(16, 6, "", "Bianka"))
        Jmena.Add(New clsJmeno(17, 6, "Adolf", "Adolf"))
        Jmena.Add(New clsJmeno(18, 6, "Milan", "Vratislav"))
        Jmena.Add(New clsJmeno(19, 6, "Leoš", "Alfréd"))
        Jmena.Add(New clsJmeno(20, 6, "Květa", "Valéria"))
        Jmena.Add(New clsJmeno(21, 6, "Alois", "Alojz"))
        Jmena.Add(New clsJmeno(22, 6, "Pavla", "Paulína"))
        Jmena.Add(New clsJmeno(23, 6, "Zdeňka", "Sidónia"))
        Jmena.Add(New clsJmeno(24, 6, "Jan", "Ján"))
        Jmena.Add(New clsJmeno(25, 6, "Ivan", "Olívia"))
        Jmena.Add(New clsJmeno(25, 6, "", "Tadeáš"))
        Jmena.Add(New clsJmeno(26, 6, "Adriana", "Adriána"))
        Jmena.Add(New clsJmeno(27, 6, "Ladislav", "Ladislav"))
        Jmena.Add(New clsJmeno(27, 6, "", "Ladislava"))
        Jmena.Add(New clsJmeno(28, 6, "Lubomír", "Beáta"))
        Jmena.Add(New clsJmeno(29, 6, "Petr", "Peter"))
        Jmena.Add(New clsJmeno(29, 6, "Pavel", "Pavol"))
        Jmena.Add(New clsJmeno(29, 6, "", "Petra"))
        Jmena.Add(New clsJmeno(30, 6, "Šárka", "Melánia"))
        Jmena.Add(New clsJmeno(1, 7, "Jaroslava", "Diana"))
        Jmena.Add(New clsJmeno(2, 7, "Patrície", "Berta"))
        Jmena.Add(New clsJmeno(3, 7, "Radomír", "Miloslav"))
        Jmena.Add(New clsJmeno(4, 7, "Prokop", "Prokop"))
        Jmena.Add(New clsJmeno(5, 7, "Cyril", "Cyril"))
        Jmena.Add(New clsJmeno(5, 7, "Metoděj", "Metod"))
        Jmena.Add(New clsJmeno(6, 7, "", "Patrik"))
        Jmena.Add(New clsJmeno(6, 7, "", "Patrícia"))
        Jmena.Add(New clsJmeno(7, 7, "Bohuslava", "Oliver"))
        Jmena.Add(New clsJmeno(8, 7, "Nora", "Ivan"))
        Jmena.Add(New clsJmeno(9, 7, "Drahoslava", "Lujza"))
        Jmena.Add(New clsJmeno(10, 7, "Libuše", ""))
        Jmena.Add(New clsJmeno(10, 7, "Amálie", "Amália"))
        Jmena.Add(New clsJmeno(11, 7, "Olga", "Milota"))
        Jmena.Add(New clsJmeno(12, 7, "Bořek", "Nina"))
        Jmena.Add(New clsJmeno(13, 7, "Markéta", "Margita"))
        Jmena.Add(New clsJmeno(14, 7, "Karolína", "Kamil"))
        Jmena.Add(New clsJmeno(15, 7, "Jindřich", "Henrich"))
        Jmena.Add(New clsJmeno(16, 7, "Luboš", "Drahomír"))
        Jmena.Add(New clsJmeno(17, 7, "Martina", "Bohuslav"))
        Jmena.Add(New clsJmeno(18, 7, "Drahomíra", "Kamila"))
        Jmena.Add(New clsJmeno(19, 7, "Čeněk", "Dušana"))
        Jmena.Add(New clsJmeno(20, 7, "Ilja", "Iľja, Eliáš"))
        Jmena.Add(New clsJmeno(21, 7, "Vítězslav", "Daniel"))
        Jmena.Add(New clsJmeno(22, 7, "Magdaléna", "Magdaléna"))
        Jmena.Add(New clsJmeno(23, 7, "Libor", "Oľga"))
        Jmena.Add(New clsJmeno(24, 7, "Kristýna", "Vladimír"))
        Jmena.Add(New clsJmeno(25, 7, "Jakub", "Jakub"))
        Jmena.Add(New clsJmeno(26, 7, "Anna", "Anna, Hana"))
        Jmena.Add(New clsJmeno(27, 7, "Věroslav", "Božena"))
        Jmena.Add(New clsJmeno(28, 7, "Viktor", "Krištof"))
        Jmena.Add(New clsJmeno(29, 7, "Marta", "Marta"))
        Jmena.Add(New clsJmeno(30, 7, "Bořivoj", "Libuša"))
        Jmena.Add(New clsJmeno(31, 7, "Ignác", "Ignác"))
        Jmena.Add(New clsJmeno(1, 8, "Oskar", "Božidara"))
        Jmena.Add(New clsJmeno(2, 8, "Gustav", "Gustáv"))
        Jmena.Add(New clsJmeno(3, 8, "Miluše", "Jerguš"))
        Jmena.Add(New clsJmeno(4, 8, "Dominik", "Dominika"))
        Jmena.Add(New clsJmeno(4, 8, "", "Dominik"))
        Jmena.Add(New clsJmeno(5, 8, "Kristián", "Hortenzia"))
        Jmena.Add(New clsJmeno(6, 8, "Oldřiška", "Jozefína"))
        Jmena.Add(New clsJmeno(7, 8, "Lada", "Štefánia"))
        Jmena.Add(New clsJmeno(8, 8, "Soběslav", "Oskar"))
        Jmena.Add(New clsJmeno(9, 8, "Roman", "Ľubomíra"))
        Jmena.Add(New clsJmeno(10, 8, "Vavřinec", "Vavrinec"))
        Jmena.Add(New clsJmeno(11, 8, "Zuzana", "Zuzana"))
        Jmena.Add(New clsJmeno(12, 8, "Klára", "Darina"))
        Jmena.Add(New clsJmeno(13, 8, "Alena", "Ľubomír"))
        Jmena.Add(New clsJmeno(14, 8, "Alan", "Mojmír"))
        Jmena.Add(New clsJmeno(15, 8, "Hana", "Marcela"))
        Jmena.Add(New clsJmeno(16, 8, "Jáchym", "Leonard"))
        Jmena.Add(New clsJmeno(17, 8, "Petra", "Milica"))
        Jmena.Add(New clsJmeno(18, 8, "", "Elena"))
        Jmena.Add(New clsJmeno(18, 8, "Helena", "Helena"))
        Jmena.Add(New clsJmeno(19, 8, "Ludvík", "Lýdia"))
        Jmena.Add(New clsJmeno(20, 8, "Bernard", "Anabela"))
        Jmena.Add(New clsJmeno(21, 8, "Johana", "Jana"))
        Jmena.Add(New clsJmeno(22, 8, "Bohuslav", "Tichomír"))
        Jmena.Add(New clsJmeno(23, 8, "Sandra", "Filip"))
        Jmena.Add(New clsJmeno(24, 8, "Bartoloměj", "Bartolomej"))
        Jmena.Add(New clsJmeno(25, 8, "Radim", "Ľudovít"))
        Jmena.Add(New clsJmeno(26, 8, "Luděk", "Samuel"))
        Jmena.Add(New clsJmeno(27, 8, "Otakar", "Silvia"))
        Jmena.Add(New clsJmeno(28, 8, "Augustýn", "Augustín"))
        Jmena.Add(New clsJmeno(29, 8, "Evelína", "Nikola"))
        Jmena.Add(New clsJmeno(29, 8, "", "Nikolaj"))
        Jmena.Add(New clsJmeno(30, 8, "Vladěna", "Ružena"))
        Jmena.Add(New clsJmeno(31, 8, "Pavlína", "Nora"))
        Jmena.Add(New clsJmeno(1, 9, "Linda", "Drahoslava"))
        Jmena.Add(New clsJmeno(1, 9, "Samuel", ""))
        Jmena.Add(New clsJmeno(2, 9, "Adéla", "Linda"))
        Jmena.Add(New clsJmeno(2, 9, "", "Rebeka"))
        Jmena.Add(New clsJmeno(3, 9, "Bronislav", "Belo"))
        Jmena.Add(New clsJmeno(4, 9, "Jindřiška", "Rozália"))
        Jmena.Add(New clsJmeno(5, 9, "Boris", "Regina"))
        Jmena.Add(New clsJmeno(6, 9, "Boleslav", "Alica"))
        Jmena.Add(New clsJmeno(7, 9, "Regina", "Marianna"))
        Jmena.Add(New clsJmeno(8, 9, "Mariana", "Miriama"))
        Jmena.Add(New clsJmeno(9, 9, "Daniela", "Martina"))
        Jmena.Add(New clsJmeno(10, 9, "Irma", "Oleg"))
        Jmena.Add(New clsJmeno(11, 9, "Denisa", "Bystrík"))
        Jmena.Add(New clsJmeno(12, 9, "Marie", "Mária"))
        Jmena.Add(New clsJmeno(13, 9, "Lubor", "Ctibor"))
        Jmena.Add(New clsJmeno(14, 9, "Radka", "Ľudomil"))
        Jmena.Add(New clsJmeno(15, 9, "Jolana", "Jolana"))
        Jmena.Add(New clsJmeno(16, 9, "Ludmila", "Ľudmila"))
        Jmena.Add(New clsJmeno(17, 9, "Naděžda", "Olympia"))
        Jmena.Add(New clsJmeno(18, 9, "Kryštof", "Eugénia"))
        Jmena.Add(New clsJmeno(19, 9, "Zita", "Konštantín"))
        Jmena.Add(New clsJmeno(20, 9, "Oleg", "Ľuboslav"))
        Jmena.Add(New clsJmeno(20, 9, "", "Ľuboslava"))
        Jmena.Add(New clsJmeno(21, 9, "Matouš", "Matúš"))
        Jmena.Add(New clsJmeno(22, 9, "Darina", "Móric"))
        Jmena.Add(New clsJmeno(23, 9, "Berta", "Zdenka"))
        Jmena.Add(New clsJmeno(24, 9, "Jaromír", "Ľuboš"))
        Jmena.Add(New clsJmeno(24, 9, "", "Ľubor"))
        Jmena.Add(New clsJmeno(25, 9, "Zlata", "Vladislav"))
        Jmena.Add(New clsJmeno(26, 9, "Andrea", "Edita"))
        Jmena.Add(New clsJmeno(27, 9, "Jonáš", "Cyprián"))
        Jmena.Add(New clsJmeno(28, 9, "Václav", "Václav"))
        Jmena.Add(New clsJmeno(29, 9, "Michal", "Michal"))
        Jmena.Add(New clsJmeno(29, 9, "Michael", "Michaela"))
        Jmena.Add(New clsJmeno(30, 9, "Jeroným", "Jarolím"))
        Jmena.Add(New clsJmeno(1, 10, "Igor", "Arnold"))
        Jmena.Add(New clsJmeno(2, 10, "Olívie", "Levoslav"))
        Jmena.Add(New clsJmeno(2, 10, "Oliver", ""))
        Jmena.Add(New clsJmeno(3, 10, "Bohumil", "Stela"))
        Jmena.Add(New clsJmeno(4, 10, "František", "František"))
        Jmena.Add(New clsJmeno(5, 10, "Eliška", "Viera"))
        Jmena.Add(New clsJmeno(6, 10, "Hanuš", "Natália"))
        Jmena.Add(New clsJmeno(7, 10, "Justýna", "Eliška"))
        Jmena.Add(New clsJmeno(8, 10, "Věra", "Brigita"))
        Jmena.Add(New clsJmeno(9, 10, "Štefan", "Dionýz"))
        Jmena.Add(New clsJmeno(9, 10, "Sára", ""))
        Jmena.Add(New clsJmeno(10, 10, "Marina", "Slavomíra"))
        Jmena.Add(New clsJmeno(11, 10, "Andrej", "Valentína"))
        Jmena.Add(New clsJmeno(12, 10, "Marcel", "Maximilián"))
        Jmena.Add(New clsJmeno(13, 10, "Renáta", "Koloman"))
        Jmena.Add(New clsJmeno(14, 10, "Agáta", "Boris"))
        Jmena.Add(New clsJmeno(15, 10, "Tereza", "Terézia"))
        Jmena.Add(New clsJmeno(16, 10, "Havel", "Vladimíra"))
        Jmena.Add(New clsJmeno(17, 10, "Hedvika", "Hedviga"))
        Jmena.Add(New clsJmeno(18, 10, "Lukáš", "Lukáš"))
        Jmena.Add(New clsJmeno(19, 10, "Michala", "Kristián"))
        Jmena.Add(New clsJmeno(19, 10, "Michaela", ""))
        Jmena.Add(New clsJmeno(20, 10, "Vendelín", "Vendelín"))
        Jmena.Add(New clsJmeno(21, 10, "Brigita", "Uršuľa"))
        Jmena.Add(New clsJmeno(22, 10, "Sabina", "Sergej"))
        Jmena.Add(New clsJmeno(23, 10, "Teodor", "Alojzia"))
        Jmena.Add(New clsJmeno(24, 10, "Nina", "Kvetoslava"))
        Jmena.Add(New clsJmeno(25, 10, "Beáta", "Aurel"))
        Jmena.Add(New clsJmeno(26, 10, "Erik", "Demeter"))
        Jmena.Add(New clsJmeno(27, 10, "Šarlota", "Sabína"))
        Jmena.Add(New clsJmeno(27, 10, "Zoe", ""))
        Jmena.Add(New clsJmeno(28, 10, "Jidáš", "Dobromila"))
        Jmena.Add(New clsJmeno(29, 10, "Silvie", "Klára"))
        Jmena.Add(New clsJmeno(29, 10, "Sylva", ""))
        Jmena.Add(New clsJmeno(30, 10, "Tadeáš", "Šimon"))
        Jmena.Add(New clsJmeno(30, 10, "", "Simona"))
        Jmena.Add(New clsJmeno(31, 10, "Štěpánka", "Aurélia"))
        Jmena.Add(New clsJmeno(1, 11, "Felix", "Denis"))
        Jmena.Add(New clsJmeno(1, 11, "", "Denisa"))
        Jmena.Add(New clsJmeno(3, 11, "Hubert", "Hubert"))
        Jmena.Add(New clsJmeno(4, 11, "Karel", "Karol"))
        Jmena.Add(New clsJmeno(4, 11, "Karla", "Karol"))
        Jmena.Add(New clsJmeno(5, 11, "Miriam", "Imrich"))
        Jmena.Add(New clsJmeno(6, 11, "Liběna", "Renáta"))
        Jmena.Add(New clsJmeno(7, 11, "Saskie", "René"))
        Jmena.Add(New clsJmeno(8, 11, "Bohumír", "Bohumír"))
        Jmena.Add(New clsJmeno(9, 11, "Bohdan", "Teodor"))
        Jmena.Add(New clsJmeno(10, 11, "Evžen", "Tibor"))
        Jmena.Add(New clsJmeno(11, 11, "Martin", "Martin"))
        Jmena.Add(New clsJmeno(11, 11, "", "Maroš"))
        Jmena.Add(New clsJmeno(12, 11, "Benedikt", "Svätopluk"))
        Jmena.Add(New clsJmeno(13, 11, "Tibor", "Stanislav"))
        Jmena.Add(New clsJmeno(14, 11, "Sáva", "Irma"))
        Jmena.Add(New clsJmeno(15, 11, "Leopold", "Leopold"))
        Jmena.Add(New clsJmeno(16, 11, "Otmar", "Agnesa"))
        Jmena.Add(New clsJmeno(17, 11, "Mahulena", "Klaudia"))
        Jmena.Add(New clsJmeno(18, 11, "Romana", "Eugen"))
        Jmena.Add(New clsJmeno(19, 11, "Alžběta", "Alžbeta"))
        Jmena.Add(New clsJmeno(20, 11, "Nikola", "Félix"))
        Jmena.Add(New clsJmeno(21, 11, "Albert", "Elvíra"))
        Jmena.Add(New clsJmeno(22, 11, "Cecílie", "Cecília"))
        Jmena.Add(New clsJmeno(23, 11, "Klement", "Klement"))
        Jmena.Add(New clsJmeno(24, 11, "Emílie", "Emília"))
        Jmena.Add(New clsJmeno(25, 11, "Kateřina", "Katarína"))
        Jmena.Add(New clsJmeno(26, 11, "Artur", "Kornel"))
        Jmena.Add(New clsJmeno(27, 11, "Xenie", "Milan"))
        Jmena.Add(New clsJmeno(28, 11, "René", "Henrieta"))
        Jmena.Add(New clsJmeno(29, 11, "Zina", "Vratko"))
        Jmena.Add(New clsJmeno(30, 11, "Ondřej", "Ondrej"))
        Jmena.Add(New clsJmeno(30, 11, "", "Andrej"))
        Jmena.Add(New clsJmeno(1, 12, "Iva", "Edmund"))
        Jmena.Add(New clsJmeno(2, 12, "Blanka", "Bibiána"))
        Jmena.Add(New clsJmeno(3, 12, "Svatoslav", "Oldrich"))
        Jmena.Add(New clsJmeno(4, 12, "Barbora", "Barbora"))
        Jmena.Add(New clsJmeno(4, 12, "", "Barbara"))
        Jmena.Add(New clsJmeno(5, 12, "Jitka", "Oto"))
        Jmena.Add(New clsJmeno(6, 12, "Mikuláš", "Mikuláš"))
        Jmena.Add(New clsJmeno(7, 12, "Benjamín", "Ambróz"))
        Jmena.Add(New clsJmeno(8, 12, "Květoslava", "Marína"))
        Jmena.Add(New clsJmeno(9, 12, "Vratislav", "Izabela"))
        Jmena.Add(New clsJmeno(10, 12, "Julie", "Radúz"))
        Jmena.Add(New clsJmeno(11, 12, "Dana", "Hilda"))
        Jmena.Add(New clsJmeno(12, 12, "Simona", "Otília"))
        Jmena.Add(New clsJmeno(13, 12, "Lucie", "Lucia"))
        Jmena.Add(New clsJmeno(14, 12, "Lýdie", "Branislava"))
        Jmena.Add(New clsJmeno(14, 12, "", "Bronislava"))
        Jmena.Add(New clsJmeno(15, 12, "Radana", "Ivica"))
        Jmena.Add(New clsJmeno(16, 12, "Albína", "Albína"))
        Jmena.Add(New clsJmeno(17, 12, "Daniel", "Kornélia"))
        Jmena.Add(New clsJmeno(18, 12, "Miloslav", "Sláva"))
        Jmena.Add(New clsJmeno(19, 12, "Ester", "Judita"))
        Jmena.Add(New clsJmeno(20, 12, "Dagmar", "Dagmara"))
        Jmena.Add(New clsJmeno(21, 12, "Natálie", "Bohdan"))
        Jmena.Add(New clsJmeno(22, 12, "Šimon", "Adela"))
        Jmena.Add(New clsJmeno(23, 12, "Vlasta", "Nadežda"))
        Jmena.Add(New clsJmeno(24, 12, "Adam", "Adam"))
        Jmena.Add(New clsJmeno(24, 12, "Eva", "Eva"))
        Jmena.Add(New clsJmeno(26, 12, "Štěpán", "Štefan"))
        Jmena.Add(New clsJmeno(27, 12, "Žaneta", "Filoména"))
        Jmena.Add(New clsJmeno(28, 12, "Bohumila", "Ivana"))
        Jmena.Add(New clsJmeno(28, 12, "", "Ivona"))
        Jmena.Add(New clsJmeno(29, 12, "Judita", "Milada"))
        Jmena.Add(New clsJmeno(30, 12, "David", "Dávid"))
        Jmena.Add(New clsJmeno(31, 12, "Silvestr", "Silvester"))

        Svatky.Add(New clsSvatek(0, 0, "Den matek", "Deň matiek"))
        Svatky.Add(New clsSvatek(0, 1, "Velikonoční pondělí", "Veľkonočný pondelok", True))
        Svatky.Add(New clsSvatek(0, 2, "Velikonoční neděle", "Boží hod velikonoční"))
        Svatky.Add(New clsSvatek(0, 3, "První jarní den", "Prvý jarný deň"))
        Svatky.Add(New clsSvatek(0, 6, "První letní den", "Prvý letný deň"))
        Svatky.Add(New clsSvatek(0, 9, "První podzimní den", "Prvý jesenný deň, False"))
        Svatky.Add(New clsSvatek(0, 12, "První zimní den", "Prvý zimný deň, False"))
        Svatky.Add(New clsSvatek(0, 13, "Velký pátek", "Veľký piatok"))
        Svatky.Add(New clsSvatek(1, 1, "Nový rok, Den obnovy samostatného českého státu 1993", "Deň vzniku Slovenskej republiky 1993", True))
        Svatky.Add(New clsSvatek(6, 1, "Tři králové (křesťanský svátek)", "Traja Králi (památný den)"))
        Svatky.Add(New clsSvatek(1, 5, "Svátek práce", "Sviatok práce"))
        Svatky.Add(New clsSvatek(12, 3, "Den vstupu ČR do NATO 1999 (významný den)", ""))
        Svatky.Add(New clsSvatek(28, 10, "Den vzniku samostatného Československa 1918", "Deň vzniku samostatného Československa 1918 (památný den)", True))
        Svatky.Add(New clsSvatek(8, 5, "Den vítězství, konec druhé světové války 1945", "Deň víťazstva nad fašizmom 1945", True))
        Svatky.Add(New clsSvatek(5, 5, "Květnové povstání českého lidu 1945 (významný den)", ""))
        Svatky.Add(New clsSvatek(1, 6, "Mezinárodní den dětí 1925 (významný den)", "Medzinárodný deň detí 1925 (památný den)"))
        Svatky.Add(New clsSvatek(17, 11, "Den boje za svobodu a demokracii 1989", "Deň boja za slobodu a demokraciu 1989"))
        Svatky.Add(New clsSvatek(24, 12, "Štědrý den", "Štedrý deň", True))
        Svatky.Add(New clsSvatek(25, 12, "1.svátek vánoční", "Prvý sviatok vianočný", True))
        Svatky.Add(New clsSvatek(26, 12, "2.svátek vánoční", "Druhý sviatok vianočný", True))
        Svatky.Add(New clsSvatek(5, 7, "Den slovanských věrozvěstů Cyrila a Metoděje 863", "Sviatok svätého Cyrila a svätého Metoda 863"))
        Svatky.Add(New clsSvatek(6, 7, "Den upálení mistra Jana Husa 1415", ""))
        Svatky.Add(New clsSvatek(28, 9, "Den české státnosti 929/935", "", True))
        Svatky.Add(New clsSvatek(29, 8, "", "Výročie Slovenského národného povstania"))
        Svatky.Add(New clsSvatek(1, 9, "", "Deň Ústavy Slovenskej republiky"))
        Svatky.Add(New clsSvatek(15, 9, "", "Sedembolestná Panna Mária"))
        Svatky.Add(New clsSvatek(1, 11, "", "Sviatok Všetkých svätých"))
        Svatky.Add(New clsSvatek(2, 11, "Památka zesnulých (křesťanský svátek)", ""))
        Svatky.Add(New clsSvatek(25, 3, "", "Deň zápasu za ľudské práva (památný den)"))
        Svatky.Add(New clsSvatek(13, 4, "", "Deň nespravodlivo stíhaných (památný den)"))
        Svatky.Add(New clsSvatek(7, 6, "", "Výročie Memoranda národa slovenského (památný den)"))
        Svatky.Add(New clsSvatek(5, 7, "", "Deň zahraničných Slovákov (památný den)"))
        Svatky.Add(New clsSvatek(17, 7, "", "Výročie Deklarácie o zvrchovanosti Slovenskej republiky (památný den)"))
        Svatky.Add(New clsSvatek(4, 8, "", "Deň Matice slovenskej"))
        Svatky.Add(New clsSvatek(9, 9, "", "Deň obetí holokaustu a rasového násilia (památný den)"))
        Svatky.Add(New clsSvatek(19, 9, "", "Deň vzniku Slovenskej národnej rady (památný den)"))
        Svatky.Add(New clsSvatek(6, 10, "", "Deň obetí Dukly (památný den)"))
        Svatky.Add(New clsSvatek(27, 10, "", "Deň černovskej tragédie (památný den)"))
        Svatky.Add(New clsSvatek(29, 10, "", "Výročie narodenia Ľudovíta Štúra (památný den)"))
        Svatky.Add(New clsSvatek(30, 10, "", "Výročie Deklarácie slovenského národa (památný den)"))
        Svatky.Add(New clsSvatek(31, 10, "", "Deň reformácie (památný den)"))
        Svatky.Add(New clsSvatek(30, 12, "", "Vyhlásenia Slovenska za samostatnú cirkevnú provinciu (památný den)"))
        Svatky.Add(New clsSvatek(8, 3, "Mezinárodní den žen (významný den)", ""))
        Svatky.Add(New clsSvatek(27, 1, "Den památky obětí holocaustu 1945 (významný den)", ""))
        Svatky.Add(New clsSvatek(28, 3, "Den narození Jana Ámose Komenského 1592 (významný den)", ""))
        Svatky.Add(New clsSvatek(7, 4, "Den vzdělanosti, založení Univerzity Karlovy 1348 (významný den)", ""))
        Svatky.Add(New clsSvatek(15, 5, "Den rodin 1993 (významný den)", "Deň rodin 1993 (památný den)"))
        Svatky.Add(New clsSvatek(10, 6, "Vyhlazení obce Lidice 1942 (významný den)", ""))
        Svatky.Add(New clsSvatek(27, 6, "Den památky obětí komunistického režimu 1950 (významný den)", ""))
        Svatky.Add(New clsSvatek(11, 11, "Den válečných veteránů 1918 (významný den)", ""))


        Sezona.Add(New Date(2000, 3, 20))
        Sezona.Add(New Date(2001, 3, 20))
        Sezona.Add(New Date(2002, 3, 20))
        Sezona.Add(New Date(2003, 3, 21))
        Sezona.Add(New Date(2004, 3, 20))
        Sezona.Add(New Date(2005, 3, 20))
        Sezona.Add(New Date(2006, 3, 20))
        Sezona.Add(New Date(2007, 3, 21))
        Sezona.Add(New Date(2008, 3, 20))
        Sezona.Add(New Date(2009, 3, 20))
        Sezona.Add(New Date(2010, 3, 20))
        Sezona.Add(New Date(2011, 3, 21))
        Sezona.Add(New Date(2012, 3, 20))
        Sezona.Add(New Date(2013, 3, 20))
        Sezona.Add(New Date(2014, 3, 20))
        Sezona.Add(New Date(2015, 3, 20))
        Sezona.Add(New Date(2016, 3, 20))
        Sezona.Add(New Date(2017, 3, 20))
        Sezona.Add(New Date(2018, 3, 20))
        Sezona.Add(New Date(2019, 3, 20))
        Sezona.Add(New Date(2020, 3, 20))
        Sezona.Add(New Date(2021, 3, 21))
        Sezona.Add(New Date(2022, 3, 20))
        Sezona.Add(New Date(2023, 3, 20))
        Sezona.Add(New Date(2024, 3, 20))
        Sezona.Add(New Date(2025, 3, 20))
        Sezona.Add(New Date(2026, 3, 20))
        Sezona.Add(New Date(2027, 3, 20))
        Sezona.Add(New Date(2028, 3, 20))
        Sezona.Add(New Date(2029, 3, 20))
        Sezona.Add(New Date(2030, 3, 20))
        Sezona.Add(New Date(2031, 3, 20))
        Sezona.Add(New Date(2032, 3, 20))
        Sezona.Add(New Date(2033, 3, 20))
        Sezona.Add(New Date(2034, 3, 20))
        Sezona.Add(New Date(2035, 3, 20))
        Sezona.Add(New Date(2036, 3, 20))
        Sezona.Add(New Date(2037, 3, 20))
        Sezona.Add(New Date(2038, 3, 20))
        Sezona.Add(New Date(2039, 3, 20))
        Sezona.Add(New Date(2040, 3, 20))
        Sezona.Add(New Date(2041, 3, 20))
        Sezona.Add(New Date(2000, 6, 21))
        Sezona.Add(New Date(2001, 6, 21))
        Sezona.Add(New Date(2002, 6, 21))
        Sezona.Add(New Date(2003, 6, 21))
        Sezona.Add(New Date(2004, 6, 21))
        Sezona.Add(New Date(2005, 6, 21))
        Sezona.Add(New Date(2006, 6, 21))
        Sezona.Add(New Date(2007, 6, 21))
        Sezona.Add(New Date(2008, 6, 21))
        Sezona.Add(New Date(2009, 6, 21))
        Sezona.Add(New Date(2010, 6, 21))
        Sezona.Add(New Date(2011, 6, 21))
        Sezona.Add(New Date(2012, 6, 21))
        Sezona.Add(New Date(2013, 6, 21))
        Sezona.Add(New Date(2014, 6, 21))
        Sezona.Add(New Date(2015, 6, 21))
        Sezona.Add(New Date(2016, 6, 21))
        Sezona.Add(New Date(2017, 6, 21))
        Sezona.Add(New Date(2018, 6, 21))
        Sezona.Add(New Date(2019, 6, 21))
        Sezona.Add(New Date(2020, 6, 21))
        Sezona.Add(New Date(2021, 6, 21))
        Sezona.Add(New Date(2022, 6, 21))
        Sezona.Add(New Date(2023, 6, 21))
        Sezona.Add(New Date(2024, 6, 20))
        Sezona.Add(New Date(2025, 6, 21))
        Sezona.Add(New Date(2026, 6, 21))
        Sezona.Add(New Date(2027, 6, 21))
        Sezona.Add(New Date(2028, 6, 20))
        Sezona.Add(New Date(2029, 6, 21))
        Sezona.Add(New Date(2030, 6, 21))
        Sezona.Add(New Date(2031, 6, 21))
        Sezona.Add(New Date(2032, 6, 20))
        Sezona.Add(New Date(2033, 6, 21))
        Sezona.Add(New Date(2034, 6, 21))
        Sezona.Add(New Date(2035, 6, 21))
        Sezona.Add(New Date(2036, 6, 20))
        Sezona.Add(New Date(2037, 6, 21))
        Sezona.Add(New Date(2038, 6, 21))
        Sezona.Add(New Date(2039, 6, 21))
        Sezona.Add(New Date(2040, 6, 20))
        Sezona.Add(New Date(2041, 6, 21))
        Sezona.Add(New Date(2000, 9, 22))
        Sezona.Add(New Date(2001, 9, 23))
        Sezona.Add(New Date(2002, 9, 23))
        Sezona.Add(New Date(2003, 9, 23))
        Sezona.Add(New Date(2004, 9, 22))
        Sezona.Add(New Date(2005, 9, 23))
        Sezona.Add(New Date(2006, 9, 23))
        Sezona.Add(New Date(2007, 9, 23))
        Sezona.Add(New Date(2008, 9, 22))
        Sezona.Add(New Date(2009, 9, 22))
        Sezona.Add(New Date(2010, 9, 23))
        Sezona.Add(New Date(2011, 9, 23))
        Sezona.Add(New Date(2012, 9, 22))
        Sezona.Add(New Date(2013, 9, 22))
        Sezona.Add(New Date(2014, 9, 23))
        Sezona.Add(New Date(2015, 9, 23))
        Sezona.Add(New Date(2016, 9, 22))
        Sezona.Add(New Date(2017, 9, 22))
        Sezona.Add(New Date(2018, 9, 23))
        Sezona.Add(New Date(2019, 9, 23))
        Sezona.Add(New Date(2020, 9, 22))
        Sezona.Add(New Date(2021, 9, 22))
        Sezona.Add(New Date(2022, 9, 23))
        Sezona.Add(New Date(2023, 9, 23))
        Sezona.Add(New Date(2024, 9, 22))
        Sezona.Add(New Date(2025, 9, 22))
        Sezona.Add(New Date(2026, 9, 23))
        Sezona.Add(New Date(2027, 9, 23))
        Sezona.Add(New Date(2028, 9, 22))
        Sezona.Add(New Date(2029, 9, 22))
        Sezona.Add(New Date(2030, 9, 23))
        Sezona.Add(New Date(2031, 9, 23))
        Sezona.Add(New Date(2032, 9, 22))
        Sezona.Add(New Date(2033, 9, 22))
        Sezona.Add(New Date(2034, 9, 23))
        Sezona.Add(New Date(2035, 9, 23))
        Sezona.Add(New Date(2036, 9, 22))
        Sezona.Add(New Date(2037, 9, 22))
        Sezona.Add(New Date(2038, 9, 22))
        Sezona.Add(New Date(2039, 9, 23))
        Sezona.Add(New Date(2040, 9, 22))
        Sezona.Add(New Date(2041, 9, 22))
        Sezona.Add(New Date(2000, 12, 21))
        Sezona.Add(New Date(2001, 12, 21))
        Sezona.Add(New Date(2002, 12, 22))
        Sezona.Add(New Date(2003, 12, 22))
        Sezona.Add(New Date(2004, 12, 21))
        Sezona.Add(New Date(2005, 12, 21))
        Sezona.Add(New Date(2006, 12, 22))
        Sezona.Add(New Date(2007, 12, 22))
        Sezona.Add(New Date(2008, 12, 21))
        Sezona.Add(New Date(2009, 12, 21))
        Sezona.Add(New Date(2010, 12, 22))
        Sezona.Add(New Date(2011, 12, 22))
        Sezona.Add(New Date(2012, 12, 21))
        Sezona.Add(New Date(2013, 12, 21))
        Sezona.Add(New Date(2014, 12, 22))
        Sezona.Add(New Date(2015, 12, 22))
        Sezona.Add(New Date(2016, 12, 21))
        Sezona.Add(New Date(2017, 12, 21))
        Sezona.Add(New Date(2018, 12, 21))
        Sezona.Add(New Date(2019, 12, 22))
        Sezona.Add(New Date(2020, 12, 21))
        Sezona.Add(New Date(2021, 12, 21))
        Sezona.Add(New Date(2022, 12, 21))
        Sezona.Add(New Date(2023, 12, 22))
        Sezona.Add(New Date(2024, 12, 21))
        Sezona.Add(New Date(2025, 12, 21))
        Sezona.Add(New Date(2026, 12, 21))
        Sezona.Add(New Date(2027, 12, 22))
        Sezona.Add(New Date(2028, 12, 21))
        Sezona.Add(New Date(2029, 12, 21))
        Sezona.Add(New Date(2030, 12, 21))
        Sezona.Add(New Date(2031, 12, 22))
        Sezona.Add(New Date(2032, 12, 21))
        Sezona.Add(New Date(2033, 12, 21))
        Sezona.Add(New Date(2034, 12, 21))
        Sezona.Add(New Date(2035, 12, 22))
        Sezona.Add(New Date(2036, 12, 21))
        Sezona.Add(New Date(2037, 12, 21))
        Sezona.Add(New Date(2038, 12, 21))
        Sezona.Add(New Date(2039, 12, 22))
        Sezona.Add(New Date(2040, 12, 21))
        Sezona.Add(New Date(2041, 12, 21))
    End Sub
#End Region

#Region " Get Den "

    Public Function GetDenJmeno(Den As Integer) As String
        Dim F As clsDen = Dny.First(Function(x) x.Den = Den)
        Return F.GetType.GetProperty(Jazyk).GetValue(F, Nothing).ToString
    End Function

    Public Function GetDenJmeno(Datum As Date) As String
        Return GetDenJmeno(CInt(Datum.DayOfWeek))
    End Function

#End Region

#Region " Get Mesic "

    Public Function PrestupnyRok(Datum As Date) As Boolean
        Return PrestupnyRok(Datum.Year)
    End Function

    Public Function PrestupnyRok(Rok As Integer) As Boolean
        Dim Prestupny As Boolean
        Dim Datum As Date
        Prestupny = Date.TryParse("29.2." & Rok.ToString, Globalization.CultureInfo.GetCultureInfo("cs-CZ"), Globalization.DateTimeStyles.None, Datum)
        'Únor
        Mesice(1).Dnu = If(Prestupny, 29, 28)
        Return Prestupny
    End Function

    Public Function GetMesic(Mesic As Integer) As String
        Dim F As clsMesic = Mesice.First(Function(x) x.Mesic = Mesic)
        Return F.GetType.GetProperty(Jazyk).GetValue(F, Nothing).ToString
    End Function

    Public Function GetMesicMaDnu(Mesic As Integer) As Integer
        Dim F As clsMesic = Mesice.First(Function(x) x.Mesic = Mesic)
        Return F.Dnu
    End Function

    Public Function GetMesic(Datum As Date) As String
        Return GetMesic(Datum.Month)
    End Function

#End Region

#Region " Get Jmeno "

    Public Function IsJmeno(Jmeno As String) As Boolean
        Dim F As clsJmeno
        If Jazyk = "CZ" Then
            F = Jmena.FirstOrDefault(Function(x) x.CZ.ToLower = Jmeno.ToLower)
        Else
            F = Jmena.FirstOrDefault(Function(x) x.SK.ToLower = Jmeno.ToLower)
        End If
        Return If(F Is Nothing, False, True)
    End Function

    Public Function GetDate(Jmeno As String, Rok As Integer) As Date
        Dim F As clsJmeno
        If Jazyk = "CZ" Then
            F = Jmena.FirstOrDefault(Function(x) x.CZ = Jmeno)
        Else
            F = Jmena.FirstOrDefault(Function(x) x.SK = Jmeno)
        End If
        If F Is Nothing Then
            Return Nothing
        Else
            Return New Date(Rok, F.Mesic, F.Den)
        End If
    End Function

    Public Function GetJmeno(Den As Integer, Mesic As Integer) As String
        Dim F As clsJmeno = Jmena.FirstOrDefault(Function(x) x.Den = Den And x.Mesic = Mesic)
        If F Is Nothing Then
            Return ""
        Else
            Return F.GetType.GetProperty(Jazyk).GetValue(F, Nothing).ToString
        End If
    End Function

    Public Function GetJmeno(Datum As Date) As String
        Return GetJmeno(Datum.Day, Datum.Month)
    End Function

    Public Function GetJmena(Den As Integer, Mesic As Integer) As String
        Dim F As IEnumerable(Of clsJmeno) = Jmena.Where(Function(x) x.Den = Den And x.Mesic = Mesic)
        If F.Count = 0 Then
            Return ""
        Else
            Dim Jmena As String = ""
            For Each one As clsJmeno In F
                Jmena += If(Jmena = "" Or one.GetType.GetProperty(Jazyk).GetValue(one, Nothing).ToString = "", "", "/") + one.GetType.GetProperty(Jazyk).GetValue(one, Nothing).ToString
            Next
            Return Jmena
        End If
    End Function

    Public Function GetJmena(Datum As Date) As String
        Return GetJmena(Datum.Day, Datum.Month)
    End Function
#End Region

#Region " Get Svatek "

    Public Function GetZavreno(Datum As Date) As Visibility
        GetZavreno = GetLgeZavreno(Svatky.FirstOrDefault(Function(x) x.Den = Datum.Day And x.Mesic = Datum.Month))
        If GetZavreno = Visibility.Collapsed Then GetZavreno = GetLgeZavreno(SpecialSvatek(Datum))
    End Function

    Private Function GetLgeZavreno(cSvatek As clsSvatek) As Visibility
        If cSvatek Is Nothing Then
            Return Visibility.Collapsed
        Else
            Return If(cSvatek.CZzavreno, Visibility.Visible, Visibility.Collapsed)
        End If
    End Function

    Public Function GetSvatek(Datum As Date) As String
        GetSvatek = GetLgeSvatek(Svatky.FirstOrDefault(Function(x) x.Den = Datum.Day And x.Mesic = Datum.Month))
        If GetSvatek = "" Then GetSvatek = GetLgeSvatek(SpecialSvatek(Datum))
    End Function

    Private Function GetLgeSvatek(cSvatek As clsSvatek) As String
        If cSvatek Is Nothing Then
            Return ""
        Else
            Return cSvatek.GetType.GetProperty(Jazyk).GetValue(cSvatek, Nothing).ToString
        End If
    End Function

    Private Function GetSvatek(Den As Integer, Mesic As Integer) As clsSvatek
        Return Svatky.FirstOrDefault(Function(x) x.Den = Den And x.Mesic = Mesic)
    End Function

    Private Function SpecialSvatek(Datum As Date) As clsSvatek
        'Den matek
        Dim i, sun As Integer
        If Datum.Month = 5 Then
            For i = 1 To 14
                If Weekday(DateSerial(Year(Datum), 5, i)) = 1 Then sun = sun + 1
                If sun = 2 Then Exit For
            Next i
            If Datum.Day = i Then Return GetSvatek(0, 0)
        End If
        'Velikonoční pondělí a neděle a Velký pátek
        Dim t, m, s, d, y, c, b, q, p, k As Single
        Dim v, u As Integer
        k = Int(CSng(Datum.Year) / 100)
        p = Int((8 * k + 13) / 25)
        q = Int(k / 4)
        i = CInt(Count(Datum.Year, 19))
        b = Count(Datum.Year, 4)
        c = Count(Datum.Year, 7)
        y = Count(15 - p + k - q, 30) + 19 * i
        d = Count(y, 30)
        y = Count(4 + k - q, 7) + 2 * b + 4 * c + 6 * d
        s = Count(y, 7)
        m = 3
        t = 22 + d + s
        If t > 31 Then m = m + 1
        If t > 31 Then t = t - 31
        u = Int(CInt(d = 29)) + Int(CInt(s = 6))
        v = Int(CInt(d = 28)) + Int(CInt(s = 6)) + Int(CInt(i > 10))
        If u = 2 Or v = 3 Then t = t - 7

        Dim Velikonoce As New Date(Datum.Year, CInt(m), CInt(t))
        If Datum = Velikonoce.AddDays(-2) Then Return GetSvatek(0, 13)
        If Datum = Velikonoce Then Return GetSvatek(0, 2)
        If Datum = Velikonoce.AddDays(1) Then Return GetSvatek(0, 1)

        'Čtvero-roční období
        Dim F As Date = Sezona.FirstOrDefault(Function(x) x.Day = Datum.Day And x.Month = Datum.Month And x.Year = Datum.Year)
        If F.Year = 1 Then
            If Sezona.FirstOrDefault(Function(x) x.Year = Datum.Year).Year = 1 Then
                If Datum.Day = 20 And Datum.Month = 3 Then Return GetSvatek(0, 3)
                If Datum.Day = 21 And Datum.Month = 6 Then Return GetSvatek(0, 6)
                If Datum.Day = 23 And Datum.Month = 9 Then Return GetSvatek(0, 9)
                If Datum.Day = 21 And Datum.Month = 12 Then Return GetSvatek(0, 12)
            End If
        Else
            Return GetSvatek(0, F.Month)
        End If
        Return Nothing
    End Function

    Private Function Count(ByVal x2 As Single, ByVal Y As Single) As Single
        Count = CSng(Int((x2 / Y - Int(x2 / Y)) * Y + 0.1))
    End Function

#End Region

End Class

Public Class clsWeek
    Inherits Collection(Of clsDay)
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal name As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

    Sub New()
        For a = 0 To 6
            Add(New clsDay)
        Next
    End Sub

    Private bDenBrush, bDenJmenoBrush, bJmeninyBrush, bNarozeninyBrush, bSvatkyBrush, bPoznamkyBrush As Brush
    Private imgSluzba As ImageSource
    Private iFontSize As Integer

#Region " Get/Set "

    Public ReadOnly Property Pondeli As clsDay
        Get
            Return ElementAt(0)
        End Get
    End Property

    Public ReadOnly Property Utery As clsDay
        Get
            Return ElementAt(1)
        End Get
    End Property

    Public ReadOnly Property Streda As clsDay
        Get
            Return ElementAt(2)
        End Get
    End Property

    Public ReadOnly Property Ctvrtek As clsDay
        Get
            Return ElementAt(3)
        End Get
    End Property

    Public ReadOnly Property Patek As clsDay
        Get
            Return ElementAt(4)
        End Get
    End Property

    Public ReadOnly Property Sobota As clsDay
        Get
            Return ElementAt(5)
        End Get
    End Property

    Public ReadOnly Property Nedele As clsDay
        Get
            Return ElementAt(6)
        End Get
    End Property

    Public Property SluzbaImg() As ImageSource
        Get
            Return imgSluzba
        End Get
        Set(ByVal value As ImageSource)
            imgSluzba = value
            OnPropertyChanged("SluzbaImg")
        End Set
    End Property

    Public Property FontSize() As Integer
        Get
            Return iFontSize
        End Get
        Set(ByVal value As Integer)
            iFontSize = value
            For a = 0 To 6
                Item(a).FontSize = value
            Next
            OnPropertyChanged("FontSize")
        End Set
    End Property

    'dolů jenom barvy
    Public Property DenBrush() As Brush
        Get
            Return bDenBrush
        End Get
        Set(ByVal value As Brush)
            bDenBrush = value
            OnPropertyChanged("DenBrush")
        End Set
    End Property

    Public Property DenJmenoBrush() As Brush
        Get
            Return bDenJmenoBrush
        End Get
        Set(ByVal value As Brush)
            bDenJmenoBrush = value
            OnPropertyChanged("DenJmenoBrush")
        End Set
    End Property

    Public Property JmeninyBrush() As Brush
        Get
            Return bJmeninyBrush
        End Get
        Set(ByVal value As Brush)
            bJmeninyBrush = value
            OnPropertyChanged("JmeninyBrush")
        End Set
    End Property

    Public Property NarozeninyBrush() As Brush
        Get
            Return bNarozeninyBrush
        End Get
        Set(ByVal value As Brush)
            bNarozeninyBrush = value
            OnPropertyChanged("NarozeninyBrush")
        End Set
    End Property

    Public Property SvatkyBrush() As Brush
        Get
            Return bSvatkyBrush
        End Get
        Set(ByVal value As Brush)
            bSvatkyBrush = value
            OnPropertyChanged("SvatkyBrush")
        End Set
    End Property

    Public Property PoznamkyBrush() As Brush
        Get
            Return bPoznamkyBrush
        End Get
        Set(ByVal value As Brush)
            bPoznamkyBrush = value
            For a = 0 To 6
                Item(a).PoznamkyBrush = value
            Next
            OnPropertyChanged("PoznamkyBrush")
        End Set
    End Property

#End Region

#Region " Day "

    Public Class clsDay
        Implements INotifyPropertyChanged

        Private iDen, iFontSize As Integer
        Private sDenJmeno, sJmeniny, sNarozeniny, sSvatky, sTagJmeno, sTagNaroz As String
        Private bDenFontBrush, bJmeninyFontBrush, bPoznamkyBrush As Brush
        Private vSluzba, vZavreno As Visibility
        Private NarozeninyCursor, JmeninyCursor As Cursor
        Private ocUpominky As New ObservableCollection(Of clsDBase.clsPoznamka)
        Private dat As Date

#Region " Get/Set "
        Public Property Datum As Date
            Get
                Return dat
            End Get
            Set(ByVal value As Date)
                dat = value
                OnPropertyChanged("Datum")
            End Set
        End Property

        Public ReadOnly Property Upominky As ObservableCollection(Of clsDBase.clsPoznamka)
            Get
                Return ocUpominky
            End Get
        End Property

        Public WriteOnly Property Poznamky As List(Of clsDBase.clsPoznamka)
            Set(ByVal value As List(Of clsDBase.clsPoznamka))
                ocUpominky.Clear()
                If value.Any(Function(x) x.Text = "") = False Then value.Add(NewNote)
                value.ForEach(Sub(x) x.PoznamkaBrush = bPoznamkyBrush)
                value.ForEach(Sub(x) x.FontSize = iFontSize)
                value.ForEach(Sub(x) x.SaveActive = True)
                value.ForEach(Sub(x) ocUpominky.Add(x))
                OnPropertyChanged("Upominky")
            End Set
        End Property

        Public Property FontSize() As Integer
            Get
                Return iFontSize
            End Get
            Set(ByVal value As Integer)
                iFontSize = value
                For Each one In Upominky
                    one.FontSize = value
                Next
            End Set
        End Property

        Public Property PoznamkyBrush() As Brush
            Get
                Return bPoznamkyBrush
            End Get
            Set(ByVal value As Brush)
                bPoznamkyBrush = value
                For Each one In Upominky
                    one.PoznamkaBrush = value
                Next
            End Set
        End Property

        Public Property Zavreno() As Visibility
            Get
                Return vZavreno
            End Get
            Set(ByVal value As Visibility)
                vZavreno = value
                OnPropertyChanged("Zavreno")
            End Set
        End Property

        Public Property Sluzba() As Visibility
            Get
                Return vSluzba
            End Get
            Set(ByVal value As Visibility)
                vSluzba = value
                OnPropertyChanged("Sluzba")
            End Set
        End Property

        Public Property CursorNarozeniny() As Cursor
            Get
                Return NarozeninyCursor
            End Get
            Set(ByVal value As Cursor)
                NarozeninyCursor = value
                OnPropertyChanged("CursorNarozeniny")
            End Set
        End Property

        Public Property CursorJmeniny() As Cursor
            Get
                Return JmeninyCursor
            End Get
            Set(ByVal value As Cursor)
                JmeninyCursor = value
                OnPropertyChanged("CursorJmeniny")
            End Set
        End Property

        Public Property Den() As Integer
            Get
                Return iDen
            End Get
            Set(ByVal value As Integer)
                iDen = value
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

        Public Property Svatky() As String
            Get
                Return sSvatky
            End Get
            Set(ByVal value As String)
                sSvatky = value
                OnPropertyChanged("Svatky")
            End Set
        End Property

        Public Property TagNaroz() As String
            Get
                Return sTagNaroz
            End Get
            Set(ByVal value As String)
                sTagNaroz = value
                CursorNarozeniny = If(sTagNaroz = "", Cursors.Arrow, Cursors.Hand)
                OnPropertyChanged("TagNaroz")
            End Set
        End Property

        Public Property TagJmeno() As String
            Get
                Return sTagJmeno
            End Get
            Set(ByVal value As String)
                sTagJmeno = value
                CursorJmeniny = If(sTagJmeno = "", Cursors.Arrow, Cursors.Hand)
                JmeninyFontBrush = If(sTagJmeno = "", Brushes.Black, Brushes.RoyalBlue)
                OnPropertyChanged("TagJmeno")
            End Set
        End Property

        'níže jen Barvy pro fonty
        Public Property JmeninyFontBrush() As Brush
            Get
                Return bJmeninyFontBrush
            End Get
            Set(ByVal value As Brush)
                bJmeninyFontBrush = value
                OnPropertyChanged("JmeninyFontBrush")
            End Set
        End Property

        Public Property DenFontBrush() As Brush
            Get
                Return bDenFontBrush
            End Get
            Set(ByVal value As Brush)
                bDenFontBrush = value
                OnPropertyChanged("DenFontBrush")
            End Set
        End Property

#End Region

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Sub New()
        End Sub

        Public Function NewNote() As clsDBase.clsPoznamka
            Dim note = New clsDBase.clsPoznamka(Now, New DateTime(Datum.Year, Datum.Month, Datum.Day), "", False, False, Now)
            note.PoznamkaBrush = bPoznamkyBrush
            note.FontSize = iFontSize
            note.SaveActive = True
            Return note
        End Function

    End Class

#End Region

End Class