Public Class Form1
#Region "Variables"
    Dim pathexe = My.Application.Info.DirectoryPath
    Dim path = pathexe & "\assets"
    Dim web As New System.Net.WebClient
    Dim prog As String = pathexe & "\PKSMScript.py"
    Dim progG As String = pathexe & "\genScripts.py"
    Dim sm As String
    Dim data(3) As String

    Dim stat As Integer = 0

    Dim ext() As String
    Dim type As Integer = 0

    Dim gt As String = "PSM"
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
                    checkUpdate()
            End Select
        End If
    End Sub


#End Region

    'Private Sub fonti()
    '    System.IO.File.WriteAllBytes(pathexe & "\Regnum_Handwriting.ttf", My.Resources.Regnum_Handwriting)
    '    Process.Start(pathexe & "\Regnum_Handwriting.ttf")

    'End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.AutoUpdating = True Then
            checkUpdate()
        End If
        'fonti()
        'System.IO.File.Delete(pathexe & "\Regnum_Handwriting.ttf")
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

        TabControl1.TabIndex += 2
        RichTextBox1.Hide()
        Button8.Hide()
        buttons()
        TabControl1.TabIndex -= 1
        Button7.Hide()
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
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim about As New about
        about.ShowDialog()
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
    Private Sub sav()
        Dim myFile As String = OpenFileDialog1.FileName
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
            'ElseIf ext(UBound(ext)) = "ek3" Or ext(UBound(ext)) = "EK3" Then
            '    Fn4 = Fn3.Replace("ek3", ".smk3")
            'ElseIf ext(UBound(ext)) = "ek2" Or ext(UBound(ext)) = "EK2" Then
            '    Fn4 = Fn3.Replace("ek2", ".smk2")
            'ElseIf ext(UBound(ext)) = "ek1" Or ext(UBound(ext)) = "EK1" Then
            '    Fn4 = Fn3.Replace("ek1", ".smk1")
        End If

        SaveFileDialog1.FileName = Fn4
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "All Supported Files (FULLWonderCards, *.ek#)|*.wc7full;*.wc6full;*.pcd;*.ek7;*.ek6;*.ek5;*.ek4|WonderCards (*.wc7full, *.wc6full, *.pcd)|*.wc7full;*.wc6full;*.pcd|Encrypted PK Files (*.ek#)|*.ek7;*.ek6;*.ek5;*.ek4;*.ek3;*.ek2;*.ek1|All files (*.*)|*.*"
        OpenFileDialog1.ShowDialog()
        Dim myFile As String = OpenFileDialog1.FileName
        Dim ext() As String = myFile.Split(".")

        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Or ext(UBound(ext)) = "wc6full" Or ext(UBound(ext)) = "WC6FULL" Or ext(UBound(ext)) = "pcd" Or ext(UBound(ext)) = "PCD" Or ext(UBound(ext)) = "ek7" Or ext(UBound(ext)) = "EK7" Or ext(UBound(ext)) = "ek6" Or ext(UBound(ext)) = "EK6" Or ext(UBound(ext)) = "ek4" Or ext(UBound(ext)) = "EK4" Or ext(UBound(ext)) = "ek5" Or ext(UBound(ext)) = "EK5" Then
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
            End If
            sav()
            stat = 1
            buttons()
        Else
            MsgBox("Not a vaild file", MsgBoxStyle.OkOnly)
        End If
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
    'Private Sub enc()
    '    ' Decrypted, Encrypt
    '    'Dim seed As UInteger = value.Seed
    '    Dim i As Integer = 0
    '    Dim wcData() As Byte = RichTextBox1.Text 'New Byte((2704) - 1) {}
    '    Do While (i < wcData.Length)
    '        BitConverter.GetBytes(CType(((BitConverter.ToUInt16(wcData, i)) + 16), System.UInt16)).CopyTo(wcData, i)
    '        i = (i + 2)
    '    Loop
    'End Sub
    'Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
    '    enc()
    'End Sub
#End Region

