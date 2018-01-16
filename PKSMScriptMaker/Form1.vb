Public Class Form1
#Region "Variables"
    Dim pathexe = My.Application.Info.DirectoryPath
    Dim path = pathexe & "\assets"
    Dim web As New System.Net.WebClient
    Dim prog As String = pathexe & "\PKSMScript.py"
    Dim sm As String
    Dim data(3) As String

    Dim stat As Integer = 0
#End Region

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        checkUpdate()
        Dim Everest_Registry As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Python\PythonCore")
        If Everest_Registry Is Nothing Then
            'key does not exist
            Dim py As Integer = MsgB("Python 3+ is required for script creation. Would you like to get python or continue without script creation?", 2, "Get Python", "Continue Anyway", "", "Python Required")
            If py = 6 Then
                Dim webAddress As String = "https://www.python.org/downloads/"
                Process.Start(webAddress)
                Application.Exit()
            ElseIf py = 7 Then
                TabPage1.Enabled = False
            End If
        Else
        End If

        TabControl1.TabIndex += 1
        RichTextBox1.Hide()
        buttons()
        TabControl1.TabIndex -= 1
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

#Region "WC7FULL to WC7"
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
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "wc7full files (*.wc7full)|*.wc7full|All files (*.*)|*.*"
        OpenFileDialog1.ShowDialog()
        Dim myFile As String = OpenFileDialog1.FileName
        Dim ext() As String = myFile.Split(".")

        If ext(UBound(ext)) = "wc7full" Or ext(UBound(ext)) = "WC7FULL" Then
            'Dim myFile As String = "..\..\0264 USUM - Item Roto Catch x1 (JPN).wc7full"
            Dim myBytes As Byte() = My.Computer.FileSystem.ReadAllBytes(myFile)
            Dim txtTemp As New System.Text.StringBuilder()
            For Each myByte As Byte In myBytes
                txtTemp.Append(myByte.ToString("X2"))
            Next
            RichTextBox1.Text = txtTemp.ToString()

            stat = 1
            buttons()
        Else
            MsgBox("Not a WC7FULL file", MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SaveFileDialog1.Filter = "wc7 files (*.wc7)|*.wc7|All files (*.*)|*.*"
        SaveFileDialog1.ShowDialog()
        Dim myFile As String = SaveFileDialog1.FileName

        'Dim myFile As String = "..\..\0264 USUM - Item Roto Catch x1 (JPN) 2.wc7"
        Dim myBytes As Byte() = HexStringToByteArray(RichTextBox1.Text)
        My.Computer.FileSystem.WriteAllBytes(myFile, myBytes, False)

        stat = 0
        buttons()
    End Sub
    Private Sub cut()
        Dim SizeWC = 528
        Dim SizeFull = 1568
        Dim data As String = RichTextBox1.Text
        Debug.Print(data.Length)
        If (data.Length = SizeFull) Then
            data = data.Skip(SizeFull - SizeWC).ToArray()
        End If
        RichTextBox1.Text = data
        MsgBox("Successfully Converted", MsgBoxStyle.OkOnly)
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        cut()
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
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Missing Value(s)", MsgBoxStyle.OkOnly)
        Else
            System.IO.File.WriteAllText(prog, My.Resources.PKSMScript)
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
            If data(3).Contains("C:\") Then
                Dim da() As String = data(3).Split("\")
                da(UBound(da)) = da(UBound(da)).Replace(" ", "_")
                If System.IO.File.Exists(pathexe & "\" & da(UBound(da))) Then
                Else
                    System.IO.File.Copy(data(3), pathexe & "\" & da(UBound(da)))
                End If
                Dim d2 As String = da(UBound(da))
                sm = "@py -3 PKSMScript.py """ & data(0) & """ -i " & data(1) & " " & data(2) & " " & """" & d2 & """" & " 1
@PUSHD %1
@MOVE *.pksm output
@del PKSMScript.py
@del " & da(UBound(da)) & "
@del output.bat"
            Else
                sm = "@py -3 PKSMScript.py """ & data(0) & """ -i " & data(1) & " " & data(2) & " " & data(3) & " 1
@PUSHD %1
@MOVE *.pksm output
@del PKSMScript.py
@del output.bat"
            End If
            System.IO.File.WriteAllText(pathexe & "\output.bat", sm)
            My.Computer.FileSystem.CreateDirectory(pathexe & "\output")
            Process.Start(pathexe & "\output.bat")
            System.Threading.Thread.Sleep(1000)
            MsgBox("Done", 0)
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpenFileDialog2.Filter = "wc7 files (*.wc7)|*.wc7|bin files (*.bin)|*.bin|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog2.ShowDialog()
        TextBox3.Text = OpenFileDialog2.FileName
    End Sub
#End Region

#Region "Info"
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Wonder Card Slot" Then
            ComboBox2.Enabled = True
            ComboBox2.Text = "--Slot #--"
            Me.ComboBox2.Items.Clear()
            ComboBox2.Items.Add("1")
            ComboBox2.Items.Add("2")
            ComboBox2.Items.Add("3")
            ComboBox2.Items.Add("4")
            ComboBox2.Items.Add("5")
            ComboBox2.Items.Add("6")
            ComboBox2.Items.Add("7")
            ComboBox2.Items.Add("8")
            ComboBox2.Items.Add("9")
            ComboBox2.Items.Add("10")
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
        Else
            ComboBox2.Enabled = False
        End If
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
#Region "SM"
        'SM WC
        If ComboBox2.Text = "1" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65D00"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65E08"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x65F10"
        ElseIf ComboBox2.Text = "4" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66018"
        ElseIf ComboBox2.Text = "5" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66120"
        ElseIf ComboBox2.Text = "6" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66228"
        ElseIf ComboBox2.Text = "7" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66330"
        ElseIf ComboBox2.Text = "8" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66438"
        ElseIf ComboBox2.Text = "9" And ComboBox3.Text = "Sun/Moon" Then
            Label5.Text = "0x66540"
        ElseIf ComboBox2.Text = "10" And ComboBox3.Text = "Sun/Moon" Then
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
#End Region
#Region "USUM"
            'USUM WC
        ElseIf ComboBox2.Text = "1" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66300"
        ElseIf ComboBox2.Text = "2" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66408"
        ElseIf ComboBox2.Text = "3" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66510"
        ElseIf ComboBox2.Text = "4" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66618"
        ElseIf ComboBox2.Text = "5" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66720"
        ElseIf ComboBox2.Text = "6" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66828"
        ElseIf ComboBox2.Text = "7" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66930"
        ElseIf ComboBox2.Text = "8" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66A38"
        ElseIf ComboBox2.Text = "9" And ComboBox3.Text = "UltraSun/UltraMoon" Then
            Label5.Text = "0x66B40"
        ElseIf ComboBox2.Text = "10" And ComboBox3.Text = "UltraSun/UltraMoon" Then
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
#End Region
        End If
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.Text = "Sun/Moon" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("Vivillon")
        ElseIf ComboBox3.Text = "UltraSun/UltraMoon" Then
            Me.ComboBox1.Items.Clear()
            ComboBox1.Items.Add("Wonder Card Slot")
            ComboBox1.Items.Add("Battle Styles")
            ComboBox1.Items.Add("Vivillon")
        End If
        ComboBox1.Enabled = True
        ComboBox2_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim dat() = Label5.Text.Split(" ")
        TabControl1.TabIndex -= 2
        TextBox2.Text = dat(LBound(dat))
        If ComboBox1.Text = "Wonder Card Slot" Then
            TextBox4.Text = "0x108"
        ElseIf ComboBox1.Text = "Battle Styles" Or ComboBox1.Text = "Vivillon" Then
            TextBox4.Text = "1"
        End If
        If Label5.Text.Contains("Data:") Then
            TextBox3.Text = dat(UBound(dat))
        End If
        TabControl1.TabIndex += 2
    End Sub
    Private Sub Label5_TextChanged(sender As Object, e As EventArgs) Handles Label5.TextChanged
        Button6.Enabled = True
    End Sub
#End Region

End Class