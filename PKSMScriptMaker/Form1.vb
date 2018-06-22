Public Class Form1
#Region "Variables"
    Dim pathexe = My.Application.Info.DirectoryPath
    Dim path = pathexe & "\assets"
    Dim web As New System.Net.WebClient
    Dim prog As String = pathexe & "\PKSMScript.py"
    Dim progG As String = pathexe & "\genScripts.py"
    Dim scm As String
    Dim data(3) As String
    Dim clicks As Integer
    Dim scripts As Integer

    Dim stat As Integer = 0

    Dim ext() As String
    Dim type As Integer = 0
    Dim dex As String
    Dim wcfn As String
    Dim t As String
    Dim webfile As String

    Dim gt As String = "PSM"
    Dim dp As String = "Diamond/Pearl"
    Dim pt As String = "Platinum"
    Dim hgss As String = "HeartGold/SoulSilver"
    Dim bw As String = "Black/White"
    Dim b2w2 As String = "Black 2/White 2"
    Dim xy As String = "X/Y"
    Dim oras As String = "OmegaRuby/AlphaSapphire"
    Dim sm As String = "Sun/Moon"
    Dim usum As String = "UltraSun/UltraMoon"

    Dim wcnum As Integer
    Dim pcd As Boolean
    Dim boxlast As String
    Dim boxnum As Integer
    Dim boxes As Integer
#End Region

#Region "System Menu"
    Public Const WM_SYSCOMMAND As Int32 = &H112
    Public Const MF_BYPOSITION As Int32 = &H400
    Public Const MYMENU1 As Int32 = 1000
    Public Const MYMENU2 As Int32 = 1001
    Public Const MYMENU3 As Int32 = 1002

    Dim hSysMenu As Integer

    Private Declare Function GetSystemMenu Lib "user32" (ByVal hwnd As Integer, ByVal bRevert As Integer) As Integer
    Public Declare Function InsertMenu Lib "user32" Alias "InsertMenuA" _
       (ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Integer, ByVal wIDNewItem As Integer, ByVal lpNewItem As String) As Boolean

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If (m.Msg = WM_SYSCOMMAND) Then
            Select Case m.WParam.ToInt32
                Case MYMENU1
                    Dim about As New about
                    about.ShowDialog()
                Case MYMENU2
                    Dim options As New options
                    options.ShowDialog()
                Case MYMENU3
                    My.Settings.ChkUpd = True
                    checkUpdate()
                    My.Settings.ChkUpd = False
            End Select
        End If
    End Sub


#End Region

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.AutoUpdating = True Then
            checkUpdate()
        End If

        chkPy()

        TabControl1.TabIndex += 2
        RichTextBox1.Hide()
        Button8.Hide()
        buttons()
        TabControl1.TabIndex -= 1

        TabControl1.TabIndex -= 1

        hSysMenu = GetSystemMenu(Me.Handle, False)
        InsertMenu(hSysMenu, 6.5, MF_BYPOSITION, MYMENU1, "About...")
        InsertMenu(hSysMenu, 6, MF_BYPOSITION, MYMENU2, "Options...")
        InsertMenu(hSysMenu, 5.5, MF_BYPOSITION, MYMENU3, "Check for Updates...")
    End Sub
    Private Function MsgB(ByVal mes As String, ByVal numB As Integer, ByVal But1 As String, ByVal But2 As String, ByVal But3 As String, ByVal head As String)
        Dim msg As New CustomMessageBox(mes, numB, But1, But2, But3, head)
        Dim result = msg.ShowDialog()
        Dim Ans As Integer
        If result = Windows.Forms.DialogResult.Yes Then
            'user clicked "B1"
            Ans = 6
        ElseIf result = Windows.Forms.DialogResult.No Then
            'user clicked "B2"
            Ans = 7
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            'user clicked "B3"
            Ans = 8
        Else
            'user closed the window without clicking a button
            Ans = -1
            Close()
        End If
        Return Ans
    End Function 'custom MsgBox
    Private Function DropdownB(ByVal button1Name As String, ByVal button2name As String, ByVal labeltext As String, ByVal dropdowntext As String, ByVal options As String(), ByVal head As String)
        Dim msg As New LangMessageBox(button1Name, button2name, labeltext, dropdowntext, options, head)
        Dim result = msg.ShowDialog()
        Dim Ans As String
        If result = Windows.Forms.DialogResult.Yes Then
            'user clicked "B1"
            Ans = My.Settings.ddres
            My.Settings.ddres = Nothing
        ElseIf result = Windows.Forms.DialogResult.No Then
            'user clicked "B2"
            Ans = Nothing
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            'user clicked "B3"
            Ans = Nothing
        Else
            'user closed the window without clicking a button
            Ans = Nothing
            Close()
        End If
        Return Ans
    End Function 'custom MsgBox
    Public Sub checkUpdate()
        Dim ver As String = My.Application.Info.Version.ToString
#If DEBUG Then
        System.IO.File.WriteAllText(pathexe & "\version.txt", ver)
#Else
        If My.Computer.Network.IsAvailable Then
            Dim msgU As New UpdateCheck
        End If
#End If
    End Sub 'AutoUpdater
    Private Sub chkPy()
        Dim Everest_Registry As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Python\PythonCore")
        If Everest_Registry Is Nothing Then
            'key does not exist
            Dim py As Integer = MsgB("Python 3.x.x is required for script creation. Would you like to get python or continue without script creation?", 2, "Get Python", "Continue Anyway", "", "Python Required")
            If py = 6 Then
                Dim webAddress As String = "https://www.python.org/downloads/"
                Process.Start(webAddress)
                Application.Exit()
            ElseIf py = 7 Then
                TabPage1.Enabled = False
            End If
        Else
        End If
    End Sub