#Region "Script Maker"
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Missing Value(s)", MsgBoxStyle.OkOnly)
        Else
            System.IO.File.WriteAllText(prog, My.Resources.PKSMScript)
            System.IO.File.WriteAllText(progG, My.Resources.genScripts)
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
            data(3) = TextBox3.Text
            If data(3).Contains(":\") Then
                Dim da() As String = data(3).Split("\")
                da(UBound(da)) = da(UBound(da)).Replace(" ", "_")
                Dim ex() As String = da(UBound(da)).Split(".")
                Dim dex As String = ex(UBound(ex)).ToLower
                Dim wcfn As String = ""
                If dex = "wc7" Then
                    wcfn = "g7wc.wc7"
                ElseIf dex = "wc6" Then
                    wcfn = "g6wc.wc6"
                ElseIf dex = "pgf" Then
                    wcfn = "g5wc.pgf"
                ElseIf dex = "pgt" Then
                    wcfn = "g4wc.pgt"
                ElseIf dex = "bin" Then
                    wcfn = "binary.bin"
                ElseIf dex = "txt" Then
                    wcfn = "text.txt"
                End If
                If System.IO.File.Exists(pathexe & "\" & wcfn) Then
                Else
                    System.IO.File.Copy(data(3), pathexe & "\" & wcfn)
                End If
                Dim d2 As String = wcfn
                sm = """" & data(0) & """ -i " & data(1) & " " & data(2) & " " & """" & d2 & """" & " 1"
            Else
                sm = """" & data(0) & """ -i " & data(1) & " " & data(2) & " " & data(3) & " 1"
            End If
            System.IO.File.WriteAllText(pathexe & "\scripts" & gt & ".txt", sm)
            Process.Start(pathexe & "\genScripts.py")
            System.Threading.Thread.Sleep(1000)
            MsgBox("Done", 0)
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'OpenFileDialog2.Filter = "Gen6/7 WonderCards (*.wc7, *.wc6)|*.wc7;*.wc6|Gen5 WonderCards (*.pgf)|*.pgf|Gen4 WonderCards (*.pgt, *.pcd)|*.pgt;*.pcd|Pokémon Files (*.pk#)|*.pk7;*.pk6;*.pk5;*.pk4|bin files (*.bin)|*.bin|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog2.Filter = "All Supported Files (WonderCards, *.smk#, Binary, and Text files)|*.wc7;*.wc6;*.pgf;*.pgt;*.bin;*.txt;*.smk7;*.smk6;*.smk5;*.smk4|WonderCards (*.wc7, *.wc6, *.pgf, *.pgt)|*.wc7;*.wc6;*.pgf;*.pgt|Script Maker PK files (*.smk#)|*.smk7;*.smk6;*.smk5;*.smk4|bin files (*.bin)|*.bin|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog2.ShowDialog()
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
        If ComboBox4.Text = "Diamond/Pearl" Then
            gt = "DP"
        ElseIf ComboBox4.Text = "Platinum" Then
            gt = "PT"
        ElseIf ComboBox4.Text = "HeartGold/SoulSilver" Then
            gt = "HGSS"
        ElseIf ComboBox4.Text = "Black/White" Then
            gt = "BW"
        ElseIf ComboBox4.Text = "Black 2/White 2" Then
            gt = "B2W2"
        ElseIf ComboBox4.Text = "X/Y" Then
            gt = "XY"
        ElseIf ComboBox4.Text = "OmegaRuby/AlphaSapphire" Then
            gt = "ORAS"
        ElseIf ComboBox4.Text = "Sun/Moon" Then
            gt = "SM"
        ElseIf ComboBox4.Text = "UltraSun/UltraMoon" Then
            gt = "USUM"
        Else
            gt = "PSM"
        End If
    End Sub
#End Region

#Region "Info"
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Wonder Card Slot" Then
            ComboBox2.Enabled = True
            ComboBox2.Text = "--Slot #--"
            Me.ComboBox2.Items.Clear()
            ComboBox2.Items.Add("WC 1")
            ComboBox2.Items.Add("WC 2")
            ComboBox2.Items.Add("WC 3")
            ComboBox2.Items.Add("WC 4")
            ComboBox2.Items.Add("WC 5")
            ComboBox2.Items.Add("WC 6")
            ComboBox2.Items.Add("WC 7")
            ComboBox2.Items.Add("WC 8")
            ComboBox2.Items.Add("WC 9")
            ComboBox2.Items.Add("WC 10")
        ElseIf ComboBox1.Text = "Battle Styles" Then
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
        ElseIf ComboBox1.Text = "Vivillon" Then
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
        ElseIf ComboBox1.Text = "Money" Then
            ComboBox2.Enabled = False
            ComboBox2.Text = "----"
            Me.ComboBox2.Items.Clear()
            If ComboBox3.Text = "UltraSun/UltraMoon" Then
                Label5.Text = "0x4404"
            ElseIf ComboBox3.Text = "Sun/Moon" Then
                Label5.Text = "0x4004"
            ElseIf ComboBox3.Text = "OmegaRuby/AlphaSapphire" Or ComboBox3.Text = "X/Y" Then
                Label5.Text = "0x4208"
            End If
        ElseIf ComboBox1.Text = "Battle Points" Then
            ComboBox2.Enabled = False
            ComboBox2.Text = "----"
            Me.ComboBox2.Items.Clear()
            If ComboBox3.Text = "UltraSun/UltraMoon" Then
                Label5.Text = "0x0451C"
            ElseIf ComboBox3.Text = "Sun/Moon" Then
                Label5.Text = "0x0411C"
            End If
        ElseIf ComboBox1.Text = "Festival Coins" Then
            ComboBox2.Enabled = False
            ComboBox2.Text = "----"
            Me.ComboBox2.Items.Clear()
            If ComboBox3.Text = "UltraSun/UltraMoon" Then
                Label5.Text = "0x51308"
            ElseIf ComboBox3.Text = "Sun/Moon" Then
                Label5.Text = "0x50D08"
            End If
        ElseIf ComboBox1.Text = "Language" Then
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
            If ComboBox3.Text = "UltraSun/UltraMoon" Or ComboBox3.Text = "Sun/Moon" Then
                ComboBox2.Items.Add("中文 (简体)")
                ComboBox2.Items.Add("中文 (繁體)")
                'ComboBox2.Items.Add("10")
            End If
        ElseIf ComboBox1.Text = "BOX 1" Then
            ComboBox2.Enabled = True
            ComboBox2.Text = "--Slot #--"
            Me.ComboBox2.Items.Clear()
            ComboBox2.Items.Add("1")
            ComboBox2.Items.Add("2")
            ComboBox2.Items.Add("3")
        Else
            ComboBox2.Enabled = False
        End If
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
#Region "DP"
        If ComboBox2.Text = "WC 1" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4A7FC"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4A900"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4AA04"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4AB08"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4AC0C"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4AD10"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4AE14"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4AF18"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4B01C"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0x4B120"

        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0xC104"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0xC18C"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Diamond/Pearl" Then
            Label5.Text = "0xC214"
#End Region
#Region "Pt"
        ElseIf ComboBox2.Text = "WC 1" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4B5C0"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4B6C4"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4B7C8"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4B8CC"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4B9D0"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4BAD4"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4BBD8"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4BCDC"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4BDE0"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0x4BEE4"

        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0xCF30"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0xCFB8"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Platinum" Then
            Label5.Text = "0xD040"
#End Region
#Region "HGSS"
        ElseIf ComboBox2.Text = "WC 1" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0x9E3C"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0x9F40"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA044"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA148"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA24C"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA350"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA454"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA558"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA65C"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xA760"

        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xF700"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xF788"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "HeartGold/SoulSilver" Then
            Label5.Text = "0xF810"
#End Region
#Region "BW"
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "Black/White" Then
            Label5.Text = "0x400"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Black/White" Then
            Label5.Text = "0x488"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Black/White" Then
            Label5.Text = "0x510"
#End Region
#Region "B2W2"
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "Black 2/White 2" Then
            Label5.Text = "0x400"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Black 2/White 2" Then
            Label5.Text = "0x488"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Black 2/White 2" Then
            Label5.Text = "0x510"
#End Region
#Region "XY"
        ElseIf ComboBox2.Text = "WC 1" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1BD00"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1BE08"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1BF10"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C018"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C120"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C228"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C330"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C438"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C540"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1C648"
            'ORAS Viv
        ElseIf ComboBox2.Text = "Icy Snow" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 0"
        ElseIf ComboBox2.Text = "Polar" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 1"
        ElseIf ComboBox2.Text = "Tundra" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 2"
        ElseIf ComboBox2.Text = "Continental" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 3"
        ElseIf ComboBox2.Text = "Garden" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 4"
        ElseIf ComboBox2.Text = "Elegant" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 5"
        ElseIf ComboBox2.Text = "Meadow" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 6"
        ElseIf ComboBox2.Text = "Modern" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 7"
        ElseIf ComboBox2.Text = "Marine" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 8"
        ElseIf ComboBox2.Text = "Archipelago" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 9"
        ElseIf ComboBox2.Text = "High-Plains" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 10"
        ElseIf ComboBox2.Text = "Sandstorm" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 11"
        ElseIf ComboBox2.Text = "River" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 12"
        ElseIf ComboBox2.Text = "Monsoon" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 13"
        ElseIf ComboBox2.Text = "Savannah" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 14"
        ElseIf ComboBox2.Text = "Sun" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 15"
        ElseIf ComboBox2.Text = "Ocean" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 16"
        ElseIf ComboBox2.Text = "Jungle" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 17"
        ElseIf ComboBox2.Text = "Fancy" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 18"
        ElseIf ComboBox2.Text = "Pokéball" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x4250 | Data: 19"
            'lang
        ElseIf ComboBox2.Text = "日本語" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x1"
        ElseIf ComboBox2.Text = "English" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x2"
        ElseIf ComboBox2.Text = "Français" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x3"
        ElseIf ComboBox2.Text = "Italiano" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x4"
        ElseIf ComboBox2.Text = "Deutsch" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x5"
        ElseIf ComboBox2.Text = "Español" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x7"
        ElseIf ComboBox2.Text = "한국어" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x1402D | Data: 0x8"
            'box 1
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x22600"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x226E8"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "X/Y" Then
            Label5.Text = "0x227D0"
#End Region
#Region "ORAS"
        ElseIf ComboBox2.Text = "WC 1" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1CD00"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1CE08"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1CF10"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D018"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D120"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D228"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D330"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D438"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D540"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1D648"
            'ORAS Viv
        ElseIf ComboBox2.Text = "Icy Snow" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 0"
        ElseIf ComboBox2.Text = "Polar" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 1"
        ElseIf ComboBox2.Text = "Tundra" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 2"
        ElseIf ComboBox2.Text = "Continental" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 3"
        ElseIf ComboBox2.Text = "Garden" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 4"
        ElseIf ComboBox2.Text = "Elegant" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 5"
        ElseIf ComboBox2.Text = "Meadow" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 6"
        ElseIf ComboBox2.Text = "Modern" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 7"
        ElseIf ComboBox2.Text = "Marine" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 8"
        ElseIf ComboBox2.Text = "Archipelago" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 9"
        ElseIf ComboBox2.Text = "High-Plains" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 10"
        ElseIf ComboBox2.Text = "Sandstorm" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 11"
        ElseIf ComboBox2.Text = "River" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 12"
        ElseIf ComboBox2.Text = "Monsoon" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 13"
        ElseIf ComboBox2.Text = "Savannah" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 14"
        ElseIf ComboBox2.Text = "Sun" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 15"
        ElseIf ComboBox2.Text = "Ocean" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 16"
        ElseIf ComboBox2.Text = "Jungle" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 17"
        ElseIf ComboBox2.Text = "Fancy" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 18"
        ElseIf ComboBox2.Text = "Pokéball" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x4244 | Data: 19"
            'lang
        ElseIf ComboBox2.Text = "日本語" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x1"
        ElseIf ComboBox2.Text = "English" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x2"
        ElseIf ComboBox2.Text = "Français" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x3"
        ElseIf ComboBox2.Text = "Italiano" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x4"
        ElseIf ComboBox2.Text = "Deutsch" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x5"
        ElseIf ComboBox2.Text = "Español" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x7"
        ElseIf ComboBox2.Text = "한국어" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x1402D | Data: 0x8"
            'box 1
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x33000"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x330E8"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Label5.Text = "0x331D0"
#End Region
#Region "SM"
            'SM WC
        ElseIf ComboBox2.Text = "WC 1" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65D00"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65E08"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65F10"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66018"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66120"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66228"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66330"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66438"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66540"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66648"
            'SM Viv
        ElseIf ComboBox2.Text = "Icy Snow" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 0"
        ElseIf ComboBox2.Text = "Polar" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 1"
        ElseIf ComboBox2.Text = "Tundra" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 2"
        ElseIf ComboBox2.Text = "Continental" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 3"
        ElseIf ComboBox2.Text = "Garden" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 4"
        ElseIf ComboBox2.Text = "Elegant" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 5"
        ElseIf ComboBox2.Text = "Meadow" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 6"
        ElseIf ComboBox2.Text = "Modern" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 7"
        ElseIf ComboBox2.Text = "Marine" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 8"
        ElseIf ComboBox2.Text = "Archipelago" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 9"
        ElseIf ComboBox2.Text = "High-Plains" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 10"
        ElseIf ComboBox2.Text = "Sandstorm" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 11"
        ElseIf ComboBox2.Text = "River" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 12"
        ElseIf ComboBox2.Text = "Monsoon" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 13"
        ElseIf ComboBox2.Text = "Savannah" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 14"
        ElseIf ComboBox2.Text = "Sun" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 15"
        ElseIf ComboBox2.Text = "Ocean" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 16"
        ElseIf ComboBox2.Text = "Jungle" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 17"
        ElseIf ComboBox2.Text = "Fancy" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 18"
        ElseIf ComboBox2.Text = "Pokéball" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4130 | Data: 19"
            'lang
        ElseIf ComboBox2.Text = "日本語" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x1"
        ElseIf ComboBox2.Text = "English" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x2"
        ElseIf ComboBox2.Text = "Français" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x3"
        ElseIf ComboBox2.Text = "Italiano" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x4"
        ElseIf ComboBox2.Text = "Deutsch" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x5"
        ElseIf ComboBox2.Text = "Español" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x7"
        ElseIf ComboBox2.Text = "한국어" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x8"
        ElseIf ComboBox2.Text = "中文 (简体)" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0x9"
        ElseIf ComboBox2.Text = "中文 (繁體)" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x1235 | Data: 0xA"
            'box 1
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4E00"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4EE8"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x4FD0"
#End Region
#Region "USUM"
            'USUM WC
        ElseIf ComboBox2.Text = "WC 1" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66300"
        ElseIf ComboBox2.Text = "WC 2" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66408"
        ElseIf ComboBox2.Text = "WC 3" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66510"
        ElseIf ComboBox2.Text = "WC 4" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66618"
        ElseIf ComboBox2.Text = "WC 5" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66720"
        ElseIf ComboBox2.Text = "WC 6" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66828"
        ElseIf ComboBox2.Text = "WC 7" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66930"
        ElseIf ComboBox2.Text = "WC 8" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66A38"
        ElseIf ComboBox2.Text = "WC 9" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66B40"
        ElseIf ComboBox2.Text = "WC 10" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66C48"
            'USUM BS
        ElseIf ComboBox2.Text = "Normal" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 0"
        ElseIf ComboBox2.Text = "Elegant" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 1"
        ElseIf ComboBox2.Text = "Girlish" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 2"
        ElseIf ComboBox2.Text = "Reverant" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 3"
        ElseIf ComboBox2.Text = "Smug" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 4"
        ElseIf ComboBox2.Text = "Left-Handed" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 5"
        ElseIf ComboBox2.Text = "Passionate" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 6"
        ElseIf ComboBox2.Text = "Idol" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 7"
        ElseIf ComboBox2.Text = "Nihilist" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x147A | Data: 8"
            'USUM Viv
        ElseIf ComboBox2.Text = "Icy Snow" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 0"
        ElseIf ComboBox2.Text = "Polar" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 1"
        ElseIf ComboBox2.Text = "Tundra" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 2"
        ElseIf ComboBox2.Text = "Continental" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 3"
        ElseIf ComboBox2.Text = "Garden" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 4"
        ElseIf ComboBox2.Text = "Elegant" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 5"
        ElseIf ComboBox2.Text = "Meadow" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 6"
        ElseIf ComboBox2.Text = "Modern" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 7"
        ElseIf ComboBox2.Text = "Marine" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 8"
        ElseIf ComboBox2.Text = "Archipelago" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 9"
        ElseIf ComboBox2.Text = "High-Plains" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 10"
        ElseIf ComboBox2.Text = "Sandstorm" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 11"
        ElseIf ComboBox2.Text = "River" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 12"
        ElseIf ComboBox2.Text = "Monsoon" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 13"
        ElseIf ComboBox2.Text = "Savannah" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 14"
        ElseIf ComboBox2.Text = "Sun" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 15"
        ElseIf ComboBox2.Text = "Ocean" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 16"
        ElseIf ComboBox2.Text = "Jungle" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 17"
        ElseIf ComboBox2.Text = "Fancy" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 18"
        ElseIf ComboBox2.Text = "Pokéball" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x4530 | Data: 19"
            'Lang
        ElseIf ComboBox2.Text = "日本語" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x1"
        ElseIf ComboBox2.Text = "English" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x2"
        ElseIf ComboBox2.Text = "Français" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x3"
        ElseIf ComboBox2.Text = "Italiano" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x4"
        ElseIf ComboBox2.Text = "Deutsch" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x5"
        ElseIf ComboBox2.Text = "Español" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x7"
        ElseIf ComboBox2.Text = "한국어" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x8"
        ElseIf ComboBox2.Text = "中文 (简体)" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0x9"
        ElseIf ComboBox2.Text = "中文 (繁體)" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x1435 | Data: 0xA"
            'box 1
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x5200"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x52E8"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x53D0"
#End Region
        End If
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.Text = "Sun/Moon" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("Vivillon")
            ComboBox1.Items.Add("Language")
            ComboBox1.Items.Add("Money")
            ComboBox1.Items.Add("Battle Points")
            ComboBox1.Items.Add("Festival Coins")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "UltraSun/UltraMoon" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("Battle Styles")
            ComboBox1.Items.Add("Vivillon")
            ComboBox1.Items.Add("Language")
            ComboBox1.Items.Add("Money")
            ComboBox1.Items.Add("Battle Points")
            ComboBox1.Items.Add("Festival Coins")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "OmegaRuby/AlphaSapphire" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("Vivillon")
            ComboBox1.Items.Add("Language")
            ComboBox1.Items.Add("Money")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "X/Y" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("Vivillon")
            ComboBox1.Items.Add("Language")
            ComboBox1.Items.Add("Money")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "Diamond/Pearl" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "Platinum" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "HeartGold/SoulSilver" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "Black/White" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)
        ElseIf ComboBox3.Text = "Black 2/White 2" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("BOX 1")
            ComboBox1.Enabled = True
            ComboBox1_SelectedIndexChanged(sender, e)
            ComboBox2_SelectedIndexChanged(sender, e)

        End If
        ComboBox1.Text = "-------Offsets-------"
        Label5.Text = ""
        ComboBox2.Enabled = False
        ComboBox1_SelectedIndexChanged(sender, e)
        ComboBox2_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim dat() = Label5.Text.Split(" ")
        TabControl1.TabIndex -= 2
        TextBox2.Text = dat(LBound(dat))
        If ComboBox1.Text = "Wonder Card Slot" Then
            If ComboBox3.Text = "Diamond/Pearl" Or ComboBox3.Text = "Platinum" Or ComboBox3.Text = "HeartGold/SoulSilver" Then
                TextBox4.Text = "0x104"
            Else
                TextBox4.Text = "0x108"
            End If
        ElseIf ComboBox1.Text = "Battle Styles" Or ComboBox1.Text = "Vivillon" Or ComboBox1.Text = "Language" Then
            TextBox4.Text = "1"
        ElseIf ComboBox1.Text = "BOX 1" Then
            If ComboBox3.Text = "Diamond/Pearl" Or ComboBox3.Text = "Platinum" Or ComboBox3.Text = "HeartGold/SoulSilver" Or ComboBox3.Text = "Black/White" Or ComboBox3.Text = "Black 2/White 2" Then
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
        TabControl1.TabIndex += 2
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
#End Region

End Class