#Region "Converter"
    Private Shared Function HexStringToByteArray(ByRef strInput As String) As Byte()
        Dim length As Integer
        Dim bOutput As Byte()
        Dim c(1) As Integer
        length = strInput.Length / 2
        ReDim bOutput(length - 1)
        For i As Integer = 0 To (length - 1)
            For j As Integer = 0 To 1
                c(j) = Asc(strInput.Chars(i * 2 + j))
                If ((c(j) >= Asc("0")) And (c(j) <= Asc("9"))) Then
                    c(j) = c(j) - Asc("0")
                ElseIf ((c(j) >= Asc("A")) And (c(j) <= Asc("F"))) Then
                    c(j) = c(j) - Asc("A") + &HA
                ElseIf ((c(j) >= Asc("a")) And (c(j) <= Asc("f"))) Then
                    c(j) = c(j) - Asc("a") + &HA
                End If
            Next j
            bOutput(i) = (c(0) * &H10 + c(1))
        Next i
        Return (bOutput)
    End Function
    Private Sub sav(ByVal myFile As String)
        'Dim ext() As String = OpenFileDialog1.FileName.Split(".")
        'Dim Fn() As String = OpenFileDialog1.FileName.Split("\")
        Dim ext() As String = myFile.Split(".")
        Dim Fn() As String = myFile.Split("\")
        Dim Fn2() As String = Fn(UBound(Fn)).Split(".")
        Fn2(UBound(Fn2)) = Fn2(UBound(Fn2)).ToLower
        Dim Fn3 As String = ""
        For i = 0 To UBound(Fn2) Step 1
            Fn3 = Fn3 & Fn2(i)
        Next i
        Dim Fn4 As String = ""
        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Then
            Fn4 = Fn3.Replace("wc7full", ".wc7")
        ElseIf ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Then
            Fn4 = Fn3.Replace("wc6full", ".wc6")
        ElseIf ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Then
            Fn4 = Fn3.Replace("pcd", ".pgt")
        ElseIf ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Then
            Fn4 = Fn3.Replace("ek7", ".smk7")
        ElseIf ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Then
            Fn4 = Fn3.Replace("ek6", ".smk6")
        ElseIf ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then
            Fn4 = Fn3.Replace("ek5", ".smk5")
        ElseIf ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Then
            Fn4 = Fn3.Replace("ek4", ".smk4")
        End If
        SaveFileDialog1.FileName = Fn4
    End Sub
    Private Sub open(ByVal myFile As String)
        'OpenFileDialog1.Filter = "All Supported Files (FULLWonderCards, *.ek#)|*.wc7full;*.wc6full;*.pcd;*.ek7;*.ek6;*.ek5;*.ek4|WonderCards (*.wc7full, *.wc6full, *.pcd)|*.wc7full;*.wc6full;*.pcd|Encrypted PK Files (*.ek#)|*.ek7;*.ek6;*.ek5;*.ek4;*.ek3;*.ek2;*.ek1|All files (*.*)|*.*"
        'OpenFileDialog1.ShowDialog()
        'Dim myFile As String = OpenFileDialog1.FileName
        Dim ext() As String = myFile.Split(".")

        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Or ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Or ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Or ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Or ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Or ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Or ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then 'Or ext(UBound(ext)) = "pk5" Then
            Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
            Dim txtTemp As New System.Text.StringBuilder()
            For Each myByte As Byte In myBytes
                txtTemp.Append(myByte.ToString("X2"))
            Next
            RichTextBox1.Text = txtTemp.ToString()

            If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Or ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Then
                type = 1
            ElseIf ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Then
                type = 2
            ElseIf ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Or ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Then
                type = 3
            ElseIf ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Then
                type = 4
            ElseIf ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then
                type = 5
                'ElseIf ext(UBound(ext)) = "pk5" Then
                '    type = 6
            ElseIf ext(UBound(ext)) = "pgt" Or ext(UBound(ext)) = "PGT" And TextBox4.Text = "0x358" Then
                type = 7
            End If
            sav(myFile)
            stat = 1
            buttons()
            convert()
        Else
            'MsgBox("Not a vaild file", MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "All Supported Files (FULLWonderCards, *.ek#)|*.wc7full;*.wc6full;*.pcd;*.ek7;*.ek6;*.ek5;*.ek4|WonderCards (*.wc7full, *.wc6full, *.pcd)|*.wc7full;*.wc6full;*.pcd|Encrypted PK Files (*.ek#)|*.ek7;*.ek6;*.ek5;*.ek4;*.ek3;*.ek2;*.ek1|All files (*.*)|*.*"
        OpenFileDialog1.ShowDialog()
        Dim myFile As String = OpenFileDialog1.FileName
        Dim ext() As String = myFile.Split(".")

        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Or ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Or ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Or ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Or ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Or ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Or ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then 'Or ext(UBound(ext)) = "pk5" Then
            Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
            Dim txtTemp As New System.Text.StringBuilder()
            For Each myByte As Byte In myBytes
                txtTemp.Append(myByte.ToString("X2"))
            Next
            RichTextBox1.Text = txtTemp.ToString()

            If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Or ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Then
                type = 1
            ElseIf ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Then
                type = 2
            ElseIf ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Or ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Then
                type = 3
            ElseIf ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Then
                type = 4
            ElseIf ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then
                type = 5
                'ElseIf ext(UBound(ext)) = "pk5" Then
                '    type = 6
            End If
            sav(myFile)
            stat = 1
            buttons()
        Else
            MsgBox("Not a vaild file", MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub save(ByVal myFile As String)
        Dim myBytes As Byte() = HexStringToByteArray(RichTextBox1.Text)
        My.Computer.FileSystem.WriteAllBytes(myFile, myBytes, False)

        stat = 0
        buttons()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim myFileO As String = OpenFileDialog1.FileName
        Dim ext() As String = myFileO.Split(".")

        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Then
            SaveFileDialog1.Filter = "wc7 files (*.wc7)|*.wc7|All files (*.*)|*.*"
        ElseIf ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Then
            SaveFileDialog1.Filter = "wc6 files (*.wc6)|*.wc6|All files (*.*)|*.*"
        ElseIf ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Then
            SaveFileDialog1.Filter = "pgt files (*.pgt)|*.pgt|All files (*.*)|*.*"
        ElseIf ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Then
            SaveFileDialog1.Filter = "Script Maker PK7 files (*.smk7)|*.smk7|All files (*.*)|*.*"
        ElseIf ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Then
            SaveFileDialog1.Filter = "Script Maker PK6 files (*.smk6)|*.smk6|All files (*.*)|*.*"
        ElseIf ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Then
            SaveFileDialog1.Filter = "Script Maker PK4 files (*.smk4)|*.smk4|All files (*.*)|*.*"
        ElseIf ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then
            SaveFileDialog1.Filter = "Script Maker PK5 files (*.smk5)|*.smk5|All files (*.*)|*.*"
        End If
        SaveFileDialog1.ShowDialog()
        Dim myFile As String = SaveFileDialog1.FileName

        Dim myBytes As Byte() = HexStringToByteArray(RichTextBox1.Text)
        My.Computer.FileSystem.WriteAllBytes(myFile, myBytes, False)

        stat = 0
        buttons()
    End Sub
    Private Sub cutWC()
        Dim SizeWC = 528
        Dim SizeFull = 1568
        Dim data As String = RichTextBox1.Text
        If (data.Length = SizeFull) Then
            data = data.Skip(SizeFull - SizeWC).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub cutPCD()
        Dim SizeWC = 520
        Dim SizeFull = 1712
        Dim data As String = RichTextBox1.Text
        If (data.Length = SizeFull) Then
            data = data.Remove(SizeWC, SizeFull - SizeWC).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub buildPCD()
        Dim data As String = RichTextBox1.Text
        data = data & My.Resources.PCD
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub cutEK67()
        Dim want = 464
        Dim chuck = 520
        Dim data As String = RichTextBox1.Text
        If (data.Length = chuck) Then
            data = data.Remove(want, chuck - want).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub cutEK4()
        Dim want = 272
        Dim chuck = 472
        Dim data As String = RichTextBox1.Text
        If (data.Length = chuck) Then
            data = data.Remove(want, chuck - want).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub cutEK5()
        Dim want = 272
        Dim chuck = 440
        Dim data As String = RichTextBox1.Text
        If (data.Length = chuck) Then
            data = data.Remove(want, chuck - want).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
#Region "Encryption"
    Public seed As UInt32
    Public Sub srnd(ByVal newSeed As UInt32)
        seed = newSeed
    End Sub
    Public Function rand() As UInt32
        seed = ((((&H41C64E6D * seed) + &H6073)) And &HFFFFFFFF) >> 16
        '(n + 1) = (&H41C64E6D * n + &H6073)
        Return seed ' >> 16
    End Function
    Private Sub pkmenc()
        Dim want = 16
        Dim chuck = 272
        Dim chksum
        Dim pv
        Dim data As String = RichTextBox1.Text
        pv = data.Remove(8, chuck - 8)
        chksum = data.Skip(12)

        Dim bs As Integer = (pv >> (13) And 31) Mod 24
        Dim b1 = Nothing
        Dim b2 = Nothing
        Dim b3 = Nothing
        Dim b4 = Nothing
        Dim ba = (data.Remove(80, chuck - 80)).Skip(16)
        Dim bb = (data.Remove(144, chuck - 144)).Skip(80)
        Dim bc = (data.Remove(208, chuck - 208)).Skip(144)
        Dim bd = data.Skip(208)

        Select Case bs
            Case 0
                b1 = ba
                b2 = bb
                b3 = bc
                b4 = bd
            Case 1
                b1 = ba
                b2 = bb
                b3 = bd
                b4 = bc
            Case 2
                b1 = ba
                b2 = bc
                b3 = bb
                b4 = bd
            Case 3
                b1 = ba
                b2 = bd
                b3 = bb
                b4 = bc
            Case 4
                b1 = ba
                b2 = bc
                b3 = bd
                b4 = bb
            Case 5
                b1 = ba
                b2 = bd
                b3 = bc
                b4 = bb
            Case 6
                b1 = bb
                b2 = ba
                b3 = bc
                b4 = bd
            Case 7
                b1 = bb
                b2 = ba
                b3 = bd
                b4 = bc
            Case 8
                b1 = bc
                b2 = ba
                b3 = bb
                b4 = bd
            Case 9
                b1 = bd
                b2 = ba
                b3 = bb
                b4 = bc
            Case 10
                b1 = bc
                b2 = ba
                b3 = bd
                b4 = bb
            Case 11
                b1 = bd
                b2 = ba
                b3 = bc
                b4 = bb
            Case 12
                b1 = bb
                b2 = bc
                b3 = ba
                b4 = bd
            Case 13
                b1 = bb
                b2 = bd
                b3 = ba
                b4 = bc
            Case 14
                b1 = bc
                b2 = bb
                b3 = ba
                b4 = bd
            Case 15
                b1 = bd
                b2 = bb
                b3 = ba
                b4 = bc
            Case 16
                b1 = bc
                b2 = bd
                b3 = ba
                b4 = bb
            Case 17
                b1 = bd
                b2 = bc
                b3 = ba
                b4 = bb
            Case 18
                b1 = bb
                b2 = bc
                b3 = bd
                b4 = ba
            Case 19
                b1 = bb
                b2 = bd
                b3 = bc
                b4 = ba
            Case 20
                b1 = bc
                b2 = bb
                b3 = bd
                b4 = ba
            Case 21
                b1 = bd
                b2 = bb
                b3 = bc
                b4 = ba
            Case 22
                b1 = bc
                b2 = bd
                b3 = bb
                b4 = ba
            Case 23
                b1 = bd
                b2 = bc
                b3 = bb
                b4 = ba
        End Select
        Dim shdata = b1 & b2 & b3 & b4
        srnd(chksum)
        rand()

        Dim z = 0
        'Select Case b1
        '    Case ba
        'While z < 16
        '    shdata(z) = ba(z) Xor rand()
        '    z = z + 1
        'End While
        'End Select


        'Dim n = "&H" & chksum
        '(n + 1) = (&H41C64E6D * n + &H6073)
    End Sub
    Private Sub enc()
        Dim myData = RichTextBox1.Text
        Dim pkx As Byte() = HexStringToByteArray(myData)
        Dim ekx() As Byte = encrypt(pkx)

        Dim txtTemp As New System.Text.StringBuilder()
        For Each myByte As Byte In ekx
            txtTemp.Append(myByte.ToString("X2"))
        Next
        RichTextBox1.Text = txtTemp.ToString()
    End Sub
    Public Function encrypt(ByVal pkm() As Byte)
        Dim pid As UInt32 = 0
        Dim checksum As UInt16 = 0
        pid = BitConverter.ToUInt32(pkm, 0)
        checksum = BitConverter.ToUInt16(pkm, 6)
        Dim order As Integer = (pid >> (13) And 31) Mod 24
        Dim firstblock As String
        Dim secondblock As String
        Dim thirdblock As String
        Dim fourthblock As String

        firstblock = 0
        secondblock = 0
        thirdblock = 0
        fourthblock = 0
        Select Case order
            Case 0
                firstblock = "A"
                secondblock = "B"
                thirdblock = "C"
                fourthblock = "D"
            Case 1
                firstblock = "A"
                secondblock = "B"
                thirdblock = "D"
                fourthblock = "C"
            Case 2
                firstblock = "A"
                secondblock = "C"
                thirdblock = "B"
                fourthblock = "D"
            Case 3
                firstblock = "A"
                secondblock = "C"
                thirdblock = "D"
                fourthblock = "B"
            Case 4
                firstblock = "A"
                secondblock = "D"
                thirdblock = "B"
                fourthblock = "C"
            Case 5
                firstblock = "A"
                secondblock = "D"
                thirdblock = "C"
                fourthblock = "B"
            Case 6
                firstblock = "B"
                secondblock = "A"
                thirdblock = "C"
                fourthblock = "D"
            Case 7
                firstblock = "B"
                secondblock = "A"
                thirdblock = "D"
                fourthblock = "C"
            Case 8
                firstblock = "B"
                secondblock = "C"
                thirdblock = "A"
                fourthblock = "D"
            Case 9
                firstblock = "B"
                secondblock = "C"
                thirdblock = "D"
                fourthblock = "A"
            Case 10
                firstblock = "B"
                secondblock = "D"
                thirdblock = "A"
                fourthblock = "C"
            Case 11
                firstblock = "B"
                secondblock = "D"
                thirdblock = "C"
                fourthblock = "A"
            Case 12
                firstblock = "C"
                secondblock = "A"
                thirdblock = "B"
                fourthblock = "D"
            Case 13
                firstblock = "C"
                secondblock = "A"
                thirdblock = "D"
                fourthblock = "B"
            Case 14
                firstblock = "C"
                secondblock = "B"
                thirdblock = "A"
                fourthblock = "D"
            Case 15
                firstblock = "C"
                secondblock = "B"
                thirdblock = "D"
                fourthblock = "A"
            Case 16
                firstblock = "C"
                secondblock = "D"
                thirdblock = "A"
                fourthblock = "B"
            Case 17
                firstblock = "C"
                secondblock = "D"
                thirdblock = "B"
                fourthblock = "A"
            Case 18
                firstblock = "D"
                secondblock = "A"
                thirdblock = "B"
                fourthblock = "C"
            Case 19
                firstblock = "D"
                secondblock = "A"
                thirdblock = "C"
                fourthblock = "B"
            Case 20
                firstblock = "D"
                secondblock = "B"
                thirdblock = "A"
                fourthblock = "C"
            Case 21
                firstblock = "D"
                secondblock = "B"
                thirdblock = "C"
                fourthblock = "A"
            Case 22
                firstblock = "D"
                secondblock = "C"
                thirdblock = "A"
                fourthblock = "B"
            Case 23
                firstblock = "D"
                secondblock = "C"
                thirdblock = "B"
                fourthblock = "A"
        End Select
        Dim z As Integer = 0
        Dim v As Integer = 8
        'Block A
        Dim blocka(16) As UInt16
        While z < 16
            blocka(z) = BitConverter.ToUInt16(pkm, v)
            z = z + 1
            v = v + 2
        End While
        z = 0
        v = 40
        'Block B
        Dim blockb(16) As UInt16
        While z < 16
            blockb(z) = BitConverter.ToUInt16(pkm, v)
            z = z + 1
            v = v + 2
        End While
        z = 0
        'Block C
        v = 72
        Dim blockc(16) As UInt16
        While z < 16
            blockc(z) = BitConverter.ToUInt16(pkm, v)
            z = z + 1
            v = v + 2
        End While
        z = 0
        'Block D
        Dim blockd(16) As UInt16
        v = 104
        While z < 16
            blockd(z) = BitConverter.ToUInt16(pkm, v)
            z = z + 1
            v = v + 2
        End While
        z = 0
        srnd(checksum)
        Dim byter(16) As UInt16
        z = 0
        v = 8
        Debug.Print(pid)
        Debug.Print(seed)
        Dim y As UInt32
        Select Case firstblock
            Case "A"
                While z < 16
                    y = rand()
                    byter(z) = blocka(z) Xor y
                    z = z + 1
                End While
            Case "B"
                While z < 16
                    y = rand()
                    Debug.Print(y)
                    byter(z) = CType((blockb(z) Xor rand()), System.UInt16)
                    z = z + 1
                End While
            Case "C"
                While z < 16
                    y = rand()
                    byter(z) = blockc(z) Xor y
                    z = z + 1
                End While
            Case "D"
                While z < 16
                    y = rand()
                    byter(z) = blockd(z) Xor y
                    z = z + 1
                End While
        End Select
        z = 0
        v = 8
        While z < 16
            pkm(v) = byter(z) And 255
            pkm(v + 1) = byter(z) >> 8
            z = z + 1
            v = v + 2
        End While
        z = 0
        v = 40
        Select Case secondblock
            Case "A"
                While z < 16
                    byter(z) = blocka(z) Xor rand()
                    z = z + 1
                End While
            Case "B"
                While z < 16
                    byter(z) = blockb(z) Xor rand()
                    z = z + 1
                End While
            Case "C"
                While z < 16
                    byter(z) = blockc(z) Xor rand()
                    z = z + 1
                End While
            Case "D"
                While z < 16
                    byter(z) = blockd(z) Xor rand()
                    z = z + 1
                End While
        End Select
        z = 0
        While z < 16
            pkm(v) = byter(z) And 255
            pkm(v + 1) = byter(z) >> 8
            z = z + 1
            v = v + 2
        End While
        z = 0
        v = 72
        Select Case thirdblock
            Case "A"
                While z < 16
                    byter(z) = blocka(z) Xor rand()
                    z = z + 1
                End While
            Case "B"
                While z < 16
                    byter(z) = blockb(z) Xor rand()
                    z = z + 1
                End While
            Case "C"
                While z < 16
                    byter(z) = blockc(z) Xor rand()
                    z = z + 1
                End While
            Case "D"
                While z < 16
                    byter(z) = blockd(z) Xor rand()
                    z = z + 1
                End While
        End Select
        z = 0
        While z < 16
            pkm(v) = byter(z) And 255
            pkm(v + 1) = byter(z) >> 8
            z = z + 1
            v = v + 2
        End While
        z = 0
        v = 104
        Select Case fourthblock
            Case "A"
                While z < 16
                    byter(z) = blocka(z) Xor rand()
                    z = z + 1
                End While
            Case "B"
                While z < 16
                    byter(z) = blockb(z) Xor rand()
                    z = z + 1
                End While
            Case "C"
                While z < 16
                    byter(z) = blockc(z) Xor rand()
                    z = z + 1
                End While
            Case "D"
                While z < 16
                    byter(z) = blockd(z) Xor rand()
                    z = z + 1
                End While
        End Select
        z = 0
        While z < 16
            pkm(v) = byter(z) And 255
            pkm(v + 1) = byter(z) >> 8
            z = z + 1
            v = v + 2
        End While
        z = 0
        Return pkm
    End Function
#End Region
    Private Sub convert()
        If type = 1 Then
            cutWC()
        ElseIf type = 2 Then
            cutPCD()
        ElseIf type = 3 Then
            cutEK67()
        ElseIf type = 4 Then
            cutEK4()
        ElseIf type = 5 Then
            cutEK5()
            'ElseIf type = 6 Then
            '    enc()
        ElseIf type = 7 Then
            buildPCD()
        End If
        stat = 2
        buttons()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If type = 1 Then
            cutWC()
        ElseIf type = 2 Then
            cutPCD()
        ElseIf type = 3 Then
            cutEK67()
        ElseIf type = 4 Then
            cutEK4()
        ElseIf type = 5 Then
            cutEK5()
            'ElseIf type = 6 Then
            '    enc()
        End If
        stat = 2
        buttons()
    End Sub
    Private Sub buttons()
        If stat = 0 Then
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = False
        ElseIf stat = 1 Then
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = True
        ElseIf stat = 2 Then
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = False
        End If
    End Sub
#End Region

#Region "Script Maker"
    Public Sub prep()
        wcfn = ""
        Dim f As String = Nothing
        If dex = "wc7" Then
            wcfn = "g7wc.wc7"
        ElseIf dex = "wc6" Then
            wcfn = "g6wc.wc6"
        ElseIf dex = "pgf" Then
            wcfn = "g5wc.pgf"
        ElseIf dex = "pgt" Then
            If TextBox4.Text = "0x358" Then
                open(data(3))
                f = pathexe & "\g4pcd.pcd"
                save(f)
                TextBox3.Text = f
                t = 1
            Else
                wcfn = "g4wc.pgt"
            End If
        ElseIf dex = "pcd" Or dex = "PCD" Then
            If TextBox4.Text = "0x358" Then
                wcfn = "g4pcd.pcd"
            Else
                open(data(3))
                f = pathexe & "\g4wc.pgt"
                save(f)
                TextBox3.Text = f
                t = 1
            End If
        ElseIf dex = "bin" Then
            wcfn = "binary.bin"
        ElseIf dex = "txt" Then
            wcfn = "text.txt"
        ElseIf dex = "smk4" Then
            wcfn = "pkm4.smk4"
        ElseIf dex = "smk5" Then
            wcfn = "pkm5.smk5"
        ElseIf dex = "smk6" Then
            wcfn = "pkm6.smk6"
        ElseIf dex = "smk7" Then
            wcfn = "pkm7.smk7"
        ElseIf dex = "wc7full" Or dex = "wc6full" Or dex = "pcd" Or dex = "ek7" Or dex = "ek6" Or dex = "ek4" Or dex = "ek5" Then
            open(data(3))
            If dex = "wc7full" Or dex = "WC7FULL" Then
                f = pathexe & "\g7wc.wc7"
                save(f)
            ElseIf dex = "wc6full" Or dex = "WC6FULL" Then
                f = pathexe & "\g6wc.wc6"
                save(f)
            ElseIf dex = "ek7" Or dex = "EK7" Then
                f = pathexe & "\pkm7.smk7"
                save(f)
            ElseIf dex = "ek6" Or dex = "EK6" Then
                f = pathexe & "\pkm6.smk6"
                save(f)
            ElseIf dex = "ek5" Or dex = "EK5" Then
                f = pathexe & "\pkm5.smk5"
                save(f)
            ElseIf dex = "ek4" Or dex = "EK4" Then
                f = pathexe & "\pkm4.smk4"
                save(f)
            End If
            TextBox3.Text = f
            t = 1
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Missing Value(s)", MsgBoxStyle.OkOnly)
        Else
            ' web.DownloadFileAsync(New Uri("https://github.com/BernardoGiordano/PKSM-Tools/raw/master/PKSMScript/PKSMScript.py"), prog)
            data(0) = TextBox1.Text
            If TextBox2.Text.Contains("0x") Then
                data(1) = TextBox2.Text
            Else
                data(1) = "0x" & TextBox2.Text
            End If
            If TextBox4.Text.Contains("0x") Then
                data(2) = TextBox4.Text
            Else
                data(2) = "0x" & TextBox4.Text
            End If
1:
            data(3) = TextBox3.Text
            If data(3).Contains("\") Then
                Dim da() As String = data(3).Split("\")
                Dim ex() As String = da(UBound(da)).Split(".")
                dex = ex(UBound(ex)).ToLower
                prep()
                If t = 0 Then
                ElseIf t = 1 Then
                    t = 0
                        GoTo 1
                    End If
                If System.IO.File.Exists(pathexe & "\" & wcfn) Then
                Else
                    System.IO.File.Copy(data(3), pathexe & "\" & wcfn)
                End If
                If System.IO.File.Exists(webfile) Then
                    System.IO.File.Delete(webfile)
                End If
                scm = """" & data(0) & """ -i " & data(1) & " " & data(2) & " " & """" & wcfn & """" & " 1"
                ElseIf data(3).Contains("/") Then
                    dex = DropdownB("Confirm", "Cancel", "What's the file's extension?", "Type", {"pgt", "pgf", "wc6", "wc7", "pcd", "wc6full", "wc7full", "smk4", "smk5", "smk6", "smk7", "bin", "txt"}, "File Type?")
                    Dim dlfile As String = pathexe & "\webfile." & dex
                If System.IO.File.Exists(dlfile) Then
                    System.IO.File.Delete(dlfile)
                    web.DownloadFile(New Uri(data(3)), dlfile)
                Else
                    web.DownloadFile(New Uri(data(3)), dlfile)
                    'web.DownloadDataAsync(New Uri(data(3)), dlfile)
                    'My.Computer.Network.DownloadFile(data(3), dlfile)
                End If
                TextBox3.Text = dlfile
                    webfile = dlfile
                    GoTo 1
                Else
                    scm = """" & data(0) & """ -i " & data(1) & " " & data(2) & " " & data(3) & " 1"
            End If
            System.IO.File.WriteAllText(prog, My.Resources.PKSMScript)
            System.IO.File.WriteAllText(progG, My.Resources.genScripts)
            System.IO.File.WriteAllText(pathexe & "\scripts" & gt & ".txt", scm)
            Dim gs As New ProcessStartInfo(pathexe & "\genScripts.py")
            gs.WindowStyle = ProcessWindowStyle.Hidden
            Process.Start(gs)
            System.Threading.Thread.Sleep(1500)
            scripts = scripts + 1
            TextBox1.Text = Nothing
            TextBox3.Text = Nothing
            Dim objShell = CreateObject("WScript.Shell")
            Dim X As Integer
            X = objShell.Popup("Done", 3, "Script Made ☻", vbOKOnly)
            Select Case X
                Case vbOK
                    clicks = clicks + 1
                Case Else
            End Select
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'OpenFileDialog2.Filter = "Gen6/7 WonderCards (*.wc7, *.wc6)|*.wc7;*.wc6|Gen5 WonderCards (*.pgf)|*.pgf|Gen4 WonderCards (*.pgt, *.pcd)|*.pgt;*.pcd|Pokémon Files (*.pk#)|*.pk7;*.pk6;*.pk5;*.pk4|bin files (*.bin)|*.bin|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog2.Filter = "All Supported Files (FULLWonderCards, WonderCards, *.ek#, *.smk#, Binary, and Text files)|*.wc7full;*.wc6full;*.pcd;*.ek7;*.ek6;*.ek5;*.ek4;*.wc7;*.wc6;*.pgf;*.pgt;*.bin;*.txt;*.smk7;*.smk6;*.smk5;*.smk4|Recommended Supported Files (WonderCards, *.smk#, Binary, and Text files)|*.wc7;*.wc6;*.pgf;*.pgt;*.bin;*.txt;*.smk7;*.smk6;*.smk5;*.smk4|WonderCards (*.wc7, *.wc6, *.pgf, *.pgt)|*.wc7;*.wc6;*.pgf;*.pgt|Script Maker PK files (*.smk#)|*.smk7;*.smk6;*.smk5;*.smk4|bin files (*.bin)|*.bin|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog2.ShowDialog()
        Dim ex() As String = OpenFileDialog2.FileName.Split(".")
        TextBox3.Text = OpenFileDialog2.FileName
    End Sub
    Private Sub TabPage1_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles TabPage1.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each pathfd In files
            TextBox3.Text = pathfd
        Next
    End Sub
    Private Sub TabPage1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles TabPage1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        If ComboBox4.Text = dp Then
            gt = "DP"
        ElseIf ComboBox4.Text = pt Then
            gt = "PT"
        ElseIf ComboBox4.Text = hgss Then
            gt = "HGSS"
        ElseIf ComboBox4.Text = bw Then
            gt = "BW"
        ElseIf ComboBox4.Text = b2w2 Then
            gt = "B2W2"
        ElseIf ComboBox4.Text = xy Then
            gt = "XY"
        ElseIf ComboBox4.Text = oras Then
            gt = "ORAS"
        ElseIf ComboBox4.Text = sm Then
            gt = "SM"
        ElseIf ComboBox4.Text = usum Then
            gt = "USUM"
        Else
            gt = "PSM"
        End If
    End Sub
#End Region

#Region "Info"
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.Text
            Case "Wonder Card Slot"
                ComboBox2.Enabled = True
                ComboBox2.Text = "--Slot #--"
                Me.ComboBox2.Items.Clear()
                For i = 1 To wcnum Step 1
                    ComboBox2.Items.Add("WC " & i)
                Next i
                If pcd = True Then
                    ComboBox2.Items.Add("PCD 1")
                    ComboBox2.Items.Add("PCD 2")
                    ComboBox2.Items.Add("PCD 3")
                End If
            Case "Battle Styles"
                ComboBox2.Enabled = True
                ComboBox2.Text = "--Style--"
                Me.ComboBox2.Items.Clear()
                ComboBox2.Items.Add("Normal")
                ComboBox2.Items.Add("Elegant")
                ComboBox2.Items.Add("Girlish")
                ComboBox2.Items.Add("Reverant")
                ComboBox2.Items.Add("Smug")
                ComboBox2.Items.Add("Left-Handed")
                ComboBox2.Items.Add("Passionate")
                ComboBox2.Items.Add("Idol")
                ComboBox2.Items.Add("Nihilist")
            Case "Vivillon"
                ComboBox2.Enabled = True
                ComboBox2.Text = "--Form--"
                Me.ComboBox2.Items.Clear()
                ComboBox2.Items.Add("Icy Snow")
                ComboBox2.Items.Add("Polar")
                ComboBox2.Items.Add("Tundra")
                ComboBox2.Items.Add("Continental")
                ComboBox2.Items.Add("Garden")
                ComboBox2.Items.Add("Elegant")
                ComboBox2.Items.Add("Meadow")
                ComboBox2.Items.Add("Modern")
                ComboBox2.Items.Add("Marine")
                ComboBox2.Items.Add("Archipelago")
                ComboBox2.Items.Add("High-Plains")
                ComboBox2.Items.Add("Sandstorm")
                ComboBox2.Items.Add("River")
                ComboBox2.Items.Add("Monsoon")
                ComboBox2.Items.Add("Savannah")
                ComboBox2.Items.Add("Sun")
                ComboBox2.Items.Add("Ocean")
                ComboBox2.Items.Add("Jungle")
                ComboBox2.Items.Add("Fancy")
                ComboBox2.Items.Add("Pokéball")
            Case "Money"
                ComboBox2.Enabled = False
                ComboBox2.Text = "----"
                Me.ComboBox2.Items.Clear()
                If ComboBox3.Text = usum Then
                    Label5.Text = "0x4404"
                ElseIf ComboBox3.Text = sm Then
                    Label5.Text = "0x4004"
                ElseIf ComboBox3.Text = oras Or ComboBox3.Text = xy Then
                    Label5.Text = "0x4208"
                End If
            Case "Battle Points"
                ComboBox2.Enabled = False
                ComboBox2.Text = "----"
                Me.ComboBox2.Items.Clear()
                If ComboBox3.Text = usum Then
                    Label5.Text = "0x0451C"
                ElseIf ComboBox3.Text = sm Then
                    Label5.Text = "0x0411C"
                End If
            Case "Festival Coins"
                ComboBox2.Enabled = False
                ComboBox2.Text = "----"
                Me.ComboBox2.Items.Clear()
                If ComboBox3.Text = usum Then
                    Label5.Text = "0x51308"
                ElseIf ComboBox3.Text = sm Then
                    Label5.Text = "0x50D08"
                End If
            Case "Language"
                ComboBox2.Enabled = True
                ComboBox2.Text = "--Lang--"
                Me.ComboBox2.Items.Clear()
                ComboBox2.Items.Add("日本語")
                ComboBox2.Items.Add("English")
                ComboBox2.Items.Add("Français")
                ComboBox2.Items.Add("Italiano")
                ComboBox2.Items.Add("Deutsch")
                ComboBox2.Items.Add("Español")
                ComboBox2.Items.Add("한국어")
                If ComboBox3.Text = usum Or ComboBox3.Text = sm Then
                    ComboBox2.Items.Add("中文 (简体)")
                    ComboBox2.Items.Add("中文 (繁體)")
                End If
            Case "BOX 1", "BOX 2", "BOX 3", "BOX 4", "BOX 5", "BOX 6", "BOX 7", "BOX 8", "BOX 9", "BOX 10", "BOX 11", "BOX 12", "BOX 13", "BOX 14", "BOX 15", "BOX 16", "BOX 17", "BOX 18", "BOX 19", "BOX 20", "BOX 21", "BOX 22", "BOX 23", "BOX 24", "BOX 25", "BOX 26", "BOX 27", "BOX 28", "BOX 29", "BOX 30", "BOX 31", "BOX 32"
                ComboBox2.Enabled = True
                ComboBox2.Text = "--Slot #--"
                Me.ComboBox2.Items.Clear()
                If ComboBox1.Text.Length = 5 Then
                    boxlast = ComboBox1.Text.Last
                    boxnum = Val(boxlast)
                ElseIf ComboBox1.Text.Length = 6 Then
                    boxlast = ComboBox1.Text.Replace("BOX ", "")
                    Dim boxdiv() As Char = boxlast.ToCharArray
                    boxnum = Val(boxlast(0)) & Val(boxlast(1))
                End If
                For i = 1 To 30 Step 1
                    ComboBox2.Items.Add("Slot " & i)
                Next i
            Case "-------Offsets-------"
                ComboBox2.Enabled = False
        End Select
        Label5.Text = Nothing
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Select Case ComboBox3.Text
            Case dp
                'WC
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    If cb2 < 8 Then
                        Dim v As Integer = (&H4A7FC + (cb2 * &H104))
                        Label5.Text = "0x" & Hex(v)
                    Else
                        Dim v As Integer = (&HB01C + ((cb2 - 8) * &H358))
                        Label5.Text = "0x" & Hex(v)
                    End If
                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&HC104 + ((((boxnum - 1) * 30) + cb2) * &H88))
                    Label5.Text = "0x" & Hex(v)
                End If
            Case pt
                'WC
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    If cb2 < 8 Then
                        Dim v As Integer = (&H4B5C0 + (cb2 * &H104))
                        Label5.Text = "0x" & Hex(v)
                    Else
                        Dim v As Integer = (&H4BDE0 + ((cb2 - 8) * &H358))
                        Label5.Text = "0x" & Hex(v)
                    End If
                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&HCF30 + ((((boxnum - 1) * 30) + cb2) * &H88))
                    Label5.Text = "0x" & Hex(v)
                End If
            Case hgss
                'WC
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    If cb2 < 8 Then
                        Dim v As Integer = (&H9E3C + (cb2 * &H104))
                        Label5.Text = "0x" & Hex(v)
                    Else
                        Dim v As Integer = (&HA65C + ((cb2 - 8) * &H358))
                        Label5.Text = "0x" & Hex(v)
                    End If
                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                        Dim v As Integer = (&HF700 + ((((boxnum - 1) * 30) + cb2) * &H88))
                    Label5.Text = "0x" & Hex(v)
                End If
            Case bw
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                'PC
                If ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&H400 + ((((boxnum - 1) * 30) + cb2) * &H88))
                    Label5.Text = "0x" & Hex(v)
                End If
            Case b2w2
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                'PC
                If ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&H400 + ((((boxnum - 1) * 30) + cb2) * &H88))
                    Label5.Text = "0x" & Hex(v)
                End If
            Case xy
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    Dim v As Integer = (&H1BD00 + (cb2 * &H108))
                    Label5.Text = "0x" & Hex(v)

                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&H22600 + ((((boxnum - 1) * 30) + cb2) * &HE8))
                    Label5.Text = "0x" & Hex(v)

                    'Viv
                ElseIf ComboBox2.Text = "Icy Snow" Then
                    Label5.Text = "0x4250 | Data: 0"
                ElseIf ComboBox2.Text = "Polar" Then
                    Label5.Text = "0x4250 | Data: 1"
                ElseIf ComboBox2.Text = "Tundra" Then
                    Label5.Text = "0x4250 | Data: 2"
                ElseIf ComboBox2.Text = "Continental" Then
                    Label5.Text = "0x4250 | Data: 3"
                ElseIf ComboBox2.Text = "Garden" Then
                    Label5.Text = "0x4250 | Data: 4"
                ElseIf ComboBox2.Text = "Elegant" Then
                    Label5.Text = "0x4250 | Data: 5"
                ElseIf ComboBox2.Text = "Meadow" Then
                    Label5.Text = "0x4250 | Data: 6"
                ElseIf ComboBox2.Text = "Modern" Then
                    Label5.Text = "0x4250 | Data: 7"
                ElseIf ComboBox2.Text = "Marine" Then
                    Label5.Text = "0x4250 | Data: 8"
                ElseIf ComboBox2.Text = "Archipelago" Then
                    Label5.Text = "0x4250 | Data: 9"
                ElseIf ComboBox2.Text = "High-Plains" Then
                    Label5.Text = "0x4250 | Data: 10"
                ElseIf ComboBox2.Text = "Sandstorm" Then
                    Label5.Text = "0x4250 | Data: 11"
                ElseIf ComboBox2.Text = "River" Then
                    Label5.Text = "0x4250 | Data: 12"
                ElseIf ComboBox2.Text = "Monsoon" Then
                    Label5.Text = "0x4250 | Data: 13"
                ElseIf ComboBox2.Text = "Savannah" Then
                    Label5.Text = "0x4250 | Data: 14"
                ElseIf ComboBox2.Text = "Sun" Then
                    Label5.Text = "0x4250 | Data: 15"
                ElseIf ComboBox2.Text = "Ocean" Then
                    Label5.Text = "0x4250 | Data: 16"
                ElseIf ComboBox2.Text = "Jungle" Then
                    Label5.Text = "0x4250 | Data: 17"
                ElseIf ComboBox2.Text = "Fancy" Then
                    Label5.Text = "0x4250 | Data: 18"
                ElseIf ComboBox2.Text = "Pokéball" Then
                    Label5.Text = "0x4250 | Data: 19"
                    'lang
                ElseIf ComboBox2.Text = "日本語" Then
                    Label5.Text = "0x1402D | Data: 0x1"
                ElseIf ComboBox2.Text = "English" Then
                    Label5.Text = "0x1402D | Data: 0x2"
                ElseIf ComboBox2.Text = "Français" Then
                    Label5.Text = "0x1402D | Data: 0x3"
                ElseIf ComboBox2.Text = "Italiano" Then
                    Label5.Text = "0x1402D | Data: 0x4"
                ElseIf ComboBox2.Text = "Deutsch" Then
                    Label5.Text = "0x1402D | Data: 0x5"
                ElseIf ComboBox2.Text = "Español" Then
                    Label5.Text = "0x1402D | Data: 0x7"
                ElseIf ComboBox2.Text = "한국어" Then
                    Label5.Text = "0x1402D | Data: 0x8"

                End If
            Case oras
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    Dim v As Integer = (&H1CD00 + (cb2 * &H108))
                    Label5.Text = "0x" & Hex(v)

                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&H33000 + ((((boxnum - 1) * 30) + cb2) * &HE8))
                    Label5.Text = "0x" & Hex(v)

                    'ORAS Viv
                ElseIf ComboBox2.Text = "Icy Snow" Then
                    Label5.Text = "0x4244 | Data: 0"
                ElseIf ComboBox2.Text = "Polar" Then
                    Label5.Text = "0x4244 | Data: 1"
                ElseIf ComboBox2.Text = "Tundra" Then
                    Label5.Text = "0x4244 | Data: 2"
                ElseIf ComboBox2.Text = "Continental" Then
                    Label5.Text = "0x4244 | Data: 3"
                ElseIf ComboBox2.Text = "Garden" Then
                    Label5.Text = "0x4244 | Data: 4"
                ElseIf ComboBox2.Text = "Elegant" Then
                    Label5.Text = "0x4244 | Data: 5"
                ElseIf ComboBox2.Text = "Meadow" Then
                    Label5.Text = "0x4244 | Data: 6"
                ElseIf ComboBox2.Text = "Modern" Then
                    Label5.Text = "0x4244 | Data: 7"
                ElseIf ComboBox2.Text = "Marine" Then
                    Label5.Text = "0x4244 | Data: 8"
                ElseIf ComboBox2.Text = "Archipelago" Then
                    Label5.Text = "0x4244 | Data: 9"
                ElseIf ComboBox2.Text = "High-Plains" Then
                    Label5.Text = "0x4244 | Data: 10"
                ElseIf ComboBox2.Text = "Sandstorm" Then
                    Label5.Text = "0x4244 | Data: 11"
                ElseIf ComboBox2.Text = "River" Then
                    Label5.Text = "0x4244 | Data: 12"
                ElseIf ComboBox2.Text = "Monsoon" Then
                    Label5.Text = "0x4244 | Data: 13"
                ElseIf ComboBox2.Text = "Savannah" Then
                    Label5.Text = "0x4244 | Data: 14"
                ElseIf ComboBox2.Text = "Sun" Then
                    Label5.Text = "0x4244 | Data: 15"
                ElseIf ComboBox2.Text = "Ocean" Then
                    Label5.Text = "0x4244 | Data: 16"
                ElseIf ComboBox2.Text = "Jungle" Then
                    Label5.Text = "0x4244 | Data: 17"
                ElseIf ComboBox2.Text = "Fancy" Then
                    Label5.Text = "0x4244 | Data: 18"
                ElseIf ComboBox2.Text = "Pokéball" Then
                    Label5.Text = "0x4244 | Data: 19"
                    'lang
                ElseIf ComboBox2.Text = "日本語" Then
                    Label5.Text = "0x1402D | Data: 0x1"
                ElseIf ComboBox2.Text = "English" Then
                    Label5.Text = "0x1402D | Data: 0x2"
                ElseIf ComboBox2.Text = "Français" Then
                    Label5.Text = "0x1402D | Data: 0x3"
                ElseIf ComboBox2.Text = "Italiano" Then
                    Label5.Text = "0x1402D | Data: 0x4"
                ElseIf ComboBox2.Text = "Deutsch" Then
                    Label5.Text = "0x1402D | Data: 0x5"
                ElseIf ComboBox2.Text = "Español" Then
                    Label5.Text = "0x1402D | Data: 0x7"
                ElseIf ComboBox2.Text = "한국어" Then
                    Label5.Text = "0x1402D | Data: 0x8"

                End If
            Case sm
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    Dim v As Integer = (&H65D00 + (cb2 * &H108))
                    Label5.Text = "0x" & Hex(v)

                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&H4E00 + ((((boxnum - 1) * 30) + cb2) * &HE8))
                    Label5.Text = "0x" & Hex(v)

                    'SM Viv
                ElseIf ComboBox2.Text = "Icy Snow" Then
                    Label5.Text = "0x4130 | Data: 0"
                ElseIf ComboBox2.Text = "Polar" Then
                    Label5.Text = "0x4130 | Data: 1"
                ElseIf ComboBox2.Text = "Tundra" Then
                    Label5.Text = "0x4130 | Data: 2"
                ElseIf ComboBox2.Text = "Continental" Then
                    Label5.Text = "0x4130 | Data: 3"
                ElseIf ComboBox2.Text = "Garden" Then
                    Label5.Text = "0x4130 | Data: 4"
                ElseIf ComboBox2.Text = "Elegant" Then
                    Label5.Text = "0x4130 | Data: 5"
                ElseIf ComboBox2.Text = "Meadow" Then
                    Label5.Text = "0x4130 | Data: 6"
                ElseIf ComboBox2.Text = "Modern" Then
                    Label5.Text = "0x4130 | Data: 7"
                ElseIf ComboBox2.Text = "Marine" Then
                    Label5.Text = "0x4130 | Data: 8"
                ElseIf ComboBox2.Text = "Archipelago" Then
                    Label5.Text = "0x4130 | Data: 9"
                ElseIf ComboBox2.Text = "High-Plains" Then
                    Label5.Text = "0x4130 | Data: 10"
                ElseIf ComboBox2.Text = "Sandstorm" Then
                    Label5.Text = "0x4130 | Data: 11"
                ElseIf ComboBox2.Text = "River" Then
                    Label5.Text = "0x4130 | Data: 12"
                ElseIf ComboBox2.Text = "Monsoon" Then
                    Label5.Text = "0x4130 | Data: 13"
                ElseIf ComboBox2.Text = "Savannah" Then
                    Label5.Text = "0x4130 | Data: 14"
                ElseIf ComboBox2.Text = "Sun" Then
                    Label5.Text = "0x4130 | Data: 15"
                ElseIf ComboBox2.Text = "Ocean" Then
                    Label5.Text = "0x4130 | Data: 16"
                ElseIf ComboBox2.Text = "Jungle" Then
                    Label5.Text = "0x4130 | Data: 17"
                ElseIf ComboBox2.Text = "Fancy" Then
                    Label5.Text = "0x4130 | Data: 18"
                ElseIf ComboBox2.Text = "Pokéball" Then
                    Label5.Text = "0x4130 | Data: 19"
                    'lang
                ElseIf ComboBox2.Text = "日本語" Then
                    Label5.Text = "0x1235 | Data: 0x1"
                ElseIf ComboBox2.Text = "English" Then
                    Label5.Text = "0x1235 | Data: 0x2"
                ElseIf ComboBox2.Text = "Français" Then
                    Label5.Text = "0x1235 | Data: 0x3"
                ElseIf ComboBox2.Text = "Italiano" Then
                    Label5.Text = "0x1235 | Data: 0x4"
                ElseIf ComboBox2.Text = "Deutsch" Then
                    Label5.Text = "0x1235 | Data: 0x5"
                ElseIf ComboBox2.Text = "Español" Then
                    Label5.Text = "0x1235 | Data: 0x7"
                ElseIf ComboBox2.Text = "한국어" Then
                    Label5.Text = "0x1235 | Data: 0x8"
                ElseIf ComboBox2.Text = "中文 (简体)" Then
                    Label5.Text = "0x1235 | Data: 0x9"
                ElseIf ComboBox2.Text = "中文 (繁體)" Then
                    Label5.Text = "0x1235 | Data: 0xA"

                End If
            Case usum
                Dim cb2 As Integer = ComboBox2.SelectedIndex
                If ComboBox1.Text = "Wonder Card Slot" Then
                    Dim v As Integer = (&H66300 + (cb2 * &H108))
                    Label5.Text = "0x" & Hex(v)

                    'PC
                ElseIf ComboBox1.Text = "BOX " & boxnum Then
                    Dim v As Integer = (&H5200 + ((((boxnum - 1) * 30) + cb2) * &HE8))
                    Label5.Text = "0x" & Hex(v)

                    'USUM BS
                ElseIf ComboBox2.Text = "Normal" Then
                    Label5.Text = "0x147A | Data: 0"
                ElseIf ComboBox2.Text = "Elegant" Then
                    Label5.Text = "0x147A | Data: 1"
                ElseIf ComboBox2.Text = "Girlish" Then
                    Label5.Text = "0x147A | Data: 2"
                ElseIf ComboBox2.Text = "Reverant" Then
                    Label5.Text = "0x147A | Data: 3"
                ElseIf ComboBox2.Text = "Smug" Then
                    Label5.Text = "0x147A | Data: 4"
                ElseIf ComboBox2.Text = "Left-Handed" Then
                    Label5.Text = "0x147A | Data: 5"
                ElseIf ComboBox2.Text = "Passionate" Then
                    Label5.Text = "0x147A | Data: 6"
                ElseIf ComboBox2.Text = "Idol" Then
                    Label5.Text = "0x147A | Data: 7"
                ElseIf ComboBox2.Text = "Nihilist" Then
                    Label5.Text = "0x147A | Data: 8"
                    'USUM Viv
                ElseIf ComboBox2.Text = "Icy Snow" Then
                    Label5.Text = "0x4530 | Data: 0"
                ElseIf ComboBox2.Text = "Polar" Then
                    Label5.Text = "0x4530 | Data: 1"
                ElseIf ComboBox2.Text = "Tundra" Then
                    Label5.Text = "0x4530 | Data: 2"
                ElseIf ComboBox2.Text = "Continental" Then
                    Label5.Text = "0x4530 | Data: 3"
                ElseIf ComboBox2.Text = "Garden" Then
                    Label5.Text = "0x4530 | Data: 4"
                ElseIf ComboBox2.Text = "Elegant" Then
                    Label5.Text = "0x4530 | Data: 5"
                ElseIf ComboBox2.Text = "Meadow" Then
                    Label5.Text = "0x4530 | Data: 6"
                ElseIf ComboBox2.Text = "Modern" Then
                    Label5.Text = "0x4530 | Data: 7"
                ElseIf ComboBox2.Text = "Marine" Then
                    Label5.Text = "0x4530 | Data: 8"
                ElseIf ComboBox2.Text = "Archipelago" Then
                    Label5.Text = "0x4530 | Data: 9"
                ElseIf ComboBox2.Text = "High-Plains" Then
                    Label5.Text = "0x4530 | Data: 10"
                ElseIf ComboBox2.Text = "Sandstorm" Then
                    Label5.Text = "0x4530 | Data: 11"
                ElseIf ComboBox2.Text = "River" Then
                    Label5.Text = "0x4530 | Data: 12"
                ElseIf ComboBox2.Text = "Monsoon" Then
                    Label5.Text = "0x4530 | Data: 13"
                ElseIf ComboBox2.Text = "Savannah" Then
                    Label5.Text = "0x4530 | Data: 14"
                ElseIf ComboBox2.Text = "Sun" Then
                    Label5.Text = "0x4530 | Data: 15"
                ElseIf ComboBox2.Text = "Ocean" Then
                    Label5.Text = "0x4530 | Data: 16"
                ElseIf ComboBox2.Text = "Jungle" Then
                    Label5.Text = "0x4530 | Data: 17"
                ElseIf ComboBox2.Text = "Fancy" Then
                    Label5.Text = "0x4530 | Data: 18"
                ElseIf ComboBox2.Text = "Pokéball" Then
                    Label5.Text = "0x4530 | Data: 19"
                    'Lang
                ElseIf ComboBox2.Text = "日本語" Then
                    Label5.Text = "0x1435 | Data: 0x1"
                ElseIf ComboBox2.Text = "English" Then
                    Label5.Text = "0x1435 | Data: 0x2"
                ElseIf ComboBox2.Text = "Français" Then
                    Label5.Text = "0x1435 | Data: 0x3"
                ElseIf ComboBox2.Text = "Italiano" Then
                    Label5.Text = "0x1435 | Data: 0x4"
                ElseIf ComboBox2.Text = "Deutsch" Then
                    Label5.Text = "0x1435 | Data: 0x5"
                ElseIf ComboBox2.Text = "Español" Then
                    Label5.Text = "0x1435 | Data: 0x7"
                ElseIf ComboBox2.Text = "한국어" Then
                    Label5.Text = "0x1435 | Data: 0x8"
                ElseIf ComboBox2.Text = "中文 (简体)" Then
                    Label5.Text = "0x1435 | Data: 0x9"
                ElseIf ComboBox2.Text = "中文 (繁體)" Then
                    Label5.Text = "0x1435 | Data: 0xA"

                End If
        End Select
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Select Case ComboBox3.Text
            Case dp
                wcnum = 8
                pcd = True
                boxes = 18
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case pt
                wcnum = 8
                pcd = True
                boxes = 18
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case hgss
                wcnum = 8
                pcd = True
                boxes = 18
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case bw
                wcnum = 12
                pcd = False
                boxes = 24
                Me.ComboBox1.Items.Clear()
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case b2w2
                wcnum = 12
                pcd = False
                boxes = 24
                Me.ComboBox1.Items.Clear()
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case xy
                wcnum = 24
                pcd = False
                boxes = 31
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                ComboBox1.Items.Add("Vivillon")
                ComboBox1.Items.Add("Language")
                ComboBox1.Items.Add("Money")
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case oras
                wcnum = 24
                pcd = False
                boxes = 31
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                ComboBox1.Items.Add("Vivillon")
                ComboBox1.Items.Add("Language")
                ComboBox1.Items.Add("Money")
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case sm
                wcnum = 48
                pcd = False
                boxes = 32
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                ComboBox1.Items.Add("Vivillon")
                ComboBox1.Items.Add("Language")
                ComboBox1.Items.Add("Money")
                ComboBox1.Items.Add("Battle Points")
                ComboBox1.Items.Add("Festival Coins")
                boxy()
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
            Case usum
                wcnum = 48
                pcd = False
                boxes = 32
                Me.ComboBox1.Items.Clear()
                ComboBox1.Items.Add("Wonder Card Slot")
                ComboBox1.Items.Add("Battle Styles")
                ComboBox1.Items.Add("Vivillon")
                ComboBox1.Items.Add("Language")
                ComboBox1.Items.Add("Money")
                ComboBox1.Items.Add("Battle Points")
                ComboBox1.Items.Add("Festival Coins")
                boxy()
                'ComboBox1.Items.Add("BOX 2")
                ComboBox1.Enabled = True
                ComboBox1_SelectedIndexChanged(sender, e)
                ComboBox2_SelectedIndexChanged(sender, e)
        End Select

        ComboBox1.Text = "-------Offsets-------"
        Label5.Text = ""
        Button6.Enabled = False
        ComboBox2.Enabled = False
        ComboBox1_SelectedIndexChanged(sender, e)
        ComboBox2_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub boxy()
        For i = 1 To boxes Step 1
            ComboBox1.Items.Add("BOX " & i)
        Next i
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim dat() = Label5.Text.Split(" ")
        TabControl1.TabIndex -= 1
        TextBox2.Text = dat(LBound(dat))
        If ComboBox1.Text = "Wonder Card Slot" And ComboBox2.Text.Contains("WC ") Then
            If ComboBox3.Text = dp Or ComboBox3.Text = pt Or ComboBox3.Text = hgss Then
                TextBox4.Text = "0x104"
            Else
                TextBox4.Text = "0x108"
            End If
        ElseIf ComboBox1.Text = "Wonder Card Slot" And ComboBox2.Text.Contains("PCD ") Then
            If ComboBox3.Text = dp Or ComboBox3.Text = pt Or ComboBox3.Text = hgss Then
                TextBox4.Text = "0x358"
            End If
        ElseIf ComboBox1.Text = "Battle Styles" Or ComboBox1.Text = "Vivillon" Or ComboBox1.Text = "Language" Then
            TextBox4.Text = "1"
        ElseIf ComboBox1.Text = "BOX 1" Then
            If ComboBox3.Text = dp Or ComboBox3.Text = pt Or ComboBox3.Text = hgss Or ComboBox3.Text = bw Or ComboBox3.Text = b2w2 Then
                TextBox4.Text = "0x88"
            Else
                TextBox4.Text = "0xE8"
            End If
        End If
        If Label5.Text.Contains("Data:") Then
            TextBox3.Text = dat(UBound(dat))
        End If
        If ComboBox1.Text = "Money" Or ComboBox1.Text = "Battle Points" Or ComboBox1.Text = "Festival Coins" Then
            TextBox4.Text = "4"
        End If
        ComboBox4.Text = ComboBox3.Text
        ComboBox4_SelectedIndexChanged(sender, e)
        'TabControl1.TabIndex += 1
        TabControl1.SelectedTab = TabPage1
    End Sub
    Private Sub Label5_TextChanged(sender As Object, e As EventArgs) Handles Label5.TextChanged
        Button6.Enabled = True
    End Sub
    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text = "0x4404" Or TextBox2.Text = "0x4004" Or TextBox2.Text = "0x4208" Then
            TextBox3.MaxLength = 7
        ElseIf TextBox2.Text = "0x0451C" Or TextBox2.Text = "0x0411C" Then
            TextBox3.MaxLength = 4
        ElseIf TextBox2.Text = "0x51308" Or TextBox2.Text = "0x50D08" Then
            TextBox3.MaxLength = 7
        Else
            TextBox3.MaxLength = TextBox1.MaxLength
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim x = 43 Xor 178654430
        MsgBox(x)
    End Sub
#End Region

End Class